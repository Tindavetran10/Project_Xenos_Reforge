using _Scripts.Player.Input;
using _Scripts.Player.PlayerStates.SubStates;
using Script.Player.Data;
using Script.Player.PlayerStates.SubStates;
using UnityEngine;

namespace Script.Player.PlayerStateMachine
{
    public class Player : MonoBehaviour
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
        public PlayerWallGrabState WallGrabState { get; private set; }
        public PlayerWallClimbState WallClimbState { get; private set; }
        public PlayerWallJumpState WallJumpState { get; private set; }
        public PlayerLedgeClimbState LedgeClimbState { get; private set; }
        public PlayerDashState DashState { get; private set; }
        public PlayerCrouchIdleState CrouchIdleState { get; private set; }
        public PlayerCrouchMoveState CrouchMoveState { get; private set; }
        [SerializeField] private PlayerData playerData;
        #endregion

        #region Components
        public _Scripts.CoreSystem.Core Core { get; private set; }
        public Animator Anim { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        public Rigidbody2D Rb { get; private set; }
        public Transform DashDirectionIndicator { get; private set; }
        private CapsuleCollider2D MovementCollider2D { get; set; }
        #endregion
    
        #region Other Variables
        private Vector2 _workspace;
        #endregion

        #region Unity Callback Functions
        private void Awake()
        {
            Core = GetComponentInChildren<_Scripts.CoreSystem.Core>();
            
            StateMachine = new PlayerStateMachine();
        
            // Declare each state of the player, which take the required information from different State class
            IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
            MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
            JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
            InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
            LandState = new PlayerLandState(this, StateMachine, playerData, "land");
            WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
            WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
            WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
            WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
            LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
            DashState = new PlayerDashState(this, StateMachine, playerData, "dash");
            CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
            CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "crouchMove");
        }

        private void Start()
        {
            Anim = GetComponentInChildren<Animator>();
            InputHandler = GetComponent<PlayerInputHandler>();
            Rb = GetComponent<Rigidbody2D>();
            DashDirectionIndicator = transform.Find("DashDirectionIndicator");
            MovementCollider2D = GetComponent<CapsuleCollider2D>();

            StateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            Core.LogicUpdate();
            StateMachine.CurrentState.LogicUpdate();
        }
    
        private void FixedUpdate() => StateMachine.CurrentState.PhysicsUpdate();
        #endregion

        #region Other Functions
        // Change the height of the collider when the player is crouching
        public void SetColliderHeight(float height)
        {
            var center = MovementCollider2D.offset;
            _workspace.Set(MovementCollider2D.size.x, height);

            center.y += (height - MovementCollider2D.size.y)/2;
            
            //Change the height of the size of the Collider
            MovementCollider2D.size = _workspace;

            //Change the center point of the Collider
            MovementCollider2D.offset = center;
        }
        private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
        private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
        #endregion
    }
}
