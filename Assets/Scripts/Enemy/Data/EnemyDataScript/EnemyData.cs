using UnityEngine;

namespace Enemy.Data.EnemyDataScript
{
    [CreateAssetMenu(fileName = "newEnemyData", menuName = "Data/Enemy Data/Base Data")]
    public class EnemyData : ScriptableObject
    {
        public float agroDistance = 3f;
        public float closeRangeActionDistance = 1f;

        public GameObject hitParticle;
        
        public LayerMask whatIsPlayer;
        
        public Rect[] hitBox;
        
        public float hitStopDuration;
    }
}