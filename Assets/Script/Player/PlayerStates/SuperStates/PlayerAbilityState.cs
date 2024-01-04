using _Scripts.CoreSystem.CoreComponents;
using Script.Player.Data;
using Script.Player.PlayerStateMachine;

namespace Script.Player.PlayerStates.SuperStates
{
    public class PlayerAbilityState : PlayerState {
        protected bool IsAbilityDone;
        
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
        private CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses 
            : Core.GetCoreComponent(ref _collisionSenses);

        private Movement _movement;
        private CollisionSenses _collisionSenses;

        private bool _isGrounded;
        
        protected PlayerAbilityState(PlayerStateMachine.Player player, PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName){}

        protected override void DoChecks() {
            base.DoChecks();

            if (CollisionSenses) 
                _isGrounded = CollisionSenses.Ground;
        }

        public override void Enter() {
            base.Enter();
            IsAbilityDone = false;
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
