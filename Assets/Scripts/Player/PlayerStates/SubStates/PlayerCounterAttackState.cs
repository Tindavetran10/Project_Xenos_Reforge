using Controller;
using Player.Data;
using Player.PlayerStates.SuperStates;
using UnityEngine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerCounterAttackState : PlayerAbilityState
    {
        private bool _counterInputStop;
        private static readonly int CounterAttack = Animator.StringToHash("successfulCounterAttack");
        
        private readonly Collider2D[] _results = new Collider2D[5];
        
        public PlayerCounterAttackState(global::Player.PlayerStateMachine.Player player, 
            global::Player.PlayerStateMachine.PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
            : base(player, stateMachine, playerData, animBoolName) {}

        public override void Enter()
        {
            base.Enter();
            IsHolding = true;
            StartTime = Time.time;
            Player.inputManager.UseCounterInput();

            _counterInputStop = Player.inputManager.CounterInputStop;
            Player.Anim.SetBool(CounterAttack, false);
            Movement?.SetVelocityX(0f);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsHolding)
            {
                var playerTransform = Player.attackPosition.transform;
                var playerPosition = playerTransform.position;

                Offset.Set(playerPosition.x + PlayerData.hitBox[ComboCounter].center.x * Movement.FacingDirection,
                    playerPosition.y + PlayerData.hitBox[ComboCounter].center.y);
            
                var numColliders = Physics2D.OverlapBoxNonAlloc(Offset, PlayerData.hitBox[ComboCounter].size, 0f, _results);

                for(var i = 0; i < numColliders; i++)
                {
                    var hit = _results[i];
                    if (hit == null) continue;
                    
                    var projectileController = hit.GetComponent<ProjectileController>();
                    if (projectileController != null)
                    {
                        projectileController.FlipProjectile();
                        SuccessfulCounterAttack();
                    }
                    
                    var enemy = hit.GetComponent<Enemy.EnemyStateMachine.Enemy>();
                    if (enemy != null)
                    {
                        if(enemy.TryCloseCounterAttackWindow())
                        {
                            SuccessfulCounterAttack();
                            
                            Player.Skill.Parry.UseSkill();
                            Player.Skill.Parry.MakeMirageOnParry(hit.transform);
                        }
                    }
                }
                if(_counterInputStop || Time.time >= StartTime + PlayerData.counterAttackDuration) 
                    IsHolding = false;
            }
            else StateMachine.ChangeState(Player.IdleState);
        }

        private void SuccessfulCounterAttack() => Player.Anim.SetBool(CounterAttack, true);
    }
}