using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _Scripts.Player.Input
{
    // This class handles player input using the Unity Input System
    [CreateAssetMenu(menuName = "PlayerInputHandler")]
    public class InputManager : ScriptableObject, PlayerInput.IGameplayActions, PlayerInput.IUIActions
    {
        private static InputManager _instance;

        private static PlayerInput _playerInput;
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
        public event UnityAction MenuClickEvent;
        public event UnityAction OptionsOpenEvent;
        public event UnityAction OptionsCloseEvent;
        public event UnityAction InventoryOpenEvent;
        public event UnityAction InventoryCloseEvent;
        public event UnityAction SkillTreeOpenEvent;
        public event UnityAction SkillTreeCloseEvent;
        public event UnityAction MapOpenEvent;
        public event UnityAction MapCloseEvent;
        public event UnityAction CraftingOpenEvent;
        public event UnityAction CraftingCloseEvent;
        #endregion
        
        
        public void Update()
        {
            if(Camera.main != null)
            {
                _cam=Camera.main;
            }
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }
        
        private void OnEnable()
        {
            // Create a new instance of the PlayerInput asset
            if (_playerInput != null) return;
            _playerInput = new PlayerInput();

            // Set up callbacks for gameplay and UI actions
            _playerInput.Gameplay.SetCallbacks(this);
            _playerInput.UI.SetCallbacks(this);
            
            SetGameplay();
            
            var count = Enum.GetValues(typeof(CombatInputs)).Length;
            NormalAttackInputs = new bool[count];
            NormalAttackInputsStop = new bool[count];
            
            _cam = Camera.main;
        }

        private static void SetGameplay()
        {
            _playerInput.Gameplay.Enable();
            _playerInput.UI.Disable();
        }

        private static void SetUI()
        {
            _playerInput.Gameplay.Disable();
            _playerInput.UI.Enable();
        }
        
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
            if (_cam != null)
                RawDashDirectionInput = _cam.ScreenToWorldPoint(RawDashDirectionInput) - PlayerTransform.position;
            
            DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
        }
        
        public void OnOptionsOpen(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                OptionsOpenEvent?.Invoke();
                SetUI();
            }
        }

        public void OnInventoryOpen(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                InventoryOpenEvent?.Invoke();
                SetUI();
            }
        }

        public void OnSkillTreeOpen(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                SkillTreeOpenEvent?.Invoke();
                SetUI();
            }
        }

        public void OnMapOpen(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                MapOpenEvent?.Invoke();
                SetUI();
            }
        }

        public void OnCraftingOpen(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                CraftingOpenEvent?.Invoke();
                SetUI();
            }
        }
        public void OnPoint(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                MenuClickEvent?.Invoke();
                SetUI();
            }
        }
        #endregion

        #region UI Input
        public void OnNavigate(InputAction.CallbackContext context) {}

        public void OnSubmit(InputAction.CallbackContext context){}

        public void OnCancel(InputAction.CallbackContext context){}


        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) 
                MenuClickEvent?.Invoke();
        }

        public void OnScrollWheel(InputAction.CallbackContext context){}

        public void OnMiddleClick(InputAction.CallbackContext context){}

        public void OnRightClick(InputAction.CallbackContext context) {}
        
        public void OnOptionsClose(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                OptionsCloseEvent?.Invoke();
                SetGameplay();
            }
        }

        public void OnInventoryClose(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                InventoryCloseEvent?.Invoke();
                SetGameplay();
            }
        }

        public void OnSkillTreeClose(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                SkillTreeCloseEvent?.Invoke();
                SetGameplay();
            }
        }

        public void OnMapClose(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                MapCloseEvent?.Invoke();
                SetGameplay();
            }
        }

        public void OnCraftingClose(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                CraftingCloseEvent?.Invoke();
                SetGameplay();
            }
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context){}

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context){}

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
