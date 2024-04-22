using Manager;
using Player.PlayerStats;
using UnityEngine;

namespace InventorySystem_and_Items.Effects
{
    [CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item Effect/Heal Effect")]
    public class HealEffect : ItemEffect
    {
        [Range(0f, 1f)]
        [SerializeField] private float healPercent;
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

            var healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);
            playerStats.IncreaseHealthBy(healAmount);
        }
    }
}