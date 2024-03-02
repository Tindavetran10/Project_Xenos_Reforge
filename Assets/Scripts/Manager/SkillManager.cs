using Player.Skills;
using UnityEngine;

namespace Manager
{
    public class SkillManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public static SkillManager Instance;

        public DashSkill Dash { get; private set; }
        public CloneSkill Clone { get; private set; }

        public SlashSkill Slash { get; private set; }
        public FocusSkill Focus { get; private set; }
        
        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);
            else Instance = this;
        }

        private void Start()
        {
            Dash = GetComponent<DashSkill>();
            Clone = GetComponent<CloneSkill>();
            Slash = GetComponent<SlashSkill>();
            Focus = GetComponent<FocusSkill>();
        }
    }
}
