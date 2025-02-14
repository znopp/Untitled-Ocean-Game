using Audio.Managers_etc;
using Player;
using Shop;
using UI_Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Weapon_Scripts
{
    public class BulletTime : MonoBehaviour
    {
        public MusicManager musicManager;
        
        public Rigidbody2D playerRb;
        
        private float _originalGravityScale;
        private float _collectibleGravityScale;

        public ToggleShop toggleShop;
        public PauseMenu pauseMenu;
        public Death death;

        private float _slowFactor;
        public float duration;
        public float regenerationFactor;
        public float maxDuration;
        
        public Image bulletTimeImage;
        
        private bool _canUse;
        public bool isUsing;
        private bool _hasLifted;
        
        private int _i;

        private void Start()
        {
            regenerationFactor = 8;
            _slowFactor = 4;
            maxDuration = 10f;
            duration = maxDuration;
            isUsing = false;
            _hasLifted = true;

            _originalGravityScale = playerRb.gravityScale;
        }
        
        public void Update()
        {
            if (toggleShop.isShopActive || pauseMenu.isPaused || death.isDead) return;
            
            if (Input.GetButton("Bullet Time") && duration > 0f && _hasLifted)
            {
                isUsing = true;
                _canUse = true;
        
                _i++;

                duration -= Time.unscaledDeltaTime;
                
                bulletTimeImage.fillAmount = duration / maxDuration;
                
                Time.timeScale = 1/_slowFactor;
                Time.fixedDeltaTime = 0.005f * Time.timeScale;
                
                if (_i == 1)
                {
                    musicManager.MuffleAudio(true);
                    
                    playerRb.gravityScale = _originalGravityScale * (_slowFactor * 16);
                    
                    
                }
            }
            

            if (duration <= 0f && _canUse)
            {
                // ran out of time to burn (not using bullet time)
                
                _canUse = false;
                _hasLifted = false;
                
                _i = 0;
                
                playerRb.gravityScale = _originalGravityScale;
                
                musicManager.MuffleAudio(false);
                duration = 0f;
                Time.timeScale = 1f;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                isUsing = false;
            }

            if (Input.GetButtonUp("Bullet Time"))
            {
                // not using bullet time
                
                if (toggleShop.isShopActive || pauseMenu.isPaused) return;
                
                _i = 0;
                
                playerRb.gravityScale = _originalGravityScale;
                
                musicManager.MuffleAudio(false);
                Time.timeScale = 1f;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                isUsing = false;
                _hasLifted = true;
                
            }

            if (!isUsing && duration < maxDuration)
            {
                // recharging
                duration += Time.unscaledDeltaTime * (1 / regenerationFactor);
                bulletTimeImage.fillAmount = duration / maxDuration;
            }
            
            if (duration > maxDuration) duration = maxDuration;
            
        }
        
    }
}
