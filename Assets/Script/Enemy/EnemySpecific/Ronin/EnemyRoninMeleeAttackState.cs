using Script.Enemy.EnemyState.StateData;
using Script.Enemy.EnemyState.SubState;

namespace Script.Enemy.EnemySpecific.Ronin
{
    public class EnemyRoninMeleeAttackState : MeleeAttackState
    {
        private readonly EnemyRonin _enemyRonin;

        public EnemyRoninMeleeAttackState(EnemyStateMachine.Enemy enemyBase, 
            EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_MeleeAttackState stateData, 
            EnemyRonin enemyRonin) : base(enemyBase, stateMachine, animBoolName, stateData) =>
            _enemyRonin = enemyRonin;
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (_enemyRonin.isAnimationFinished)
            {
                if(IsPlayerInAgroRange)
                    StateMachine.ChangeState(_enemyRonin.PlayerDetectedState);
                else StateMachine.ChangeState(_enemyRonin.LookForPlayerState);
            }
        }
    }
}