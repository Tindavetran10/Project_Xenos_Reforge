using Manager;
using Player.Data;
using Player.PlayerStates.SuperStates;
using UnityEngine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerFocusSwordState : PlayerAbilityState
    {
        private bool _focusSwordInput;
        private bool _focusSwordInputStop;
        private Vector2 _focusSwordPositionInput;
        
        public PlayerFocusSwordState(Player.PlayerStateMachine.Player player, PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Enter Focus Sword State");
            
            IsHolding = true;
            
            // Set the amount of time that allow the player to hold the focus sword input in REAL-TIME
            Time.timeScale = PlayerData.focusSwordDuration;
            // Save the StartTime when entering the Dash State without getting affected by Time.timeScale
            StartTime = Time.unscaledTime;
            
            Player.InputHandler.UseFocusSwordInput();
            Movement?.SetVelocityZero();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!IsExitingState)
            {
                if (IsHolding)
                {
                    _focusSwordInput = Player.InputHandler.FocusSwordInput;
                    _focusSwordInputStop = Player.InputHandler.FocusSwordInputStop;
                    _focusSwordPositionInput = Player.InputHandler.FocusSwordPositionInput;
                    
                    Player.Stats.MakeInvincible(true);
                    
                    if(_focusSwordPositionInput != Vector2.zero)
                        FocusSword();
                    else ClearFocusSword();
                    
                    // If a certain amount of real-time (from the start time point to the maxHoldTime point)
                    if (_focusSwordInputStop)
                    {
                        Player.Stats.MakeInvincible(true);
                        
                        IsHolding = false;
                        Time.timeScale = 1f;
                        StartTime = Time.time;
                        ClearFocusSword();
                    }
                }
                else IsAbilityDone = true;
            }
        }

        public override void Exit()
        {
            base.Exit();
            Player.Stats.MakeInvincible(false);
            Debug.Log("Exit Focus Sword State");
        }

        private static void FocusSword() => SkillManager.Instance.Focus.Slice();
        private static void ClearFocusSword() => SkillManager.Instance.Focus.ClearMousePositions();
    }
}
