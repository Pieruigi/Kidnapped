using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Kidnapped
{
    public class FlyingHeadOnBuildingSite : TripleStateProcessor
    {
        [SerializeField]
        PlayerWalkInAndLookTrigger trigger;

        [SerializeField]
        GameObject headPrefab;

        [SerializeField]
        Transform headTarget;

        [SerializeField]
        GameObject headStuckPrefab;

        [SerializeField]
        Transform headStuckTarget;

        [SerializeField]
        GameObject sign;

        [SerializeField]
        Transform signTarget;

        GameObject head;

        protected override void Awake()
        {
            base.Awake();

            var state = GetState();

        }

        private void OnEnable()
        {
            trigger.OnEnter += HandleOnTriggerEnter;
        }

        private void OnDisable()
        {
            trigger.OnEnter -= HandleOnTriggerEnter;
        }

        private async void HandleOnTriggerEnter(PlayerWalkInAndLookTrigger trigger)
        {
            trigger.gameObject.SetActive(false);

            // Spawn head
            head = Instantiate(headPrefab, headTarget.position, headTarget.rotation);

            // Launch head 
            var rb = head.GetComponent<Rigidbody>();
            rb.AddForce(head.transform.forward * 10, ForceMode.VelocityChange);
            rb.AddTorque(new Vector3(62, 89, 74));

            // Wait 
            await Task.Delay(2000);

            // Flicker
            FlashlightFlickerController.Instance.FlickerOnce(() =>
            {
                // Destroy flying head
                Destroy(head);

                // Set the completed state
                SetState(State.Completed);

                // Show the stuck head
                Initialize(GetState());
            });
        }

        void Initialize(State state)
        {
            switch (state)
            {
                case State.NotReady:
                    trigger.gameObject.SetActive(false);
                    break;
                case State.Ready:
                    trigger.gameObject.SetActive(true);
                    // Move sign
                    sign.transform.position = signTarget.position;
                    sign.transform.rotation = signTarget.rotation;
                    break;
                case State.Completed:
                    trigger.gameObject.SetActive(false);
                    // Move sign
                    sign.transform.position = signTarget.position;
                    sign.transform.rotation = signTarget.rotation;
                    // Show the stuck head
                    head = Instantiate(headStuckPrefab, headStuckTarget.position, headStuckTarget.rotation);
                    break;
            }
        }

    }

}
