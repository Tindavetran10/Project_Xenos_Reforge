using Player.Skills.SkillController;
using UnityEngine;

namespace Player.Skills
{
    public class CloneSkill : Skill
    {
        [Header("Clone Info")]
        [SerializeField] private GameObject clonePrefab;
        
        [SerializeField] private float cloneDuration;
        [SerializeField] private bool canAttack;
        

        public void CreateClone(Transform clonePosition)
        {
            var newClone = Instantiate(clonePrefab);
            newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration, canAttack);
        }
    }
}