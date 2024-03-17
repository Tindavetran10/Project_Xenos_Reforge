using UnityEngine;

namespace Enemy.EnemyState.StateData
{
    [CreateAssetMenu(fileName = "newGetAttackedStateData", menuName = "Data/State Data/GetAttacked State")]
    public class D_GetAttacked : ScriptableObject
    {
        public float getAttackedTime;
        
        public float getAttackedKnockbackTime = 0.2f;
        public float getAttackedKnockbackSpeed = 20f;
        public Vector2 getAttackedKnockbackAngle;
    }
}