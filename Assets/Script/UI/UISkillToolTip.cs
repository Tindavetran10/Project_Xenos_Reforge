using TMPro;
using UnityEngine;

namespace Script.UI
{
    public class UISkillToolTip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI skillText;
        [SerializeField] private TextMeshProUGUI skillName;

        public void ShowToolTip(string skillDescription, string _skillName)
        {
            skillName.text = _skillName;
            skillText.text = skillDescription;
            gameObject.SetActive(true);
        }

        public void HideToolTip() => gameObject.SetActive(false);
    }
}
