using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SubState;

namespace Enemy.EnemySpecific.Ranger
{
    public class EnemyRangerDodgeState : DodgeState
    {
        private readonly EnemyRanger _enemyRanger;

        public EnemyRangerDodgeState(EnemyStateMachine.Enemy enemyBase, 
            EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_DodgeState stateData, 
            EnemyRanger enemyRanger) : base(enemyBase, stateMachine, animBoolName, stateData) =>
            _enemyRanger = enemyRanger;

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsDodgeOver)
            {
                if(PerformLongRangeAction && !PerformCloseRangeAction)
                    StateMachine.ChangeState(_enemyRanger.RangedAttackState);
                else if(!PerformLongRangeAction)
                    StateMachine.ChangeState(_enemyRanger.LookForPlayerState);  
            }
        }
    }
}