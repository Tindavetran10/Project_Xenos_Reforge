using Script.Player.Data;
using Script.Player.PlayerStateMachine;
using Script.Player.PlayerStates.SuperStates;

namespace _Scripts.Player.PlayerStates.SubStates
{
    public class PlayerWallClimbState : PlayerTouchingWallState
    {
        public PlayerWallClimbState(Script.Player.PlayerStateMachine.Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : 
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
