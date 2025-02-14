using Audio;
using Player;
using Shop;
using UI_Scripts;
using UnityEngine;

namespace Weapon_Scripts
{
    public class Harpoon : MonoBehaviour
    {
        public bool canShoot = true;
        public float spearheadVelocity;
        public GameObject spearheadPrefab;
        public GameObject spearheadLocation;
        public SpriteRenderer harpoonSpriteRenderer;
        public Sprite[] harpoonSprites;
        public Sprite[] spearlessHarpoonSprites;
        public int currentHarpoon;
        public Transform firePoint;

        private ToggleShop _toggleShop;
        private Death _death;
        private PauseMenu _pauseMenu;

        private void Start()
        {
            _toggleShop = FindObjectOfType<ToggleShop>();
            _death = FindObjectOfType<Death>();
            _pauseMenu = FindObjectOfType<PauseMenu>();
            
            currentHarpoon = 0;
            harpoonSpriteRenderer.sprite = harpoonSprites[currentHarpoon];
        }

        private void Update()
        {
            if (Mathf.Approximately(Time.timeScale, 0f)) return;
            
            if (_toggleShop.isShopActive || _death.isDead || _pauseMenu.isPaused)
            {
                return;
            }
        
            Rotate();

            if ((Input.GetButtonDown("Shoot") || Input.GetAxisRaw("Shoot") > 0.5) && canShoot)
            {
                Shoot();
            }
        }

        private void Rotate()
        {
            Vector2 mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            var direction = mousePos - (Vector2)transform.position;

            var angleRad = Mathf.Atan2(direction.y, direction.x);
            var angleDeg = angleRad * Mathf.Rad2Deg;
            var flipSprite = angleDeg > 90f || angleDeg < -90f;

            transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);
            transform.localScale = flipSprite ? new Vector3(.25f, -.25f, .25f) : new Vector3(.25f, .25f, .25f);
        }

        private void Shoot()
        {
            SoundFXManager.Instance.playShootFX.Play();
            
            var projectile = Instantiate(spearheadPrefab, firePoint.position, firePoint.rotation);
            var spearhead = projectile.GetComponent<Spearhead>();
            
            // Set initial velocity directly instead of using AddForce
            Vector2 initialVelocity = firePoint.right * spearheadVelocity;
            spearhead.SetInitialVelocity(initialVelocity);
            
            canShoot = false;
        }
    }
}