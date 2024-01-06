using _Scripts.Enemies.EnemyState.StateData;
using Script.Enemy.EnemyState.SubState;

namespace Script.Enemy.EnemySpecific.Ronin
{
    public class EnemyRoninLookForPlayerState : LookForPlayerState
    {
        private readonly EnemyRonin _enemyRonin;

        public EnemyRoninLookForPlayerState(EnemyStateMachine.Enemy enemyBase, 
            EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_LookForPlayerState stateData, 
            EnemyRonin enemyRonin) : base(enemyBase, stateMachine, animBoolName, stateData) =>
            _enemyRonin = enemyRonin;

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(IsPlayerInAgroRange)
                StateMachine.ChangeState(_enemyRonin.PlayerDetectedState);
            else if (IsAllTurnsTimeDone)
                StateMachine.ChangeState(_enemyRonin.MoveState);
        }
    }
}