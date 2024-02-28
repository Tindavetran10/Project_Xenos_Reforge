using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Enemy.EnemyState.SubState
{
    public class MeleeAttackState : BattleState
    {
        

        private readonly D_MeleeAttackState _stateData;
        
        protected MeleeAttackState(global::Enemy.EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_MeleeAttackState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;

        public override void Enter()
        {
            base.Enter();
            
            // If the current value of comboCounter is higher the value of comboValue (the total number of combo)
            // or If the lastTimeAttacked (the time that an entity do a combo) is passed by the game's realtime
            // Reset the entity's combo to the start
            if (_comboCounter > _comboWindow || Time.time >= EnemyBase.lastTimeAttacked + _comboWindow)
            {
                EnemyBase.lastTimeAttacked = Time.time;
                _comboCounter = 0;
            }
            
            EnemyBase.Anim.SetInteger(ComboCounter, _comboCounter);
            EnemyBase.isAnimationFinished = false;
            
            _comboWindow = _stateData.comboTotal;
        }
        
        public override void Exit()
        {
            base.Exit();
            _comboCounter++;
        }
    }
}