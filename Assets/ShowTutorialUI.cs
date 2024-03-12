using System.Collections;
using Manager;
using UnityEngine;

public class ShowTutorialUI : MonoBehaviour
{
    private Player.PlayerStateMachine.Player _player;
    
    // Reference to the tutorial UI elements
    public GameObject[] tutorialUIs;
    public int currentIndex;

    private void Start() => _player = PlayerManager.GetInstance().player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is the player and the current index is within bounds
        if (other.CompareTag("Player") && currentIndex < tutorialUIs.Length)
        {
            // Activate the tutorial UI at the current index
            tutorialUIs[currentIndex].SetActive(true);
            
            currentIndex++; // Move to the next tutorial UI
            
            if (currentIndex == 5)
            {
                // slow down the game
                StartCoroutine(SlowDownGame());
            }
            // If the player press parry button, continue the game
            else if(ContinueForParryTutorial())
            {
                Time.timeScale = 1;
            }
        }
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
        if (other.CompareTag("Player") && currentIndex <= tutorialUIs.Length)
        {
            foreach (var tutorialUI in tutorialUIs)
            {
                // Show the current tutorial UI element
                tutorialUI.SetActive(false);
            }
        }
    }
}