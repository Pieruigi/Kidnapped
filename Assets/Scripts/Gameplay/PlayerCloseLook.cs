using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class PlayerCloseLook : MonoBehaviour
    {
        public UnityAction OnPlayerLook;

        [SerializeField]
        float lookDistance = 1.5f;

        [SerializeField]
        float lookDuration = 0.5f;

        [SerializeField]
        Collider lookCollider;

        bool inside = false;
        float lookElapsed = 0;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!inside)
                return;

            // Raycast
            int layerMask = ~LayerMask.GetMask(Layers.Player);
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, lookDistance, layerMask))
            {
                if(hit.collider == lookCollider)
                {
                    lookElapsed += Time.deltaTime;
                    if (lookElapsed > lookDuration)
                    {
                        Debug.Log("LOOOOOOOOOOOOK");
                        OnPlayerLook?.Invoke();
                    }
                }
                else
                {
                    lookElapsed = 0;
                }
            }
            else
            {
                lookElapsed = 0;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(Tags.Player))
                inside = true;

            lookElapsed = 0;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.Player))
                inside = false;


        }
    }

}
