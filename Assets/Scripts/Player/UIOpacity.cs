using System.Collections;
using TMPro;
using UI_Scripts;
using UnityEngine;
using UnityEngine.UI;
using Weapon_Scripts;

namespace Player
{
    public class UIOpacity : MonoBehaviour
    {
        public HideUI hideUI;
        public AoeAttack aoeAttack;
        public Opacity opacity;
        
        public bool becomingTranslucent;

        private void Start()
        {
            becomingTranslucent = false;
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("ui"))
            {
                if (hideUI.isHidden) return;
            
                if (other.gameObject.name == "AoE Cooldown Indicator" || other.gameObject.name == "AoE Text")
                {
                    becomingTranslucent = true;
                }
                
                var img = other.gameObject.GetComponent<Image>();
                var imgChildren = other.gameObject.GetComponentsInChildren<Image>();
                var text = other.gameObject.GetComponentInChildren<TextMeshProUGUI>();

                if (img)
                {
                    if (img.color.a > 0.25f)
                    {
                        opacity.SetAlpha(img, img.color.a - Time.unscaledDeltaTime * 4f);
                    }
                    else
                    {
                        opacity.SetAlpha(img, 0.25f);
                    }
                }

                foreach (var child in imgChildren)
                {
                    if (child.color.a > 0.25f)
                    {
                        opacity.SetAlpha(child, img.color.a - Time.unscaledDeltaTime * 4f);
                    }
                    else
                    {
                        opacity.SetAlpha(child, 0.25f);
                    }
                }

                if (text)
                {
                    if (text.color.a > 0.25f)
                    {
                        opacity.SetAlpha(text, img.color.a - Time.unscaledDeltaTime * 4f);
                    }
                    else
                    {
                        opacity.SetAlpha(text, 0.25f);
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("ui"))
            {
                
                if (hideUI.isHidden) return;
            
                if (other.gameObject.name == "AoE Cooldown Indicator" || other.gameObject.name == "AoE Text")
                {
                    becomingTranslucent = false;
                    
                    if (aoeAttack.isHidden) return;
                }
                
                StartFadeInCoroutines(other.gameObject);
            }
        }

        public void ExternalFadeIn(GameObject obj)
        {
            if (aoeAttack.fadeIsRunning) return;
            aoeAttack.fadeIsRunning = true;
            StartFadeInCoroutines(obj);
        }

        private void StartFadeInCoroutines(GameObject obj)
        {
            var img = obj.GetComponent<Image>();
            var imgChildren = obj.GetComponentsInChildren<Image>();
            var text = obj.GetComponentInChildren<TextMeshProUGUI>();

            if (img) StartCoroutine(FadeIn(img));
            foreach (var child in imgChildren)
            {
                StartCoroutine(FadeIn(child));
            }
            if (text) StartCoroutine(FadeIn(text));
            
            if (gameObject.name == "AoE Cooldown Indicator" || gameObject.name == "AoE Text")
            {
                aoeAttack.fadeIsRunning = false;
            }
        }

        private IEnumerator FadeIn(Graphic graphic)
        {
            while (graphic.color.a < 1f)
            {
                opacity.SetAlpha(graphic, graphic.color.a + Time.unscaledDeltaTime * 4f);
                yield return null;
            }

            opacity.SetAlpha(graphic, 1f);
        }
    }
}
