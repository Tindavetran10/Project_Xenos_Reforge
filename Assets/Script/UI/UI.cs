using System.Collections;
using Script.Manager;
using UnityEngine;

namespace Script.UI
{
    public class UI : MonoBehaviour
    {
        [Header("End screen")]
        [SerializeField] private UIFadeScreen fadeScreen;
        [SerializeField] private GameObject endText;
        [SerializeField] private GameObject winText;
        [SerializeField] private GameObject restartButton;
        [Space]
        
        [SerializeField] private GameObject skillTreeUI;
        [SerializeField] private GameObject inGameUI;
        
        public UISkillToolTip skillToolTip;
        
        // we need this to assign events on skill tree slots before we assign events on skill scripts
        private void Awake()
        {
            SwitchTo(skillTreeUI);
            fadeScreen.gameObject.SetActive(true);
        }

        private void Start() => SwitchTo(inGameUI);

        public void SwitchTo(GameObject menu)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                // we need this to keep fade screen game object active
                var isFadeScreen = transform.GetChild(i).GetComponent<UIFadeScreen>() != null;
                if(!isFadeScreen)
                    transform.GetChild(i).gameObject.SetActive(false);
            }

            if(menu != null) menu.SetActive(true);

            if (GameManager.Instance != null) 
                GameManager.PauseGame(menu != inGameUI);
        }

        private void SwitchWithKeyTo(GameObject menu)
        {
            if (menu != null && menu.activeSelf)
            {
                menu.SetActive(false);
                Time.timeScale = 0;
                CheckForInGameUI();
                return;
            }

            SwitchTo(menu);
        }

        private void CheckForInGameUI()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf
                    && transform.GetChild(i).GetComponent<UIFadeScreen>() == null)
                    return;
            }

            SwitchTo(inGameUI);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
                SwitchWithKeyTo(skillTreeUI);
            else if (Input.GetKeyDown(KeyCode.Escape)) 
                SwitchWithKeyTo(inGameUI);
        }

        public void SwitchOnEndScreen()
        {
            fadeScreen.FadeOut();
            StartCoroutine(EndScreenCoroutine());
        }

        public void SwitchOnWinScreen()
        {
            fadeScreen.FadeOut();
            StartCoroutine(WinScreenCoroutine());
        }

        private IEnumerator EndScreenCoroutine()
        {
            yield return new WaitForSeconds(1);
            endText.SetActive(true);
            yield return new WaitForSeconds(1);
            restartButton.SetActive(true);
        }

        private IEnumerator WinScreenCoroutine()
        {
            yield return new WaitForSeconds(1);
            winText.SetActive(true);
            yield return new WaitForSeconds(1);
            restartButton.SetActive(true);
        }
        
        public void RestartGameButton() => GameManager.RestartScene();
    }
}
