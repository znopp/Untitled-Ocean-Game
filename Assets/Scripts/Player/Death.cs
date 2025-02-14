using Audio;
using Audio.Managers_etc;
using Saves;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class Death : MonoBehaviour
    {
        public MusicManager musicManager;
        
        private PlayerHealth _playerHealth;
    
        private SceneManager _sceneManager;

        public GameObject deathOverlay;
        
        public SaveManager saveManager;

        private int _i;

        public bool isDead;
    
        private void Start()
        {
            _playerHealth = FindObjectOfType<PlayerHealth>();
        
            deathOverlay.SetActive(false);
            Time.timeScale = 1;
            _i = 0;
        }
    
        private void Update()
        {
            if (_playerHealth.hp > 0)
            {
                return;
            }

            _i++;

            if (_i == 1) SoundFXManager.Instance.playDeathFX.Play();
            
            isDead = true;
            Time.timeScale = 0;
            
            deathOverlay.SetActive(true);
            musicManager.Stop();
        }

        public void Retry()
        {
            saveManager.DeleteSave();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Menu()
        {
            saveManager.SaveTime();
            saveManager.DeleteSave();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        public void Quit()
        {
            saveManager.DeleteSave();
            Application.Quit();
        }
    }
}
