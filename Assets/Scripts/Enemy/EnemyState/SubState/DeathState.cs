using Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Enemy.EnemyState.SubState
{
    public class DeathState : GroundedState
    {
        protected DeathState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName) : base(enemyBase, stateMachine, animBoolName) {}

        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityZero();
            
            // Change the rb to static so the enemy doesn't move
            Movement.Rb.bodyType = RigidbodyType2D.Static;
        }
    }
}