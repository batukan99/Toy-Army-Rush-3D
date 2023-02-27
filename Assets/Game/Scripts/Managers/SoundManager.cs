using UnityEngine;
using UnityEngine.Events;
using Game.Core.Events;
using EventType = Game.Core.Enums.EventType;
using Game.Core;

namespace Game.Managers
{
    public class SoundManager : MonoBehaviour, IProvidable
    {
        [SerializeField] private AudioClip[] RifleFireSounds;
        [SerializeField] private AudioClip[] SniperFireSounds;
        [SerializeField] private AudioClip[] RocketFireSounds;
        [SerializeField] private AudioClip[] RocketHitSounds;
        [SerializeField] private AudioClip[] ExplosionSounds;

        private void PlaySoundEffect(AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        private AudioClip ChooseRandomSound(AudioClip[] audioClips)
        {
            int randomIndex = Random.Range(0, audioClips.Length);
            return audioClips[randomIndex];
        }

        public void PlayRifleFireSound(AudioSource audioSource) 
        {
            AudioClip randomSound = ChooseRandomSound(RifleFireSounds);
            PlaySoundEffect(audioSource, randomSound);
        }
        public void PlaySniperFireSound(AudioSource audioSource) 
        {
            AudioClip randomSound = ChooseRandomSound(SniperFireSounds);
            PlaySoundEffect(audioSource, randomSound);
        }
        public void PlayRocketFireSound(AudioSource audioSource) 
        {
            AudioClip randomSound = ChooseRandomSound(RocketFireSounds);
            PlaySoundEffect(audioSource, randomSound);
        }
        public void PlayRocketHitSound(AudioSource audioSource) 
        {
            AudioClip randomSound = ChooseRandomSound(RocketHitSounds);
            PlaySoundEffect(audioSource, randomSound);
        }
        public void PlayExplosionSound(AudioSource audioSource) 
        {
            AudioClip randomSound = ChooseRandomSound(ExplosionSounds);
            PlaySoundEffect(audioSource, randomSound);
        }
        
        private void OnEnable()
        {
            ManagerProvider.Register(this);
        }
    }
}
