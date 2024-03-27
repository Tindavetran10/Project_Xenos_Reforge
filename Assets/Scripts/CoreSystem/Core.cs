using System.Collections.Generic;
using System.Linq;
using CoreSystem.CoreComponents;
using UnityEngine;

namespace CoreSystem
{
    public class Core : MonoBehaviour
    {
        private readonly List<CoreComponent> _coreComponents = new();
    
        public void LogicUpdate()
        {
            foreach (var component in _coreComponents) 
                component.LogicUpdate();
        }

        public void AddComponent(CoreComponent component)
        {
            if(!_coreComponents.Contains(component))
                _coreComponents.Add(component);
        }

        private T GetCoreComponent<T>() where T : CoreComponent
        {
            var comp = _coreComponents.OfType<T>().FirstOrDefault();

            if (comp) return comp;

            comp = GetComponentInChildren<T>();

            if (comp) return comp;

            Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
            return null;
        }

        public T GetCoreComponent<T>(ref T value) where T : CoreComponent
        {
            value = GetCoreComponent<T>();
            return value;
        }
    }
}
