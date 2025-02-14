using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Title_Screen
{
    public class SplashtextManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI splashText;
        
        [Header("Animation Settings")]
        [SerializeField] private float bpm = 120f;            // Beats per minute
        [SerializeField] private float minScale = 0.95f;      // Minimum scale during pulse
        [SerializeField] private float maxScale = 1.05f;      // Maximum scale during pulse

        private Vector3 _originalScale;
        private float _currentPulseTime;
        private float PulseSpeed => (bpm * 2f * Mathf.PI) / 60f;  // Convert BPM to pulse speed
        
        private string[] _splashMessages;

        private void Start()
        {
            
            _originalScale = splashText.gameObject.transform.localScale;
            _currentPulseTime = 0f;
            
            // Load the text file from Resources folder
            var textFile = Resources.Load<TextAsset>("splashes");
            if (textFile)
            {
                _splashMessages = textFile.text.Split('\n');
                SetRandomSplash();
            }
            else
            {
                Debug.LogError("Splash text file not found in Resources folder!");
            }
        }

        private void Update()
        {
            AnimateDisplay();
        }

        public void SetRandomSplash()
        {
            if (_splashMessages is { Length: > 0 })
            {
                var randomIndex = Random.Range(0, _splashMessages.Length);
                splashText.text = _splashMessages[randomIndex].Trim();
            }
        }
        
        
        private void AnimateDisplay()
        {
            // Update the pulse time
            _currentPulseTime += Time.deltaTime * PulseSpeed;

            // Calculate the current scale using a sine wave
            var scaleMultiplier = Mathf.Lerp(minScale, maxScale, 
                (Mathf.Cos(_currentPulseTime) + 1f) * 0.5f);
    
            // Apply the new scale
            splashText.gameObject.transform.localScale = _originalScale * scaleMultiplier;

            // Ensure pulse time doesn’t drift too far over time
            if (_currentPulseTime > Mathf.PI * 2f)
            {
                _currentPulseTime -= Mathf.PI * 2f;
            }
        }
    }
}