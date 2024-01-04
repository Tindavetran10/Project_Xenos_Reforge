﻿using UnityEngine;

namespace _Scripts.Generics
{
    public static class GenericNotImplementedError<T>
    {
        public static T TryGet(T value, string name)
        {
            if (value != null)
                return value;
        
            Debug.LogError(typeof(T) + "noi implemented on" + name);
            return default;
        }
    }
}
