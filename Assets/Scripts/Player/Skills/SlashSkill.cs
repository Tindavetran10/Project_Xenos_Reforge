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

        //private Vector3 slashPosition = Vector3.zero;

        protected override void Start()
        {
            base.Start();
            //slashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSlash);
        }

        /*protected override void Update()
        {
            base.Update();
            *//*if (Player.transform.position != null)
            {
                var slashPosition = Player.transform.position;
            }*//*
        }*/

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

            /*if (Player != null*//* && Player.transform.position != null*//*)
            {
                var slashPosition = Player.transform.position;
                var slashRotation = Quaternion.Euler(0f, 0f, transform.rotation.x * playerDir);
                // Create the slash and handle potential null references
                var newSlash = Instantiate(slashPrefab, slashPosition, slashRotation);
                if (newSlash == null)
                {
                    Debug.LogError("Failed to instantiate slashPrefab!");
                    return; // Exit the function if instantiation fails
                }

                // Flip the object if the player is facing left
                *//*if (playerDir < 0)
                {
                    newSlash.transform.localScale = new Vector3(-newSlash.transform.localScale.x, newSlash.transform.localScale.y, newSlash.transform.localScale.z);
                }*//*

                if (playerDir < 0)
                {
                    var newScale = newSlash.transform.localScale;
                    newScale.x *= -1; // Flip the object along the X-axis
                    newSlash.transform.localScale = newScale;
                }

                // Destroy the slash after a delay
                Destroy(newSlash, destroyDelay);

                // Get the SlashSkillController component (handle potential null reference)
                var newSlashSkillController = newSlash.GetComponent<SlashSkillController>();

                newSlashSkillController.SetupSlash(launchDir * playerDir, slashGravity);
            }*/
        }

        //protected override void CheckUnlock() => UnlockSlash();

        private void UnlockSlash()
        {
            if(slashUnlockButton.unlocked)
                slashUnlocked = true;
        }
    }
}
