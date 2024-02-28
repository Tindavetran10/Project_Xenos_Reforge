using Player.Data;
using Player.PlayerStates.SuperStates;
using UnityEngine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerFocusSwordState : PlayerAbilityState
    {
        private bool _focusSwordInputStop;
        
        protected PlayerFocusSwordState(Player.PlayerStateMachine.Player player, PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}

        public override void Enter()
        {
            base.Enter();
            IsHolding = false;
            StartTime = Time.time;
            
            Player.InputHandler.UseFocusSwordInput();
            _focusSwordInputStop = Player.InputHandler.FocusSwordInput;
            
            Movement?.SetVelocityZero();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (_focusSwordInputStop)
                IsHolding = false;
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
