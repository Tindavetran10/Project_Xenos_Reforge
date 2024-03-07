using Manager;
using Player.Data;
using Player.PlayerStates.SuperStates;
using UnityEngine;
using UnityEngine.InputSystem;

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
            
            /*// Set the amount of time that allow the player to hold the focus sword input in REAL-TIME
            Time.timeScale = PlayerData.focusSwordDuration;
            // Save the StartTime when entering the Dash State without getting affected by Time.timeScale
            StartTime = Time.unscaledTime;*/
            
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
                    
                    //if(CanSlice()) FocusSword();
                    
                    FocusSword();
                    
                    // If a certain amount of real-time (from the start time point to the maxHoldTime point)
                    if (_focusSwordInputStop)
                    {
                        Player.Stats.MakeInvincible(true);
                        
                        IsHolding = false;
                        Time.timeScale = 1f;
                        //StartTime = Time.time;
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

        /*private bool CanSlice()
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(_focusSwordPositionInput, new Vector2(0.1f, 0.1f), 
                0f, PlayerData.whatIsEnemy);

            foreach (var hit in collider2Ds)
            {
                if (hit.GetComponent<Enemy.EnemyStateMachine.Enemy>() != null)
                {
                    var target = hit.GetComponentInChildren<Enemy.EnemyStateMachine.Enemy>();
                    if (target.CanBeStunned())
                        return true;
                }
            }
            return false;
        }*/

        private static void FocusSword() => SkillManager.Instance.Focus.Slice();
        private static void ClearFocusSword() => SkillManager.Instance.Focus.ClearMousePositions();
    }
}
