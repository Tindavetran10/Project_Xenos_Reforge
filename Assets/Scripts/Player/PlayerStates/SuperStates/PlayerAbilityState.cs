using Controller;
using Player.Data;
using Player.PlayerStateMachine;
using UnityEngine;

namespace Player.PlayerStates.SuperStates
{
    public class PlayerAbilityState : PlayerState {
        
        protected Vector2 Offset;
        protected int ComboCounter;
        
        private bool _isGrounded;
        protected bool IsAbilityDone;
        
        protected bool IsHolding;
        
        protected GhostTrailController GhostTrailController;
        
        protected PlayerAbilityState(global::Player.PlayerStateMachine.Player player, global::Player.PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName){}

        protected override void DoChecks() {
            base.DoChecks();

            if (CollisionSenses) 
                _isGrounded = CollisionSenses.Ground;
        }

        public override void Enter() {
            if(Player.isActiveAndEnabled == false) return;
            
            base.Enter();
            IsAbilityDone = false;
            
            GhostTrailController = Player.GhostTrailController;
            GhostTrailController.enabled = false;
        }

        public override void LogicUpdate() {
            base.LogicUpdate();
            
            if (IsAbilityDone)
            {
                if (_isGrounded && Movement != null && Movement.CurrentVelocity.y < 0.01f)
                    StateMachine.ChangeState(Player.IdleState);
                else StateMachine.ChangeState(Player.InAirState);
            }
        }
    }
}
