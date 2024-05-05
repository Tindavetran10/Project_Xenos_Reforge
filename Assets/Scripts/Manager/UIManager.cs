using System.Collections;
using _Scripts.Player.Input;
using UI;
using UnityEngine;

namespace Manager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private InputManager inputManager;
        
        [Header("End screen")]
        [SerializeField] private UIFadeScreen fadeScreen;
        [SerializeField] private GameObject endText;
        [SerializeField] private GameObject winText;
        [SerializeField] private GameObject restartButton;
        [Space]

        #region Tab UI system
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private GameObject skillTreeUI;
        [SerializeField] private GameObject largeMapUI;
        [SerializeField] private GameObject miniMapUI;
        [SerializeField] private GameObject craftUI;
        [SerializeField] private GameObject optionsUI;
        [SerializeField] private GameObject inGameUI;
        #endregion
        
        public UISkillToolTip skillToolTip;
        public UICraftWindow craftWindow;

        private void Start()
        {
            // Show the UI like player's Health or score by default, not the menu
            SwitchTo(inGameUI);
            RegisterEventHandlers();
            miniMapUI.SetActive(true);
        }

        private void RegisterEventHandlers()
        {
            inputManager.OptionsOpenEvent += () => ToggleUIWithKey(optionsUI, true);
            inputManager.OptionsCloseEvent += () => ToggleUIWithKey(optionsUI, false);
            
            inputManager.InventoryOpenEvent += () => ToggleUIWithKey(inventoryUI, true);
            inputManager.InventoryCloseEvent += () => ToggleUIWithKey(inventoryUI, false);

            inputManager.SkillTreeOpenEvent += () => ToggleUIWithKey(skillTreeUI, true);
            inputManager.SkillTreeCloseEvent += () => ToggleUIWithKey(skillTreeUI, false);
            
            inputManager.MapOpenEvent += () => ToggleUIWithKey(largeMapUI, true);
            inputManager.MapCloseEvent += () => ToggleUIWithKey(largeMapUI, false);
            
            inputManager.CraftingOpenEvent += () => ToggleUIWithKey(craftUI, true);
            inputManager.CraftingCloseEvent += () => ToggleUIWithKey(craftUI, false);
        }
        
        private void ToggleUIWithKey(GameObject uiElement, bool isActive)
        {
            if (uiElement.activeSelf != isActive && uiElement != null)
            {
                uiElement.SetActive(isActive);
                CheckForInGameUI();
                miniMapUI.SetActive(!isActive);
                Time.timeScale = isActive ? 0f : 1f;
            }
        }

        #region Old Function
        public void SwitchTo(GameObject tabUI)
        {
            // Register every child object and switch it off by default
            for (var i = 0; i < transform.childCount; i++)
            {
                // we need this to keep fade screen game object active
                var isFadeScreen = transform.GetChild(i).GetComponent<UIFadeScreen>() != null;
                if(!isFadeScreen)
                    transform.GetChild(i).gameObject.SetActive(false);
            }

            if(tabUI != null) tabUI.SetActive(true);

            if (GameManager.Instance != null) 
                GameManager.PauseGame(tabUI != inGameUI);
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
            //fadeScreen.FadeOut();
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
