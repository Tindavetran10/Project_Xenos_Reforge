using Enemy.EnemyState.SubState;
using Scripts.Enemy.EnemyState.SubState;
using UnityEngine;

namespace Enemy.EnemySpecific.Ranger
{
    public class EnemyRangerDeathState : DeathState
    {
        private readonly EnemyRanger _enemyRanger;
        private static readonly int Die = Animator.StringToHash("die");

        public EnemyRangerDeathState(Scripts.Enemy.EnemyStateMachine.Enemy enemyBase, 
            Scripts.Enemy.EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, EnemyRanger enemyRanger) 
            : base(enemyBase, stateMachine, animBoolName) =>
            _enemyRanger = enemyRanger;
        
        public override void Enter()
        {
            base.Enter();
            _enemyRanger.Anim.SetBool(Die, true);
        }
    }
}