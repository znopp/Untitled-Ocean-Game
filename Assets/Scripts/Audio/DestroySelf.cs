using System.Collections;
using UnityEngine;

namespace Audio
{
    public class DestroySelf : MonoBehaviour
    {
        public AudioSource self;

        private void Start()
        {
            DontDestroyOnLoad(self.gameObject);
            StartCoroutine(DestroyAfterClip(self, self.clip.length));
            
        }
        
        private IEnumerator DestroyAfterClip(AudioSource audioSource, float clipLength)
        {
            yield return new WaitForSecondsRealtime(clipLength);
    
            Destroy(audioSource.gameObject);
        }
        
    }
}
