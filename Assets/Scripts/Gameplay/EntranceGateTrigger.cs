using CSA;
using DG.Tweening;
using EvolveGames;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class EntranceGateTrigger : MonoBehaviour
    {
        [SerializeField]
        Collider _collider;

        [SerializeField]
        MMF_Player[] lockedPlayers;

        [SerializeField]
        Transform leftDoor;

        [SerializeField]
        Transform rightDoor;

        [SerializeField]
        AudioSource lockedAudioSource;

        [SerializeField]
        AudioSource closeAudioSource;

        

        bool isOpen = false;
        bool isInside = false;

        float leftEulerDefault, rightEulerDefault;

        float openAngle = 90;
        float openTime = 1;
        float closeTime = .25f;

        private void Awake()
        {
            leftEulerDefault = leftDoor.localEulerAngles.z;
            rightEulerDefault = rightDoor.localEulerAngles.z;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                if (isOpen) { CloseTheGate(); }
                else { OpenTheGate(); }
            }

            if (isInside)
            {
                if (!isOpen)
                {
                    // You can try to open it, but its closed
                    if (Input.GetKeyDown(KeyBindings.InteractionKey))
                    {

                        RaycastHit hit;
                        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, GameplaySettings.InteractionDistance))
                        {
                            if (hit.collider == _collider)
                            {
                                Debug.Log("The gate is closed");
                                // Call feel
                                foreach (var player in lockedPlayers)
                                {
                                    player.PlayFeedbacks();
                                    lockedAudioSource.Play();   
                                }
                            }

                        }

                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;

            Debug.Log("Entering...");
            isInside = true;


        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;

            isInside = false;

            if (isOpen)
            {
                Vector3 dir = transform.position - other.transform.position;
                Vector3 otherFwd = other.transform.forward;
                if (Vector3.Dot(dir, otherFwd) > 0)
                {
                    // You are inside the school, close the gate
                    isOpen = false;

                }
            }
        }

        void CloseTheGate()
        {
            if (!isOpen)
                return;
            Debug.Log("AAAAAAAAAAAAAAAAAAAAA");
            isOpen = false;
            
            Vector3 leftEndValue = leftDoor.eulerAngles - Vector3.up * openAngle;
            leftDoor.DORotate(leftEndValue, closeTime, RotateMode.Fast);
            Vector3 rightEndValue = rightDoor.eulerAngles + Vector3.up * openAngle;
            rightDoor.DORotate(rightEndValue, closeTime, RotateMode.Fast);
            closeAudioSource.Play();
        }

        void OpenTheGate()
        {
            if (isOpen)
                return;
            Debug.Log("BBBBBBBBBBBBBBBBBBBB");
            isOpen = true;
            Vector3 leftEndValue = leftDoor.eulerAngles + Vector3.up * openAngle;
            leftDoor.DORotate(leftEndValue, openTime, RotateMode.Fast);
            Vector3 rightEndValue = rightDoor.eulerAngles - Vector3.up * openAngle;
            rightDoor.DORotate(rightEndValue, openTime, RotateMode.Fast);
        }
    }

}

