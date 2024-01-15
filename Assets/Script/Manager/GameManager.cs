using System;
using System.Collections.Generic;
using Script;
using Script.Manager;
using Script.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager Instance;
    [SerializeField] private Checkpoint[] _checkpoints;
    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else Instance = this;
    }

    private void Start()
    {
        _checkpoints = FindObjectsOfType<Checkpoint>();
    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData data)
    {
        foreach (KeyValuePair<string, bool> pair  in data.checkpoints)
        {
            foreach (Checkpoint checkpoint in _checkpoints)
            {
                if (checkpoint.id == pair.Key && pair.Value) 
                    checkpoint.ActivateCheckPoint();
            }
        }

        foreach (Checkpoint checkpoint in _checkpoints)
        {
            if (data.closetCheckpointID == checkpoint.id)
                PlayerManager.Instance.player.transform.position = checkpoint.transform.position;
        }
    }

    public void SaveData(ref GameData data)
    {
        data.closetCheckpointID = FindClosestCheckPoint().id;
        data.checkpoints.Clear();
        
        foreach (var checkpoint in _checkpoints)
        {
            data.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
        }
    }

    private Checkpoint FindClosestCheckPoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckPoint = null;

        foreach (var checkpoint in _checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(PlayerManager.Instance.player.transform.position,
                checkpoint.transform.position);
            if (distanceToCheckpoint < closestDistance && checkpoint.activationStatus)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckPoint = checkpoint;
            }
        }

        return closestCheckPoint;
    }
}
