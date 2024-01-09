using Script.Player.Data;
using Script.Player.PlayerStates.SuperStates;
using UnityEngine;

namespace Script.Player.PlayerStates.SubStates
{
    public class PlayerCounterAttackState : PlayerAbilityState
    {
        private bool _counterInputStop;
        
        private static readonly int SuccessfulCounterAttack = Animator.StringToHash("successfulCounterAttack");

        public PlayerCounterAttackState(PlayerStateMachine.Player player, 
            PlayerStateMachine.PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
            : base(player, stateMachine, playerData, animBoolName) {}

        public override void Enter()
        {
            base.Enter();
            IsHolding = true;
            StartTime = Time.time;
            Player.InputHandler.UseCounterInput();

            _counterInputStop = Player.InputHandler.CounterInputStop;
            Player.Anim.SetBool(SuccessfulCounterAttack, false);
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
            
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Offset, PlayerData.hitBox[ComboCounter].size, 
                     0f, PlayerData.whatIsEnemy);

                foreach (var hit in collider2Ds)
                {
                    if (hit.GetComponent<Enemy.EnemyStateMachine.Enemy>() != null)
                    {
                        if(hit.GetComponent<Enemy.EnemyStateMachine.Enemy>().CanBeStunned())
                            Player.Anim.SetBool(SuccessfulCounterAttack, true); 
                    }
                }
                if(_counterInputStop || Time.time >= StartTime + PlayerData.counterAttackDuration) 
                    IsHolding = false;
            }
            else StateMachine.ChangeState(Player.IdleState);
        }
    }
}