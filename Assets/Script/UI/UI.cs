using System.Collections;
using UnityEngine;

namespace Script.UI
{
    public class UI : MonoBehaviour
    {
        [Header("End screen")]
        [SerializeField] private UiFadeScreen fadeScreen;
        [SerializeField] private GameObject endText;
        [SerializeField] private GameObject restartButton;
        [Space]
        
        [SerializeField] private GameObject skillTreeUI;
        [SerializeField] private GameObject inGameUI;
        
        public UISkillToolTip skillToolTip;
        private const float UIToggleCooldown = 0.5f; // Set an appropriate cooldown time
        private float _lastToggleTime;
        //public Player.PlayerStateMachine.Player player;
        
        private void Awake() => SwitchTo(skillTreeUI); // we need this to assign events on skill tree slots before we assign events on skill scripts

        private void Start() => SwitchTo(inGameUI);

        public void SwitchTo(GameObject menu)
        {
            
            for (int i = 0; i < transform.childCount; i++)
            {
                bool isFadeScreen = transform.GetChild(i).GetComponent<UiFadeScreen>() != null;
                if(!isFadeScreen)
                    transform.GetChild(i).gameObject.SetActive(false);
            }

            if(menu != null)
                menu.SetActive(true);
        }

        public void SwitchWithKeyTo(GameObject menu)
        {
            if (menu != null && menu.activeSelf)
            {
                menu.SetActive(false);
                CheckForInGameUI();
                return;
            }

            SwitchTo(menu);
        }

        private void CheckForInGameUI()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf)
                    return;
            }

            SwitchTo(inGameUI);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (Time.time - _lastToggleTime > UIToggleCooldown)
                {
                    SwitchWithKeyTo(skillTreeUI);
                    _lastToggleTime = Time.time;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SwitchWithKeyTo(inGameUI);
            }
        }

        public void SwitchOnEndScreen()
        {
            fadeScreen.FadeOut();
            StartCoroutine(EndScreenCoroutine());
        }

        private IEnumerator EndScreenCoroutine()
        {
            yield return new WaitForSeconds(1);
            endText.SetActive(true);
            yield return new WaitForSeconds(1);
            restartButton.SetActive(true);
        }
        
        public void RestartGameButton() => GameManager.Instance.RestartScene();
    }
}
