using System;
using _Scripts.Player.Input;
using Script.Player.PlayerStateMachine;
using UnityEngine;

namespace Script.Player
{
    public class AnimationEventHandler : MonoBehaviour
    {
        #region AnimationCancel Trigger requirements
        private PlayerInputHandler _playerInputHandler;
        private PlayerState _playerState;
        private int _currentAttackIndex;
        protected bool IsAnimationCancel;
        #endregion
        
        public event Action OnFinish;
        public event Action OnStartMovement;
        public event Action OnStopMovement;
        public event Action OnAttackAction;

        public event Action<bool> OnFlipSetActive; 

        private void Start() => _playerInputHandler = GetComponentInParent<PlayerInputHandler>();
        
        private void AnimationCancelTrigger()
        {
            if (_playerInputHandler.AttackInputs[_currentAttackIndex] ||
                _playerInputHandler.NormInputX == 1 || _playerInputHandler.NormInputX == -1 ||
                _playerInputHandler.JumpInput ||
                _playerInputHandler.DashInput)
                IsAnimationCancel = true;

        }

        
        private void AttackActionTrigger() => OnAttackAction?.Invoke();
        
        private void SetFlipActive() => OnFlipSetActive?.Invoke(true);
        private void SetFlipInActive() => OnFlipSetActive?.Invoke(false);
    }
}
