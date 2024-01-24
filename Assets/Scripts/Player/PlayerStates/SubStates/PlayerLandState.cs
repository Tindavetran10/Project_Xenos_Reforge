using Player.Data;
using Player.PlayerStates.SuperStates;

namespace Player.PlayerStates.SubStates
{ 
    public class PlayerLandState : PlayerGroundedState
    {
        public PlayerLandState(global::Player.PlayerStateMachine.Player player, global::Player.PlayerStateMachine.PlayerStateMachine stateMachine, 
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
