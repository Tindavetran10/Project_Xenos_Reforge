using Player.Data;
using Player.PlayerStates.SuperStates;

namespace Player.PlayerStates.SubStates
{
    public class PlayerWallSlideState : PlayerTouchingWallState
    {
        public PlayerWallSlideState(global::Player.PlayerStateMachine.Player player, global::Player.PlayerStateMachine.PlayerStateMachine stateMachine, PlayerData playerData, 
            string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}


        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (!IsExitingState) {
                Movement?.SetVelocityY(-PlayerData.wallSlideVelocity);

                if (GrabInput && YInput == 0) 
                    StateMachine.ChangeState(Player.WallGrabState);
            }
        }
    }
}
