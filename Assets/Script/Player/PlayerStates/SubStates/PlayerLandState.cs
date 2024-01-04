using Script.Player.Data;
using Script.Player.PlayerStates.SuperStates;

namespace Script.Player.PlayerStates.SubStates
{ 
    public class PlayerLandState : PlayerGroundedState
    {
        public PlayerLandState(PlayerStateMachine.Player player, PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!IsExitingState)
            {
                if(XInput!=0)
                    StateMachine.ChangeState(Player.MoveState);
                else if (YInput == -1)
                    StateMachine.ChangeState(Player.CrouchIdleState);
                else if (IsAnimationFinished) 
                    StateMachine.ChangeState(Player.IdleState);
            }
        }
    }
}
