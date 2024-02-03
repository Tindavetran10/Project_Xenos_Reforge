using Enemy.EnemyState.StateData;
using Scripts.Enemy.EnemyState.SubState;

namespace Enemy.EnemySpecific.Ranger
{
    public class EnemyRangerDodgeState : DodgeState
    {
        private readonly EnemyRanger _enemyRanger;

        public EnemyRangerDodgeState(EnemyStateMachine.Enemy enemyBase, 
            Scripts.Enemy.EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_DodgeState stateData, 
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