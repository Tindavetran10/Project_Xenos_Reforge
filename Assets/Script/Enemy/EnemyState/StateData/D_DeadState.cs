using UnityEngine;

namespace Script.Enemy.EnemyState.StateData
{
    [CreateAssetMenu(fileName = "newDeadStateData", menuName = "Data/State Data/Dead State")]
    public class D_DeadState : ScriptableObject
    {
        public GameObject deathChunkParticle;
        public GameObject deathBloodParticle;
    }
}