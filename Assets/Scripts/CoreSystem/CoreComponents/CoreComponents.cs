using UnityEngine;

namespace CoreSystem.CoreComponents
{
    // Basically, this is a base class for all components that will be added to the Core.
    public class CoreComponent : MonoBehaviour
    {
        protected Core Core;

        protected virtual void Awake()
        {
            // Find the script that holds the Core class and add whatever inherited this class to it.
            Core = transform.parent.GetComponent<Core>();
            if(Core == null)
                Debug.LogError("There is no Core on the parent");
            Core.AddComponent(this);
        }
        
        public virtual void LogicUpdate(){} 
    }
}