using UnityEngine;

namespace Enemy.EnemyState.StateData
{
    [CreateAssetMenu(fileName = "newLookForPlayerStateData", menuName = "Data/State Data/Look For Player State")]
    public class D_LookForPlayerState : ScriptableObject
    {
        public int amountOfTurns = 2;
        public float timeBetweenTurns = 0.75f;
    }
}