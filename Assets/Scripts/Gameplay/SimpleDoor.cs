using DG.Tweening;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class SimpleDoor : MonoBehaviour
    {
        [SerializeField]
        float openTime = 1;

        [SerializeField]
        float openAngle = 90;

        [SerializeField]
        MMF_Player lockedEffect;


        DoorController controller;
        Collider coll;

        float angleDefault = 0;

        private void Awake()
        {
            controller = GetComponentInParent<DoorController>();
            coll = GetComponent<Collider>();
            angleDefault = transform.localEulerAngles.y;
            Initialize();
        }

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
            DoorController.OnDoorOpened += HandleOnDoorOpened;
            DoorController.OnDoorOpenFailed += HandleOnDoorOpenFailed;
            DoorController.OnDoorClosed += HandleOnDoorClosed;
            DoorController.OnDoorInitialized += HandleOnDoorInitialized;
        }

        private void OnDisable()
        {
            DoorController.OnDoorOpened -= HandleOnDoorOpened;
            DoorController.OnDoorOpenFailed -= HandleOnDoorOpenFailed;
            DoorController.OnDoorClosed -= HandleOnDoorClosed;
            DoorController.OnDoorInitialized -= HandleOnDoorInitialized;
        }

        void Initialize()
        {
            transform.localEulerAngles = Vector3.up * angleDefault;
            if (controller.IsOpen)
                transform.localEulerAngles += Vector3.up * openAngle;
        }

        private void HandleOnDoorInitialized(DoorController controller)
        {
            if (this.controller != controller)
                return;

            Initialize();
            
        }

        private void HandleOnDoorOpenFailed(DoorController arg0)
        {
            Debug.Log("The door is locked and can't be opened");
            lockedEffect.PlayFeedbacks();
        }

        private void HandleOnDoorClosed(DoorController controller)
        {
            Debug.Log("HandleOnCloseDoor");
            if (this.controller != controller)
                return;
            Vector3 endValue = transform.eulerAngles - Vector3.up * openAngle;
            Debug.Log($"Target angle:{endValue}");
            transform.DORotate(endValue, openTime, RotateMode.Fast);
        }

        private void HandleOnDoorOpened(DoorController controller)
        {
            if (this.controller != controller)
                return;
            // Disable collider to avoid hitting the player
            //coll.enabled = false;
            Vector3 endValue = transform.eulerAngles + Vector3.up * openAngle;
            transform.DORotate(endValue, openTime, RotateMode.Fast);//.onComplete += () => { coll.enabled = true; };
        }
    }

}
