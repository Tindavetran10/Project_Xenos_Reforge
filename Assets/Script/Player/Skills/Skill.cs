using UnityEngine;

namespace Script.Player.Skills
{
    public class Skill : MonoBehaviour
    {
        [SerializeField] protected float coolDown;
        protected float CoolDownTimer;
        
        protected void Update() => CoolDownTimer -= Time.deltaTime;

        public virtual bool CanUseSkill()
        {
            if (CoolDownTimer < 0)
            {
                UseSkill();
                CoolDownTimer = coolDown;
                return true;
            }

            Debug.Log("Skill is on cool down");
            return false;
        }

        public virtual void UseSkill()
        {
            
        }
    }
}