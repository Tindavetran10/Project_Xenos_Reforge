using Script.Enemy.EnemyState.StateData;
using Script.Enemy.EnemyState.SubState;

namespace Script.Enemy.EnemySpecific.Ranger
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
                if(IsPlayerInAgroRange && !PerformCloseRangeAction)
                    StateMachine.ChangeState(_enemyRanger.RangedAttackState);
                else if(!IsPlayerInAgroRange)
                    StateMachine.ChangeState(_enemyRanger.LookForPlayerState);    
            }
        }
    }
}