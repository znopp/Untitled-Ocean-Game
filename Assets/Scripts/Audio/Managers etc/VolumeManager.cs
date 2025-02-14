using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio.Managers_etc
{
    public class VolumeManager : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public bool isPlaying;
        public float newVolume;

        public void SetMasterVolume(float volume, bool playSound = true)
        {
            newVolume = volume > 0 ? volume : 0.001f;
            if (playSound) PlayFX(newVolume);
            audioMixer.SetFloat("masterVolume", Mathf.Log10(newVolume) * 20);
        }

        public void SetMusicVolume(float volume, bool playSound = true)
        {
            newVolume = volume > 0 ? volume : 0.001f;
            if (playSound) PlayFX(newVolume);
            audioMixer.SetFloat("musicVolume", Mathf.Log10(newVolume) * 20);
        }

        public void SetFXVolume(float volume, bool playSound = true)
        {
            newVolume = volume > 0 ? volume : 0.001f;
            if (playSound) PlayFX(newVolume);
            audioMixer.SetFloat("soundFXVolume", Mathf.Log10(newVolume) * 20);
        }

        public float GetMasterVolume()
        {
            audioMixer.GetFloat("masterVolume", out var volume);
            return Mathf.Pow(10, volume / 20); // Convert from dB to linear scale
        }

        public float GetMusicVolume()
        {
            audioMixer.GetFloat("musicVolume", out var volume);
            return Mathf.Pow(10, volume / 20); // Convert from dB to linear scale
        }

        public float GetFXVolume()
        {
            audioMixer.GetFloat("soundFXVolume", out var volume);
            return Mathf.Pow(10, volume / 20); // Convert from dB to linear scale
        }

        private void PlayFX(float volume)
        {
            if (!isPlaying) StartCoroutine(PlayFXCoroutine(volume));
        }

        private IEnumerator PlayFXCoroutine(float volume)
        {
            isPlaying = true;
            SoundFXManager.Instance.playVolumeSliderFX.Play(volume);
            yield return new WaitForSecondsRealtime(SoundFXManager.Instance.playVolumeSliderFX.clip.length);
            isPlaying = false;
        }
    }
}