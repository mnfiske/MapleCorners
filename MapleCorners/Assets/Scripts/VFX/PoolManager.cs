// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonMonoBehavior<PoolManager>
{
    private Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();
    [SerializeField] private Pool[] pool = null;
    [SerializeField] private Transform objectPoolTransform = null;


    [System.Serializable]
    public struct Pool
    {
        public int poolSize;
        public GameObject prefab;
    }

    private void Start()
    {
        // Create object pools on start
        for (int i = 0; i < pool.Length; i++)
        {
            CreatePool(pool[i].prefab, pool[i].poolSize);
        }
    }


    private void CreatePool(GameObject prefab, int poolSize)
    {
        // Get the key and name from the prefab
        int poolKey = prefab.GetInstanceID();
        string prefabName = prefab.name; 

        // Create a parent object
        GameObject parentGameObject = new GameObject(prefabName + "Anchor");

        // Set the parent of the new parent object to the objectPoolTransform
        parentGameObject.transform.SetParent(objectPoolTransform);

        // Create the game objects as a part of our pool
        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<GameObject>());

            for (int i = 0; i < poolSize; i++)
            {
                GameObject newObject = Instantiate(prefab, parentGameObject.transform) as GameObject;
                newObject.SetActive(false);

                poolDictionary[poolKey].Enqueue(newObject);
            }
        }
    }

    public GameObject ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        // Get the key
        int poolKey = prefab.GetInstanceID();

        // Check if the dictionary contains the key
        if (poolDictionary.ContainsKey(poolKey))
        {
            // If so, get the item to reuse from the pool, using the key
            GameObject objectToReuse = GetObjectFromPool(poolKey);

            // Set the object's position & rotation
            ResetObject(position, rotation, objectToReuse, prefab);

            // Return the object
            return objectToReuse;
        }
        else
        {
            Debug.Log("No object pool for " + prefab);
            return null;
        }
    }


    private GameObject GetObjectFromPool(int poolKey)
    {
        // Dequeue the object from the pool
        GameObject objectToReuse = poolDictionary[poolKey].Dequeue();
        // Add  it back to the queue
        poolDictionary[poolKey].Enqueue(objectToReuse);

        // Check if the object is active
        if (objectToReuse.activeSelf == true)
        {
            // Set it inactive
            objectToReuse.SetActive(false);
        }

        // Return the object
        return objectToReuse;
    }
    private static void ResetObject(Vector3 position, Quaternion rotation, GameObject objectToReuse, GameObject prefab)
    {
        // Set the object's position & rotation
        objectToReuse.transform.position = position;
        objectToReuse.transform.rotation = rotation;
        // Set the local scale back to the prefab's local scale
        objectToReuse.transform.localScale = prefab.transform.localScale;

    }
}
