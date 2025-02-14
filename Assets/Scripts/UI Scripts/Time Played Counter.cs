using Player;
using Shop;
using TMPro;
using UnityEngine;

namespace UI_Scripts
{
    public class TimePlayedCounter : MonoBehaviour
    {
        public TextMeshProUGUI timePlayedCounter;

        public float timer;
        
        public PauseMenu pauseMenu;
        public ToggleShop toggleShop;
        public Death death;
    
        // Start is called before the first frame update
        private void Start()
        {
            timePlayedCounter.text = "00:00";
            timer = 0;
        }

        // Update is called once per frame
        private void Update()
        {
            if (pauseMenu.isPaused || toggleShop.isShopActive || death.isDead)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
            }

            var totalSeconds = Mathf.FloorToInt(timer);
            var minutes = totalSeconds / 60;
            var seconds = totalSeconds % 60;
        
            timePlayedCounter.text = $"{minutes:00}:{seconds:00}";
        }
    }
}