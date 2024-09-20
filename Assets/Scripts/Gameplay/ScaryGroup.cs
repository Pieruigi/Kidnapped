using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.AI;

namespace Kidnapped
{
    public class ScaryGroup : MonoBehaviour
    {
        [System.Serializable]
        class PrefabGroup
        {
            [SerializeField]
            public ObjectPool dummyPool;

            [SerializeField]
            public Transform[] dummyTargetList;

            public List<GameObject> dummies = new List<GameObject>();
        }

        
        [SerializeField]
        GameObject targetToReach;

        [SerializeField]
        List<PrefabGroup> prefabGroups;

        //[SerializeField]
        

        

        //private void OnEnable()
        //{
        //    Create();    
        //}

        //private void OnDisable()
        //{
        //    Release();
        //}

        public void Create()
        {
            

            foreach(var pg in prefabGroups)
            {
                pg.dummies.Clear();
                foreach (var target in pg.dummyTargetList)
                {
                    GameObject dummy = pg.dummyPool.GetFromPool();
                    dummy.GetComponent<NavMeshAgent>().enabled = false;
                    dummy.transform.position = target.position;
                    dummy.transform.rotation = target.rotation;
                    dummy.GetComponent<NavMeshAgent>().enabled = true;
                    pg.dummies.Add(dummy);
                }
            }

           
            targetToReach.SetActive(true);
        }

        public void Release()
        {
            foreach(var pg in prefabGroups)
            {
                foreach (var dummy in pg.dummies)
                {
                    dummy.GetComponent<NavMeshAgent>().enabled = false;
                    pg.dummyPool.ReturnToPool(dummy);
                }
                pg.dummies.Clear();
            }

            targetToReach.SetActive(false);
        }

    }

}
