using Audio;
using Audio.Managers_etc;
using UI_Scripts;
using Weapon_Scripts;
using UnityEngine;

namespace Player
{
    public class Controls : MonoBehaviour
    {
        // Control axes
        private float _horizontal;
        private float _vertical;
        private float _previousHorizontalInput;
        
        // Speeds
        public float speed;
        
        public MusicManager musicManager;

        // Player Rigidbody
        public Rigidbody2D playerRigidbody;
    
        // Player sprite
        public SpriteRenderer spriteRenderer;
        
        // Player transform
        public Transform player;
        
        public PauseMenu pauseMenu;
        public BulletTime bulletTime;
        
        // Secret sound effect
        private int _count;

        private void Start()
        {
            
            // Check if the sprite is initially flipped
            if (spriteRenderer.flipX)
            {
                _previousHorizontalInput = 1f;
            }
            else
            {
                _previousHorizontalInput = -1f;
            }
        }
        
        private void FixedUpdate()
        {
            // Get input
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");

            // Calculate movement vector
            var horizontalInput = new Vector2(_horizontal * speed, 0f);
            var verticalInput = new Vector2(0f, _vertical * (speed + 0.25f));
            var movementVector = (horizontalInput + verticalInput).normalized * Mathf.Max(horizontalInput.magnitude, verticalInput.magnitude);


            var timeType = !bulletTime.isUsing ? Time.deltaTime : Time.fixedUnscaledDeltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + movementVector * timeType);
        }

        private void Update()
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");
            
            if (_horizontal == 0)
            {
                spriteRenderer.flipX = _previousHorizontalInput > 0;
                return;
            }
            
            if (_horizontal > 0)
            {
                if (!spriteRenderer.flipX && pauseMenu.isPaused) _count++;
                spriteRenderer.flipX = true;
                _previousHorizontalInput = _horizontal;
            }
            
            if (_horizontal < 0)
            {
                if (spriteRenderer.flipX && pauseMenu.isPaused) _count++;
                spriteRenderer.flipX = false;
                _previousHorizontalInput = _horizontal;
            }
            
            if (pauseMenu.isPaused && _count == 50)
            {
                _count = 0;
                SoundFXManager.Instance.playWiggleFX.Play();
            }
        }
    }
}