using Script.CoreSystem;
using Script.CoreSystem.CoreComponents;
using UnityEngine;

namespace Script.Enemy.EnemyStateMachine
{
    public class EnemyState
    {
        protected readonly Core Core;
        protected readonly Enemy EnemyBase;
        protected readonly EnemyStateMachine StateMachine;
        
        protected float StartTime { get; private set; }
        
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
        private Movement _movement;

        protected CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses 
            : Core.GetCoreComponent(ref _collisionSenses);
        private CollisionSenses _collisionSenses;

        
        private readonly string _animBoolName;
        protected EnemyState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName)
        {
            EnemyBase = enemyBase;
            StateMachine = stateMachine;
            _animBoolName = animBoolName;
            Core = enemyBase.Core;
        }

        public virtual void Enter()
        {
            DoChecks();
            EnemyBase.Anim.SetBool(_animBoolName, true);
            StartTime = Time.time;
        }
        
        public virtual void Exit() => 
            EnemyBase.Anim.SetBool(_animBoolName, false);

        public virtual void LogicUpdate(){}
        public void PhysicsUpdate() => DoChecks();

        protected virtual void DoChecks(){}
    }
}