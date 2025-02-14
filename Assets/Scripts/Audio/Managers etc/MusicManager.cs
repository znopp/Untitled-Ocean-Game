using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

namespace Audio.Managers_etc
{
    public class MusicManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip[] audioClips;
        public AudioMixer audioMixer;
        
        [Tooltip("Controls how sharp the muffling transition curve is. Higher values = sharper curve")]
        public float transitionSharpness = 6f;
        
        [Tooltip("Initial drop in frequency when muffling begins (percentage)")]
        [Range(0f, 1f)]
        public float initialMuffleDropPercent = 0.4f;

        private int _lastPlayedIndex = -1;
        private bool _isPlaying = true;
        private Coroutine _muffleCoroutine;

        private const float MaxCutoff = 22000f;
        private const float MinCutoff = 500f;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            StartCoroutine(Play());
            audioMixer.SetFloat("muffleCutoff", MaxCutoff);
        }

        private IEnumerator Play()
        {
            while (_isPlaying)
            {
                int newIndex;
                do
                {
                    newIndex = Random.Range(0, audioClips.Length);
                } while (newIndex == _lastPlayedIndex);

                _lastPlayedIndex = newIndex;
                audioSource.clip = audioClips[newIndex];
                audioSource.Play();

                yield return new WaitForSeconds(audioSource.clip.length);
            }

            audioSource.Stop();
        }

        public void Stop()
        {
            _isPlaying = false;
            audioSource.Stop();
        }

        public void MuffleAudio(bool shouldMuffle)
        {
            var targetCutoff = shouldMuffle ? MinCutoff : MaxCutoff;
            
            if (_muffleCoroutine != null)
            {
                StopCoroutine(_muffleCoroutine);
            }

            _muffleCoroutine = StartCoroutine(Muffle(targetCutoff, 0.5f)); // Reduced duration to 0.5s
        }

        private IEnumerator Muffle(float targetCutoff, float duration)
        {
            audioMixer.GetFloat("muffleCutoff", out var startCutoff);
            var elapsedTime = 0f;
            var isMuffling = targetCutoff < startCutoff;

            // If we're muffling, apply immediate partial effect
            if (isMuffling)
            {
                // Calculate immediate drop value
                var immediateDropAmount = (MaxCutoff - MinCutoff) * initialMuffleDropPercent;
                var immediateValue = MaxCutoff - immediateDropAmount;
                audioMixer.SetFloat("muffleCutoff", immediateValue);
                startCutoff = immediateValue; // Update start value for the rest of the transition
            }
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                var t = elapsedTime / duration;

                float curveT;
                if (isMuffling)
                {
                    // Faster initial drop for muffling
                    curveT = 1f - Mathf.Pow(1f - t, transitionSharpness * 1.5f);
                }
                else
                {
                    // Keep the same unmuffling behavior
                    curveT = Mathf.Pow(t, transitionSharpness);
                }

                var newCutoff = Mathf.Lerp(startCutoff, targetCutoff, curveT);
                audioMixer.SetFloat("muffleCutoff", newCutoff);
                yield return null;
            }

            audioMixer.SetFloat("muffleCutoff", targetCutoff);
        }
    }
}