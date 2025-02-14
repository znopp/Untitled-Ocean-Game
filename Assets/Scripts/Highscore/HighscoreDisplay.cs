using TMPro;
using UnityEngine;

namespace Highscore
{
    public class HighscoreDisplay : MonoBehaviour
    {
        public TextMeshProUGUI easyTimeText;
        public TextMeshProUGUI normalTimeText;
        public TextMeshProUGUI hardTimeText;
        private HighscoreManager _highscoreManager;
        
        public GameObject highscoreDisplay;

        [Header("Animation Settings")]
        [SerializeField] private float bpm = 120f;            // Beats per minute
        [SerializeField] private float minScale = 0.95f;      // Minimum scale during pulse
        [SerializeField] private float maxScale = 1.05f;      // Maximum scale during pulse

        private Vector3 _originalScale;
        private float _currentPulseTime;
        private float PulseSpeed => (bpm * 2f * Mathf.PI) / 60f;  // Convert BPM to pulse speed
        
        private void Start()
        {
            DisplayHighscores();
            _originalScale = highscoreDisplay.transform.localScale;
            _currentPulseTime = 0f;
        }

        private void Update()
        {
            AnimateDisplay();
        }

        private void AnimateDisplay()
        {
            // Update the pulse time
            _currentPulseTime += Time.deltaTime * PulseSpeed;

            // Calculate the current scale using a sine wave
            var scaleMultiplier = Mathf.Lerp(minScale, maxScale, 
                (Mathf.Sin(_currentPulseTime) + 1f) * 0.5f);
    
            // Apply the new scale
            highscoreDisplay.transform.localScale = _originalScale * scaleMultiplier;

            // Ensure pulse time doesn’t drift too far over time
            if (_currentPulseTime > Mathf.PI * 2f)
            {
                _currentPulseTime -= Mathf.PI * 2f;
            }
        }


        private void DisplayHighscores()
        {
            _highscoreManager = FindObjectOfType<HighscoreManager>();
            if (_highscoreManager)
            {
                easyTimeText.text = easyTimeText.text.Replace("%s", _highscoreManager.GetBestTimeString("easy"));
                normalTimeText.text = normalTimeText.text.Replace("%s", _highscoreManager.GetBestTimeString("normal"));
                hardTimeText.text = hardTimeText.text.Replace("%s", _highscoreManager.GetBestTimeString("hard"));
            }
        }
    }
}