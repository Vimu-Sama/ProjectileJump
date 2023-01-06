using UnityEngine;


namespace GameAudio
{   
    public enum GameSoundEnum
    {
        backGroundSound,
        jumpSound,
        landSound
    }
    public class AudioManager : GenericSingleton<AudioManager>
    {
        [SerializeField] private AudioClip backgroundSound;
        [SerializeField] private AudioClip jumpLandSound;
        [SerializeField] private AudioClip jumpTakeOffSound;
        [SerializeField] private float soundEffectSound;
        private AudioSource audioSource;
        private AudioClip tempClip;

        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(this);
        }

        public void PlayClip(GameSoundEnum soundEnum)
        {
            if(soundEnum== GameSoundEnum.jumpSound)
                tempClip = jumpTakeOffSound;
            else if(soundEnum== GameSoundEnum.landSound)
                tempClip = jumpLandSound;
            audioSource.PlayOneShot(tempClip, soundEffectSound) ;
        }

    }
}