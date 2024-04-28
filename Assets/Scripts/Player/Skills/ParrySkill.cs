using Manager;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Skills
{
    public class ParrySkill : Skill
    {
        [Header("Parry Heal")]
        [SerializeField] private UISkillTreeSlot parryHealUnlockButton;
        [Range(0f, 1f)] [SerializeField] private float restoreHealthPercentage;
        private bool ParryHealUnlocked { get; set; }
        
        [Header("Clone on parry")] 
        [SerializeField] private UISkillTreeSlot cloneOnParryUnlockButton;

        private bool CloneOnParryUnlocked { get; set; }
        
        protected override void Start()
        {
            base.Start();
            parryHealUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryHeal);
            cloneOnParryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneParry);
        }
        
        public override void UseSkill()
        {
            base.UseSkill();
            if (ParryHealUnlocked)
            {
                var restoreAmount = Mathf.RoundToInt(Player.Stats.GetMaxHealthValue() * restoreHealthPercentage);
                Player.Stats.IncreaseHealthBy(restoreAmount);
            }
        }
        
        protected override void CheckUnlock()
        {
            UnlockParryHeal();
            UnlockCloneParry();
        }

        private void UnlockParryHeal()
        {
            if (parryHealUnlockButton.unlocked)
                ParryHealUnlocked = true;
        }
        
        private void UnlockCloneParry()
        {
            if(cloneOnParryUnlockButton.unlocked)
                CloneOnParryUnlocked = true;
        }

        public void MakeMirageOnParry(Transform respawnTransform)
        {
            if (CloneOnParryUnlocked)
                SkillManager.instance.Clone.CreateCloneWithDelay(respawnTransform);
        }
    }
}