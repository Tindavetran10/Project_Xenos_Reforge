using Player.Data;
using Player.PlayerStates.SuperStates;

namespace Player.PlayerStates.SubStates
{
    public class PlayerWallClimbState : PlayerTouchingWallState
    {
        public PlayerWallClimbState(global::Player.PlayerStateMachine.Player player, global::Player.PlayerStateMachine.PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : 
            base(player, stateMachine, playerData, animBoolName) {}


        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (!IsExitingState) {
                Movement?.SetVelocityY(PlayerData.wallClimbVelocity);

                if (YInput != 1) {
                    StateMachine.ChangeState(Player.WallGrabState);
                }
            }
        }
    }
}
