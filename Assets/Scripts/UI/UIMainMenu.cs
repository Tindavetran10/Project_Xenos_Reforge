using System.Collections;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private string sceneName = "MainScene";
        [SerializeField] private GameObject continueButton;
        [SerializeField] private UIFadeScreen fadeScreen;
        
        private void Start()
        {
            if(SaveManager.Instance.HasSaveData() == false)
                continueButton.SetActive(false);
        }

        public void ContinueGame() => StartCoroutine(LoadScreenWithAfterEffect(1.5f));

        public void NewGame()
        {
            SaveManager.Instance.DeleteSaveData();
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
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            SceneManager.LoadScene(sceneName);
        }
    }
}
