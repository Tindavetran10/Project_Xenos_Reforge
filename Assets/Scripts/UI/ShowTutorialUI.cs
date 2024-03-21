using System.Collections;
using Manager;
using UnityEngine;

namespace UI
{
    public class ShowTutorialUI : MonoBehaviour
    {
        private Player.PlayerStateMachine.Player _player;
        // Reference to the tutorial UI elements
        public GameObject tutorialUIs;

        private void Start() => _player = PlayerManager.GetInstance().player;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Player") && tutorialUIs.name == "Parry")
            {
                StartCoroutine(SlowDownGame());
                tutorialUIs.SetActive(true);
            }

            if (other.CompareTag("Player")) 
                tutorialUIs.SetActive(true);
        }
    
        private bool ContinueForParryTutorial() => _player.playerInputHandler.CounterInput;
    
        private IEnumerator SlowDownGame()
        {
            Time.timeScale = 0.5f;
            yield return new WaitUntil(ContinueForParryTutorial);
            Time.timeScale = 1;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player")) 
                InvokeDisableTutorialUI();
        }
    
        //Invoke method to disable the tutorial UI after a certain amount of time
        private void InvokeDisableTutorialUI() => Invoke(nameof(DisableTutorialUI), 0.15f);
        private void DisableTutorialUI() => tutorialUIs.SetActive(false);
    }
}