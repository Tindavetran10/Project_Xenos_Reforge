using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Script.SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager _instance;

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
            if (_instance != null)
                Destroy(_instance.gameObject);
            else _instance = this;
            
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

            foreach (ISaveManager saveManager in _saveManagers) 
                saveManager.LoadData(_gameData);
        }

        private void SaveGame()
        {
            foreach (ISaveManager saveManager in _saveManagers) 
                saveManager.SaveData(ref _gameData);
            
            _dataHandler.Save(_gameData);
        }

        private void OnApplicationQuit() => SaveGame();

        private List<ISaveManager> FindAllSaveManagers()
        {
            IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
            return new List<ISaveManager>(saveManagers);
        }

        public bool HasSaveData()
        {
            return _dataHandler.Load() != null;
        }
    }
}
