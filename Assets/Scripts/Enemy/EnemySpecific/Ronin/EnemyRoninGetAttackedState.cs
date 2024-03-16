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
            if(!PerformLongRangeAction) 
                StateMachine.ChangeState(_enemyRonin.LookForPlayerState);
            else if(PerformLongRangeAction)
                StateMachine.ChangeState(_enemyRonin.PlayerDetectedState);
            else StateMachine.ChangeState(_enemyRonin.IdleState);
            
        }
    }
}