using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Manager
{
    public class ObjectPoolManager : MonoBehaviour
    {
        private static readonly List<PooledObjectInfo> ObjectPools = new();
        
        public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
        {
            // Attempt to find the pool that matches the object to spawn. If not, create a new pool
            var pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name) 
                       ?? new PooledObjectInfo {LookupString = objectToSpawn.name};
            
            // If the pool for this object is not in the list, add it
            if (!ObjectPools.Contains(pool)) ObjectPools.Add(pool);

            // Try to get an inactive object from the pool, if not, instantiate a new one
            var spawnableObject = pool.InactiveObjects.FirstOrDefault() ?? Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            
            // Set the position and rotation of the spawned object to the desired values
            spawnableObject.transform.position = spawnPosition;
            spawnableObject.transform.rotation = spawnRotation;
            
            // Activate the spawned object so it's visible and can interact with the world
            spawnableObject.SetActive(true);
            
            // Remove the object from the inactive objects list
            pool.InactiveObjects.Remove(spawnableObject);

            // Return the spawned or activated object
            return spawnableObject;
        }
        
        public static void ReturnObjectToPool(GameObject objectToReturn)
        {
            var goName = objectToReturn.name.Replace("(Clone)", string.Empty);
            var pool = ObjectPools.Find(p => p.LookupString == goName);
            
            if (pool == null)
            {
                Debug.LogWarning("Trying to release an object that is not pooled: " + objectToReturn.name);
                return;
            }
            
            objectToReturn.SetActive(false);
            pool.InactiveObjects.Add(objectToReturn);
        }
    }

    public class PooledObjectInfo
    {
        public string LookupString;
        public readonly List<GameObject> InactiveObjects = new();
    }
}
