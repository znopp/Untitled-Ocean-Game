using Player;
using UnityEngine;

namespace Enemy
{
    public class AIChase : MonoBehaviour
    {
        // Shark movement speed
        public float speed;
        
        // Shark sprite
        public SpriteRenderer sharkSprite;

        // Import of Controls script
        private Controls _controls;


        private void Start()
        {
            _controls = FindObjectOfType<Controls>();
        
            Vector3 direction = _controls.player.transform.position - transform.position;
        
            if (direction.x > 0)
            {
                // Face right
                sharkSprite.flipX = true;
            }
            else
            {
                // Face left
                sharkSprite.flipX = true;
                sharkSprite.flipY = true;
            }

        }

        private void Update()
        {
            // Move towards the player
            transform.position = Vector2.MoveTowards(transform.position, _controls.player.transform.position, speed * Time.deltaTime);
        
            // Calculate the direction to the player
            Vector3 direction = _controls.player.transform.position - transform.position;

            // Flip the sprite based on player position
            if (direction.x > 0)
            {
                // Face right
                sharkSprite.flipX = true;
            }

            // Rotate to face the player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    
    }
}
