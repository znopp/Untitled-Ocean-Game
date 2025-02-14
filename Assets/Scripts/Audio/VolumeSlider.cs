using Audio.Managers_etc;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Audio
{
    public class VolumeSlider : MonoBehaviour
    {
        [FormerlySerializedAs("soundMixerManager")] public VolumeManager volumeManager;
        public Slider masterVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider fxVolumeSlider;
        private bool _isInitializing = true;

        private void Start()
        {
            // Load saved values if they exist
            LoadVolumeSettings();
            
            // Set up slider listeners
            /*masterVolumeSlider.onValueChanged.AddListener(volumeManager.SetMasterVolume);*/
            masterVolumeSlider.onValueChanged.AddListener(value => volumeManager.SetMasterVolume(value, !_isInitializing));
            musicVolumeSlider.onValueChanged.AddListener(value => volumeManager.SetMusicVolume(value, !_isInitializing));
            fxVolumeSlider.onValueChanged.AddListener(value => volumeManager.SetFXVolume(value, !_isInitializing));
            
            // Sync slider positions with current mixer values
            masterVolumeSlider.value = volumeManager.GetMasterVolume();
            musicVolumeSlider.value = volumeManager.GetMusicVolume();
            fxVolumeSlider.value = volumeManager.GetFXVolume();
        }

        private void OnDisable()
        {
            SaveVolumeSettings();
        }

        private void SaveVolumeSettings()
        {
            PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
            PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
            PlayerPrefs.SetFloat("FXVolume", fxVolumeSlider.value);
            PlayerPrefs.Save();
        }

        private void LoadVolumeSettings()
        {
            _isInitializing = true;
            
            if (PlayerPrefs.HasKey("MasterVolume"))
            {
                var masterVolume = PlayerPrefs.GetFloat("MasterVolume");
                var musicVolume = PlayerPrefs.GetFloat("MusicVolume");
                var fxVolume = PlayerPrefs.GetFloat("FXVolume");

                volumeManager.SetMasterVolume(masterVolume, false);
                volumeManager.SetMusicVolume(musicVolume, false);
                volumeManager.SetFXVolume(fxVolume, false);

                // Update sliders to match the loaded settings
                masterVolumeSlider.value = masterVolume;
                musicVolumeSlider.value = musicVolume;
                fxVolumeSlider.value = fxVolume;
            }
            
            _isInitializing = false;
        }
    }
}