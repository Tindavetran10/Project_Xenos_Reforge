using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    
    // Create a list to keep track of all the enemies
    public List<GameObject> EnemiesToSpawnGradualList = new List<GameObject>();
    
    // Create a list to keep track of all the enemies
    public List<GameObject> EnemiesToSpawnAllList = new List<GameObject>();

    private bool isPlayerReached;
    
    // Start is called before the first frame update
    private void Start()
    {
        isPlayerReached = false;
    }

    // Update is called once per frame
    private void Update()
    {
        //if player reached the trigger, spawn the first enemy from the list EnemiesToSpawnGradualList
        if (isPlayerReached && EnemiesToSpawnGradualList.Count > 0)
        {
            var enemy = EnemiesToSpawnGradualList[0];
            EnemiesToSpawnGradualList.RemoveAt(0);
            Instantiate(enemy, transform.position, Quaternion.identity);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPlayerReached)
        {
            isPlayerReached = true;
            Debug.Log("Player reached the trigger");
        }
    }
}
