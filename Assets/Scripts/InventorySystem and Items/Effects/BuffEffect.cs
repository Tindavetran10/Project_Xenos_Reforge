using Player.PlayerStats;
using UnityEngine;

namespace InventorySystem_and_Items.Effects
{
    [CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item Effect/Buff Effect")]
    public class BuffEffect : ItemEffect
    {
        private PlayerStats _playerStats;
        [SerializeField] private EnumList.StatType buffType;
        [SerializeField] private int buffAmount;
        [SerializeField] private float buffDuration;
        
        public override void ExecuteEffect(Transform enemyPosition)
        {
            var (playerManager, playerStats) = GetPlayerManagerAndPlayerStats();
            if (playerManager == null || playerStats == null)
            {
                Debug.LogError("PlayerManager or PlayerStats is null");
                return;
            }
            
            // Increase the stat by the buff amount for the buff duration base on the stat type
            playerStats.IncreaseStatBy(buffAmount, buffDuration, _playerStats.GetStat(buffType));
        }
    }
}