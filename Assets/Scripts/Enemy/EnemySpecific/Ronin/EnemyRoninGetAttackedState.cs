using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SubState;

namespace Enemy.EnemySpecific.Ronin
{
    public class EnemyRoninGetAttackedState : GetAttackedState
    {
        private readonly EnemyRonin _enemyRonin;

        public EnemyRoninGetAttackedState(EnemyStateMachine.Enemy enemyBase, 
            EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_GetAttacked stateData, 
            EnemyRonin enemyRonin) : base(enemyBase, stateMachine, animBoolName, stateData) =>
            _enemyRonin = enemyRonin;


        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsGetAttackedTimeOver)
            {
                if(PerformCloseRangeAction)
                    StateMachine.ChangeState(_enemyRonin.MeleeAttackState);
                else if(PerformLongRangeAction)
                    StateMachine.ChangeState(_enemyRonin.ChargeState);
                else if(EnemyBase.Stats.IsDead)
                    StateMachine.ChangeState(_enemyRonin.DeathState);
                else
                {
                    _enemyRonin.LookForPlayerState.SetTurnImmediately(true);
                    StateMachine.ChangeState(_enemyRonin.LookForPlayerState);
                }
                
            }
            
        }
    }
}