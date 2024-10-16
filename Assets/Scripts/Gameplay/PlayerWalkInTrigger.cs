using Kidnapped;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class PlayerWalkInTrigger : MonoBehaviour
    {
        public UnityAction<PlayerWalkInTrigger> OnEnter;
        public UnityAction<PlayerWalkInTrigger> OnExit;

        //[SerializeField]
        //MonoBehaviour controller;

        //[SerializeField]
        //string functionName;


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

            OnEnter?.Invoke(this);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;

            OnExit?.Invoke(this);
        }

    }

}
