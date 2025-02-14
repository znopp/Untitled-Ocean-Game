using Audio;
using UnityEngine;
using Enemy;

namespace Weapon_Scripts
{
    public class Spearhead : MonoBehaviour
    {
        public Rigidbody2D spearheadRigidbody;
        public GameObject ropePrefab;
        private LineRenderer _lineRenderer;
        private bool _hasCollided;
        private Vector2 _currentVelocity;

        private Harpoon _harpoon;
        private EnemyDeath _enemyDeath;
        
        private BulletTime _bulletTime;

        public void Start()
        {
            _harpoon = FindObjectOfType<Harpoon>();
            _enemyDeath = FindObjectOfType<EnemyDeath>();
            _bulletTime = FindObjectOfType<BulletTime>();

            _harpoon.harpoonSpriteRenderer.sprite = _harpoon.spearlessHarpoonSprites[_harpoon.currentHarpoon];

            var ropeInstance = Instantiate(ropePrefab, _harpoon.transform, true);
            _lineRenderer = ropeInstance.GetComponent<LineRenderer>();
            _lineRenderer.positionCount = 2;
        }

        private void FixedUpdate()
        {
            if (_hasCollided)
            {
                // Continuously calculate the direction to the player's current position
                Vector2 harpoonPosition = _harpoon.spearheadLocation.transform.position;
                Vector2 spearheadPosition = transform.position;
        
                var moveToPlayer = (harpoonPosition - spearheadPosition).normalized;

                // Set _currentVelocity's direction toward the player but keep speed constant
                _currentVelocity = moveToPlayer * _harpoon.spearheadVelocity;
            }

            // Apply movement using MovePosition with timescale independence
            var timeType = !_bulletTime.isUsing ? Time.deltaTime : Time.fixedUnscaledDeltaTime;
            spearheadRigidbody.MovePosition(spearheadRigidbody.position + _currentVelocity * timeType);

            // Update rope position here instead of Update for consistent timing
            if (_lineRenderer)
            {
                Vector2 harpoonPosition = _harpoon.spearheadLocation.transform.position;
                Vector2 spearheadPosition = transform.position;

                _lineRenderer.SetPosition(0, harpoonPosition);
                _lineRenderer.SetPosition(1, spearheadPosition);

                var distance = Vector2.Distance(harpoonPosition, spearheadPosition);
                _lineRenderer.material.mainTextureScale = new Vector2(distance, 1f);
            }
        }


        public void SetInitialVelocity(Vector2 velocity)
        {
            _currentVelocity = velocity;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (Input.GetButton("Piercing Enemies"))
            {
                Debug.Log("Piercing Enemies");

                if (_harpoon.currentHarpoon == 3 && !col.gameObject.CompareTag("boundingbox") && !col.gameObject.CompareTag("player"))
                {
                    Debug.Log(_harpoon.currentHarpoon);
                    _hasCollided = false;
                }
                else
                {
                    _hasCollided = true;
                }
            }
            else
            {
                _hasCollided = true;
            }
            
            if (col.gameObject.CompareTag("enemy"))
            {
                SoundFXManager.Instance.playEnemyDeathFX.Play();
                var position = col.transform.position;
                _enemyDeath.KillGeneric(col.gameObject, position);
            }

            if (col.gameObject.CompareTag("bigEnemy"))
            {
                SoundFXManager.Instance.playEnemyDeathFX.Play();
                var position = col.transform.position;
                _enemyDeath.KillBigShark(col.gameObject, position);
            }

            if (col.gameObject.CompareTag("player"))
            {
                Destroy(gameObject);
                _harpoon.canShoot = true;
                Destroy(_lineRenderer.gameObject);
                _harpoon.harpoonSpriteRenderer.sprite = _harpoon.harpoonSprites[_harpoon.currentHarpoon];
            }

            if (col.gameObject.CompareTag("boundingbox"))
            {
                SoundFXManager.Instance.playHitObjectFX.Play();
            }
        }
    }
}