using System.Collections.Generic;
using System.Linq;
using Script.SaveSystem;
using UnityEngine;

namespace Script.Manager
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance;

        [SerializeField] private string fileName;
        [SerializeField] private bool encryptData;
        
        private GameData _gameData;
        private List<ISaveManager> _saveManagers;
        private FileDataHandler _dataHandler;

        [ContextMenu("Delete Save File")]
        public void DeleteSaveData()
        {
            _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
            _dataHandler.Delete();
        }
        
        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);
            else Instance = this;
            
            _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
            _saveManagers = FindAllSaveManagers();
            LoadGame();
        }

        private void NewGame() => _gameData = new GameData();

        private void LoadGame()
        {
            _gameData = _dataHandler.Load();
            
            if (_gameData == null)
            {
                Debug.Log("No saved data found");
                NewGame();
            }

            foreach (var saveManager in _saveManagers) 
                saveManager.LoadData(_gameData);
        }

        public void SaveGame()
        {
            foreach (var saveManager in _saveManagers) 
                saveManager.SaveData(ref _gameData);
            
            _dataHandler.Save(_gameData);
        }

        private void OnApplicationQuit() => SaveGame();

        private static List<ISaveManager> FindAllSaveManagers()
        {
            var saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
            return new List<ISaveManager>(saveManagers);
        }

        public bool HasSaveData() => _dataHandler.Load() != null;
    }
}
