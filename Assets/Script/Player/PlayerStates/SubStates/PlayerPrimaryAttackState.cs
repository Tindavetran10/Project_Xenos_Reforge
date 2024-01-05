using System;
using System.Linq;
using Script.CoreSystem.CoreComponents;
using Script.Player.Data;
using Script.Player.PlayerStateMachine;
using UnityEngine;

namespace Script.Player.PlayerStates.SubStates
{
    public class PlayerPrimaryAttackState : PlayerState
    {
        #region Combo variables
        private int _comboCounter;

        private float _lastTimeAttacked;
        private static readonly int ComboCounter = Animator.StringToHash("comboCounter");
        #endregion

        private Movement _movement;
        private Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
        
        public PlayerPrimaryAttackState(PlayerStateMachine.Player player, PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}
        
        public override void Enter()
        {
            base.Enter();

            if (_comboCounter >= PlayerData.numberOfAttacks || Time.time >= _lastTimeAttacked + PlayerData.comboWindow)
                _comboCounter = 0;
            
            Debug.Log(_comboCounter);
            
            Player.Anim.SetInteger(ComboCounter, _comboCounter);
        }

        public override void Exit()
        {
            base.Exit();

            _comboCounter++;
            _lastTimeAttacked = Time.time;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if(IsAnimationFinished || IsAnimationCancel)
                StateMachine.ChangeState(Player.IdleState);
            
        }
        
        public override void AnimationCancelTrigger()
        {
            base.AnimationCancelTrigger();

            foreach (var combatInput in Enum.GetValues(typeof(CombatInputs)).Cast<CombatInputs>())
            {
                if (Player.InputHandler.AttackInputs[(int)combatInput] ||
                    Player.InputHandler.NormInputX == 1 || Player.InputHandler.NormInputX == -1 ||
                    Player.InputHandler.JumpInput ||
                    Player.InputHandler.DashInput)
                {
                    IsAnimationCancel = true;
                    break; // Exit the loop if any condition is met
                }
            }
        }


        public override void StartMovementTrigger()
        {
            base.StartMovementTrigger();
            Movement.SetVelocity(PlayerData.attackVelocity[_comboCounter], PlayerData.direction[_comboCounter], Movement.FacingDirection);
        }

        public override void StopMovementTrigger()
        {
            base.StopMovementTrigger();
            Movement.SetVelocityZero();
        }
        
        
    }
}