using CSA;
using EvolveGames;
using Suburb;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
   
    public class DoorController : MonoBehaviour
    {
        public static UnityAction<DoorController> OnCameraOver; // Tell the player they can press a key to open the door
        public static UnityAction<DoorController> OnCameraExit; // Stop telling the player
        public static UnityAction<DoorController> OnDoorOpened;
        public static UnityAction<DoorController> OnDoorOpenFailed;
        public static UnityAction<DoorController> OnDoorLocked;
        public static UnityAction<DoorController> OnDoorUnlocked;
        public static UnityAction<DoorController> OnDoorClosed;
        public static UnityAction<DoorController> OnDoorInitialized;

        [SerializeField]
        Transform distanceChecker;

        [SerializeField]
        Collider interactionCollider;
            

        [SerializeField]
        bool isOpen = false;
        public bool IsOpen
        {
            get { return isOpen; }
        }

        [SerializeField]
        bool isLocked = false;
        public bool IsLocked
        {
            get { return isLocked; }
        }


        float interactiondDistance = 1.5f;
        bool interactionDisabled = false;
        public bool InteractionDisabled
        {
            get { return interactionDisabled; }
        }

        bool cameraOver = false;
       
        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.C))
            {
                CloseDoor();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                LockDoor();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                OpenDoor();
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                UnlockDoor();
            }
#endif

            // If the player is too far return
            if (Vector3.Distance(Camera.main.transform.position, distanceChecker.position) > interactiondDistance)
                return;

            // If interaction is disabled return
            if(PlayerController.Instance.InteractionDisabled || interactionDisabled) return;

            // Check for interaction
            // Ray cast
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, interactiondDistance))
            {
                //Debug.Log($"Hit:{hit.collider.gameObject.name}");
                if(!cameraOver)
                {
                    cameraOver = true;
                    OnCameraOver?.Invoke(this);
                }

                // Input
                if (Input.GetKeyDown(KeyBindings.InteractionKey))
                {
                    if(!isLocked)
                    {
                        Debug.Log("Open the door");
                        isOpen = true;
                        SetInteractionDisable(true);
                        OnDoorOpened?.Invoke(this);
                    }
                    else
                    {
                        Debug.Log("Door is locked");
                        OnDoorOpenFailed?.Invoke(this);
                    }
                }
            }
            else
            {
                if(cameraOver)
                {
                    cameraOver = false;
                    OnCameraExit?.Invoke(this);
                }
            }

            
        }

       

        void SetInteractionDisable(bool value)
        {
            interactionDisabled = value;
            if (cameraOver)
            {
                cameraOver = false;
                OnCameraExit?.Invoke(this);
            }
        }

        public void CloseDoor()
        {
            Debug.Log("Close door");
            if(!isOpen) return;
            Debug.Log("Door is open");
            isOpen = false;
            SetInteractionDisable(false);
            OnDoorClosed?.Invoke(this);
        }

        public void OpenDoor()
        {
            if(isLocked || isOpen) return;
            isOpen = true;
            SetInteractionDisable(true);
            OnDoorOpened?.Invoke(this);
        }

        public void UnlockDoor()
        {
            isLocked = false;
            OnDoorUnlocked?.Invoke(this);
        }

        public void LockDoor()
        {
            if (isOpen)
                return;
            isLocked = true;
            OnDoorLocked?.Invoke(this);
        }

        public void Init(bool isLocked, bool isOpened, bool interactionDisabled)
        {
            if(isLocked && isOpen)
            {
                Debug.LogWarning($"Door can't be at the same time locked and open: {gameObject.name}");
                return;
            }

            this.isLocked = isLocked;
            this.isOpen = isOpened;
            this.interactionDisabled = interactionDisabled;
           
            OnDoorInitialized?.Invoke(this);
        }
    }

}
