using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Item_Scripts
{
    public class Collision : MonoBehaviour
    {
        private List<GameObject> _collectibles;
        private bool _collisionToggleActive;

        private void Start()
        {
            // Cache collectibles at the start
            _collectibles = GameObject.FindGameObjectsWithTag("health")
                .Concat(GameObject.FindGameObjectsWithTag("coin"))
                .ToList();
        }

        private void Update()
        {
            // Update cached collectibles if needed (optional optimization)
            if (Input.GetButtonDown("Toggle Collision"))
            {
                _collectibles = GameObject.FindGameObjectsWithTag("health")
                    .Concat(GameObject.FindGameObjectsWithTag("coin"))
                    .ToList();
            }

            if (Input.GetButton("Toggle Collision") && !_collisionToggleActive)
            {
                _collisionToggleActive = true;
                ToggleCollision(true);
            }

            if (Input.GetButtonUp("Toggle Collision") && _collisionToggleActive)
            {
                _collisionToggleActive = false;
                ToggleCollision(false);
            }
        }

        private void ToggleCollision(bool enableSpearhead)
        {
            var targetLayer = LayerMask.NameToLayer(enableSpearhead ? "Items_Spearhead" : "Items");

            foreach (var item in _collectibles)
            {
                if (!item) continue;
                
                if (item.layer != targetLayer) // Only change layers if necessary
                {
                    item.layer = targetLayer;
                }
            }
        }

    }
}
