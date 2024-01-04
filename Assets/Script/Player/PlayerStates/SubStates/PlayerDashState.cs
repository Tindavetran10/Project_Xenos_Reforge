using Script.Player.Data;
using Script.Player.PlayerStates.SuperStates;
using UnityEngine;

namespace Script.Player.PlayerStates.SubStates
{
    public class PlayerDashState : PlayerAbilityState
    {
        private bool CanDash { get; set; }
        private bool _isHolding;
        private bool _dashInputStop;

        private float _lastDashTime;
        
        private Vector2 _dashDirection;
        private Vector2 _dashDirectionInput;
        //private Vector2 _lastAIPos;
        
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");
        
        public PlayerDashState(PlayerStateMachine.Player player, PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}

        public override void Enter()
        {
            base.Enter();
            
            CanDash = false;
            // Identify the dash Input
            Player.InputHandler.UseDashInput();

            // Check the player whether holding the dash input or not
            _isHolding = true;
            _dashDirection = Vector2.right * Movement.FacingDirection;

            // Set the amount of time that allow the player to hold the dash input in REAL-TIME
            Time.timeScale = PlayerData.holdTimeScale;
            // Save the StartTime when entering the Dash State without getting affected by Time.timeScale
            StartTime = Time.unscaledTime;
            
            // Show the DirectionIndicator 
            Player.DashDirectionIndicator.gameObject.SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();

            // Adjust the y velocity if the player exiting the dash state
            if (Movement?.CurrentVelocity.y > 0) {
                Movement?.SetVelocityY(Movement.CurrentVelocity.y * PlayerData.dashEndYMultiplier);
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!IsExitingState)
            {
                // Change the animation base on the x and y velocity
                Player.Anim.SetFloat(YVelocity, Movement.CurrentVelocity.y);
                Player.Anim.SetFloat(XVelocity, Mathf.Abs(Movement.CurrentVelocity.x));

                // If the player hold the dash button (he want to dash into a specific direction)
                if (_isHolding) {
                    _dashDirectionInput = Player.InputHandler.DashDirectionInput;
                    _dashInputStop = Player.InputHandler.DashInputStop;

                    if (_dashDirectionInput != Vector2.zero)
                    {
                        _dashDirection = _dashDirectionInput;
                        _dashDirection.Normalize();
                    }
                    
                    // Calculate the angle (in degree) between 2 vectors
                    var angle = Vector2.SignedAngle(Vector2.right, _dashDirection);
                    // Rotate the z axis of DashDirectionIndicator 45 degree
                    Player.DashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle - 45f);
                    Movement.CheckIfShouldFlip(Mathf.RoundToInt(_dashDirection.x));

                    // If a certain amount of real-time (from the start time point to the maxHoldTime point)
                    if (_dashInputStop || Time.unscaledTime >= StartTime + PlayerData.maxHoldTime)
                    {
                        _isHolding = false;
                        Time.timeScale = 1f;
                        StartTime = Time.time;
                        Movement?.CheckIfShouldFlip(Mathf.RoundToInt(_dashDirection.x));
                        Player.Rb.drag = PlayerData.drag;
                        Movement?.SetVelocity(PlayerData.dashVelocity, _dashDirection);
                        Player.DashDirectionIndicator.gameObject.SetActive(false);
                    }
                }
                else
                {
                    // Automatically make the character dash when out of the holding time
                    Movement?.SetVelocity(PlayerData.dashVelocity, _dashDirection);
                    
                    // After the amount of Dash Time, stop the player
                    if (!(Time.time >= StartTime + PlayerData.dashTime)) return;
                    Player.Rb.drag = 0f;
                    IsAbilityDone = true;
                    _lastDashTime = Time.time;
                }
            }
        }

        // Check if the Dash has cooled down or not
        public bool CheckIfCanDash() => 
            CanDash && Time.time >= _lastDashTime + PlayerData.dashCooldown;

        public void ResetCanDash() => CanDash = true;
    }
}
