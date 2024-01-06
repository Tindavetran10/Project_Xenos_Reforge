using UnityEngine;

namespace _Scripts.Enemies.EnemyState.StateData
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