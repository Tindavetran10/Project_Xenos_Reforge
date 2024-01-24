using Player.Data;
using Player.PlayerStates.SuperStates;
using UnityEngine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerWallGrabState : PlayerTouchingWallState
    {
        private Vector2 _holdPosition;
        public PlayerWallGrabState(global::Player.PlayerStateMachine.Player player, global::Player.PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}


        public override void Enter()
        {
            _holdPosition = Player.transform.position;
            HoldPosition();
        }

        public override void Exit() {}

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (!IsExitingState) {
                HoldPosition();

                if (YInput > 0) {
                    StateMachine.ChangeState(Player.WallClimbState);
                } else if (YInput < 0 || !GrabInput) {
                    StateMachine.ChangeState(Player.WallSlideState);
                }
            }
        }

        private void HoldPosition()
        {
            Player.transform.position = _holdPosition;
            Movement?.SetVelocityX(0f);
            Movement?.SetVelocityY(0f);
        }

        public override void PhysicsUpdate() {}
        
        public override void AnimationTrigger() {}

        public override void AnimationFinishTrigger() {}
    }
}
