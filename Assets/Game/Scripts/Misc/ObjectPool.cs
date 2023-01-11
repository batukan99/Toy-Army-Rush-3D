using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Managers;
public class ObjectPool : MonoBehaviour
{
    [Serializable]
    public struct Pool
    {
        public Queue<GameObject> pooledObjects;
        public GameObject objectPrefab;
        public int poolSize;
    }

    [SerializeField] private Pool[] pools = null;

    private void Start() {
        ManagerProvider.GetManager<PoolManager>().objectPool = this;
    }
    public GameObject GetPooledObject(int objectType, Vector3 position, Quaternion rotation, Transform parent
    )
    {
        if (objectType >= pools.Length)
        {
            return null;
        }
        GameObject obj =  pools[objectType].pooledObjects.Dequeue();
        obj.SetActive(true);
        obj.transform.SetParent(parent);
        obj.transform.localPosition = position;
        obj.transform.rotation = rotation;

        return obj;
    }

    public void AddToPool(int objectType, GameObject obj) 
    {
        if (objectType >= pools.Length)
        {
           Debug.LogError("There is no object type in pool with index: " + objectType);
        }
        else 
        {
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = transform.position;
            pools[objectType].pooledObjects.Enqueue(obj);
        }
        
    }

    private void OnEnable()
    {
        for (int j = 0; j < pools.Length; j++)
        {
            pools[j].pooledObjects = new Queue<GameObject>();

            for (int i = 0; i <  pools[j].poolSize; i++)
            {
                GameObject obj = Instantiate( pools[j].objectPrefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                pools[j].pooledObjects.Enqueue(obj);
            }
        }
    }
}