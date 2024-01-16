using Script.Enemy.EnemyState.StateData;
using UnityEngine;

namespace Script.Enemy.EnemySpecific.Ranger
{
    public class EnemyRanger : EnemyStateMachine.Enemy
    {
        #region State Variables
        public EnemyRangerIdleState IdleState { get; private set; }
        public EnemyRangerMoveState MoveState { get; private set; }
        public EnemyRangerPlayerDetectedState PlayerDetectedState { get; private set; }
        public EnemyRangerLookForPlayerState LookForPlayerState { get; private set; }
        private EnemyRangerStunState StunState { get; set; }
        private EnemyRangerDeathState DeathState { get; set; }
        public EnemyRangerRangedAttackState RangedAttackState { get; private set; }
        public EnemyRangerDodgeState DodgeState { get; private set; }
        #endregion
        
        #region Enemy Data
        [Header("State Data")]
        [SerializeField] protected D_IdleState idleStateData;
        [SerializeField] protected D_MoveState moveStateData;
        [SerializeField] protected D_PlayerDetectedState playerDetectedStateData;
        [SerializeField] protected D_LookForPlayerState lookForPlayerStateData;
        [SerializeField] protected D_StunState stunStateData;
        [SerializeField] protected D_RangedAttackState rangedAttackStateData;
        [SerializeField] public D_DodgeState dodgeStateData;
        #endregion
        
        
        protected override void Awake()
        {
            base.Awake();
            IdleState = new EnemyRangerIdleState(this, StateMachine, "idle", idleStateData, this);
            MoveState = new EnemyRangerMoveState(this, StateMachine, "move", moveStateData, this);
            PlayerDetectedState = new EnemyRangerPlayerDetectedState(this, StateMachine, "playerDetected", playerDetectedStateData, this);
            LookForPlayerState =
                new EnemyRangerLookForPlayerState(this, StateMachine, "lookForPlayer", lookForPlayerStateData, this);
            StunState = new EnemyRangerStunState(this, StateMachine, "stun", stunStateData, this);
            DeathState = new EnemyRangerDeathState(this, StateMachine, "die", this);
            RangedAttackState = new EnemyRangerRangedAttackState(this, StateMachine, "shoot", rangedAttackStateData, this);
            DodgeState = new EnemyRangerDodgeState(this, StateMachine, "dodge", dodgeStateData, this);
        }
        
        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(IdleState);
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
            if (Core != null)
            {
                var playerCheckPosition = attackPosition.position;

                Gizmos.DrawWireSphere(playerCheckPosition + (Vector3)(Vector2.right * enemyData.agroDistance * Movement.FacingDirection), 0.2f);
                Gizmos.DrawWireSphere(playerCheckPosition + (Vector3)(Vector2.right * enemyData.closeRangeActionDistance * Movement.FacingDirection), 0.2f);
            }
        }
    }
}
