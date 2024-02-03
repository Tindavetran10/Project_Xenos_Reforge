using Enemy.EnemyState.StateData;
using Scripts.Enemy.EnemyState.SubState;

namespace Enemy.EnemySpecific.Ronin
{
    public class EnemyRoninIdleState : IdleState
    {
        private readonly EnemyRonin _enemyRonin;

        public EnemyRoninIdleState(EnemyStateMachine.Enemy enemyBase, 
            Scripts.Enemy.EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_IdleState enemyData, 
            EnemyRonin enemyRonin) : base(enemyBase, stateMachine, animBoolName, enemyData) =>
            _enemyRonin = enemyRonin;

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(IsIdleTimeOver)
                StateMachine.ChangeState(_enemyRonin.MoveState);
        }
    }
}