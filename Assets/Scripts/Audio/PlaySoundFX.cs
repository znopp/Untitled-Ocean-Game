using UnityEngine;

namespace Audio
{
    public class PlaySoundFX : MonoBehaviour
    {
        public AudioClip clip;

        public void Play()
        {
            SoundFXManager.Instance.PlaySoundFX(clip, transform, 1f);
        }
        
        public void Play(float volume)
        {
            SoundFXManager.Instance.PlaySoundFX(clip, transform, volume);
        }
    }
}
