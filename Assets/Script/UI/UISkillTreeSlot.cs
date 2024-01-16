using System.Linq;
using Script.Manager;
using Script.SaveSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.UI
{
    public class UISkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
    {
        private UI _ui;
        private Image _skillImage;

        [SerializeField] private int skillCost;
        [SerializeField] private string skillName;

        [TextArea]
        [SerializeField] private string skillDescription;
        [SerializeField] private Color lockedSkillColor;

        public bool unlocked;

        [SerializeField] private UISkillTreeSlot[] shouldBeUnlocked;
        [SerializeField] private UISkillTreeSlot[] shouldBeLocked;


        private void OnValidate() => gameObject.name = "UISkillTreeSlot - " + skillName;

        public void Awake() => GetComponent<Button>().onClick.AddListener(UnlockSkillSlot);
        
        private void Start()
        {
            _skillImage = GetComponent<Image>();
            _ui = GetComponentInParent<UI>();
            
            _skillImage.color = lockedSkillColor;
            
            if(unlocked) _skillImage.color =Color.white;
        }

        private void UnlockSkillSlot()
        {
            if(PlayerManager.Instance.HaveEnoughSoul(skillCost) == false)
                return;
            
            if (shouldBeUnlocked.Any(t => t.unlocked == false))
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
            
            if (shouldBeLocked.Any(t => t.unlocked == false))
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
            
            unlocked = true;
            _skillImage.color = Color.white;
        }


        public void LoadData(GameData data)
        {
            if (data.skillTree.TryGetValue(skillName, out bool value)) 
                unlocked = value;
        }

        public void SaveData(ref GameData data)
        {
            if (data.skillTree.Remove(skillName, out _))
                data.skillTree.Add(skillName, unlocked);
            else data.skillTree.Add(skillName, unlocked);
        }
        
        public void OnPointerEnter(PointerEventData eventData) => _ui.skillToolTip.ShowToolTip(skillDescription, skillName);
        public void OnPointerExit(PointerEventData eventData) => _ui.skillToolTip.HideToolTip();
    }
}
