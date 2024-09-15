using CSA;
using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Kidnapped
{
    public class MoonTrigger : MonoBehaviour
    {

        float targetValue;

        bool inside = false;
        

        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (!inside)
                return;

            // Set the target strength depending whether the player is looking in or out
            if (Vector3.Dot(PlayerController.Instance.transform.forward, transform.forward) > 0) // Looking inside
            {
                MoonController.Instance.TargetStrength = MoonController.InternalStrength;
            }
            else // Looking outside
            {
                MoonController.Instance.TargetStrength = MoonController.ExternalStrength;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;

            inside = true;

            
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;

            inside = false;

            // Forward axis always indicates indoor
            Vector3 dir = Vector3.ProjectOnPlane(transform.position - other.transform.position, Vector3.up);
            if (Vector3.Dot(dir, transform.forward) > 0) // Back
            {
                MoonController.Instance.TargetStrength = MoonController.ExternalStrength;
            }
            else // Front
            {
                MoonController.Instance.TargetStrength = MoonController.InternalStrength;
            }
        }
    }

}
