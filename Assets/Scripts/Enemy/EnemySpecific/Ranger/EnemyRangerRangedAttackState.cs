using Enemy.EnemyState.StateData;
using Scripts.Enemy.EnemyState.SubState;

namespace Enemy.EnemySpecific.Ranger
{
    public class EnemyRangerRangedAttackState : RangedAttackState
    {
        private readonly EnemyRanger _enemyRanger;

        public EnemyRangerRangedAttackState(Scripts.Enemy.EnemyStateMachine.Enemy enemyBase, 
            Scripts.Enemy.EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_RangedAttackState stateData, 
            EnemyRanger enemyRanger) : base(enemyBase, stateMachine, animBoolName, stateData) =>
            _enemyRanger = enemyRanger;
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (_enemyRanger.isAnimationFinished)
            {
                if(IsPlayerInAgroRange)
                    StateMachine.ChangeState(_enemyRanger.PlayerDetectedState);
                else StateMachine.ChangeState(_enemyRanger.LookForPlayerState);
            }
        }
    }
}