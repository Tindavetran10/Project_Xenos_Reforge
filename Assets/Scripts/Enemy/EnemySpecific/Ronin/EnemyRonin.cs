using Enemy.EnemyState.StateData;
using Enemy.Intermediaries;
using HitStop;
using UnityEngine;

namespace Enemy.EnemySpecific.Ronin
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
        private EnemyRoninStunState StunState { get; set; }
        public EnemyRoninGetAttackedState GetAttackedState { get; private set; }
        public EnemyRoninDeathState DeathState { get; private set; }
        #endregion
        
        #region Enemy Data
        [Header("State Data")]
        [SerializeField] protected D_IdleState idleStateData;
        [SerializeField] protected D_MoveState moveStateData;
        [SerializeField] protected D_PlayerDetectedState playerDetectedStateData;
        [SerializeField] protected D_ChargeState chargeStateData;
        [SerializeField] protected D_LookForPlayerState lookForPlayerStateData;
        [SerializeField] protected D_MeleeAttackState meleeAttackStateData;
        [SerializeField] protected D_StunState stunStateData;
        [SerializeField] protected D_GetAttacked getAttackedStateData;
        #endregion
        
        protected override void Awake()
        {
            base.Awake();
            IdleState = new EnemyRoninIdleState(this, StateMachine, "idle", idleStateData, this);
            MoveState = new EnemyRoninMoveState(this, StateMachine, "move", moveStateData, this);
            PlayerDetectedState = new EnemyRoninPlayerDetectedState(this, StateMachine, "playerDetected", playerDetectedStateData, this);
            ChargeState = new EnemyRoninChargeState(this, StateMachine, "charge", chargeStateData, this);
            LookForPlayerState = new EnemyRoninLookForPlayerState(this, StateMachine, "lookForPlayer", lookForPlayerStateData, this);
            MeleeAttackState = new EnemyRoninMeleeAttackState(this, StateMachine, "meleeAttack", meleeAttackStateData, this);
            StunState = new EnemyRoninStunState(this, StateMachine, "stun", stunStateData, this);
            GetAttackedState = new EnemyRoninGetAttackedState(this, StateMachine, "getAttacked", getAttackedStateData, this);
            DeathState = new EnemyRoninDeathState(this, StateMachine, "die", this);
        }
        
        protected override void Start()
        {
            base.Start();
            
            GetComponent<EnemyAnimationToStateMachine>(); 
            HitStopController = HitStopController.Instance;
            StateMachine.Initialize(IdleState);
        }

        // Only for testing purposes
        protected override void Update()
        {
            base.Update();
            if(Input.GetKeyDown(KeyCode.U))
                StateMachine.ChangeState(GetAttackedState);
        }

        public override bool CanBeStunned()
        {
            if (!base.CanBeStunned()) return false;
            StateMachine.ChangeState(StunState);
            return true;
        }

        public override void Die()
        {
            base.Die();
            StateMachine.ChangeState(DeathState);
        }

        public override void OnDrawGizmos()
        {
            if (Core != null)
            {
                var playerCheckPosition = attackPosition.position;

                Gizmos.DrawWireSphere(playerCheckPosition + (Vector3)(Vector2.right * enemyData.agroDistance * Movement.FacingDirection), 0.2f);
                Gizmos.DrawWireSphere(playerCheckPosition + (Vector3)(Vector2.right * enemyData.closeRangeActionDistance * Movement.FacingDirection), 0.2f);
            }
            
            foreach (var item in enemyData.hitBox) 
                Gizmos.DrawWireCube(attackPosition.transform.position + (Vector3)item.center, item.size);
        }

        
    }
}