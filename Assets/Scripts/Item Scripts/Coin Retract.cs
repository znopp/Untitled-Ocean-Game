using Audio;
using Player;
using UnityEngine;
using Weapon_Scripts;

namespace Item_Scripts
{
    public class CoinRetract : MonoBehaviour
    {
        private Rigidbody2D _coinRigidbody;
        private float _coinVelocity;
        private GameObject _coin;
        private Vector2 _moveToPlayer;
        private Harpoon _harpoon;
        private Controls _controls;

        // You can set these values in the inspector
        public Vector2 detectionBoxSize = new Vector2(2.0f, 2.0f);
        public LayerMask layerMask;
        public int maxColliders = 10;

        // Pre-allocated array to store results
        private Collider2D[] _results;

        private void Start()
        {
            _harpoon = FindObjectOfType<Harpoon>();
            _controls = FindObjectOfType<Controls>();
            _results = new Collider2D[maxColliders];
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (!col.gameObject.CompareTag("bullet")) return;
            
            SoundFXManager.Instance.playHitObjectFX.Play();
            
            _coinRigidbody = gameObject.GetComponent<Rigidbody2D>();
            _coin = gameObject;

            _coinVelocity = _harpoon.spearheadVelocity;
            _moveToPlayer = (_controls.player.transform.position - _coin.transform.position).normalized;
            _coinRigidbody.velocity = _moveToPlayer * _coinVelocity;

            // Find all objects within the detection box
            int hitCount = Physics2D.OverlapBoxNonAlloc(_coin.transform.position, detectionBoxSize, 0, _results, layerMask);

            for (int i = 0; i < hitCount; i++)
            {
                var rb = _results[i].GetComponent<Rigidbody2D>();
                if (rb != null && rb != _coinRigidbody)
                {
                    Vector2 moveToPlayer = (_controls.player.transform.position - rb.transform.position).normalized;
                    rb.velocity = moveToPlayer * _coinVelocity;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Draw the detection box in the editor for debugging
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, detectionBoxSize);
        }
    }
}
