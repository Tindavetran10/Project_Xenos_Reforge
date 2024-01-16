using Script.Manager;
using UnityEngine;

namespace Script.Player.Skills
{
    public class Skill : MonoBehaviour
    {
        [SerializeField] protected float coolDown;
        private float _coolDownTimer;

        protected PlayerStateMachine.Player Player;

        protected virtual void Start()
        {
            Player = PlayerManager.Instance.player;
            CheckUnlock();
        }

        protected void Update() => _coolDownTimer -= Time.deltaTime;

        protected virtual void CheckUnlock() {}
        
        public bool CanUseSkill()
        {
            if (_coolDownTimer < 0)
            {
                UseSkill();
                _coolDownTimer = coolDown;
                return true;
            }
            return false;
        }

        private static void UseSkill() {}
    }
}