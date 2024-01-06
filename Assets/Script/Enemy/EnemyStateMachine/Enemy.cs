using System;
using Script.CoreSystem;
using Script.CoreSystem.CoreComponents;
using Script.Enemy.Data;
using UnityEngine;

namespace Script.Enemy.EnemyStateMachine
{
    public class Enemy : Entity.Entity
    {
        #region Components
        private Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
        private Movement _movement;
        
        protected EnemyStateMachine StateMachine;
        public EnemyData enemyData;
        #endregion

        private Vector2 _velocityWorkspace;
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");

        [SerializeField] private Transform playerCheck;

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
            Debug.Log(CheckPlayerInAgroRange());
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            StateMachine.CurrentState.PhysicsUpdate();
        }
        

        #region CheckFunctions
        public bool CheckPlayerInAgroRange() => 
            Physics2D.Raycast(playerCheck.position, transform.right, enemyData.agroDistance, 
                enemyData.whatIsPlayer);
        
        public bool CheckPlayerInCloseRangeAction() => 
            Physics2D.Raycast(playerCheck.position, transform.right, enemyData.closeRangeActionDistance, enemyData.whatIsPlayer);

        public void OnDrawGizmos()
        {
            if (Core != null)
            {
                var playerCheckPosition = playerCheck.position;
                
                Gizmos.DrawWireSphere(playerCheckPosition + (Vector3)(Vector2.right * enemyData.closeRangeActionDistance), 0.2f);
                Gizmos.DrawWireSphere(playerCheckPosition + (Vector3)(Vector2.right * enemyData.agroDistance), 0.2f);
            }
        }
        #endregion
    }
}