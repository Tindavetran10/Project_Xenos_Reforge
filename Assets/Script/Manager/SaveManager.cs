using System.Collections.Generic;
using System.Linq;
using Script.SaveSystem;
using UnityEngine;

namespace Script.Manager
{
    public class SaveManager : MonoBehaviour
    {
        // Singleton pattern instance
        public static SaveManager Instance;

        [SerializeField] private string fileName; // Name of the save file
        [SerializeField] private bool encryptData; // Flag to indicate whether data should be encrypted
        
        private GameData _gameData; // Data representing the game state
        private List<ISaveManager> _saveManagers; // List of objects that implement the ISaveManager interface
        private FileDataHandler _dataHandler; // Handles reading and writing data to a file

        // Context menu function to delete the save file
        [ContextMenu("Delete Save File")]
        public void DeleteSaveData()
        {
            _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
            _dataHandler.Delete();
        }
        
        // Called when the script instance is being loaded
        private void Awake()
        {
            // Ensure only one instance of SaveManager exists
            if (Instance != null)
                Destroy(Instance.gameObject);
            else Instance = this;
            
            // Initialize FileDataHandler with file path, name, and encryption flag
            _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
            
            // Find all objects implementing ISaveManager in the scene
            _saveManagers = FindAllSaveManagers();
            
            // Load the game state
            LoadGame();
        }

        // Create a new game with default data
        private void NewGame() => _gameData = new GameData();

        // Load game state from the save file
        private void LoadGame()
        {
            // Attempt to load game data from the save file
            _gameData = _dataHandler.Load();
            
            // If no saved data is found, create a new game
            if (_gameData == null)
            {
                Debug.Log("No saved data found");
                NewGame();
            }

            // Load data into each ISaveManager object
            foreach (var saveManager in _saveManagers) 
                saveManager.LoadData(_gameData);
        }

        // Save the current game state
        public void SaveGame()
        {
            // Save data from each ISaveManager object to the game data
            foreach (var saveManager in _saveManagers) 
                saveManager.SaveData(ref _gameData);
            
            // Save the game data to the file
            _dataHandler.Save(_gameData);
        }

        // Called when the application is quitting
        private void OnApplicationQuit() => SaveGame();

        // Find all objects in the scene implementing ISaveManager
        private static List<ISaveManager> FindAllSaveManagers()
        {
            // Use LINQ to find all MonoBehaviour objects implementing ISaveManager
            var saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
            return new List<ISaveManager>(saveManagers);
        }

        // Check if there is any save data available
        public bool HasSaveData() => _dataHandler.Load() != null;
    }
}
