using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level{
    [RequireComponent(typeof(AudioSource))]
    public class SoundController : MonoBehaviour{
        [SerializeField] private AudioClip gameOverSound;
        [SerializeField] private AudioClip collisionSound;
        private AudioSource audioSource;
        
        private float musicVolume;
        private float effectVolume;
        
        void Awake(){
            audioSource = GetComponent<AudioSource>();

            // Read and store the sound settings            
            bool sound = PlayerPrefs.GetInt("Sound", 1) == 1;
            bool music = PlayerPrefs.GetInt("Music", 1) == 1;
            bool soundEffects = PlayerPrefs.GetInt("Sound Effect", 1) == 1;
            musicVolume = sound && music? PlayerPrefs.GetFloat("Music Volume") : 0;
            effectVolume = sound && soundEffects? PlayerPrefs.GetFloat("Sound Effect Volume") : 0;

            // Set the volume of the audio source to the music volume
            audioSource.volume = musicVolume;
        }

        // Plays the game over sound effect
        public void GameOver() => 
            audioSource.PlayOneShot(gameOverSound, effectVolume);

        // Plays the collision sound effect
        public void Collision() =>
            audioSource.PlayOneShot(collisionSound, effectVolume);
    }
}
