using UnityEngine;

namespace Script.SaveSystem
{
    [System.Serializable]
    public class GameData
    {
        public int playerHealth;
        public Vector3 playerPosition;
        public int currency;
        public SerializableDictionary<string, bool> skillTree = new();
        public SerializableDictionary<string, bool> checkpoints = new();
        public string closestCheckpointID = string.Empty;
    }
}
