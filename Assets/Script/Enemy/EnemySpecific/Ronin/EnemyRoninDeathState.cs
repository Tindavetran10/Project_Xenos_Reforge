using Script.Enemy.EnemyState.SubState;
using UnityEngine;

namespace Script.Enemy.EnemySpecific.Ronin
{
    public class EnemyRoninDeathState : DeathState
    {
        private readonly EnemyRonin _enemyRonin;
        private static readonly int Die = Animator.StringToHash("die");

        public EnemyRoninDeathState(EnemyStateMachine.Enemy enemyBase, 
            EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, EnemyRonin enemyRonin) 
            : base(enemyBase, stateMachine, animBoolName) => _enemyRonin = enemyRonin;

        public override void Enter()
        {
            base.Enter();
            _enemyRonin.Anim.SetBool(Die, true);
        }
    }
}