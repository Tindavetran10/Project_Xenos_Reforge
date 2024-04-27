using Manager;
using UnityEngine;

namespace InventorySystem_and_Items.Effects
{
    [CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Data/Item Effect/Thunder Strike Effect")]
    public class ThunderStrikeEffect : ItemEffect
    {
        [SerializeField] private GameObject thunderStrikePrefab;
        public override void ExecuteEffect(Transform enemyPosition)
        {
            var (playerManager, playerStats) = GetPlayerManagerAndPlayerStats();
            if (playerManager == null || playerStats == null)
            {
                Debug.LogError("PlayerManager or PlayerStats is null");
                return;
            }
            
            var newThunderStrike = ObjectPoolManager.SpawnObject(thunderStrikePrefab, 
                enemyPosition.position + new Vector3(0, 0.3f, 0), 
                Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
            
            Destroy(newThunderStrike, 1f);
        }
    }
}