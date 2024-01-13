using Script.Player.Skills.SkillController;
using UnityEngine;

namespace Script.Player.Skills
{
    public class SlashSkill : Skill
    {
        [Header("Skill Info")] 
        [SerializeField] private GameObject slashPrefab;
        [SerializeField] private Vector2 launchDir;
        [SerializeField] private float slashGravity;
        [SerializeField] private float  destroyDelay = 1f;
        
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
    }
}
