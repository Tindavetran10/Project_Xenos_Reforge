using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Player.Input
{
    // This class handles player input using the Unity Input System
    [CreateAssetMenu(menuName = "PlayerInputHandler")]
    public class InputManager : ScriptableObject, PlayerInput.IGameplayActions, PlayerInput.IUIActions
    {
        public static InputManager Instance;
        
        public static PlayerInput PlayerInput;
        public Transform PlayerTransform { get; set;}
        private Camera _cam;

        #region Input for Gameplay

        private Vector2 RawMovementInput { get; set; }
        private Vector2 RawDashDirectionInput { get; set; }
        public Vector2Int DashDirectionInput { get; private set; }
        
        public int NormInputX { get; private set; }
        public int NormInputY { get; private set; }

        // Properties to track various input states
        public bool JumpInput { get; private set; }
        public bool JumpInputStop { get; private set; }
        public bool DashInput { get; private set; }
        public bool DashInputStop { get; private set; }
        
        public bool[] NormalAttackInputs { get; private set; }
        private bool[] NormalAttackInputsStop { get; set; }
        
        public bool AimSwordInput { get; private set; }
        public bool AimSwordInputStop { get; private set; }

        private Vector3 FocusSwordPositionInput { get; set; }
        public bool FocusSwordInput { get; private set; }
        public bool FocusSwordInputStop { get; private set; }
        public bool FocusSwordMouseClick { get; private set; }
        
        public bool CounterInput { get; private set; }
        public bool CounterInputStop { get; private set; }
        
        public bool MenuOpenInput { get; private set; }
        #endregion

        #region Input Holdtime for Gameplay
        [SerializeField] private float inputHoldTime;
        private float _jumpInputStartTime;
        private float _dashInputStartTime;
        private float _counterInputStartTime;
        private float _attackInputsStartTime;
        private float _aimSwordInputStartTime;
        private float _focusSwordInputStartTime;
        #endregion

        #region Events for UI
        public event Action MenuOpenEvent;
        public event Action MenuCloseEvent;
        #endregion
        
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        
        private void OnEnable()
        {
            // Create a new instance of the PlayerInput asset
            if (PlayerInput != null) return;
            PlayerInput = new PlayerInput();

            // Set up callbacks for gameplay and UI actions
            PlayerInput.Gameplay.SetCallbacks(this);
            PlayerInput.UI.SetCallbacks(this);
            
            var count = Enum.GetValues(typeof(CombatInputs)).Length;
            NormalAttackInputs = new bool[count];
            NormalAttackInputsStop = new bool[count];
            
            _cam = Camera.main;

            // Set initial input mode to Gameplay
            SetGameplay();
        }

        #region Change Input Action Map
        public static void SetGameplay()
        {
            PlayerInput.Gameplay.Enable();
            PlayerInput.UI.Disable();
        }
        
        public static void SetUI()
        {
            PlayerInput.Gameplay.Disable();
            PlayerInput.UI.Enable();
        }
        #endregion

        #region Check for Gameplay Input
        public void CheckAllInputHoldTimes()
        {
            CheckJumpInputHoldTime();
            CheckDashInputHoldTime();
            CheckAttackInputsHoldTime();
            CheckAimSwordInputHoldTime();
            CheckFocusSwordInputHoldTime();
            CheckCounterInputHoldTime();
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

        private void CheckAttackInputsHoldTime()
        {
            if (Time.time >= _attackInputsStartTime + inputHoldTime)
                NormalAttackInputs[(int)CombatInputs.Normal] = false;
        }

        private void CheckAimSwordInputHoldTime()
        {
            if (Time.time >= _aimSwordInputStartTime + inputHoldTime)
                AimSwordInput = false;
        }
        
        private void CheckFocusSwordInputHoldTime()
        {
            if (Time.time >= _focusSwordInputStartTime + inputHoldTime)
                FocusSwordInput = false;
        }
        
        private void CheckCounterInputHoldTime()
        {
            if (Time.time >= _counterInputStartTime + inputHoldTime)
                CounterInput = false;
        }
        #endregion
        
        #region Gameplay Input
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
        
        public void OnFocusSword(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                FocusSwordInput = true;
                FocusSwordInputStop = false;
                _focusSwordInputStartTime = Time.time;
            }
            else if (context.canceled) FocusSwordInputStop = true;
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
            {
                NormalAttackInputs[(int)CombatInputs.Normal] = true;
                NormalAttackInputsStop[(int)CombatInputs.Normal] = false;
                _attackInputsStartTime = Time.time;
            }
            else if (context.canceled) NormalAttackInputsStop[(int)CombatInputs.Normal] = true;
        }

        public void OnHeavyAttack(InputAction.CallbackContext context)
        {
            if (context.started)
                NormalAttackInputs[(int)CombatInputs.Heavy] = true;
            if (context.canceled)
                NormalAttackInputs[(int)CombatInputs.Heavy] = false;
        }

        public void OnCounterAttack(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                CounterInput = true;
                CounterInputStop = false;
                _counterInputStartTime = Time.time;
            }
            else if (context.canceled) CounterInputStop = true;
        }

        public void OnAimSword(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                AimSwordInput = true;
                AimSwordInputStop = false;
                _aimSwordInputStartTime = Time.time;
            }
            else if (context.canceled) AimSwordInputStop = true;
        }
        public void OnFocusSwordMousePos(InputAction.CallbackContext context)
        {
            FocusSwordPositionInput = context.ReadValue<Vector3>();
            FocusSwordPositionInput = _cam.ScreenToWorldPoint(FocusSwordPositionInput);
        }

        public void OnFocusSwordMouseClick(InputAction.CallbackContext context)
        {
            if(context.started) FocusSwordMouseClick = true;
            if(context.canceled) FocusSwordMouseClick = false;
        }
        
        // Callback for dash direction input
        public void OnDashDirection(InputAction.CallbackContext context)
        {
            RawDashDirectionInput= context.ReadValue<Vector2>();
            RawDashDirectionInput = _cam.ScreenToWorldPoint(RawDashDirectionInput) - PlayerTransform.position;
            DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
        }
        
        public void OnMenuOpen(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                MenuOpenEvent?.Invoke();
                SetUI();
            }
        }
        #endregion

        #region UI Input
        public void OnNavigate(InputAction.CallbackContext context) {}

        public void OnSubmit(InputAction.CallbackContext context){}

        public void OnCancel(InputAction.CallbackContext context){}

        public void OnPoint(InputAction.CallbackContext context){}

        public void OnClick(InputAction.CallbackContext context){}

        public void OnScrollWheel(InputAction.CallbackContext context){}

        public void OnMiddleClick(InputAction.CallbackContext context){}

        public void OnRightClick(InputAction.CallbackContext context){}
        
        public void OnMenuClose(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                MenuCloseEvent?.Invoke();
                SetGameplay();
            }
        }
        #endregion

        #region Use Input
        public void UseJumpInput() => JumpInput = false;
        public void UseDashInput() => DashInput = false;
        public void UseAttackInput() => NormalAttackInputs[(int)CombatInputs.Normal] = false;
        public void UseAimSwordInput() => AimSwordInput = false;
        public void UseFocusSwordInput() => FocusSwordInput = false;
        public void UseCounterInput() => CounterInput = false;
        #endregion
    }
}

public enum CombatInputs
{
    Normal,
    Heavy
}
