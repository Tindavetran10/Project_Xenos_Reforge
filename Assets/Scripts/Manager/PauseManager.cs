using _Scripts.Player.Input;
using UnityEngine;

namespace Manager
{
    public class PauseManager : MonoBehaviour
    {
        private static PauseManager _instance;

        [SerializeField] private InputManager inputManager;
        [SerializeField] private GameObject optionsMenu;
        
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }

        private void Start()
        {
            inputManager.MenuOpenEvent += HandlePause;
            inputManager.MenuCloseEvent += HandleResume;
        }

        private void HandlePause()
        {
            optionsMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        private void HandleResume()
        {
            optionsMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
