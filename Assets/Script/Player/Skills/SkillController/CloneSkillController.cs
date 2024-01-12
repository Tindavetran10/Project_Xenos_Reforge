using UnityEngine;

namespace Script.Player.Skills.SkillController
{
    public class CloneSkillController : MonoBehaviour
    {
        public void SetupClone(Transform newTransform)
        {
            transform.position = newTransform.position;
        }
    
    }
}
