using CoreSystem;
using CoreSystem.CoreComponents;
using UnityEngine;

namespace Scripts.Enemy.EnemyStateMachine
{
    public class EnemyState
    {
        private readonly Core _core;
        protected readonly global::Enemy.EnemyStateMachine.Enemy EnemyBase;
        protected readonly EnemyStateMachine StateMachine;

        public float StartTime { get; private set; }
        
        protected Movement Movement => _movement ? _movement : _core.GetCoreComponent(ref _movement);
        private Movement _movement;

        protected CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses 
            : _core.GetCoreComponent(ref _collisionSenses);
        private CollisionSenses _collisionSenses;

        
        private readonly string _animBoolName;
        protected EnemyState(global::Enemy.EnemyStateMachine.Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName)
        {
            EnemyBase = enemyBase;
            StateMachine = stateMachine;
            _animBoolName = animBoolName;
            _core = enemyBase.Core;
        }

        public virtual void Enter()
        {
            DoChecks();
            EnemyBase.Anim.SetBool(_animBoolName, true);
            StartTime = Time.time;
        }
        
        
        public virtual void Exit() => EnemyBase.Anim.SetBool(_animBoolName, false);

        public virtual void LogicUpdate(){}
        public void PhysicsUpdate() => DoChecks();

        protected virtual void DoChecks(){}
    }
}