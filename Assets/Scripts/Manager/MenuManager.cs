using System.Collections;
using _Scripts.Player.Input;
using UI;
using UnityEngine;

namespace Manager
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private InputManager inputManager;
        [SerializeField] private GameObject optionsMenu;
        
        [Header("End screen")]
        [SerializeField] private UIFadeScreen fadeScreen;
        [SerializeField] private GameObject endText;
        [SerializeField] private GameObject winText;
        [SerializeField] private GameObject restartButton;

        [Space]

        #region Tab UI system
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private GameObject skillTreeUI;
        [SerializeField] private GameObject mapUI;
        [SerializeField] private GameObject craftUI;
        [SerializeField] private GameObject optionsUI;
        [SerializeField] private GameObject inGameUI;
        #endregion
        
        public UISkillToolTip skillToolTip;
        

        #region Old Function
        public void SwitchTo(GameObject menu)
        {
            // Register every child object and switch it off by default
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
        #endregion
    }
}
