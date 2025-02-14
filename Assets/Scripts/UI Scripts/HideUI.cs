using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Weapon_Scripts;

namespace UI_Scripts
{
    public class HideUI : MonoBehaviour
    {
        public bool isHidden;
        [SerializeField] private GameObject[] _hiddenObjects;
        public AoeAttack aoeAttack;
        
        // Start is called before the first frame update
        private void Start()
        {
            
            
            isHidden = false;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetButtonDown("HideUI"))
            {
                _hiddenObjects = GameObject.FindGameObjectsWithTag("ui");
                
                if (!isHidden)
                {
                    isHidden = true;
                    ToggleHideUI(0f);
                    return;
                }
                
                if (isHidden)
                {
                    isHidden = false;
                    ToggleHideUI(1f);
                }
            }
        }

        private void ToggleHideUI(float opacity)
        {
            foreach (var uiObject in _hiddenObjects)
            {
                /*if (uiObject.name == "AoE Cooldown Indicator" && aoeAttack.isHidden)
                {
                    continue;
                    
                }
                
                if (uiObject.name == "AoE Text" && aoeAttack.isHidden)
                {
                    continue;
                }*/
                
                var image = uiObject.GetComponent<Image>();
                var text = uiObject.GetComponentInChildren<TextMeshProUGUI>();
                
                var imageChildren = uiObject.GetComponentsInChildren<Image>();
                var textChildren = uiObject.GetComponentsInChildren<TextMeshProUGUI>();
                        
                if (image)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, opacity);
                }

                if (text)
                {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, opacity);
                }

                foreach (var imgChild in imageChildren)
                {
                    imgChild.color = new Color(imgChild.color.r, imgChild.color.g, imgChild.color.b, opacity);
                }

                foreach (var textChild in textChildren)
                {
                    textChild.color = new Color(textChild.color.r, textChild.color.g, textChild.color.b, opacity);
                }
                
            }
        }
        
    }
}
