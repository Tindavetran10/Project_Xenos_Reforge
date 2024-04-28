using Player.Skills;
using UnityEngine;

namespace Manager
{
    public class SkillManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public static SkillManager instance;

        public DashSkill Dash { get; private set; }
        public CloneSkill Clone { get; private set; }

        public SlashSkill Slash { get; private set; }
        public FocusSkill Focus { get; private set; }
        
        public ParrySkill Parry { get; private set; }
        
        private void Awake()
        {
            if (instance != null)
                Destroy(instance.gameObject);
            else instance = this;
        }

        private void Start()
        {
            Dash = GetComponent<DashSkill>();
            Clone = GetComponent<CloneSkill>();
            Slash = GetComponent<SlashSkill>();
            Focus = GetComponent<FocusSkill>();
            Parry = GetComponent<ParrySkill>();
        }
    }
}
