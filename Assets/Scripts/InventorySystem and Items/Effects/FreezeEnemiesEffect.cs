using UnityEngine;

namespace InventorySystem_and_Items.Effects
{
    [CreateAssetMenu(fileName = "Freeze Enemies effect", menuName = "Data/Item Effect/Freeze Enemies Effect")]
    public class FreezeEnemiesEffect : ItemEffect
    {
        [SerializeField] private float freezeDuration;

        public override void ExecuteEffect(Transform enemyPosition)
        {
            var (playerManager, playerStats) = GetPlayerManagerAndPlayerStats();
            if (playerManager == null || playerStats == null)
            {
                Debug.LogError("PlayerManager or PlayerStats is null");
                return;
            }
            
            // If the player's health is above 10% of the max health, return
            //if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * .1f) return;
            
            // If the effect of the current armor has been cooled down yet, return
            if(!InventoryManager.instance.CanUseArmorEffect()) return;
            
            const int maxEnemies = 5;
            var colliders = new Collider2D[maxEnemies];
            
            // Get all the enemies in the radius of 2 units
            var size = Physics2D.OverlapCircleNonAlloc(enemyPosition.position, 2f, colliders, 
                LayerMask.GetMask("Enemy"));
            
            // Freeze the movement of all the enemies for the specified duration
            for (var i = 0; i < size; i++)
            {
                var hit = colliders[i];
                hit.GetComponent<Enemy.EnemyStateMachine.Enemy>()?.FreezeMovementFor(freezeDuration);
            }
        }
    }
}