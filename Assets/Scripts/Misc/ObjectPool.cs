using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class ObjectPool : MonoBehaviour
    {
        // Object prefab
        [SerializeField] private GameObject prefab;

        // Initial size
        [SerializeField] private int poolSize = 10;

        // Polled objects
        private List<GameObject> pool;

        private void Awake()
        {
            InitializePool();
        }

        void InitializePool()
        {
            Debug.Log("INitialize pool");
            if (pool != null)
                return;

            // Initialize 
            pool = new List<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                // Create the object and deactivate it
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }

        // Get from pool
        public GameObject GetFromPool()
        {
            Debug.Log("GetFromPool");
            if (pool == null)
                InitializePool();

            
            // Get an object from the pool
            foreach (GameObject obj in pool)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    Debug.Log("Get object:"+obj);
                    return obj;
                }
            }

            // No object found, create a new one
            GameObject newObj = Instantiate(prefab);
            pool.Add(newObj);
            return newObj;
        }

        // Back to pool
        public void ReturnToPool(GameObject obj)
        {
            Debug.Log("Return to pool");
            obj.SetActive(false);
        }
    }
}
