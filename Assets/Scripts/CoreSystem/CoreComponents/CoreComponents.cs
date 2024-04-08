using UnityEngine;

namespace CoreSystem.CoreComponents
{
    // Basically, this is a base class for all components that will be added to the Core.
    public class CoreComponent : MonoBehaviour
    {
        // Reference to the Core class.
        protected Core Core;

        protected virtual void Awake()
        {
            // Get the Core component from the parent and add this component to the Core.
            Core = transform.parent.GetComponent<Core>();
            if(Core == null)
                Debug.LogError("There is no Core on the parent");
            Core.AddComponent(this);
        }
        
        public virtual void LogicUpdate(){} 
    }
}