using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPoolBase<T> : MonoBehaviour where T : MonoBehaviour
{
	[SerializeField] int poolSize = 20;
	[SerializeField] T poolingPrefab;
	Queue<T> objectPool = new Queue<T>();
	private void Start()
	{
		InitializePool();
	}

	public void InitializePool()
	{
		for (int i = 0; i < poolSize; i++)
		{
			var poolObject = Instantiate(poolingPrefab, Vector3.zero, Quaternion.identity, transform);
			poolObject.gameObject.SetActive(false);
			objectPool.Enqueue(poolObject);
		}
	}
	public T GetPooledObject()
	{
		if (objectPool.Count > 0)
		{
			T pooledObject = objectPool.Dequeue();
			return pooledObject;
		}
		else
		{
			InitializePool();
			return GetPooledObject();
		}
	}
	public void ReturnPooledObject(T returnedObject)
	{
		returnedObject.gameObject.SetActive(false);
		objectPool.Enqueue(returnedObject);
	}
}
