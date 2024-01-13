using Script.Manager;
using UnityEngine;

namespace Script.Player.Skills
{
    public class Skill : MonoBehaviour
    {
        [SerializeField] protected float coolDown;
        private float _coolDownTimer;

        protected PlayerStateMachine.Player Player;

        protected virtual void Start() => Player = PlayerManager.Instance.player;

        protected void Update() => _coolDownTimer -= Time.deltaTime;

        public virtual bool CanUseSkill()
        {
            if (_coolDownTimer < 0)
            {
                UseSkill();
                _coolDownTimer = coolDown;
                return true;
            }

            Debug.Log("Skill is on cool down");
            return false;
        }

        protected virtual void UseSkill() {}
    }
}