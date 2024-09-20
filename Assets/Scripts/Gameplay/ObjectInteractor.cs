using Kidnapped;
using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class ObjectInteractor : MonoBehaviour
    {
   
        [SerializeField]
        float interactionDistance = 1.5f;

        [SerializeField]
        Collider interactionCollider;

        [SerializeField]
        bool multipleInteractionsAllowed = false;

        bool inside = false;
        
        List<UnityAction<ObjectInteractor>> callbacks = new List<UnityAction<ObjectInteractor>>();

        private void Update()
        {
            if (!inside)
                return;

            if(Input.GetKeyDown(KeyBindings.InteractionKey))
            {
                Debug.Log("Button clicked");
                // Raycast
                int layerMask = LayerMask.GetMask(Layers.Interaction); 
                RaycastHit hit;
                if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance, layerMask))
                {
                    Debug.Log($"Hit:{hit.collider.gameObject.name}");
                    if (hit.collider == interactionCollider)
                    {
                        if (!multipleInteractionsAllowed)
                            interactionCollider.enabled = false;

                        foreach (var callback in callbacks)
                            callback.Invoke(this);
                        
                    }
                }
            }
        }

        private void OnEnable()
        {
            callbacks.Clear();
        }

        private void OnDisable()
        {
            callbacks.Clear();
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
        }

        public void SetCallback(UnityAction<ObjectInteractor> callback)
        {
            callbacks.Add(callback);
        }
    }

}
