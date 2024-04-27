using Manager;
using Player.PlayerStats;
using UnityEngine;

namespace InventorySystem_and_Items.Effects
{
    public class ItemEffect : ScriptableObject
    {
        protected static (Player.PlayerStateMachine.Player playerManager, PlayerStats playerStats) 
            GetPlayerManagerAndPlayerStats()
        {
            var playerManager = PlayerManager.GetInstance().player;
            if (playerManager == null)
            {
                Debug.LogError("PlayerManager instance is null");
                return (null, null);
            }

            var playerStats = playerManager.GetComponentInChildren<PlayerStats>();
            if (playerStats == null)
            {
                Debug.LogError("PlayerStats component is null");
                return (null, null);
            }

            return (playerManager, playerStats);
        }
        
        public virtual void ExecuteEffect(Transform enemyPosition)
        {
            var (playerManager, playerStats) = GetPlayerManagerAndPlayerStats();
            if (playerManager == null || playerStats == null)
            {
                Debug.LogError("PlayerManager or PlayerStats is null");
                return;
            }

            Debug.Log("Item effect executed");
        }
    }
}