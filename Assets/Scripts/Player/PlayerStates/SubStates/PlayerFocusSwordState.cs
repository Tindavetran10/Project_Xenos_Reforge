using System.Linq;
using Enemy.EnemyStats;
using Manager;
using Player.Data;
using Player.PlayerStates.SuperStates;
using UnityEngine;

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
                
                return (from hit 
                    in collider2Ds 
                    where hit.GetComponent<Enemy.EnemyStateMachine.Enemy>() != null 
                    select hit.GetComponentInChildren<EnemyStats>()).Any(target => target.IsStunned);
            }
            return false;
        }

        
        private static void FocusSwordTrail() => SkillManager.Instance.Focus.Trail();
        private static void FocusSwordSlice()
        {
            SkillManager.Instance.Focus.Slice();
            FocusSwordFragment();
        }

        private static void FocusSwordFragment() => SkillManager.Instance.Focus.Fragments();

        private static void ClearFocusSword() => SkillManager.Instance.Focus.ClearMousePositions();
    }
}
