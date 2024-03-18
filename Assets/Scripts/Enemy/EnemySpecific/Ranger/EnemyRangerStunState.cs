using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SubState;

namespace Enemy.EnemySpecific.Ranger
{
    public class EnemyRangerStunState : StunState
    {
        private readonly EnemyRanger _enemyRanger;

        public EnemyRangerStunState(EnemyStateMachine.Enemy enemyBase, 
            EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_StunState stateData, 
            EnemyRanger enemyRanger) : base(enemyBase, stateMachine, animBoolName, stateData) =>
            _enemyRanger = enemyRanger;
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsStunTimeOver)
            {
                if(PerformLongRangeAction)
                    StateMachine.ChangeState(_enemyRanger.PlayerDetectedState);
                else if(EnemyBase.Stats.IsDead)
                    StateMachine.ChangeState(_enemyRanger.DeathState);
                StateMachine.ChangeState(_enemyRanger.LookForPlayerState);
            }
        }
    }
}