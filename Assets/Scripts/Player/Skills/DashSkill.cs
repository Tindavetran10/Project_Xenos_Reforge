using Manager;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Skills
{
    public class DashSkill : Skill
    {
        [Header("Dash")] 
        public bool dashUnlocked;
        [SerializeField] private UISkillTreeSlot dashUnlockButton;
        
        [Header("Clone on dash")] 
        public bool cloneOnDashUnlocked;
        [SerializeField] private UISkillTreeSlot cloneOnDashUnlockButton;
       
        
        protected override void Start()
        {
            base.Start();
            dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
            cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneDash);
        }

        protected override void CheckUnlock()
        {
            UnlockDash();
            UnlockCloneDash();
        }

        private void UnlockDash()
        {
            if(dashUnlockButton.unlocked)
                dashUnlocked = true;
        }

        private void UnlockCloneDash()
        {
            if(cloneOnDashUnlockButton.unlocked)
                cloneOnDashUnlocked = true;
        }

        public void CreateCloneOnDash()
        {
            if(cloneOnDashUnlocked)
                SkillManager.Instance.Clone.CreateClone(Player.transform);
        }
    }
}