using Audio;
using Player;
using UnityEngine;
using Weapon_Scripts;

namespace Item_Scripts
{
    public class HealthCollectible : MonoBehaviour
    {
        private float _healthVelocity;
        private Vector2 _moveToPlayer;
        private Rigidbody2D _healthRigidbody;
        private PlayerHealth _playerHealth;
        private Harpoon _harpoon;
        private Controls _controls;

        // You can set these values in the inspector
        public Vector2 detectionBoxSize = new(2.0f, 2.0f);
        public LayerMask layerMask;
        public int maxColliders = 10;

        // Pre-allocated array to store results
        private Collider2D[] _results;

        private void Start()
        {
            _playerHealth = FindObjectOfType<PlayerHealth>();
            _harpoon = FindObjectOfType<Harpoon>();
            _controls = FindObjectOfType<Controls>();
            _results = new Collider2D[maxColliders];
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("player"))
            {
                if (_playerHealth.currentHealthFloor == _playerHealth.highestHealth)
                {
                    SoundFXManager.Instance.playFullHealthFX.Play();
                    Destroy(gameObject);
                }
                else
                {
                    SoundFXManager.Instance.playHealedFX.Play();
                    _playerHealth.hp += 1;
                    Destroy(gameObject);
                }
            }

            if (col.gameObject.CompareTag("bullet"))
            {
                SoundFXManager.Instance.playHitObjectFX.Play();
                _healthRigidbody = gameObject.GetComponent<Rigidbody2D>();
                _healthVelocity = _harpoon.spearheadVelocity;
                _moveToPlayer = (_controls.player.transform.position - gameObject.transform.position).normalized;
                _healthRigidbody.velocity = _moveToPlayer * _healthVelocity;

                // Find all objects within the detection box
                var hitCount = Physics2D.OverlapBoxNonAlloc(_healthRigidbody.transform.position, detectionBoxSize, 0, _results, layerMask);

                for (var i = 0; i < hitCount; i++)
                {
                    var rb = _results[i].GetComponent<Rigidbody2D>();
                    if (rb && rb != _healthRigidbody)
                    {
                        Vector2 moveToPlayer = (_controls.player.transform.position - rb.transform.position).normalized;
                        rb.velocity = moveToPlayer * _healthVelocity;
                    }
                }
            }
        }
    }
}
