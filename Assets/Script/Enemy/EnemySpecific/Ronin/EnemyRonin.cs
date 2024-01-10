using Script.Enemy.EnemyState.StateData;
using UnityEngine;

namespace Script.Enemy.EnemySpecific.Ronin
{
    public class EnemyRonin : EnemyStateMachine.Enemy
    {
        #region State Variables
        public EnemyRoninIdleState IdleState { get; private set; }
        public EnemyRoninMoveState MoveState { get; private set; }
        public EnemyRoninPlayerDetectedState PlayerDetectedState { get; private set; }
        public EnemyRoninChargeState ChargeState { get; private set; }
        public EnemyRoninLookForPlayerState LookForPlayerState { get; private set; }
        public EnemyRoninMeleeAttackState MeleeAttackState { get; private set; }
        public EnemyRoninStunState StunState { get; private set; }
        public EnemyRoninDeathState DeathState { get; private set; }
        #endregion
        
        protected override void Awake()
        {
            base.Awake();
            IdleState = new EnemyRoninIdleState(this, StateMachine, "idle", idleStateData, this);
            MoveState = new EnemyRoninMoveState(this, StateMachine, "move", moveStateData, this);
            PlayerDetectedState = new EnemyRoninPlayerDetectedState(this, StateMachine, "playerDetected", playerDetectedStateData, this);
            ChargeState = new EnemyRoninChargeState(this, StateMachine, "charge", chargeStateData, this);
            LookForPlayerState =
                new EnemyRoninLookForPlayerState(this, StateMachine, "lookForPlayer", lookForPlayerStateData, this);
            MeleeAttackState =
                new EnemyRoninMeleeAttackState(this, StateMachine, "meleeAttack", meleeAttackStateData, this);
            StunState = new EnemyRoninStunState(this, StateMachine, "stun", stunStateData, this);
            DeathState = new EnemyRoninDeathState(this, StateMachine, "die", this);
        }
        

        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(IdleState);
        }

        protected override void Update()
        {
            base.Update();
            if(Input.GetKeyDown(KeyCode.U))
                StateMachine.ChangeState(StunState);
        }

        public override bool CanBeStunned()
        {
            if (base.CanBeStunned())
            {
                StateMachine.ChangeState(StunState);
                return true;
            }
            return false;
        }

        public override void Die()
        {
            base.Die();
            StateMachine.ChangeState(DeathState);
        }

        public override void OnDrawGizmos()
        {
            foreach (var item in enemyData.hitBox) 
                Gizmos.DrawWireCube(attackPosition.transform.position + (Vector3)item.center, item.size);
        }
    }
}