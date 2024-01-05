using _Scripts.Player.Input;
using Script.CoreSystem.CoreComponents;
using Script.Player.Data;
using Script.Player.PlayerStateMachine;
using UnityEngine;

namespace Script.Player.PlayerStates.SubStates
{
    public class PlayerPrimaryAttackState : PlayerState
    {
        private int _comboCounter;

        private float _lastTimeAttacked;
        private const float comboWindow = 2;
        
        private Movement _movement;
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);

        private CollisionSenses _collisionSenses;
        private CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses 
            : Core.GetCoreComponent(ref _collisionSenses);

        public PlayerPrimaryAttackState(PlayerStateMachine.Player player, PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}
        
        public override void Enter()
        {
            base.Enter();

            if (_comboCounter > Player.InputHandler.AttackInputs.Length || Time.time >= _lastTimeAttacked + comboWindow)
                _comboCounter = 0;
            
            Player.Anim.SetInteger("comboCounter", _comboCounter);
            
            
        }

        public override void Exit()
        {
            base.Exit();

            _comboCounter++;
            _lastTimeAttacked = Time.time;
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
            if (Player.InputHandler.AttackInputs.Length > _comboCounter && Player.InputHandler.AttackInputs[_comboCounter] ||
                Player.InputHandler.NormInputX == 1 || Player.InputHandler.NormInputX == -1 ||
                Player.InputHandler.JumpInput ||
                Player.InputHandler.DashInput)
                IsAnimationCancel = true;
        }

        public override void StartMovementTrigger()
        {
            base.StartMovementTrigger();
            Movement.SetVelocity(PlayerData.movementVelocity, new Vector2(1,0) ,Movement.FacingDirection);
        }

        public override void StopMovementTrigger()
        {
            base.StopMovementTrigger();
            Movement.SetVelocityZero();
        }
    }
}