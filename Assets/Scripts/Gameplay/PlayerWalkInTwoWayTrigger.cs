using Kidnapped;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class PlayerWalkInTwoWayTrigger : MonoBehaviour
    {
        public UnityAction</*From behind*/bool> OnExit;
       
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;

            

            // Forward axis always indicates indoor
            Vector3 dir = Vector3.ProjectOnPlane(transform.position - other.transform.position, Vector3.up);
            if (Vector3.Dot(dir, transform.forward) > 0) // Back
            {
                OnExit?.Invoke(true);
            }
            else // Front
            {
                OnExit?.Invoke(false);
            }
        }
    }

}
