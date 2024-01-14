namespace Script.SaveSystem
{
    [System.Serializable]
    public class GameData
    {
        public int currency;
        public SerializableDictionary<string, bool> SkillTree;
        
        
        public GameData()
        {
            currency = 0;
            SkillTree = new SerializableDictionary<string, bool>();
        }
            
    }
}
