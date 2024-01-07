using Script.Enemy.EnemyState.State_Data;
using Script.Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Script.Enemy.EnemyState.SubState
{
    public class MeleeAttackState : BattleState
    {
        protected readonly D_MeleeAttackState StateData;
        
        protected MeleeAttackState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_MeleeAttackState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            StateData = stateData;

        public override void Enter()
        {
            base.Enter();
            ComboWindow = StateData.comboTotal;
        }
    }
}