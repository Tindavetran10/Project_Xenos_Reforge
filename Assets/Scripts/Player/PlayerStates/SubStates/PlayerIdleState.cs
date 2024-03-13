using Player.Data;
using Player.PlayerStates.SuperStates;

namespace Player.PlayerStates.SubStates
{
    public class PlayerIdleState : PlayerGroundedState {
        public PlayerIdleState(global::Player.PlayerStateMachine.Player player, global::Player.PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}
        
        public override void Enter() {
            base.Enter();
            GhostTrailController.enabled = false;
            Movement?.SetVelocityX(0f);
        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            if (!IsExitingState)
            {
                if (XInput != 0)
                    StateMachine.ChangeState(Player.MoveState);
                else if (YInput == -1)
                    StateMachine.ChangeState(Player.CrouchIdleState);
            }
        }
    }
}