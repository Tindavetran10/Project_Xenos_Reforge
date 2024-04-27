using Enemy.EnemyStats;
using Manager;
using Player.PlayerStats;
using UnityEngine;

namespace Controller
{
    public class ThunderStrikeController : MonoBehaviour
    {
        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.GetComponent<Enemy.EnemyStateMachine.Enemy>() != null)
            {
                var playerStats = PlayerManager.GetInstance().player.GetComponentInChildren<PlayerStats>();
                var enemyTarget = collision.GetComponentInChildren<EnemyStats>();
                playerStats.DoMagicalDamage(enemyTarget);
            }
        }
    }
}
