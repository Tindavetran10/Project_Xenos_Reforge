using Script.Player.Data;
using Script.Player.PlayerStateMachine;
using UnityEngine;

namespace Script.Player.PlayerStates.SubStates
{
    public class PlayerDeathState : PlayerState
    {
        public PlayerDeathState(PlayerStateMachine.Player player, PlayerStateMachine.PlayerStateMachine stateMachine, 
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