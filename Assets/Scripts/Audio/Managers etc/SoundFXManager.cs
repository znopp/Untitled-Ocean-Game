using UnityEngine;

namespace Audio
{
    public class SoundFXManager : MonoBehaviour
    {
        public static SoundFXManager Instance;
        
        public AudioSource soundFXObject;
        
        // Sound effects
        [Header("UI")]
        public PlaySoundFX playExitMenuFX;
        public PlaySoundFX playButtonClickFX;
        public PlaySoundFX playSpecialButtonClickFX;
        public PlaySoundFX playVolumeSliderFX;
        
        [Header("Player")]
        public PlaySoundFX playDamagedFX;
        public PlaySoundFX playDeathFX;
        
        [Header("Weapons")]
        public PlaySoundFX playAoeFX;
        public PlaySoundFX playShootFX;
        public PlaySoundFX playHitObjectFX;
        
        [Header("Enemy")]
        public PlaySoundFX playEnemyDeathFX;
        
        [Header("Items")]
        public PlaySoundFXList playHealedFX;
        public PlaySoundFXList playCoinFX;
        public PlaySoundFX playFullHealthFX;
        
        [Header("Misc.")]
        public PlaySoundFX playWiggleFX;

        [Header("Shop")]
        public PlaySoundFX playPurchaseFX;
        public PlaySoundFX playInsufficientFundsFX;
        

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
        }

        public void PlaySoundFX(AudioClip audioClip, Transform spawnTransform, float volume)
        {
            var audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
    
            audioSource.clip = audioClip;
            audioSource.volume = volume;
    
            audioSource.Play();
        }
        
        public void PlayRandomSoundFX(AudioClip[] audioClip, Transform spawnTransform, float volume)
        {
            var rand  = Random.Range(0, audioClip.Length);
            
            var audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
            
            audioSource.clip = audioClip[rand];
            
            audioSource.volume = volume;
            
            audioSource.Play();
        }
        
        
    }
}
