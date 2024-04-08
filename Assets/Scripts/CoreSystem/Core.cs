using System.Collections.Generic;
using System.Linq;
using CoreSystem.CoreComponents;
using UnityEngine;

namespace CoreSystem
{
    // This is the Core class that will hold all the CoreComponents.
    public class Core : MonoBehaviour
    {
        // The core will have a list of CoreComponents.
        private readonly List<CoreComponent> _coreComponents = new();
    
        // The LogicUpdate will call the LogicUpdate of all the CoreComponents.
        public void LogicUpdate()
        {
            foreach (var component in _coreComponents) 
                component.LogicUpdate();
        }

        // The AddComponent will add a CoreComponent to the list
        // if the component is not already in the list.
        public void AddComponent(CoreComponent component)
        {
            if(!_coreComponents.Contains(component))
                _coreComponents.Add(component);
        }

        /*private T GetCoreComponent<T>() where T : CoreComponent
        {
            var comp = _coreComponents.OfType<T>().FirstOrDefault();

            if (comp) return comp;

            comp = GetComponentInChildren<T>();

            if (comp) return comp;

            Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
            return null;
        }*/

        // The GetCoreComponent will return the CoreComponent with the specified type from the _coreComponent list.
        public T GetCoreComponent<T>(ref T value) where T : CoreComponent
        {
            /*value = GetCoreComponent<T>();
            return value;*/
            
            value = _coreComponents.OfType<T>().FirstOrDefault() ?? GetComponentInChildren<T>();
            if (value == null)
                Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
            return value;
        }
    }
}
