using Player.Skills.SkillController;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Skills
{
    public class SlashSkill : Skill
    {
        [Header("Skill Info")] 
        [SerializeField] private GameObject slashPrefab;
        [SerializeField] private Vector2 launchDir;
        [SerializeField] private float slashGravity;
        [SerializeField] private float  destroyDelay = 1f;
        
        [Header("Skill Unlock")] 
        public bool slashUnlocked;
        [SerializeField] private UISkillTreeSlot slashUnlockButton;

        protected override void Start()
        {
            base.Start();
            slashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSlash);
        }

        public void CreateSlash(int playerDir)
        {
            var slashPosition = Player.transform.position;
            var slashRotation = Quaternion.Euler(0f, 0f, transform.rotation.x * playerDir);

            var newSlash = Instantiate(slashPrefab, slashPosition, slashRotation);

            // Flip the object if the player is facing left
            if (playerDir < 0)
            {
                var newScale = newSlash.transform.localScale;
                newScale.x *= -1; // Flip the object along the X-axis
                newSlash.transform.localScale = newScale;
            }
            Destroy(newSlash, destroyDelay);

            var newSlashSkillController = newSlash.GetComponent<SlashSkillController>();
            newSlashSkillController.SetupSlash(launchDir * playerDir, slashGravity);
        }

        protected override void CheckUnlock() => UnlockSlash();

        private void UnlockSlash()
        {
            if(slashUnlockButton.unlocked)
                slashUnlocked = true;
        }
    }
}
