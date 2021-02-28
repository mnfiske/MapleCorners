using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MesObjectPooler : MonoBehaviour
{
	[System.Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int size;
	}

	#region Singleton
	public static MesObjectPooler Instance;
	private void Awake()
	{
		Instance = this;
	}
	#endregion


	public List<Pool> pools;
	public Dictionary<string, Queue<GameObject>> poolDictionary;

	void Start()
	{
		poolDictionary = new Dictionary<string, Queue<GameObject>>();

		foreach (Pool pool in pools)
		{
			Queue<GameObject> objectPool = new Queue<GameObject>();

			// Instantiate objects to reach pool size
			for (int i = 0; i < pool.size; i++)
			{
				GameObject obj = Instantiate(pool.prefab);
				obj.SetActive(false);
				objectPool.Enqueue(obj);
			}

			// Add pool to dictionary
			poolDictionary.Add(pool.tag, objectPool);
		}
	}

	// Create pool
	public GameObject SpawnFromPool(string tag)
	{
		// check if tag exists
		if (!poolDictionary.ContainsKey(tag))
		{
			Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
			return null;
		}

		// May need to be a Vector3 with z-value
		Vector2 newRandomPosition = new Vector2(
			Random.Range(-5, 5),
			Random.Range(-4, 2)
		);
		/* Consider whole screen random
			Random.Range(-screenBounds.x, screenBounds.x),
			Random.Range(-screenBounds.y, screenBounds.y)
		*/

		// Get object out of pool
		GameObject objectToSpawn = poolDictionary[tag].Dequeue();
		objectToSpawn.SetActive(true);
		objectToSpawn.AddComponent<ModifyPlayerSpeed>();
		objectToSpawn.transform.position = newRandomPosition;

		poolDictionary[tag].Enqueue(objectToSpawn);
		return objectToSpawn;
	}

	// Check that pool has minimum number of objects
	public void RefillPool(string tag)
	{
		// check if tag exists
		if (!poolDictionary.ContainsKey(tag))
		{
			Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
		}

		foreach (Pool pool in pools)
		{
			Queue<GameObject> objectPool = new Queue<GameObject>();

			// Empty Queueue
			poolDictionary[tag].Clear();

			// Instantiate objects to reach pool size
			for (int i = 0; i < pool.size; i++)
			{
				GameObject obj = Instantiate(pool.prefab);
				obj.SetActive(false);
				// Need to explicitly add script
				obj.AddComponent<ModifyPlayerSpeed>();

				// Add to queue
				poolDictionary[tag].Enqueue(obj);
			}
		}
	}
}