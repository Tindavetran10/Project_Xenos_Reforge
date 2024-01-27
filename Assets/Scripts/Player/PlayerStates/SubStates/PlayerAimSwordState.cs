using Manager;
using Player.Data;
using Player.PlayerStates.SuperStates;
using UnityEngine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerAimSwordState : PlayerAbilityState
    {
        private bool _aimSwordInputStop;
        
        public PlayerAimSwordState(global::Player.PlayerStateMachine.Player player, global::Player.PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}
        
        public override void Enter()
        {
            base.Enter();
            IsHolding = false;
            StartTime = Time.time;

            Player.InputHandler.UseAimSwordInput();
            _aimSwordInputStop = Player.InputHandler.AimSwordInputStop;
            
            Movement?.SetVelocityZero();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (_aimSwordInputStop || Time.time >= StartTime + PlayerData.maxHoldTime)
                IsHolding = false;

            if (IsAnimationFinished)
                IsAbilityDone = true;
        }
        
        public override void ThrowSlash()
        {
            if(SkillManager.Instance.Slash.slashUnlocked)
                SkillManager.Instance.Slash.CreateSlash(Movement.FacingDirection);
        }
    }
}