using System.Collections;
using Script.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.UI
{
    public class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private string sceneName = "MainScene";
        [SerializeField] private GameObject continueButton;
        [SerializeField] private UiFadeScreen fadeScreen;
        
        private void Start()
        {
            if(SaveManager._instance.HasSaveData() == false)
                continueButton.SetActive(false);
        }

        public void ContinueGame()
        {
            StartCoroutine(LoadScreenWithAfterEffect(1.5f));
        }

        public void NewGame()
        {
            SaveManager._instance.DeleteSaveData();
            StartCoroutine(LoadScreenWithAfterEffect(1.5f));
            
        }

        public void ExitGame()
        {
            Debug.Log("Exit Game");
            Application.Quit();
        }

        private IEnumerator LoadScreenWithAfterEffect(float delay)
        {
            fadeScreen.FadeOut();
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(sceneName);
        }
    }
}
