using CSA;
using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class ByDistanceComponentActivator : MonoBehaviour
    {
        [SerializeField]
        List<MonoBehaviour> components;

        [SerializeField]
        float distance = 6;

        List<MonoBehaviour> notEnabledList;
        List<MonoBehaviour> enabledList;

        private void Awake()
        {
            notEnabledList = components;
            enabledList = new List<MonoBehaviour>();    

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            List<MonoBehaviour> tmp = new List<MonoBehaviour>();
            foreach(var component in notEnabledList)
            {
                if(Vector3.Distance(PlayerController.Instance.transform.position, component.gameObject.transform.position) <= distance)
                    tmp.Add(component);
            }
            foreach(var component in tmp)
            {
                if(!component.enabled)
                    component.enabled = true;
                notEnabledList.Remove(component);
                enabledList.Add(component);
            }
        }

       
    }

}
