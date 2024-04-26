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
            var (playerManager, playerStats) = GetPlayerManagerAndPlayerStats();
            if (playerManager == null || playerStats == null)
            {
                Debug.LogError("PlayerManager or PlayerStats is null");
                return;
            }

            var healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);
            playerStats.IncreaseHealthBy(healAmount);
        }
    }
}