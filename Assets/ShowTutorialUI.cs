using System.Collections;
using Manager;
using UnityEngine;

public class ShowTutorialUI : MonoBehaviour
{
    private Player.PlayerStateMachine.Player _player;
    // Reference to the tutorial UI elements
    public GameObject tutorialUIs;

    private void Start() => _player = PlayerManager.GetInstance().player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
            tutorialUIs.SetActive(true);
    }
    
    private bool ContinueForParryTutorial() => _player.InputHandler.CounterInput;
    
    private static IEnumerator SlowDownGame()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(1);
        Time.timeScale = 1;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
            tutorialUIs.SetActive(false);
    }
}