using Manager;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Skills
{
    public class DashSkill : Skill
    {
        [Header("Dash")] 
        [SerializeField] private UISkillTreeSlot dashUnlockButton;
        public bool DashUnlocked { get; private set; }
        
        [Header("Clone on dash")] 
        [SerializeField] private UISkillTreeSlot cloneOnDashUnlockButton;
        public bool CloneOnDashUnlocked { get; private set; }
       
        
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
                DashUnlocked = true;
        }

        private void UnlockCloneDash()
        {
            if(cloneOnDashUnlockButton.unlocked)
                CloneOnDashUnlocked = true;
        }

        public void CreateCloneOnDash()
        {
            if(CloneOnDashUnlocked)
                SkillManager.instance.Clone.CreateClone(Player.transform, Vector3.zero);
        }
    }
}