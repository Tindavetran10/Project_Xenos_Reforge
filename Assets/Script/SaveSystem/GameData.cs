namespace Script.SaveSystem
{
    [System.Serializable]
    public class GameData
    {
        public int currency;
        public SerializableDictionary<string, bool> SkillTree;

        public SerializableDictionary<string, bool> checkpoints;
        public string closetCheckpointID;
        
        public GameData()
        {
            currency = 0;
            SkillTree = new SerializableDictionary<string, bool>();

            closetCheckpointID = string.Empty;
            checkpoints = new SerializableDictionary<string, bool>();
        }
            
    }
}
