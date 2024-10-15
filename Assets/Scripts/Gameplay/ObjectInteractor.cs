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

        public UnityAction<ObjectInteractor> OnInteraction;

        [SerializeField]
        float interactionDistance = 1.5f;

        [SerializeField]
        Collider interactionCollider;

        [SerializeField]
        bool keepEnabled = false;

        [SerializeField]
        float interactionCooldown = .5f;

        bool inside = false;

        System.DateTime lastInteractionTime;


        private void Update()
        {
            if (!inside)
                return;

            if ((System.DateTime.Now - lastInteractionTime).TotalSeconds < interactionCooldown)
                return;

            if(Input.GetKeyDown(KeyBindings.InteractionKey))
            {
                Debug.Log("Button clicked");
                // Raycast
                int layerMask = ~LayerMask.GetMask(Layers.Player); 
                RaycastHit hit;
                if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance, layerMask))
                {
                    Debug.Log($"Hit:{hit.collider.gameObject.name}");
                    if (hit.collider == interactionCollider)
                    {
                        if (!keepEnabled)
                            interactionCollider.enabled = false;

                        //foreach (var callback in callbacks)
                        //    callback.Invoke(this);
                        lastInteractionTime = System.DateTime.Now;
                        OnInteraction?.Invoke(this);

                        
                    }
                }
            }
        }

        //private void OnEnable()
        //{
        //    callbacks.Clear();
        //}

        //private void OnDisable()
        //{
        //    callbacks.Clear();
        //}

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

        //public void SetCallback(UnityAction<ObjectInteractor> callback)
        //{
        //    callbacks.Add(callback);
        //}
    }

}
