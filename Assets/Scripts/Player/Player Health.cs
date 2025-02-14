using System.Collections;
using Audio;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public float hp;
        private const float CooldownTime = 0.5f;

        private bool _isHit;
    
        public int highestHealth;
        public int currentHealth;
        public int currentHealthFloor;

        // All heart imports

        public GameObject[] fullHearts;
        public GameObject[] halfHearts;
        public GameObject[] emptyHearts;

        public Shop.Shop shop;

        private void Start()
        {
            hp = 6;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("enemy") && _isHit == false)
            {
                SoundFXManager.Instance.playDamagedFX.Play();
                hp -= 1;
                _isHit = true;
                StartCoroutine(HitCooldown());
                
            }
            
            if (col.gameObject.CompareTag("bigEnemy") && _isHit == false)
            {  
                SoundFXManager.Instance.playDamagedFX.Play();
                hp -= 2;
                _isHit = true;
                StartCoroutine(HitCooldown());
            }
            

        }

        private void OnCollisionStay2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("enemy") && _isHit == false)
            {
                SoundFXManager.Instance.playDamagedFX.Play();
                hp -= 1;
                _isHit = true;
                StartCoroutine(HitCooldown());
            }
            
            if (col.gameObject.CompareTag("bigEnemy") && _isHit == false)
            {
                SoundFXManager.Instance.playDamagedFX.Play();
                hp -= 2;
                _isHit = true;
                StartCoroutine(HitCooldown());
            }

        }

        private IEnumerator HitCooldown()
        {

            yield return new WaitForSeconds(CooldownTime);
            _isHit = false;

        }
    
        private void Update()
        {
            highestHealth = 3 + (shop.suitHealthPurchased);

            if (hp > (highestHealth * 2)) hp = (highestHealth * 2);
            
            currentHealth = Mathf.CeilToInt(hp / 2);
            currentHealthFloor = Mathf.FloorToInt(hp / 2);

            ClearAllHearts();

            RenderEmptyHearts(3 + shop.suitHealthPurchased);

            RenderHalfHearts(currentHealth);

            RenderFullHearts(currentHealthFloor);
        }


        private void ClearAllHearts()
        {
            // Deactivate all empty hearts
            foreach (var heart in emptyHearts)
            {
                heart.SetActive(false);
            }

            // Deactivate all half hearts
            foreach (var heart in halfHearts)
            {
                heart.SetActive(false);
            }

            // Deactivate all full hearts
            foreach (var heart in fullHearts)
            {
                heart.SetActive(false);
            }
        }

        private void RenderEmptyHearts(int limit)
        {
            var counter = 1;

            foreach (var heart in emptyHearts)
            {
                if (counter > limit)
                {
                    return;
                }

                heart.SetActive(true);
                counter += 1;
            }
        }

        private void RenderHalfHearts(int limit)
        {
            var counter = 1;

            foreach (var heart in halfHearts)
            {
                if (counter > limit)
                {
                    return;
                }

                heart.SetActive(true);
                counter += 1;
            }
        }

        private void RenderFullHearts(int limit)
        {
            var counter = 1;

            foreach (var heart in fullHearts)
            {
                if (counter > limit)
                {
                    return;
                }

                heart.SetActive(true);
                counter += 1;
            }
        }
    }
}
    

