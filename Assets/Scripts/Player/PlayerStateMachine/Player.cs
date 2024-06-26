using _Scripts.Player.Input;
using Controller;
using InventorySystem_and_Items;
using Manager;
using Player.Data;
using Player.PlayerStates.SubStates;
using UnityEngine;

namespace Player.PlayerStateMachine
{
    public class Player : global::Entity.Entity
    {
        #region State Variables
        // By declaring the state machine, we can access all the function
        // including changing or initializing different states
        private PlayerStateMachine StateMachine { get; set; }
        
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerInAirState InAirState { get; private set; }
        public PlayerLandState LandState { get; private set; }
        public PlayerWallSlideState WallSlideState { get; private set; }
        public PlayerWallJumpState WallJumpState { get; private set; }
        public PlayerLedgeClimbState LedgeClimbState { get; private set; }
        public PlayerDashState DashState { get; private set; }
        public PlayerCrouchIdleState CrouchIdleState { get; private set; }
        public PlayerCrouchMoveState CrouchMoveState { get; private set; }
        public PlayerPrimaryAttackState PrimaryAttackState { get; private set; }
        public PlayerCounterAttackState CounterAttackState { get; private set; }
        private PlayerDeathState DeathState { get; set; }
        
        public PlayerAimSwordState AimSwordState { get; private set; }
        public PlayerFocusSwordState FocusSwordState { get; private set; }
        
        [SerializeField] private PlayerData playerData;
        #endregion

        #region Components
        //public PlayerInputHandler InputHandler { get; private set; }
        [SerializeField] public InputManager inputManager;
        
        [Space] public Transform playerTransform;
        
        public Transform DashDirectionIndicator { get; private set; }
        #endregion
        
        #region Others
        private Vector2 _workspace;
        public GhostTrailController GhostTrailController { get; private set; }
        public SkillManager Skill { get; private set; }
        #endregion

        #region Unity Callback Functions
        protected override void Awake()
        {
            base.Awake();
            
            StateMachine = new PlayerStateMachine();
        
            // Declare each state of the player, which take the required information from different State class
            IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
            MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
            JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
            InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
            LandState = new PlayerLandState(this, StateMachine, playerData, "land");
            WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
            WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
            LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
            DashState = new PlayerDashState(this, StateMachine, playerData, "dash");
            CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
            CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "crouchMove");

            PrimaryAttackState = new PlayerPrimaryAttackState(this, StateMachine, playerData, "attack");
            CounterAttackState = new PlayerCounterAttackState(this, StateMachine, playerData, "counterAttack");

            DeathState = new PlayerDeathState(this, StateMachine, playerData, "die");

            AimSwordState = new PlayerAimSwordState(this, StateMachine, playerData, "aimSword");
            FocusSwordState = new PlayerFocusSwordState(this, StateMachine, playerData, "focusSword");
        }
        
        protected override void Start()
        {
            base.Start();
            Skill = SkillManager.instance;
            
            //playerInputHandler = GetComponent<PlayerInputHandler>();
            
            if(inputManager != null && gameObject != null)
                inputManager.PlayerTransform = playerTransform;
            
            
            DashDirectionIndicator = transform.Find("DashDirectionIndicator");
            MovementCollider2D = GetComponent<CapsuleCollider2D>();
            
            GhostTrailController = GetComponent<GhostTrailController>();
            
            StateMachine.Initialize(IdleState);
        }

        protected override void Update()
        {
            if(Time.timeScale == 0) return;
            
            base.Update();
            Core.LogicUpdate();
            StateMachine.CurrentState.LogicUpdate();
            inputManager.CheckAllInputHoldTimes();
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            StateMachine.CurrentState.PhysicsUpdate();
        }
        #endregion

        #region Animation Functions
        private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
        private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
        private void AnimationCancelTrigger() => StateMachine.CurrentState.AnimationCancelTrigger();
        private void StartMovementTrigger() => StateMachine.CurrentState.StartMovementTrigger();
        private void StopMovementTrigger() => StateMachine.CurrentState.StopMovementTrigger();
        protected void SetFlipActive() => StateMachine.CurrentState.SetFlipActive();
        private void SetFlipInactive() => StateMachine.CurrentState.SetFlipInactive();
        private void AttackTrigger() => StateMachine.CurrentState.AttackTrigger();
        #endregion

        #region Other Functions
        // Change the height of the collider when the player is crouching
        public void SetColliderHeight(float height)
        {
            var center = MovementCollider2D.offset;
            _workspace.Set(MovementCollider2D.size.x, height);

            center.y += (height - MovementCollider2D.size.y)/2;
            
            //Change the height of the Collider
            MovementCollider2D.size = _workspace;

            //Change the center point of the Collider
            MovementCollider2D.offset = center;
        }
        
        private void OnDrawGizmos()
        {
            foreach (var item in playerData.hitBox) 
                Gizmos.DrawWireCube(attackPosition.transform.position + (Vector3)item.center, item.size);
        }
        #endregion

        public override void Die()
        {
            base.Die();
            StateMachine.ChangeState(DeathState);
        }
    }
}
