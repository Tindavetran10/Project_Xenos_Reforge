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
            var pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);
            
            if (pool == null)
            {
                pool = new PooledObjectInfo {LookupString = objectToSpawn.name};
                ObjectPools.Add(pool);
            }

            var spawnableObject = pool.InactiveObjects.FirstOrDefault();
            
            if (spawnableObject == null)
            {
                spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
                //pool.InactiveObjects.Add(spawnableObject);
            }
            else
            {
                spawnableObject.transform.position = spawnPosition;
                spawnableObject.transform.rotation = spawnRotation;
                spawnableObject.SetActive(true);
                pool.InactiveObjects.Remove(spawnableObject);
            }

            return spawnableObject;
        }
        
        public static void ReturnObjectToPool(GameObject objectToReturn)
        {
            var goName = objectToReturn.name.Replace("(Clone)", string.Empty);
            var pool = ObjectPools.Find(p => p.LookupString == goName);
            
            if (pool == null)
                Debug.LogWarning("Trying to release an object that is not pooled: " + objectToReturn.name);
            else
            {
                objectToReturn.SetActive(false);
                pool.InactiveObjects.Add(objectToReturn);
            }
        }
    }

    public class PooledObjectInfo
    {
        public string LookupString;
        public readonly List<GameObject> InactiveObjects = new();
    }
}
