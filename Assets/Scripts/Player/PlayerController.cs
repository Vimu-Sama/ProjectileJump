using UnityEngine;
using System.Collections;
using UI;
using GameAudio;

namespace Player
{
    public class PlayerController : GenericSingleton<PlayerController>
    {
        [SerializeField] private float angleInDeg;
        [SerializeField] private float speedMaxima = 5f;
        [SerializeField] private float initialVelocity;
        [SerializeField] private float lineRendererLOD = 10;
        [SerializeField] private float step;
        [SerializeField] private bool isMainMenu;
        [SerializeField] private Transform deathColliderTransform ;
        private bool lineDirection = true ; //false- back direction || true- front direction
        private bool isJumping = false ;
        private int count = 0 ;
        private float currTime = 0f ;
        private float itr ;
        private float angleInRad;
        private float maxTime = 60f;
        private float originalVelocity = 0f ;
        private Vector3 currTransform ;
        private Vector3 lineTrajectoryTransform ;
        private Vector3 initialPos;
        private Vector3 deathColliderPos;
        private LineRenderer lineRenderer;
        private WaitForSeconds waitForSeconds;
        private UiManager uiManager;
        private AudioManager audioManager;

        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            initialPos = transform.position;
            waitForSeconds = new WaitForSeconds(100f);
            originalVelocity = initialVelocity;
            uiManager = UiManager.Instance;
            audioManager=  AudioManager.Instance;
        }
        private void Update()
        {
            if (lineDirection)
            {
                if (initialVelocity >= (speedMaxima + originalVelocity))
                    lineDirection = false;
                initialVelocity += Time.deltaTime;
            }
            else
            {
                if (initialVelocity <= (originalVelocity - speedMaxima))
                    lineDirection = true;
                initialVelocity -= Time.deltaTime;
            }
            angleInRad = Mathf.Deg2Rad * angleInDeg;
            DrawLine(angleInRad, initialVelocity, step);
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isMainMenu)
            {
                isJumping = true;
                StopAllCoroutines();
                audioManager.PlayClip(GameAudio.GameSoundEnum.jumpSound);
                StartCoroutine(MakeProjectileDisappear());
                StartCoroutine(MakeProjectile(angleInRad, initialVelocity)); ;
            }
        }

        private void DrawLine(float _angle, float _velocity, float _step)
        {
            _step = Mathf.Max(0.01f, _step);
            lineRenderer.positionCount = (int)(lineRendererLOD / _step) + 2;
            count = 0;
            lineTrajectoryTransform.x = 0;
            lineTrajectoryTransform.y = 0;
            lineTrajectoryTransform.z = 0;
            for (itr = 0; itr < lineRendererLOD; itr += _step)
            {
                lineTrajectoryTransform.x = _velocity * Mathf.Cos(_angle) * itr;
                lineTrajectoryTransform.y = (_velocity * Mathf.Sin(_angle) * itr) - ((0.5f) *
                        (-Physics.gravity.y) * itr * itr);
                lineRenderer.SetPosition(count, initialPos + lineTrajectoryTransform);
                count++;
            }
            lineTrajectoryTransform.x = _velocity * Mathf.Cos(_angle) * lineRendererLOD;
            lineTrajectoryTransform.y = _velocity * Mathf.Sin(_angle) - (0.5f) * (-Physics.gravity.y) * lineRendererLOD * lineRendererLOD;
            lineRenderer.SetPosition(count, initialPos + lineTrajectoryTransform);
        }

        IEnumerator MakeProjectile(float _angle, float _velocity)
        {
            currTime = 0f;
            while (currTime < maxTime)
            {
                currTransform.x = _velocity * Mathf.Cos(_angle) * currTime;
                currTransform.y = _velocity * (Mathf.Sin(_angle) * currTime) - ((0.5f) * (-Physics.gravity.y) * currTime * currTime);
                transform.position = initialPos + currTransform;
                currTime += Time.deltaTime;
                yield return null;
            }
        }

        IEnumerator MakeProjectileDisappear()
        {
            lineRenderer.enabled = false;
            yield return waitForSeconds;
        }


        private void OnCollisionEnter(Collision collision)
        {
            audioManager.PlayClip(GameAudio.GameSoundEnum.landSound) ;
            StopAllCoroutines();
            if (isMainMenu)
                return ;
            if (collision.gameObject.layer == 9)
            {
                isJumping = false;
                uiManager.GameOver();
                return;
            }
            isJumping = false;
            initialPos = transform.position;
            deathColliderPos.x = initialPos.x;
            deathColliderPos.y = deathColliderTransform.position.y;
            deathColliderPos.z = deathColliderTransform.position.z;
            deathColliderTransform.position = deathColliderPos;
            uiManager.UpdateScore();
            lineRenderer.enabled = true;
        }

        public void SetJump(bool jmpBool)
        {
            isJumping = !jmpBool;
        }
    }
}