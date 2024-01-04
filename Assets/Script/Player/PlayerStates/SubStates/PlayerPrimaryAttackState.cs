using _Scripts.Player.Input;
using Script.Player.Data;
using Script.Player.PlayerStateMachine;
using UnityEngine;

namespace Script.Player.PlayerStates.SubStates
{
    public class PlayerPrimaryAttackState : PlayerState
    {
        private int comboCounter;

        private float lastTimeAttacked;
        private float comboWindow = 2;
        public PlayerPrimaryAttackState(PlayerStateMachine.Player player, PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}
        
        public override void Enter()
        {
            base.Enter();

            if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
                comboCounter = 0;
            
            Player.Anim.SetInteger("comboCounter", comboCounter);
            
            Debug.Log(comboCounter);
        }

        public override void Exit()
        {
            base.Exit();

            comboCounter++;
            lastTimeAttacked = Time.time;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if(IsAnimationFinished || IsAnimationCancel)
                StateMachine.ChangeState(Player.IdleState);
            
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        protected override void DoChecks()
        {
            base.DoChecks();
        }

        public override void AnimationCancelTrigger()
        {
            base.AnimationCancelTrigger();
            if (Player.InputHandler.AttackInputs[comboCounter] ||
                Player.InputHandler.NormInputX == 1 || Player.InputHandler.NormInputX == -1 ||
                Player.InputHandler.JumpInput ||
                Player.InputHandler.DashInput)
                IsAnimationCancel = true;
        }
    }
}