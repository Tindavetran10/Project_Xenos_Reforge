using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SubState;
using UnityEngine;

namespace Enemy.EnemySpecific.Ranger
{
    public class EnemyRangerPlayerDetectedState : PlayerDetectedState
    {
        private readonly EnemyRanger _enemyRanger;

        public EnemyRangerPlayerDetectedState(EnemyStateMachine.Enemy enemyBase, 
            EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData, 
            EnemyRanger enemyRanger) : base(enemyBase, stateMachine, animBoolName, stateData) =>
            _enemyRanger = enemyRanger;
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(PerformCloseRangeAction)
            {
                if(Time.time > _enemyRanger.DodgeState.StartTime + _enemyRanger.dodgeStateData.dodgeCooldown)
                    StateMachine.ChangeState(_enemyRanger.DodgeState);
                else if(!CannotAttack()) StateMachine.ChangeState(_enemyRanger.RangedAttackState);
            }
            else switch (PerformLongRangeAction)
            {
                case true when !CannotAttack():
                    StateMachine.ChangeState(_enemyRanger.RangedAttackState); break;
                case false: StateMachine.ChangeState(_enemyRanger.LookForPlayerState); break;
            }
        }

        private bool CannotAttack()
        {
            if (Time.time > _enemyRanger.lastTimeAttacked + _enemyRanger.attackCoolDown)
            {
                _enemyRanger.lastTimeAttacked = Time.time;
                return false;
            }
            return true;
        }
    }
}