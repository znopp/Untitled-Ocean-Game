using UnityEngine;

namespace Audio
{
    public class WelcomeScript : MonoBehaviour
    {
        public AudioClip clip;
        public float volume;
        private static bool _hasPlayed = false;

        private void Awake()
        {
            if (!_hasPlayed)
            {
                PlayWelcome();
                _hasPlayed = true;
            }
        }

        private void PlayWelcome()
        {
            SoundFXManager.Instance.PlaySoundFX(clip, transform, volume);
        }
        
    }
}
