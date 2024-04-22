using Manager;
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
            var playerManager = PlayerManager.GetInstance().player;
            if (playerManager == null)
            {
                Debug.LogError("PlayerManager instance is null");
                return;
            }

            var playerStats = playerManager.GetComponentInChildren<PlayerStats>();
            if (playerStats == null)
            {
                Debug.LogError("PlayerStats component is null");
                return;
            }
            
            if (_playerStats == null)
            {
                Debug.LogError("PlayerStats instance is null");
                return;
            }

            playerStats.IncreaseStatBy(buffAmount, buffDuration, _playerStats.GetStat(buffType));
        }
    }
}