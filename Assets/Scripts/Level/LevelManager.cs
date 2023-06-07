using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace Level{
    public class LevelManager : MonoBehaviour{
        [SerializeField] private Box[] boxPrefabs;
        [SerializeField] private Score score;
        [SerializeField] private float spawnOffset = 3;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private UnityEvent gameOverEvent;
        [SerializeField] private UnityEvent pauseEvent;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private SoundController soundController;

        private CameraController cameraController;
        private Box currentBox;
        private float movement;
        private float rotation;
        private float spawnHeight = 0;
        private bool gameOver;
        private bool paused;

        void SpawnBox(){
            // Select a random box
            int prefabIndex = Random.Range(0, boxPrefabs.Length);
            Box prefab = boxPrefabs[prefabIndex];

            // Set the spawn position
            Vector3 spawnPosition = new Vector3(0, spawnHeight + spawnOffset, 0);

            // Create the box
            currentBox = Instantiate(prefab, spawnPosition, prefab.transform.rotation);
            currentBox.Initialize(this, soundController);
            pauseEvent.AddListener(currentBox.Pause);
        }

        // Start is called before the first frame update
        void Start(){
            // Retrieve the camera controller and audio source
            cameraController = Camera.main.GetComponent<CameraController>();

            // Spawn the first box
            SpawnBox();
        }

        // Update is called once per frame
        void Update(){
            if(!paused){
                // Move and rotate the box dependent on input
                currentBox.Move(movement * Time.deltaTime);
                currentBox.Rotate(rotation * Time.deltaTime * rotationSpeed);
            }
        }

        // Sets the direction the box should move, based on input
        void OnMove(InputValue value) => movement = value.Get<float>();

        // Sets the direction the box should turn, based on input
        void OnRotate(InputValue value) => rotation = value.Get<float>();

        // Drops the box, based on a key press
        void OnDrop(InputValue value){
            // Only drop if the game isn't paused
            if(!paused){
                currentBox.Drop();
            }
        }

        public void BoxLanded(){
            // Remove the box from the pause event
            pauseEvent.RemoveListener(currentBox.Pause);

            // We don't want to adjust the camera or spawn a new box when the game is over
            if(gameOver){
                return;
            }

            // Add the value of the box to the score
            score.AddPoints(currentBox.value);

            // Adjust the height of the camera, if the box stopped too high
            if(currentBox.transform.position.y >= spawnHeight){
                spawnHeight = currentBox.transform.position.y;
                cameraController.AdjustCameraHeight(currentBox.transform.position.y);
            }

            // Spawn the next box
            SpawnBox();
        }

        public void GameOver(){
            // The game is over, disable gameplay and show the game over menu
            gameOver = true;
            gameOverEvent.Invoke();
            gameManager.SaveScore(score.score);
        }

        public bool IsGameOver() => gameOver;

        public void Restart(){
            // Reload the current scene
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadSceneAsync(sceneName);
        }

        private void OnPause(){
            // Pause the game, and display the pause menu
            paused = true;
            pauseEvent.Invoke();
        }

        public void Resume(){
            // Resume the game
            paused = false;
            currentBox.Resume();
        }
    }
}
