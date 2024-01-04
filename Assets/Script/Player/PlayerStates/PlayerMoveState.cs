using Script.Player.Data;
using Script.Player.PlayerStateMachine;
using UnityEngine;

namespace Script.Player.PlayerStates
{
    public class PlayerMoveState : PlayerState
    {
        public PlayerMoveState(PlayerStateMachine.Player player, PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Enter Move State");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}