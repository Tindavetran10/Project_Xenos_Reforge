using Player.Data;
using Player.PlayerStateMachine;
using UnityEngine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerDeathState : PlayerState
    {
        public PlayerDeathState(global::Player.PlayerStateMachine.Player player, global::Player.PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}

        public override void Enter()
        {
            base.Enter();
            GameObject.Find("Canvas").GetComponent<UI.UI>().SwitchOnEndScreen();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            Movement?.SetVelocityZero();
        }
        
    }
}