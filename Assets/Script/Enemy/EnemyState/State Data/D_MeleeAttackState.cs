using UnityEngine;

namespace _Scripts.Enemies.EnemyState.StateData
{
    [CreateAssetMenu(fileName = "newMeleeAttackStateData", menuName = "Data/State Data/Melee Attack State")]
    public class D_MeleeAttackState : ScriptableObject
    {
        public int comboTotal = 2;
        
        public float attackRadius = 0.5f;
        public float attackDamage = 10f;

        public Vector2 knockbackAngle = Vector2.one;
        public float knockbackStrength = 10f;

        public float poiseDamage;

        public LayerMask whatIsPlayer;
    }
}