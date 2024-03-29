using Player.Data;
using Player.PlayerStates.SuperStates;
using Projectile;
using UnityEngine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerCounterAttackState : PlayerAbilityState
    {
        private bool _counterInputStop;
        private static readonly int CounterAttack = Animator.StringToHash("successfulCounterAttack");
        
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
            
                var collider2Ds = Physics2D.OverlapBoxAll(Offset, PlayerData.hitBox[ComboCounter].size, 0f);

                foreach (var hit in collider2Ds)
                {
                    if (hit.GetComponent<ProjectileController>() != null)
                    {
                        hit.GetComponent<ProjectileController>().FlipProjectile();
                        SuccessfulCounterAttack();
                    }
                    
                    if (hit.GetComponent<Enemy.EnemyStateMachine.Enemy>() != null)
                    {
                        if(hit.GetComponent<Enemy.EnemyStateMachine.Enemy>().TryCloseCounterAttackWindow())
                            SuccessfulCounterAttack(); 
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