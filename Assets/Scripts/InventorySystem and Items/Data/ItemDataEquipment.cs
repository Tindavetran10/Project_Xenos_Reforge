using UnityEngine;

namespace InventorySystem_and_Items.Data
{
    public enum EquipmentType
    {
        Weapon,
        Armor,
        Gauntlet,
        Helmet,
        Boots
    }
    
    [CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
    public class ItemDataEquipment : ItemData
    {
        public EquipmentType equipmentType;
    }
}