using System.Collections;
using Audio;
using UnityEngine;
using UnityEngine.UI;
using Enemy;
using Player;
using Shop;
using TMPro;
using UI_Scripts;

namespace Weapon_Scripts
{
    public class AoeAttack : MonoBehaviour
    {
        // AoE Range Sprite variables
        public float radius;
        public Image aoeCircleImage;

        // Layer of enemies
        public LayerMask enemyLayerMask;

        // Cooldown
        public float aoeCooldown;

        // AoE Recharge Indicator variables
        public float indicatorTimer;
        public Image aoeIndicatorImage;
        
        // Text
        public TextMeshProUGUI aoeText;

        // Cooldown variables
        public bool canUse;
        private bool _heldWhileCooldown;

        // Imported scripts
        private EnemyDeath _enemyDeath;
        private Death _death;
        private ToggleShop _toggleShop;
        private PauseMenu _pauseMenu;
        public HideUI hideUI;
        public UIOpacity uiOpacity;
        public Opacity opacity;

        public bool isHidden;
        public bool fadeIsRunning;

        private void Start()
        {
            indicatorTimer = 0f;
            canUse = true;

            _heldWhileCooldown = false;
            fadeIsRunning = false;
            
            aoeText.color = new Color(aoeText.color.r, aoeText.color.g, aoeText.color.b, 0f);
            aoeIndicatorImage.color = new Color(aoeIndicatorImage.color.r, aoeIndicatorImage.color.g, aoeIndicatorImage.color.b, 0f);
            isHidden = true;
            
            
            aoeCircleImage.gameObject.SetActive(false);

            _enemyDeath = FindObjectOfType<EnemyDeath>();
            _death = FindObjectOfType<Death>();
            _toggleShop = FindObjectOfType<ToggleShop>();
            _pauseMenu = FindObjectOfType<PauseMenu>();

        }

        private void Update()
        {
            if (_pauseMenu.isPaused || _death.isDead || _toggleShop.isShopActive) return;

            if (Mathf.Approximately(indicatorTimer, 1f))
            {
                indicatorTimer = 0f;
                canUse = true;
            
                opacity.SetAlpha(aoeIndicatorImage, 1);
                opacity.SetAlpha(aoeText, 1);

                isHidden = false;
                fadeIsRunning = false;
            }
            
            if (hideUI.isHidden)
            {
                if (!isHidden)
                {
                    isHidden = true;
                    opacity.SetAlpha(aoeIndicatorImage, 0);
                    opacity.SetAlpha(aoeText, 0);
                }
            }
            else if (indicatorTimer > 0f && isHidden)
            {
                isHidden = false;
                uiOpacity.ExternalFadeIn(aoeIndicatorImage.gameObject);
            }
            
            if (!hideUI.isHidden)
            {
                if (Mathf.Approximately(indicatorTimer, 0f))
                {
                    
                    opacity.SetAlpha(aoeIndicatorImage, 0);
                    opacity.SetAlpha(aoeText, 0);
                    isHidden = true;
                }
                else
                {
                    if (!uiOpacity.becomingTranslucent && !fadeIsRunning)
                    {
                        isHidden = false;
                        fadeIsRunning = true;
                        uiOpacity.ExternalFadeIn(aoeIndicatorImage.gameObject);
                    }
                }
            }
            
            
            if (Input.GetButtonUp("AOE Effect"))
            {
                if (canUse && !_heldWhileCooldown)
                {
                    aoeCircleImage.gameObject.SetActive(false);
                    PerformAoeAttack();
                }

                _heldWhileCooldown = false;
            }

            if (Input.GetButton("AOE Effect"))
            {
                if (!canUse)
                {
                    _heldWhileCooldown = true;
                    indicatorTimer += Time.deltaTime / aoeCooldown;
                    aoeIndicatorImage.fillAmount = indicatorTimer;
                    return;
                }

                aoeIndicatorImage.fillAmount = indicatorTimer;

                if (_heldWhileCooldown) return;

                aoeCircleImage.gameObject.SetActive(true);
                AoeCircle();
            }

            if (!canUse)
            {
                indicatorTimer += Time.deltaTime / aoeCooldown;
                aoeIndicatorImage.fillAmount = indicatorTimer;
            }

            if (canUse)
            {
                aoeIndicatorImage.fillAmount = indicatorTimer;
            }
        }

        private void PerformAoeAttack()
        {
            // Find all colliders within the AOE radius
            var colliders = new Collider2D[50];
            var count = Physics2D.OverlapCircleNonAlloc(transform.position, radius, colliders, enemyLayerMask);

            if (count == 0)
            {
                canUse = true;
                return;
            }

            SoundFXManager.Instance.playAoeFX.Play();
            SoundFXManager.Instance.playEnemyDeathFX.Play();

            // Loop through the colliders found
            for (var i = 0; i < count; i++)
            {
                switch (colliders[i].tag)
                {
                    case "enemy":
                        _enemyDeath.KillGeneric(colliders[i].gameObject, colliders[i].transform.position);
                        break;

                    case "bigEnemy":
                        _enemyDeath.KillBigShark(colliders[i].gameObject, colliders[i].transform.position);
                        break;
                }
            }

            canUse = false;
            StartCoroutine(Cooldown());
        }

        private void AoeCircle()
        {
            // Update the position and size of the AOE circle image to match the player's position and the diameter of the circle
            var worldPosition = transform.position;
            var screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                aoeCircleImage.canvas.transform as RectTransform, 
                screenPosition, 
                Camera.main, 
                out var localPoint
            );

            aoeCircleImage.rectTransform.localPosition = localPoint;
            aoeCircleImage.rectTransform.sizeDelta = new Vector2(radius * 2 * 108, radius * 2 * 108);
        }

        private void OnDrawGizmosSelected()
        {
            // Draw a visual representation of the AOE radius for debugging
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(aoeCooldown);
            indicatorTimer = 0f;
            canUse = true;
            
            opacity.SetAlpha(aoeIndicatorImage, 1);
            opacity.SetAlpha(aoeText, 1);

            isHidden = false;
            fadeIsRunning = false;
        }

    }
}
