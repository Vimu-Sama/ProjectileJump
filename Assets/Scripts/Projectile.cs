using UnityEngine;
using System.Collections;
public class Projectile : MonoBehaviour
{
    [SerializeField] private float angleInDeg;
    [SerializeField] private float initialVelocity;
    [SerializeField] private float lineRendererLOD=10;
    [SerializeField] private float step;
    private WaitForSeconds waitForSeconds;
    private LineRenderer lineRenderer;
    private float currTime = 0f;
    private float angleInRad;
    private float maxTime =  60f;
    private Vector3 currTransform;
    private Vector3 lineTrajectoryTransform;
    private Vector3 initialPos;
    private int count = 0;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        initialPos = transform.position;
        waitForSeconds = new WaitForSeconds(100f);
    }
    private void Update()
    {
        angleInRad= Mathf.Deg2Rad * angleInDeg;
        DrawLine(angleInRad, initialVelocity, step);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            StartCoroutine(MakeProjectileDisappear());
            StartCoroutine(MakeProjectile(angleInRad, initialVelocity));;
        }
    }

    private void DrawLine(float _angle, float _velocity, float _step)
    {
        _step = Mathf.Max(0.01f, _step);
        lineRenderer.positionCount = (int)(lineRendererLOD / _step)+2;
        count = 0;
        for(float i=0;i<lineRendererLOD;i+=_step)
        {
            lineTrajectoryTransform.x = _velocity * Mathf.Cos(_angle) * i;
            lineTrajectoryTransform.y = _velocity * Mathf.Sin(_angle) * i - (0.5f) * (-Physics.gravity.y) * i * i;
            lineRenderer.SetPosition(count, transform.position + lineTrajectoryTransform);
            count++;
        }
        lineTrajectoryTransform.x = _velocity * Mathf.Cos(_angle) * lineRendererLOD;
        lineTrajectoryTransform.y = _velocity * Mathf.Sin(_angle) - (0.5f) * (-Physics.gravity.y) * lineRendererLOD * lineRendererLOD;
        lineRenderer.SetPosition(count,transform.position + lineTrajectoryTransform);
    }

    IEnumerator MakeProjectile(float _angle, float _velocity)
    {
        currTime = 0f;
        while(currTime<maxTime)
        {
            currTransform.x= _velocity * Mathf.Cos(_angle) * currTime;
            currTransform.y= _velocity * (Mathf.Sin(_angle) * currTime) - (0.5f) * (-Physics.gravity.y) * currTime * currTime;
            transform.position =  initialPos + currTransform;
            currTime+= Time.deltaTime;
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
        lineRenderer.enabled = true;
        StopAllCoroutines();
    }
}
