using System;
using System.Linq;
using Enemy.EnemyStats;
using HitStop;
using InventorySystem_and_Items;
using Player.Data;
using Player.PlayerStates.SuperStates;
using UnityEngine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerPrimaryAttackState : PlayerAbilityState
    {
        #region Combo variables
        private bool _attackInputStop;
        private float _lastTimeAttacked;
        private static readonly int Counter = Animator.StringToHash("comboCounter");
        #endregion
        
        private HitStopController _hitStopController;
        
        public PlayerPrimaryAttackState(global::Player.PlayerStateMachine.Player player, global::Player.PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}
        
        public override void Enter()
        {
            base.Enter();
            
            _hitStopController = HitStopController.Instance;
            
            IsHolding = false;
            StartTime = Time.time;
            Player.inputManager.UseAttackInput();
            
            if (ComboCounter >= PlayerData.numberOfAttacks || Time.time >= _lastTimeAttacked + PlayerData.comboWindow)
                ComboCounter = 0;
            
            Player.Anim.SetInteger(Counter, ComboCounter);
        }

        public override void Exit()
        {
            base.Exit();

            ComboCounter++;
            _lastTimeAttacked = Time.time;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (_attackInputStop || Time.time >= StartTime + PlayerData.maxHoldTime)
                IsHolding = false;

            if (IsAnimationFinished || IsAnimationCancel)
                IsAbilityDone = true;
        }
        
        #region Animation Functions
        public override void AnimationCancelTrigger()
        {
            base.AnimationCancelTrigger();

            // Check if any input is pressed during the animation
            foreach (var combatInput in Enum.GetValues(typeof(CombatInputs)).Cast<CombatInputs>())
            {
                // If any input is pressed, cancel the animation
                if (Player.inputManager.NormalAttackInputs[(int)combatInput] ||
                    Player.inputManager.NormInputX == 1 || Player.inputManager.NormInputX == -1 ||
                    Player.inputManager.JumpInput ||
                    Player.inputManager.DashInput)
                {
                    IsAnimationCancel = true;
                    break; // Exit the loop if any condition is met
                }
            }
        }
        
        public override void StartMovementTrigger()
        {
            base.StartMovementTrigger();
            Movement.SetVelocity(PlayerData.attackVelocity[ComboCounter], PlayerData.direction[ComboCounter], Movement.FacingDirection);
        }

        public override void StopMovementTrigger()
        {
            base.StopMovementTrigger();
            Movement.SetVelocityZero();
        }

        public override void SetFlipActive()
        {
            base.SetFlipActive();
            Movement.CheckIfShouldFlip(Player.inputManager.NormInputX);
        }

        public override void SetFlipInactive()
        {
            base.SetFlipInactive();
            Movement.CheckIfShouldFlip(0);
        }
        #endregion
        
        public override void AttackTrigger()
        {
            base.AttackTrigger();

            var playerTransform = Player.attackPosition.transform;
            var playerPosition = playerTransform.position;
            var hitBoxCenter = PlayerData.hitBox[ComboCounter].center;
            var hitBoxSize = PlayerData.hitBox[ComboCounter].size;

            var offset = new Vector2(playerPosition.x + hitBoxCenter.x * Movement.FacingDirection,
                playerPosition.y + hitBoxCenter.y);
            
            var collider2Ds = Physics2D.OverlapBoxAll(offset, hitBoxSize, 0f, PlayerData.whatIsEnemy); 

            foreach (var hit in collider2Ds) ProcessHit(hit);
        }

        private void ProcessHit(Component hit)
        {
            var enemyComponent = hit.GetComponent<Enemy.EnemyStateMachine.Enemy>();
            if(enemyComponent == null) return;
            
            Entity.Entity.HitParticle(hit, PlayerData.hitParticle);
            
            var target = hit.GetComponentInChildren<EnemyStats>();
            if(target!=null)
            {
                Player.Stats.DoDamage(target);

                ExecuteWeaponEffect(target);

                _hitStopController.HitStop(PlayerData.hitStopDuration);
            }
        }

        private static void ExecuteWeaponEffect(EnemyStats target)
        {
            var weaponData = InventoryManager.instance.GetEquipment(EnumList.EquipmentType.Weapon);
            if (weaponData != null)
                weaponData.ExecuteItemEffect(target.transform);
        }
    }
}