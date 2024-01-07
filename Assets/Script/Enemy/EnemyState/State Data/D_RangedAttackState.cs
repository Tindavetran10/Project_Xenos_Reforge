using UnityEngine;

namespace Script.Enemy.EnemyState.State_Data
{
    [CreateAssetMenu(fileName = "newRangedAttackStateData", menuName = "Data/State Data/Ranged Attack State")]
    public class D_RangedAttackState : ScriptableObject
    {
        public GameObject projectile;
        public float projectileDamage = 10f;
        public float projectileSpeed = 12f;
        public float projectileTravelDistance;
    }
}