using System.Collections;
using System.Linq;
using _Scripts.Player.Input;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class GameManager : MonoBehaviour, ISaveManager
    {
        // Singleton pattern instance
        public static GameManager Instance;

        // Reference to the player's transform
        private Transform _player;
        [SerializeField] private PlayerInputHandler playerInputHandler;
        [SerializeField] private GameObject settingsMenu;
        

        // Array of Checkpoint objects in the scene
        [SerializeField] private Checkpoint[] checkpoints;
        // ID of the closest checkpoint
        [SerializeField] private string closestCheckpointId;

        // Called when the script instance is being loaded
        private void Awake()
        {
            // Ensure only one instance of GameManager exists
            if (Instance != null)
                Destroy(Instance.gameObject);
            else Instance = this;
        }

        // Called after Awake, used for initialization
        private void Start()
        {
            AudioManager.instance.Play("InGame");
            // Find all Checkpoint objects in the scene
            checkpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.InstanceID);
            
            // Get the player's transform
            //_player = PlayerManager.Instance.player.transform;
            _player = PlayerManager.GetInstance().player.transform;

            playerInputHandler.MenuOpenEvent += HandleMenuOpen;
            playerInputHandler.MenuCloseEvent += HandleMenuClose;
        }

        private void HandleMenuOpen() => settingsMenu.SetActive(true);

        private void HandleMenuClose() => settingsMenu.SetActive(false);

        // Restart the current scene
        public static void RestartScene()
        {
            // Save the game before restarting
            SaveManager.Instance.SaveGame();
            
            // Get the active scene and reload it
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        // Load game data from the provided GameData object
        public void LoadData(GameData data) => StartCoroutine(LoadWithDelay(data));

        // Load activated checkpoints based on the GameData
        private void LoadCheckpoints(GameData data)
        {
            foreach (var checkpoint in from pair in data.checkpoints
                                       from checkpoint in checkpoints
                                       where checkpoint.id == pair.Key && pair.Value
                                       select checkpoint) 
                checkpoint.ActivateCheckPoint();
        }

        // Coroutine to introduce a delay before loading checkpoints
        private IEnumerator LoadWithDelay(GameData data)
        {
            yield return new WaitForSeconds(.1f);
            // Load Current Player Position
            LoadCurrentPlayerPosition(data);
            
            // Load activated checkpoints
            LoadCheckpoints(data);
            
            // Load the player's position based on the closest checkpoint
            //LoadClosestCheckPoint(data);
        }

        // Save game data to the provided GameData object
        public void SaveData(ref GameData data)
        {
            data.playerHealth = PlayerManager.GetInstance().playerHealth;
            data.playerPosition = _player.position;
            
            // Find the closest checkpoint and save its ID
            if (FindClosestCheckPoint() != null)
                data.closestCheckpointID = FindClosestCheckPoint().id;

            // Clear and save the activation status of all checkpoints
            data.checkpoints.Clear();

            foreach (var checkpoint in checkpoints)
                data.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
        }

        // Load the player's position based on the closest checkpoint
        private void LoadClosestCheckPoint(GameData data)
        {
            // If there is no closest checkpoint ID, do nothing
            if (data.closestCheckpointID == null) return;

            // Set the closest checkpoint ID
            closestCheckpointId = data.closestCheckpointID;

            // Set the player's position to the position of the closest checkpoint
            foreach (var checkpoint in checkpoints)
            {
                if (closestCheckpointId == checkpoint.id)
                    _player.position = checkpoint.transform.position;
            }
        }

        private void LoadCurrentPlayerPosition(GameData data) => _player.position = data.playerPosition;

        // Find the closest activated checkpoint
        private Checkpoint FindClosestCheckPoint()
        {
            var closestDistance = Mathf.Infinity;
            Checkpoint closestCheckPoint = null;

            foreach (var checkpoint in checkpoints)
            {
                var distanceToCheckpoint = Vector2.Distance(_player.position, checkpoint.transform.position);
                // Check if the checkpoint is closer and activated
                if (distanceToCheckpoint < closestDistance && checkpoint.activationStatus)
                {
                    closestDistance = distanceToCheckpoint;
                    closestCheckPoint = checkpoint;
                }
            }
            return closestCheckPoint;
        }

        // Pause or resume the game by adjusting time scale
        public static void PauseGame(bool pause) => Time.timeScale = pause ? 0 : 1;
    }
}
