using Script.Enemy.EnemyState.StateData;
using Script.Enemy.EnemyState.SubState;

namespace Script.Enemy.EnemySpecific.Ranger
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
            if (IsAnimationFinished)
            {
                if(IsPlayerInAgroRange)
                    StateMachine.ChangeState(_enemyRanger.PlayerDetectedState);
                else StateMachine.ChangeState(_enemyRanger.LookForPlayerState);
            }
        }
    }
}