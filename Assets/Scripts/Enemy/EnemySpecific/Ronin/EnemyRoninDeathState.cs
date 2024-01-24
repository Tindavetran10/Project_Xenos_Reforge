using Enemy.EnemyState.SubState;
using Scripts.Enemy.EnemyState.SubState;
using UnityEngine;

namespace Enemy.EnemySpecific.Ronin
{
    public class EnemyRoninDeathState : DeathState
    {
        private readonly EnemyRonin _enemyRonin;
        private static readonly int Die = Animator.StringToHash("die");

        public EnemyRoninDeathState(Scripts.Enemy.EnemyStateMachine.Enemy enemyBase, 
            Scripts.Enemy.EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, EnemyRonin enemyRonin) 
            : base(enemyBase, stateMachine, animBoolName) => _enemyRonin = enemyRonin;

        public override void Enter()
        {
            base.Enter();
            _enemyRonin.Anim.SetBool(Die, true);
        }
    }
}