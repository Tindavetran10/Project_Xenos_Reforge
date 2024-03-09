using Enemy.EnemyStats;
using Manager;
using Player.Data;
using Player.PlayerStates.SuperStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.PlayerStates.SubStates
{
    public class PlayerFocusSwordState : PlayerAbilityState
    {
        private bool _focusSwordInputStop;
        private bool _focusSwordMouseClick;

        public PlayerFocusSwordState(Player.PlayerStateMachine.Player player, PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Enter Focus Sword State");
            
            IsHolding = true;
            
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
                    _focusSwordInputStop = Player.InputHandler.FocusSwordInputStop;
                    _focusSwordMouseClick = Player.InputHandler.FocusSwordMouseClick;
                    
                    if(_focusSwordMouseClick)
                    {
                        Player.Stats.MakeInvincible(true);
                        FocusSwordTrail();
                        if(CanSlice()) FocusSwordSlice();
                    }
            
                    if (_focusSwordInputStop)
                    {
                        Player.Stats.MakeInvincible(false);
                        IsHolding = false;
                        Time.timeScale = 1f;
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


        private bool CanSlice()
        {
            if (Camera.main != null)
            {
                var collider2Ds = Physics2D.OverlapBoxAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), 
                    PlayerData.focusSwordHitBox.size, 0f, PlayerData.whatIsEnemy);
            
                foreach (var hit in collider2Ds)
                {
                    if (hit.GetComponent<Enemy.EnemyStateMachine.Enemy>() != null)
                    {
                        Debug.Log("Collided with enemy");
                        var target = hit.GetComponentInChildren<EnemyStats>();
                        if(target.IsStunned)
                            return true;
                    }
                }
            }

            return false;
        }

        
        private static void FocusSwordSlice() => SkillManager.Instance.Focus.Slice();
        private static void FocusSwordTrail() => SkillManager.Instance.Focus.Trail();
        private static void ClearFocusSword() => SkillManager.Instance.Focus.ClearMousePositions();
    }
}
