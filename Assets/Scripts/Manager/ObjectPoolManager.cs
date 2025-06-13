using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Manager
{
    public class ObjectPoolManager : MonoBehaviour
    {
        private static readonly List<PooledObjectInfo> ObjectPools = new();
        
        private GameObject _objectPoolEmptyHolder;
        private static GameObject _particleSystemEmpty;
        private static GameObject _gameObjectEmpty;
        public enum PoolType
        {
            ParticleSystem, 
            GameObject,
            None
        }

        private void Awake() => SetupEmpties();

        private void SetupEmpties()
        {
            _objectPoolEmptyHolder = new GameObject("Pooled Objects");
            
            _particleSystemEmpty = new GameObject("Particle Systems");
            _particleSystemEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
            
            _gameObjectEmpty = new GameObject("Game Objects");
            _gameObjectEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
        }

        public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
        {
            // Attempt to find the pool that matches the object to spawn. If not, create a new pool
            var pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name) 
                       ?? new PooledObjectInfo {LookupString = objectToSpawn.name};
            
            // If the pool for this object is not in the list, add it
            if (!ObjectPools.Contains(pool)) ObjectPools.Add(pool);

            // Try to get an inactive object from the pool, if not, instantiate a new one
            var spawnableObject = pool.InactiveObjects.FirstOrDefault();

            if (spawnableObject == null)
            {
                //Find the parent object to set the object to spawn
                var parentObject = SetParentObject(poolType);
                
                spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
                
                if(parentObject != null) 
                    spawnableObject.transform.SetParent(parentObject.transform);
            }
            else
            {
                // Set the position and rotation of the spawned object to the desired values
                spawnableObject.transform.position = spawnPosition;
                spawnableObject.transform.rotation = spawnRotation;
                
                // Activate the spawned object so it's visible and can interact with the world
                spawnableObject.SetActive(true);
                
                // Remove the object from the inactive objects list
                pool.InactiveObjects.Remove(spawnableObject);
            }
            
            // Return the spawned or activated object
            return spawnableObject;
        }
        
        public static void ReturnObjectToPool(GameObject objectToReturn)
        {
            // Remove "(Clone)" from the object's name to find the original prefab name
            var goName = objectToReturn.name.Replace("(Clone)", string.Empty);
            
            // Find the pool that matches the object's name to return
            var pool = ObjectPools.Find(p => p.LookupString == goName);
            
            // If no matching pool is found, log a warning and exit the method
            if (pool == null)
            {
                Debug.LogWarning("Trying to release an object that is not pooled: " + objectToReturn.name);
                return;
            }
            
            // Deactivate the object to return and add it to the inactive objects list
            objectToReturn.SetActive(false);
            pool.InactiveObjects.Add(objectToReturn);
        }
        
        private static GameObject SetParentObject(PoolType poolType)
        {
            return poolType switch
            {
                PoolType.ParticleSystem => _particleSystemEmpty,
                PoolType.GameObject => _gameObjectEmpty,
                PoolType.None => null,
                _ => null
            };
        }
    }

    public class PooledObjectInfo
    {
        // The name of the object to spawn
        public string LookupString;
        // List of inactive objects that can be used to spawn new objects
        public readonly List<GameObject> InactiveObjects = new();
    }
}
