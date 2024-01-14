using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.UI
{
    public class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private string sceneName = "MainScene";

        public void ContinueGame()
        {
            SceneManager.LoadScene(sceneName);
        }

    }
}
