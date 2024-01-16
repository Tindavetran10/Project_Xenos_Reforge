namespace Script.SaveSystem
{
    [System.Serializable]
    public class GameData
    {
        public int currency;
        public SerializableDictionary<string, bool> skillTree;

        public SerializableDictionary<string, bool> checkpoints;
        public string closestCheckpointID ;
        
        public GameData()
        {
            currency = 0;
            skillTree = new SerializableDictionary<string, bool>();

            closestCheckpointID  = string.Empty;
            checkpoints = new SerializableDictionary<string, bool>();
        }
            
    }
}
