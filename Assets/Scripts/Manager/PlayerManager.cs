using SaveSystem;
using UnityEngine;

namespace Manager
{
    public class PlayerManager : MonoBehaviour, ISaveManager
    {
        public static PlayerManager Instance;
        public Player.PlayerStateMachine.Player player;

        public int currency;
        public int playerHealth;
        public Vector3 playerPosition;
        
        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);
            else Instance = this;
        }

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