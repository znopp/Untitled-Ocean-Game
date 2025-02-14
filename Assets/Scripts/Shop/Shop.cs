using Audio;
using Item_Scripts;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Weapon_Scripts;
using Button = UnityEngine.UI.Button;

namespace Shop
{
    public class Shop : MonoBehaviour
    {
        [Header("Harpoon Upgrade")]
        public TextMeshProUGUI harpoonLevelText;
        public TextMeshProUGUI harpoonCoinText;
        public RawImage harpoonCoinImage;
        public Button harpoonButton;
        public int harpoonCost;

        [Header("Health Upgrade")]
        public TextMeshProUGUI suitLevelText;
        public TextMeshProUGUI suitCoinText;
        public RawImage suitCoinImage;
        public Button suitButton;
        public int suitCost;

        [Header("Speed Upgrade")]
        public TextMeshProUGUI speedLevelText;
        public TextMeshProUGUI speedCoinText;
        public RawImage speedCoinImage;
        public Button speedButton;
        public int speedCost;

        [Header("AOE Upgrade")]
        public TextMeshProUGUI aoeLevelText;
        public TextMeshProUGUI aoeCoinText;
        public RawImage aoeCoinImage;
        public Button aoeButton;
        public int aoeCost;
        
        [Header("Bullet Time Upgrade")]
        public TextMeshProUGUI btLevelText;
        public TextMeshProUGUI btCoinText;
        public RawImage btCoinImage;
        public Button btButton;
        public int btCost;

        [Header("Harpoon Sprites (Shop GUI)")]
        public GameObject woodenHarpoon;
        public GameObject rustyHarpoon;
        public GameObject advancedHarpoon;
        public GameObject prismaticHarpoon;
        public GameObject currentHarpoonGameObject;

        [Header("Imported Scripts")]
        public PlayerHealth playerHealth;
        public Controls controls;
        public InteractCoin interactCoin;
        public AoeAttack aoeAttack;
        public Harpoon harpoon;
        public BulletTime bulletTime;

        [Header("Button Logic Variables")]
        public int harpoonPurchased;
        public int suitHealthPurchased;
        public int swimSpeedPurchased;
        public int aoePurchased;
        public int bulletTimePurchased;
        
        private void Start()
        {
            currentHarpoonGameObject = woodenHarpoon;
            
            harpoonCost = 10;
            suitCost = 10;
            speedCost = 10;
            aoeCost = 10;
            btCost = 20;

            harpoonCoinText.text = harpoonCost.ToString();
            suitCoinText.text = suitCost.ToString();
            speedCoinText.text = speedCost.ToString();
            aoeCoinText.text = aoeCost.ToString();
            btCoinText.text = btCost.ToString();
        }

        public void SuitHealth()
        {
            if (interactCoin.coins < suitCost)
            {
                SoundFXManager.Instance.playInsufficientFundsFX.Play();
                return;
            }
            
            SoundFXManager.Instance.playPurchaseFX.Play();

            PurchaseHealth();

            if (suitHealthPurchased == 3)
            {
                MoveText(suitCoinImage.gameObject, suitCoinText);

                suitCoinText.text = "max";
                suitLevelText.text = suitHealthPurchased + "/3";
                suitButton.interactable = false;
            }
        }

        public void Speed()
        {
            if (interactCoin.coins < suitCost)
            {
                SoundFXManager.Instance.playInsufficientFundsFX.Play();
                return;
            }
            
            SoundFXManager.Instance.playPurchaseFX.Play();
            
            PurchaseSpeed();

            if (swimSpeedPurchased == 3)
            {
                MoveText(speedCoinImage.gameObject, speedCoinText);

                speedCoinText.text = "max";
                speedLevelText.text = swimSpeedPurchased + "/3";
                speedButton.interactable = false;
            }
        }

        public void AreaOfEffect()
        {
            if (interactCoin.coins < aoeCost)
            {
                SoundFXManager.Instance.playInsufficientFundsFX.Play();
                return;
            }
            
            SoundFXManager.Instance.playPurchaseFX.Play();

            PurchaseAoE();

            if (aoePurchased == 4)
            {
                MoveText(aoeCoinImage.gameObject, aoeCoinText);

                aoeCoinText.text = "max";
                aoeLevelText.text = aoePurchased + "/4";
                aoeButton.interactable = false;
            }
        }
        
        public void UpgradeHarpoon()
        {
            if (interactCoin.coins < harpoonCost)
            {
                SoundFXManager.Instance.playInsufficientFundsFX.Play();
                return;
            }
            
            SoundFXManager.Instance.playPurchaseFX.Play();
            
            PurchaseHarpoon();

            if (harpoon.currentHarpoon == 3)
            {
                MoveText(harpoonCoinImage.gameObject, harpoonCoinText);
                    
                harpoonCoinText.text = "max";
                harpoonLevelText.text = "3/3";
                harpoonButton.interactable = false;
            }
            
            
            switch (harpoonPurchased)
            {
                case 0:
                    currentHarpoonGameObject = rustyHarpoon;

                    woodenHarpoon.SetActive(false);
                    rustyHarpoon.SetActive(true);
                    
                    break;
                
                case 1:
                    currentHarpoonGameObject = advancedHarpoon;
                    
                    rustyHarpoon.SetActive(false);
                    advancedHarpoon.SetActive(true);

                    break;
            
                case 2:
                    currentHarpoonGameObject = prismaticHarpoon;
                    
                    advancedHarpoon.SetActive(false);
                    prismaticHarpoon.SetActive(true);
                    break;
            }
            
            harpoonPurchased++;
        }

