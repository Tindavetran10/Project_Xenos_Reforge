using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SubState;

namespace Enemy.EnemySpecific.Ranger
{
    public class EnemyRangerRangedAttackState : RangedAttackState
    {
        private readonly EnemyRanger _enemyRanger;

        public EnemyRangerRangedAttackState(EnemyStateMachine.Enemy enemyBase, 
            EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_RangedAttackState stateData, 
            EnemyRanger enemyRanger) : base(enemyBase, stateMachine, animBoolName, stateData) =>
            _enemyRanger = enemyRanger;
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (_enemyRanger.isAnimationFinished)
            {
                if(PerformLongRangeAction)
                    StateMachine.ChangeState(_enemyRanger.PlayerDetectedState);
                else StateMachine.ChangeState(_enemyRanger.LookForPlayerState);
            }
        }
    }
}