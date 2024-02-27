using UnityEngine;

namespace InventorySystem_and_Items
{
    [CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
    public class ItemData : ScriptableObject
    {
        public string ItemName;
        public Sprite Icon;
    }
}