using Script.Enemy.EnemyState.StateData;
using Script.Enemy.EnemyState.SubState;

namespace Script.Enemy.EnemySpecific.Ronin
{
    public class EnemyRoninStunState : StunState
    {
        private readonly EnemyRonin _enemyRonin;

        public EnemyRoninStunState(EnemyStateMachine.Enemy enemyBase, 
            EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_StunState stateData, 
            EnemyRonin enemyRonin) : base(enemyBase, stateMachine, animBoolName, stateData) =>
            _enemyRonin = enemyRonin;
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsStunTimeOver)
            {
                if(PerformCloseRangeAction)
                    StateMachine.ChangeState(_enemyRonin.MeleeAttackState);
                else if (IsPlayerInAgroRange)
                    StateMachine.ChangeState(_enemyRonin.ChargeState);
                else
                {
                    _enemyRonin.LookForPlayerState.SetTurnImmediately(true);
                    StateMachine.ChangeState(_enemyRonin.LookForPlayerState);
                }
            }
        }
    }
}