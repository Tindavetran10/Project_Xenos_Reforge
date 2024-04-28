using System.Collections;
using Controller.SkillController;
using Manager;
using UnityEngine;

namespace Player.Skills
{
    public class CloneSkill : Skill
    {
        [Header("Clone")]
        [SerializeField] private GameObject clonePrefab;
        
        [SerializeField] private float cloneDuration;
        [SerializeField] private bool canAttack;
        

        public void CreateClone(Transform clonePosition, Vector3 offset)
        {
            var newClone = ObjectPoolManager.SpawnObject(clonePrefab, clonePosition.position, 
                Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
            newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration, canAttack, offset);
        }
        
        public void CreateCloneWithDelay(Transform clonePosition) =>
            StartCoroutine(CloneDelayCoroutine(clonePosition, 
                new Vector3(0.5f * Player.Movement.FacingDirection, 0.13f)));

        private IEnumerator CloneDelayCoroutine(Transform clonePosition, Vector3 offset)
        {
            yield return new WaitForSeconds(0.4f);
            CreateClone(clonePosition, offset);
        }
    }
}