        public void UpgradeBulletTime()
        {
            if (interactCoin.coins < btCost)
            {
                SoundFXManager.Instance.playInsufficientFundsFX.Play();
                return;
            }

            SoundFXManager.Instance.playPurchaseFX.Play();
            
            PurchaseBulletTime();
            
            if (bulletTimePurchased == 5)
            {
                MoveText(btCoinImage.gameObject, btCoinText);
                    
                btCoinText.text = "max";
                btButton.interactable = false;
            }
        }

        private void MoveText(GameObject coinImage, TextMeshProUGUI coinText)
        {
            coinImage.SetActive(false);
            Vector2 newPosition = coinText.rectTransform.localPosition;
            newPosition.x -= -44.5f;

            coinText.rectTransform.localPosition = newPosition;
        }

        private void PurchaseHarpoon()
        {
            interactCoin.RemoveCoins(harpoonCost);
            harpoonCost += 10;

            harpoon.spearheadVelocity += 2;
            harpoon.currentHarpoon += 1;
            harpoon.harpoonSpriteRenderer.sprite = harpoon.harpoonSprites[harpoon.currentHarpoon];
            
            harpoonCoinText.text = harpoonCost.ToString();
            harpoonLevelText.text = harpoon.currentHarpoon.ToString() + "/3";
        }

        private void PurchaseHealth()
        {
            interactCoin.RemoveCoins(suitCost);
            suitHealthPurchased++;
            playerHealth.highestHealth += 1;
            
            suitCost += 10;
            suitCoinText.text = suitCost.ToString();
            suitLevelText.text = suitHealthPurchased.ToString() + "/3";
        }

        private void PurchaseSpeed()
        {
            interactCoin.RemoveCoins(suitCost);
            swimSpeedPurchased++;
            controls.speed += 0.5f;
            
            suitCost += 10;
            speedCoinText.text = suitCost.ToString();
            speedLevelText.text = swimSpeedPurchased + "/3";
        }

        private void PurchaseAoE()
        {
            interactCoin.RemoveCoins(aoeCost);
            aoePurchased++;
            aoeAttack.radius += 0.4f;
            aoeAttack.aoeCooldown -= 2.5f;
            
            aoeCost += 10;
            aoeCoinText.text = aoeCost.ToString();
            aoeLevelText.text = aoePurchased.ToString() + "/4";
        }

        private void PurchaseBulletTime()
        {
            interactCoin.RemoveCoins(btCost);
            bulletTimePurchased++;
            bulletTime.maxDuration += 2f;
            bulletTime.regenerationFactor -= 1f;

            btCost += bulletTimePurchased * 20;
            btCoinText.text = btCost.ToString();
            btLevelText.text = bulletTimePurchased + "/5";
            
            if (bulletTime.regenerationFactor < 3f) bulletTime.regenerationFactor = 3f;
        }
        
        public void UpdateShopUI()
        {
            switch (harpoon.currentHarpoon)
            {
                
                case 1:
                    currentHarpoonGameObject = rustyHarpoon;
                    woodenHarpoon.SetActive(false);
                    rustyHarpoon.SetActive(true);

                    harpoonCoinText.text = "20";
                    break;
                case 2:
                    currentHarpoonGameObject = advancedHarpoon;
                    rustyHarpoon.SetActive(false);
                    advancedHarpoon.SetActive(true);

                    harpoonCoinText.text = "30";
                    break;
                case 3:
                    currentHarpoonGameObject = prismaticHarpoon;
                    advancedHarpoon.SetActive(false);
                    prismaticHarpoon.SetActive(true);

                    harpoonCoinText.text = "max";
                    MoveText(harpoonCoinImage.gameObject, harpoonCoinText);
                    harpoonButton.interactable = false;
                    break;
            }

            switch (suitHealthPurchased)
            {
                case 1:
                    suitCoinText.text = "20";
                    break;
                
                case 2:
                    suitCoinText.text = "30";
                    break;
                
                case 3:
                    suitCoinText.text = "max";
                    MoveText(suitCoinImage.gameObject, suitCoinText);
                    suitButton.interactable = false;
                    break;
            }

            switch (swimSpeedPurchased)
            {
                case 1:
                    speedCoinText.text = "20";
                    break;
                
                case 2:
                    speedCoinText.text = "30";
                    break;
                
                case 3:
                    speedCoinText.text = "max";
                    MoveText(speedCoinImage.gameObject, speedCoinText);
                    speedButton.interactable = false;
                    break;
            }

            switch (aoePurchased)
            {
                case 1:
                    aoeCoinText.text = "20";
                    break;
                case 2:
                    aoeCoinText.text = "30";
                    break;
                case 3:
                    aoeCoinText.text = "40";
                    break;
                case 4:
                    aoeCoinText.text = "max";
                    MoveText(aoeCoinImage.gameObject, aoeCoinText);
                    aoeButton.interactable = false;
                    break;
            }

            switch (bulletTimePurchased)
            {
                case 1:
                    btLevelText.text = "20";
                    break;
                case 2:
                    btLevelText.text = "40";
                    break;
                case 3:
                    btLevelText.text = "60";
                    break;
                case 4:
                    btLevelText.text = "80";
                    break;
                case 5:
                    btLevelText.text = "max";
                    MoveText(btCoinImage.gameObject, btLevelText);
                    btButton.interactable = false;
                    break;
            }
            
            harpoonLevelText.text = $"{harpoon.currentHarpoon}/3";
            suitLevelText.text = $"{suitHealthPurchased}/3";
            speedLevelText.text = $"{swimSpeedPurchased}/3";
            aoeLevelText.text = $"{aoePurchased}/4";
            btLevelText.text = $"{bulletTimePurchased}/5";

        }
    }
}
