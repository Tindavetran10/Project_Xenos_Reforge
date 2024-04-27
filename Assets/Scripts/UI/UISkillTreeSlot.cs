using System.Linq;
using Manager;
using SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UISkillTreeSlot : MonoBehaviour, ISaveManager
    {
        private UIManager _uiManager;
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
            _uiManager = GetComponentInParent<UIManager>();
            
            _skillImage.color = lockedSkillColor;
            
            if(unlocked) _skillImage.color =Color.white;
        }

        public void UnlockSkillSlot()
        {
            // Check if the player has enough energy to unlock the skill
            if(PlayerManager.GetInstance().HaveEnoughEnergy(skillCost) == false)
                return;
            
            // Check if the skills that should be unlocked are unlocked
            if (shouldBeUnlocked.Any(skillSlot => skillSlot.unlocked == false))
            {
                Debug.Log("Cannot unlock skill");
                return;
            }


            if (shouldBeLocked.Any(skillSlot => skillSlot.unlocked))
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
            
            unlocked = true; // Set the unlocked field to true
            _skillImage.color = Color.white; // Set the color of the skill image to white
        }


        public void LoadData(GameData data)
        {
            if (data.skillTree.TryGetValue(skillName, out var value)) 
                unlocked = value;
        }

        public void SaveData(ref GameData data)
        {
            if (data.skillTree.Remove(skillName, out _))
                data.skillTree.Add(skillName, unlocked);
            else data.skillTree.Add(skillName, unlocked);
        }
        
        /*public void OnPointerEnter(PointerEventData eventData) => _uiManager.skillToolTip.ShowToolTip(skillDescription, skillName);
        public void OnPointerExit(PointerEventData eventData) => _uiManager.skillToolTip.HideToolTip();*/
    }
}
