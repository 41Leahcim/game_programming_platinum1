using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level{
    [RequireComponent(typeof(AudioSource))]
    public class Box : MonoBehaviour{
        [SerializeField] private float movementRange = 5;

        [field:SerializeField] public int value{get;private set;} = 1;

        private Vector3 lastPosition;
        private Rigidbody2D boxRigidbody;
        private bool dropped = false;
        private bool landed = false;
        private bool stopped = false;
        private bool paused = false;
        private LevelManager manager = null;
        private SoundController sound = null;

        private RigidbodyType2D previousState = RigidbodyType2D.Dynamic;
        private Vector2 previousVelocity = Vector2.zero;

        void Start(){
            // Retrieve the rigidbody and audioSource of the box
            boxRigidbody = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate(){
            // Skip this function, if the game is paused
            if(paused){
                return;
            }

            // if the box has landed and just stopped:
            if(landed && !stopped && Stopped()){
                // Store that the object has stopped
                stopped = true;

                // Tell the manager the box has landed
                manager.BoxLanded();

                // Stop the object from moving
                boxRigidbody.bodyType = RigidbodyType2D.Static;
            }
            lastPosition = transform.position;
        }

        void OnCollisionEnter2D(Collision2D collision){
            // If the box hits an object, it landed plays a sound effect
            landed = true;
            sound.Collision();

            // If it landed on the ground, the game is over
            if(collision.gameObject.CompareTag("Ground") && !manager.IsGameOver()){
                manager.GameOver();
            }
        }

        public void Drop(){
            // Drop the box, if the game isn't paused
            if(!paused){
                dropped = true;
                boxRigidbody.bodyType = RigidbodyType2D.Dynamic;
            }
        }

        public void Move(float movement) {
            // The box should only move on user input, if the box hasn't been dropped yet
            if (!dropped) {
                // Calculate the movement
                Vector3 worldMovement = Vector3.right * movement;
                worldMovement = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * worldMovement;

                // Calculate and constrain the new position
                Vector3 newPosition = transform.position + worldMovement;
                if(newPosition.x < -movementRange){
                    newPosition.x = -movementRange;
                }else if(newPosition.x > movementRange){
                    newPosition.x = movementRange;
                }

                // Move the object
                transform.position = newPosition;
            }
        }

        public void Rotate(float rotation){
            // The box should only rotate on user input, if the box hasn't been dropped yet
            if(!dropped){
                transform.Rotate(rotation * Vector3.forward);
            }
        }

        bool Stopped(){
            // The box stopped, if it hasn't moved since the last frame
            return boxRigidbody.velocity == Vector2.zero;
        }

        public void Pause(){
            // Pause the box, make it unable to move
            paused = true;
            previousState = boxRigidbody.bodyType;
            if(boxRigidbody.bodyType != RigidbodyType2D.Static){
                boxRigidbody.bodyType = RigidbodyType2D.Static;
                previousVelocity = boxRigidbody.velocity;
            }
        }

        public void Resume(){
            // Resume the box, allow it to move again unless it shouldn't
            paused = false;
            if(previousState != RigidbodyType2D.Static){
                boxRigidbody.bodyType = previousState;
                boxRigidbody.velocity = previousVelocity;
            }
        }

        public void Initialize(LevelManager levelManager, SoundController soundController){
            // Set the manager, if it hasn't been set yet
            if(manager == null){
                manager = levelManager;
            }

            // Set the sound controller, if it hasn't been set yet
            if(sound == null){
                sound = soundController;
            }
        }
    }
}
