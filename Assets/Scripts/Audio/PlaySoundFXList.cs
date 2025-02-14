using UnityEngine;

namespace Audio
{
    public class PlaySoundFXList : MonoBehaviour
    {
        public AudioClip[] clips;
        public float volume;

        public void Play()
        {
            SoundFXManager.Instance.PlayRandomSoundFX(clips, transform, volume);
        }
    }
}
