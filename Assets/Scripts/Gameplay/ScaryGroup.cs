using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.AI;

namespace Kidnapped
{
    public class ScaryGroup : MonoBehaviour
    {
        [SerializeField]
        ObjectPool dummyPool;

        [SerializeField]
        Transform[] dummyTargetList;

        [SerializeField]
        GameObject targetToReach;

        [SerializeField]
        List<GameObject> dummies = new List<GameObject>();

        

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
            dummies.Clear();
            foreach(var target in dummyTargetList)
            {
                GameObject dummy = dummyPool.GetFromPool();
                dummy.GetComponent<NavMeshAgent>().enabled = false;
                dummy.transform.position = target.position;
                dummy.transform.rotation = target.rotation;
                dummy.GetComponent<NavMeshAgent>().enabled = true;
                dummies.Add(dummy);

            }
            targetToReach.SetActive(true);
        }

        public void Release()
        {
            foreach(var dummy in dummies)
            {
                dummy.GetComponent<NavMeshAgent>().enabled = false;
                dummyPool.ReturnToPool(dummy);
            }

            dummies.Clear();
            targetToReach.SetActive(false);
        }

    }

}
