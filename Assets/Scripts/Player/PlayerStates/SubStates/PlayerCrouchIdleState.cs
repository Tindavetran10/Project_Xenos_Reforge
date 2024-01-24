using Player.Data;
using Player.PlayerStates.SuperStates;

namespace Player.PlayerStates.SubStates
{
    public class PlayerCrouchIdleState : PlayerGroundedState
    {
        public PlayerCrouchIdleState(global::Player.PlayerStateMachine.Player player, global::Player.PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityZero();
            Player.SetColliderHeight(PlayerData.crouchColliderHeight);
        }

        public override void Exit()
        {
            base.Exit();
            Player.SetColliderHeight(PlayerData.standColliderHeight);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!IsExitingState)
            {
                if (XInput != 0)
                    StateMachine.ChangeState(Player.CrouchMoveState);
                else if (YInput != -1 && !IsTouchingCeiling) 
                    StateMachine.ChangeState(Player.IdleState);
            }
        }
    }

}