using _Scripts.CoreSystem;
using UnityEngine;

namespace Script.CoreSystem.CoreComponents
{
    public class CoreComponent : MonoBehaviour
    {
        protected Core Core;

        protected virtual void Awake()
        {
            Core = transform.parent.GetComponent<Core>();
            if(Core == null)
                Debug.LogError("There is no Core on the parent");
            Core.AddComponent(this);
        }
        
        public virtual void LogicUpdate(){} 
    }
}