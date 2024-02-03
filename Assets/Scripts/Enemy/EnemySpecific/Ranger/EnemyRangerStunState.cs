using Enemy.EnemyState.StateData;
using Scripts.Enemy.EnemyState.SubState;

namespace Enemy.EnemySpecific.Ranger
{
    public class EnemyRangerStunState : StunState
    {
        private readonly EnemyRanger _enemyRanger;

        public EnemyRangerStunState(EnemyStateMachine.Enemy enemyBase, 
            Scripts.Enemy.EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_StunState stateData, 
            EnemyRanger enemyRanger) : base(enemyBase, stateMachine, animBoolName, stateData) =>
            _enemyRanger = enemyRanger;
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsStunTimeOver)
            {
                if(IsPlayerInAgroRange)
                    StateMachine.ChangeState(_enemyRanger.PlayerDetectedState);
                StateMachine.ChangeState(_enemyRanger.LookForPlayerState);
            }
        }
    }
}