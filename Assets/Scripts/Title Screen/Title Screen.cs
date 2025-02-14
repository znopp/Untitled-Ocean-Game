using Audio;
using Highscore;
using Saves;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Button = UnityEngine.UI.Button;

namespace Title_Screen
{
    public class TitleScreen : MonoBehaviour
    {
        public SplashtextManager splashtextManager;
        public LoadManager loadManager;
        public GameObject settingsMenu;
        public GameObject startGameMenu;
        public GameObject controlsMenu;
        public GameObject soundFXMenu;
        public Button continueButton;

        private string _selectedDifficulty;
        
        private void Awake()
        {
            // Register for scene loaded events
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            // Unregister to prevent memory leaks
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Title Screen")
            {
                UpdateContinueButton();
            }
        }

        private void Start()
        {
            UpdateContinueButton();
            Time.timeScale = 1f;
        }

        private void UpdateContinueButton()
        {
            if (continueButton)
            {
                loadManager = GameObject.Find("LoadManager").GetComponent<LoadManager>();
                // Force a file system check
                var saveExists = loadManager.SaveFileExists();
                
                continueButton.interactable = saveExists;
                
            }
        }
        
        private void Update()
        {

            
            if (Input.GetButtonDown("Cancel"))
            {
                if (settingsMenu.activeSelf)
                {
                    if (controlsMenu.activeSelf)
                    {
                        SoundFXManager.Instance.playExitMenuFX.Play();
                        controlsMenu.SetActive(false);
                        return;
                    }
                    
                    if (soundFXMenu.activeSelf)
                    {
                        SoundFXManager.Instance.playExitMenuFX.Play();
                        soundFXMenu.SetActive(false);
                        return;
                    }
                    
                    SoundFXManager.Instance.playExitMenuFX.Play();
                    splashtextManager.SetRandomSplash();
                    settingsMenu.SetActive(false);
                    return;
                }

                if (startGameMenu.activeSelf)
                {
                    SoundFXManager.Instance.playExitMenuFX.Play();
                    splashtextManager.SetRandomSplash();
                    startGameMenu.SetActive(false);
                }
            }
        }
        
        public void PlayEasy() => StartGameWithDifficulty("easy");
        public void PlayNormal() => StartGameWithDifficulty("normal");
        public void PlayHard() => StartGameWithDifficulty("hard");
        
        private void StartGameWithDifficulty(string difficulty)
        {
            SoundFXManager.Instance.playSpecialButtonClickFX.Play();
            _selectedDifficulty = difficulty;
            HighscoreManager.Instance.SetDifficulty(difficulty);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void StartGameMenu()
        {
            SoundFXManager.Instance.playButtonClickFX.Play();
            startGameMenu.SetActive(true);
        }

        public void Continue()
        {
            SoundFXManager.Instance.playSpecialButtonClickFX.Play();
            loadManager.LoadGame();
        }

        public void SettingsMenu()
        {
            SoundFXManager.Instance.playButtonClickFX.Play();
            settingsMenu.SetActive(true);
        }

        public void ControlsMenu()
        {
            SoundFXManager.Instance.playButtonClickFX.Play();
            controlsMenu.SetActive(true);
        }

        public void SoundMenu()
        {
            SoundFXManager.Instance.playButtonClickFX.Play();
            soundFXMenu.SetActive(true);
        }

        public void Quit()
        {
            SoundFXManager.Instance.playExitMenuFX.Play();
            Application.Quit();
        }
    }
}
