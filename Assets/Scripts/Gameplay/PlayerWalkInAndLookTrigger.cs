using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class PlayerWalkInAndLookTrigger : MonoBehaviour
    {
        public UnityAction<PlayerWalkInAndLookTrigger> OnEnter;

        [SerializeField]
        Transform target;

        [SerializeField]
        float angleTollerance;

        bool triggered = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            triggered = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (triggered || !other.CompareTag(Tags.Player))
                return;

            if (IsInView())
            {
                triggered = true;
                OnEnter?.Invoke(this);
            }
                
        }

        bool IsInView()
        {
            // Get the camera forward direction
            Vector3 cameraFwd = Camera.main.transform.forward;
            // Get the direction to the target
            Vector3 directionToTarget = target.position - Camera.main.transform.position;

            // Get the angle between direction and camera forward
            float angleToTarget = Vector3.Angle(cameraFwd, directionToTarget);

            // Check the tollerance
            if(angleToTarget <= angleTollerance)
                return true;
            else
                return false;
        }
    }

}
