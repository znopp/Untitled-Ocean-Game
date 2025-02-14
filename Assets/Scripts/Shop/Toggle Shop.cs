using Audio;
using Audio.Managers_etc;
using Item_Scripts;
using TMPro;
using UI_Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shop
{
    public class ToggleShop : MonoBehaviour
    {
        public TextMeshProUGUI textBox;
    
        public bool isShopActive;
        public bool isPlayerInsideTrigger;
    
        public GameObject shopGUI;
        public GameObject purchasables;
        
        public MusicManager musicManager;

        public Shop shop;
        public InteractCoin interactCoin;
        public PauseMenu pauseMenu;

        private void Start()
        {
            textBox.text = "";
            isShopActive = false;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("player"))
            {
                textBox.text = "Press E to open shop";
                isPlayerInsideTrigger = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("player"))
            {
                textBox.text = "";
                isPlayerInsideTrigger = false;
            }
        }

        private void Update()
        {
            if (!isPlayerInsideTrigger)
            {
                return;
            }

            if (Input.GetButtonDown("Interact") && !isShopActive && !pauseMenu.isPaused)
            {
                musicManager.MuffleAudio(true);
                isShopActive = true;
                ToggleShopGUI();
            }
            else if(Input.GetButtonDown("Cancel"))
            {
                if (!isShopActive)
                {
                    return;
                
                }
                
                musicManager.MuffleAudio(false);
                isShopActive = false;
                ToggleShopGUI();
            }
        }

        private void ToggleShopGUI()
        {
            if (isShopActive)
            {
                SoundFXManager.Instance.playExitMenuFX.Play();
                Time.timeScale = 0;
                interactCoin.coinTextInGUI.gameObject.SetActive(true);
                textBox.text = "";
                shopGUI.SetActive(true);
                purchasables.SetActive(true);
                shop.currentHarpoonGameObject.SetActive(true);
            }
            else
            {
                SoundFXManager.Instance.playExitMenuFX.Play();
                shop.currentHarpoonGameObject.SetActive(false);
                purchasables.SetActive(false);
                shopGUI.SetActive(false);
                textBox.text = "Press E to open shop";
                interactCoin.coinTextInGUI.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
}