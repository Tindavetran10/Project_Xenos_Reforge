using UnityEngine;

namespace Script.Enemy.EnemyState.StateData
{
    [CreateAssetMenu(fileName = "newMeleeAttackStateData", menuName = "Data/State Data/Melee Attack State")]
    public class D_MeleeAttackState : ScriptableObject
    {
        public int comboTotal;

        public float attackDistance;
        
        public float attackDamage;

        public Vector2 knockbackAngle;
        public float knockbackStrength;

        public float poiseDamage;

        public float attackCooldown;

        public LayerMask whatIsPlayer;
    }
}