using CSA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class PlayerWalkInTrigger : MonoBehaviour
    {
        [SerializeField]
        MonoBehaviour controller;

        [SerializeField]
        string functionName;

    
        //[SerializeField]
        //Action

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;

            //controller.LaunchTheBall();
            controller.Invoke(functionName, 0);
            
            
        }

       

    }

}
