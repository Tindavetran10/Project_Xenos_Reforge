using System.Collections;
using System.Linq;
using Script.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Manager
{
    public class GameManager : MonoBehaviour, ISaveManager
    {
        public static GameManager Instance;
        private Transform _player;
        
        [SerializeField] private Checkpoint[] checkpoints;
        [SerializeField] private string closestCheckpointId;
        
        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);
            else Instance = this;
        }

        private void Start()
        {
            checkpoints = FindObjectsOfType<Checkpoint>();
            _player = PlayerManager.Instance.player.transform;
        }

        public static void RestartScene()
        {
            SaveManager.Instance.SaveGame();
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        public void LoadData(GameData data) => StartCoroutine(LoadWithDelay(data));

        private void LoadCheckpoints(GameData data)
        {
            foreach (var checkpoint 
                     in from pair in data.checkpoints 
                     from checkpoint in checkpoints 
                     where checkpoint.id == pair.Key && pair.Value select checkpoint) 
                checkpoint.ActivateCheckPoint();
        }
        
        
        private IEnumerator LoadWithDelay(GameData data)
        {
            yield return new WaitForSeconds(.1f);

            LoadCheckpoints(data);
            LoadClosestCheckPoint(data);
        }
        

        public void SaveData(ref GameData data)
        {
            if(FindClosestCheckPoint() != null)
                data.closestCheckpointID  = FindClosestCheckPoint().id;
            
            data.checkpoints.Clear();
        
            foreach (var checkpoint in checkpoints) 
                data.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
        }
        
        private void LoadClosestCheckPoint(GameData data)
        {
            if (data.closestCheckpointID == null) return;
            
            closestCheckpointId = data.closestCheckpointID;

            foreach (var checkpoint in checkpoints)
            {
                if (closestCheckpointId == checkpoint.id)
                    _player.position = checkpoint.transform.position;
            }
        }

        private Checkpoint FindClosestCheckPoint()
        {
            var closestDistance = Mathf.Infinity;
            Checkpoint closestCheckPoint = null;

            foreach (var checkpoint in checkpoints)
            {
                var distanceToCheckpoint = Vector2.Distance(_player.position,
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
}
