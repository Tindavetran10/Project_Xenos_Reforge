using Script.SaveSystem;
using UnityEngine;

namespace Script.Manager
{
    public class PlayerManager : MonoBehaviour, ISaveManager
    {
        public static PlayerManager Instance;
        public Player.PlayerStateMachine.Player player;

        public int currency;
        
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

        public void LoadData(GameData data)
        {
            currency = data.currency;
        }

        public void SaveData(ref GameData data)
        {
            data.currency = currency;
        }
    }
}