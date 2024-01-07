using _Scripts.Enemies.EnemyState.StateData;
using Script.Enemy.EnemyState.State_Data;
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
        #endregion
        
        #region Enemy Data
        [SerializeField] private D_IdleState idleStateData;
        [SerializeField] private D_MoveState moveStateData;
        [SerializeField] private D_PlayerDetectedState playerDetectedStateData;
        [SerializeField] private D_ChargeState chargeStateData;
        [SerializeField] private D_LookForPlayerState lookForPlayerStateData;
        [SerializeField] private D_MeleeAttackState meleeAttackStateData;
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
        }
        

        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(IdleState);
        }

        public override void OnDrawGizmos()
        {
            foreach (var item in enemyData.hitBox) 
                Gizmos.DrawWireCube(attackPosition.transform.position + (Vector3)item.center, item.size);
        }
    }
}