using UnityEngine;

namespace InventorySystem_and_Items.Effects
{
    public class ItemEffect : ScriptableObject
    {
        public virtual void ExecuteEffect(Transform enemyPosition) => 
            Debug.Log("Item effect executed");
    }
}