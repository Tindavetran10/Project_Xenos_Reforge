using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Player.Input
{
    // This class handles player input using the Unity Input System
    public class PlayerInputHandler : MonoBehaviour, PlayerInput.IGameplayActions, PlayerInput.IUIActions
    {
        private PlayerInput _playerInput;
        private Camera _cam;

        // Events for handling pause and resume actions
        public event Action OnPauseEvent;
        public event Action OnResumeEvent;

        // Properties to store raw and normalized input values
        private Vector2 RawMovementInput { get; set; }
        private Vector2 RawDashDirectionInput { get; set; }
        public Vector2Int DashDirectionInput { get; private set; }
        public int NormInputX { get; private set; }
        public int NormInputY { get; private set; }

        // Properties to track various input states
        public bool JumpInput { get; private set; }
        public bool JumpInputStop { get; private set; }
        public bool GrabInput { get; private set; }
        public bool DashInput { get; private set; }
        public bool DashInputStop { get; private set; }
        
        public bool[]  AttackInputs { get; private set; }
        
        
        [SerializeField] private float inputHoldTime;
        private float _jumpInputStartTime;
        private float _dashInputStartTime;
        
        private void Start()
        {
            var count = Enum.GetValues(typeof(CombatInputs)).Length;
            AttackInputs = new bool[count];
            
            _cam = Camera.main;
        }

        private void Update()
        {
            // Check and update jump and dash input hold times
            CheckJumpInputHoldTime();
            CheckDashInputHoldTime();
        }

        // Check if the jump input has been held for a certain duration
        private void CheckJumpInputHoldTime()
        {
            if (Time.time >= _jumpInputStartTime + inputHoldTime)
                JumpInput = false;
        }

        // Check if the dash input has been held for a certain duration
        private void CheckDashInputHoldTime()
        {
            if (Time.time >= _dashInputStartTime + inputHoldTime)
                DashInput = false;
        }
        
        // Enable input actions when the script is enabled
        private void OnEnable()
        {
            if (_playerInput != null) return;

            // Create a new instance of the PlayerInput asset
            _playerInput = new PlayerInput();

            // Set up callbacks for gameplay and UI actions
            _playerInput.Gameplay.SetCallbacks(this);
            _playerInput.UI.SetCallbacks(this);

            // Set initial input mode to Gameplay
            SetGameplay();
        }

        // Set input mode to Gameplay
        private void SetGameplay()
        {
            _playerInput.Gameplay.Enable();
            _playerInput.UI.Disable();
        }

        // Set input mode to UI
        private void SetUI()
        {
            _playerInput.Gameplay.Disable();
            _playerInput.UI.Enable();
        }

        // Callback for movement input
        public void OnMovement(InputAction.CallbackContext context)
        {
            RawMovementInput = context.ReadValue<Vector2>();
            // Get the inputs that change x axis and y axis value from the player  
            NormInputX = Mathf.RoundToInt(RawMovementInput.x);
            NormInputY = Mathf.RoundToInt(RawMovementInput.y);
        }

        // Callback for jump input
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                JumpInput = true;
                JumpInputStop = false;
                _jumpInputStartTime = Time.time;
            }
            if (context.canceled) JumpInputStop = true;
        }

        // Callback for grab input
        public void OnGrab(InputAction.CallbackContext context)
        {
            if (context.started) GrabInput = true;
            if (context.canceled) GrabInput = false;
        }

        // Callback for dash input
        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                DashInput = true;
                DashInputStop = false;
                _dashInputStartTime = Time.time;
            }
            else if (context.canceled) DashInputStop = true;
        }
        
        // Callback for normal attack input
        public void OnNormalAttack(InputAction.CallbackContext context)
        {
            if (context.started)
                AttackInputs[(int)CombatInputs.Normal] = true;
            if (context.canceled)
                AttackInputs[(int)CombatInputs.Normal] = false;
        }

        public void OnHeavyAttack(InputAction.CallbackContext context)
        {
            if (context.started)
                AttackInputs[(int)CombatInputs.Heavy] = true;
            if (context.canceled)
                AttackInputs[(int)CombatInputs.Heavy] = false;
        }

        // Callback for dash direction input
        public void OnDashDirection(InputAction.CallbackContext context)
        {
            RawDashDirectionInput = context.ReadValue<Vector2>();
            RawDashDirectionInput = _cam.ScreenToWorldPoint(RawDashDirectionInput) - transform.position;
            DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
        }

        // Callback for pause input
        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed) return;
            OnPauseEvent?.Invoke();
            SetUI();
        }

        // Callback for resume input
        public void OnResume(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed) return;
            OnResumeEvent?.Invoke();
            SetGameplay();
        }

        // Utility method to consume jump input
        public void UseJumpInput() => JumpInput = false;

        // Utility method to consume dash input
        public void UseDashInput() => DashInput = false;
    }
}

public enum CombatInputs
{
    Normal,
    Heavy
}
