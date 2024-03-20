using SaveSystem;
using UnityEngine;

namespace Manager
{
    public class PlayerManager : MonoBehaviour, ISaveManager
    {
        private static PlayerManager _instance;
        public Player.PlayerStateMachine.Player player;

        public int currency;
        public int playerHealth;
        public Vector3 playerPosition;
        
        private void Awake()
        {
            // Check if an instance already exists
            if (_instance != null && _instance != this)
            {
                // If so, destroy this instance
                Destroy(gameObject);
                return;
            }

            // Set the instance to this object
            _instance = this;

            // Make sure this object persists between scenes
            DontDestroyOnLoad(gameObject);
        }
        
        // Method to get the instance
        public static PlayerManager GetInstance() => _instance;

        public bool HaveEnoughSoul(int price)
        {
            if (price > currency)
            {
                Debug.Log("Not enough soul");
                return false;
            }

            currency -= price;
            return true;
        }

        public int GetCurrency() => currency;
        public Vector3 GetPosition() => playerPosition;

        public void LoadData(GameData data)
        {
            currency = data.currency;
            playerHealth = data.playerHealth;
            playerPosition = data.playerPosition;
        }

        public void SaveData(ref GameData data)
        {
            data.currency = currency;
            data.playerHealth = player.Stats.currentHealth;
            data.playerPosition = player.transform.position;
        }
    }
}