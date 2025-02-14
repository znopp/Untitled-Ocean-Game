using System;
using Audio;
using Audio.Managers_etc;
using Highscore;
using Player;
using Saves;
using Shop;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace UI_Scripts
{
    public class PauseMenu : MonoBehaviour
    {
        public ToggleShop toggleShop;
        public SaveManager saveManager;
        public MusicManager musicManager;
        public Death death;

        public GameObject pauseMenu;
        public GameObject controlsMenu;
        public GameObject soundFXMenu;
        public GameObject settingsMenu;

        public GameObject pauseMenuButtons;
        public GameObject settingsMenuButtons;

        public bool isPaused;

        public void Start()
        {
            pauseMenu.SetActive(false);
            isPaused = false;
        }
        
        
        private void Update()
        {
            if (Input.GetButtonDown("Pause"))
            {
                if (toggleShop.isShopActive || death.isDead) return;
                
                if (!isPaused)
                {
                    SoundFXManager.Instance.playExitMenuFX.Play();
                    musicManager.MuffleAudio(true);
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0f;
                    isPaused = true;
                }
                else
                {
                    if (!settingsMenu.activeSelf)
                    {
                        SoundFXManager.Instance.playExitMenuFX.Play();
                        musicManager.MuffleAudio(false);
                        pauseMenu.SetActive(false);
                        Time.timeScale = 1f;
                        isPaused = false;
                        return;
                    }
                    
                    if (controlsMenu.activeSelf)
                    {
                        SoundFXManager.Instance.playExitMenuFX.Play();
                        settingsMenuButtons.SetActive(true);
                        controlsMenu.SetActive(false);
                        return;
                    }
                    
                    if (soundFXMenu.activeSelf)
                    {
                        SoundFXManager.Instance.playExitMenuFX.Play();
                        settingsMenuButtons.SetActive(true);
                        soundFXMenu.SetActive(false);
                        return;
                    }
                    
                    if (settingsMenu.activeSelf)
                    {
                        SoundFXManager.Instance.playExitMenuFX.Play();
                        pauseMenuButtons.SetActive(true);
                        settingsMenu.SetActive(false);
                    }
                }
            }
        }
        

        public void Restart()
        {
            SoundFXManager.Instance.playExitMenuFX.Play();
            saveManager.DeleteSave();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void MainMenu()
        {
            SoundFXManager.Instance.playExitMenuFX.Play();
            saveManager.SaveGameThenTransition();
        }

        public void SettingsMenu()
        {
            SoundFXManager.Instance.playButtonClickFX.Play();
            pauseMenuButtons.SetActive(false);
            settingsMenu.SetActive(true);
        }

        public void Controls()
        {
            SoundFXManager.Instance.playButtonClickFX.Play();
            settingsMenuButtons.SetActive(false);
            controlsMenu.SetActive(true);
        }

        public void SoundMenu()
        {
            SoundFXManager.Instance.playButtonClickFX.Play();
            settingsMenuButtons.SetActive(false);
            soundFXMenu.SetActive(true);
        }

        public void Resume()
        {
            SoundFXManager.Instance.playButtonClickFX.Play();
            musicManager.MuffleAudio(false);
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
        
    }
}
