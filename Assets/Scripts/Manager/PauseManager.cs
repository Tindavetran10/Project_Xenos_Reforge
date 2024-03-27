using UnityEngine;

namespace Manager
{
    public class PauseManager : MonoBehaviour
    {
        public static PauseManager Instance;
        
        public bool IsPaused { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void PauseGame()
        {
            IsPaused = true;
            Time.timeScale = 0f;
        }

        public void UnpauseGame()
        {
            IsPaused = false;
            Time.timeScale = 1f;
        }
    }
}
