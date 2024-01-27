using Enemy.EnemyState.StateData;
using Scripts.Enemy.EnemyState.SubState;
using UnityEngine;

namespace Enemy.EnemySpecific.Ranger
{
    public class EnemyRangerPlayerDetectedState : PlayerDetectedState
    {
        private readonly EnemyRanger _enemyRanger;

        public EnemyRangerPlayerDetectedState(Scripts.Enemy.EnemyStateMachine.Enemy enemyBase, 
            Scripts.Enemy.EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData, 
            EnemyRanger enemyRanger) : base(enemyBase, stateMachine, animBoolName, stateData) =>
            _enemyRanger = enemyRanger;
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(PerformCloseRangeAction)
            {
                if(Time.time >= _enemyRanger.DodgeState.StartTime + _enemyRanger.dodgeStateData.dodgeCooldown)
                    StateMachine.ChangeState(_enemyRanger.DodgeState);
                else StateMachine.ChangeState(_enemyRanger.RangedAttackState);
            }
            else if(PerformLongRangeAction)
                StateMachine.ChangeState(_enemyRanger.RangedAttackState);
            else if (!IsPlayerInAgroRange) StateMachine.ChangeState(_enemyRanger.LookForPlayerState);
        }
    }
}