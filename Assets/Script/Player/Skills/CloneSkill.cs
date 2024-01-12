using Script.Player.Skills.SkillController;
using UnityEngine;

namespace Script.Player.Skills
{
    public class CloneSkill : Skill
    {
        [SerializeField] private GameObject clonePrefab;

        public void CreateClone(Transform clonePosition)
        {
            GameObject newClone = Instantiate(clonePrefab);
            
            newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition);
        }
    }
}