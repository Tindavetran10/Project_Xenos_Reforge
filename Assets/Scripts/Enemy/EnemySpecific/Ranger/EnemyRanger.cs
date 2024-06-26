using Enemy.EnemyState.StateData;
using HitStop;
using UnityEngine;

namespace Enemy.EnemySpecific.Ranger
{
    public class EnemyRanger : EnemyStateMachine.Enemy
    {
        #region State Variables
        public EnemyRangerIdleState IdleState { get; private set; }
        public EnemyRangerMoveState MoveState { get; private set; }
        public EnemyRangerPlayerDetectedState PlayerDetectedState { get; private set; }
        public EnemyRangerLookForPlayerState LookForPlayerState { get; private set; }
        private EnemyRangerStunState StunState { get; set; }
        public EnemyRangerDeathState DeathState { get; private set; }
        public EnemyRangerRangedAttackState RangedAttackState { get; private set; }
        public EnemyRangerDodgeState DodgeState { get; private set; }
        public EnemyRangerGetAttackedState GetAttackedState { get; private set; }
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
        [SerializeField] public D_GetAttacked getAttackedStateData;
        #endregion
        
        protected override void Awake()
        {
            base.Awake();
            IdleState = new EnemyRangerIdleState(this, StateMachine, "idle", idleStateData, this);
            MoveState = new EnemyRangerMoveState(this, StateMachine, "move", moveStateData, this);
            PlayerDetectedState = new EnemyRangerPlayerDetectedState(this, StateMachine, "playerDetected", playerDetectedStateData, this);
            LookForPlayerState = new EnemyRangerLookForPlayerState(this, StateMachine, "lookForPlayer", lookForPlayerStateData, this);
            StunState = new EnemyRangerStunState(this, StateMachine, "stun", stunStateData, this);
            DeathState = new EnemyRangerDeathState(this, StateMachine, "die", this);
            RangedAttackState = new EnemyRangerRangedAttackState(this, StateMachine, "shoot", rangedAttackStateData, this);
            DodgeState = new EnemyRangerDodgeState(this, StateMachine, "dodge", dodgeStateData, this);
            GetAttackedState = new EnemyRangerGetAttackedState(this, StateMachine, "getAttacked", getAttackedStateData, this);
        }
        
        protected override void Start()
        {
            base.Start();
            HitStopController = HitStopController.Instance;
            StateMachine.Initialize(IdleState);
        }
        
        public override bool ChangeStunState()
        {
            if (!base.ChangeStunState()) return false;
            StateMachine.ChangeState(StunState);
            return true;
        }
        
        public override bool ChangeGetAttackedState()
        {
            if (!base.ChangeGetAttackedState()) return false;
            StateMachine.ChangeState(GetAttackedState);
            return true;
        }
        
        /*public override void Die()
        {
            base.Die();
            StateMachine.ChangeState(DeathState);
        }*/
        
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
