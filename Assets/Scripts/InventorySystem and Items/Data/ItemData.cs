using System.Text;
using UnityEngine;

namespace InventorySystem_and_Items.Data
{
    [CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
    public class ItemData : ScriptableObject
    {
        public EnumList.ItemType itemType;
        public string itemName;
        public Sprite icon;
        
        [Range(0, 100)]
        public float dropChance;
        
        protected readonly StringBuilder Sb = new();
    }
}