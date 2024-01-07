using Script.CoreSystem;
using Script.CoreSystem.CoreComponents;
using Script.Enemy.Data;
using UnityEngine;

namespace Script.Enemy.EnemyStateMachine
{
    public class Enemy : Entity.Entity
    {
        #region Components
        public EnemyData enemyData;
        protected EnemyStateMachine StateMachine;
        
        private Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
        private Movement _movement;
        #endregion

        private Vector2 _velocityWorkspace;
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");


        protected override void Awake() {
            base.Awake();
            Core = GetComponentInChildren<Core>();
            StateMachine = new EnemyStateMachine();
        }

        protected override void Update() {
            base.Update();
            Core.LogicUpdate();
            StateMachine.CurrentState.LogicUpdate();

            Anim.SetFloat(YVelocity, Movement.Rb.velocity.y);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            StateMachine.CurrentState.PhysicsUpdate();
        }
        

        #region CheckFunctions and Draw Gizmos
        public bool CheckPlayerInAgroRange() => 
            Physics2D.Raycast(attackPosition.position, transform.right, enemyData.agroDistance, 
                enemyData.whatIsPlayer);
        
        public bool CheckPlayerInCloseRangeAction() => 
            Physics2D.Raycast(attackPosition.position, transform.right, enemyData.closeRangeActionDistance, enemyData.whatIsPlayer);

        public virtual void OnDrawGizmos()
        {
            if (Core != null)
            {
                var playerCheckPosition = attackPosition.position;
                
                Gizmos.DrawWireSphere(playerCheckPosition + (Vector3)(Vector2.right * enemyData.closeRangeActionDistance), 0.2f);
                Gizmos.DrawWireSphere(playerCheckPosition + (Vector3)(Vector2.right * enemyData.agroDistance), 0.2f);
            }
        }
        #endregion
    }
}