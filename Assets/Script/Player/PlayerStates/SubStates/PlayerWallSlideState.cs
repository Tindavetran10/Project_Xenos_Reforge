using Script.Player.Data;
using Script.Player.PlayerStates.SuperStates;

namespace Script.Player.PlayerStates.SubStates
{
    public class PlayerWallSlideState : PlayerTouchingWallState
    {
        public PlayerWallSlideState(PlayerStateMachine.Player player, PlayerStateMachine.PlayerStateMachine stateMachine, PlayerData playerData, 
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
