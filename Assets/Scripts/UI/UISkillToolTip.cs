using TMPro;
using UnityEngine;

namespace UI
{
    public class UISkillToolTip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI skillText;
        [SerializeField] private TextMeshProUGUI skillName;

        public void ShowToolTip(string skillDescription, string skillTitle)
        {
            skillName.text = skillTitle;
            skillText.text = skillDescription;
            gameObject.SetActive(true);
        }

        public void HideToolTip() => gameObject.SetActive(false);
    }
}
