using Script.Player.Data;
using Script.Player.PlayerStates.SuperStates;
using UnityEngine;

namespace Script.Player.PlayerStates.SubStates
{
    public class PlayerCounterAttackState : PlayerAbilityState
    {
        private static readonly int SuccessfulCounterAttack = Animator.StringToHash("successfulCounterAttack");

        protected PlayerCounterAttackState(PlayerStateMachine.Player player, 
            PlayerStateMachine.PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
            : base(player, stateMachine, playerData, animBoolName) {}

        protected override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();
            Player.Anim.SetBool(SuccessfulCounterAttack, false);
            
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            var playerTransform = Player.attackPosition.transform;
            var playerPosition = playerTransform.position;

            Offset.Set(playerPosition.x + PlayerData.hitBox[ComboCounter].center.x * Movement.FacingDirection,
                playerPosition.y + PlayerData.hitBox[ComboCounter].center.y);
            
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Offset, PlayerData.hitBox[ComboCounter].size, 0f, PlayerData.whatIsEnemy);

            foreach (var hit in collider2Ds)
            {
                if (hit.GetComponent<Enemy.EnemyStateMachine.Enemy>() != null)
                    hit.GetComponent<Enemy.EnemyStateMachine.Enemy>().Damage();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}