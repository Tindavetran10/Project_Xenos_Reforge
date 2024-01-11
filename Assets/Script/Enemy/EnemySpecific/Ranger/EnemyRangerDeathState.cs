using Script.Enemy.EnemyState.SubState;
using UnityEngine;

namespace Script.Enemy.EnemySpecific.Ranger
{
    public class EnemyRangerDeathState : DeathState
    {
        private readonly EnemyRanger _enemyRanger;
        private static readonly int Die = Animator.StringToHash("die");

        public EnemyRangerDeathState(EnemyStateMachine.Enemy enemyBase, 
            EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, EnemyRanger enemyRanger) 
            : base(enemyBase, stateMachine, animBoolName) =>
            _enemyRanger = enemyRanger;
        
        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityZero();
            _enemyRanger.Anim.SetBool(Die, true);
        }
    }
}