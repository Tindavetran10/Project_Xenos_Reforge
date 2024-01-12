using Script.Player.Skills;
using UnityEngine;

namespace Script.Manager
{
    public class SkillManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public static SkillManager Instance;

        public DashSkill Dash { get; private set; }

        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);
            else Instance = this;
        }

        private void Start()
        {
            Dash = GetComponent<DashSkill>();
        }
    }
}
