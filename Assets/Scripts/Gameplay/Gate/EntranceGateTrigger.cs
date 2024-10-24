using Aura2API;
using Kidnapped;
using DG.Tweening;
using EvolveGames;
using Kidnapped.SaveSystem;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class EntranceGateTrigger : MonoBehaviour, ISavable
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

        [SerializeField]
        GameObject leftBlock;

        [SerializeField]
        GameObject[] rightBlocks;

        [SerializeField]
        Light[] leftLights;

        [SerializeField]
        Light[] rightLights;

        [SerializeField]
        GameObject[] leftOthers;

        [SerializeField]
        GameObject[] rightOthers;

        [SerializeField]
        GameObject car;

        [SerializeField]
        GameObject wreckage;

        [SerializeField]
        GameObject catActivator;

        [SerializeField]
        GameObject catDeactivator;


        [SerializeField]
        Teleport teleport;

        bool isOpen = false;
        bool isInside = false;

        float leftEulerDefault, rightEulerDefault;

        float openAngle = 90;
        float openTime = 3;
        float closeTime = .25f;

        /// <summary>
        /// 0: you did nothing yet, both tunnels are blocked
        /// 1: you tried to open the gate, it's locked but now the tunnel to the left is free
        /// 2: you moved through the tunnel trigger and you get teleported in the other tunnel
        /// 3: the gate has closed after you walked through, both gates are blocked ( we save here )
        /// </summary>
        int state = 0;

        private void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = 0.ToString();

            Init(data);
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            


            if (state == 0 || state == 1 || state == 3)
            {
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
                                        // Check tunnels
                                        if (state == 0)
                                        {
                                            state = 1;
                                            FreeLeftTunnel();
                                        }
                                    }
                                }

                            }

                        }


                    }
                }
            }
            else if(state == 2)
            {
                if (!isOpen)
                {
                    // Check the player distance to open the gate
                    float distance = Vector3.Distance(PlayerController.Instance.transform.position, transform.position);
                    if (distance < 8f)
                    {
                        OpenTheGate();
                        // Activate the cat trigger
                        catActivator.SetActive(true);
                        catDeactivator.SetActive(true);
                    }
                        
                }
            }
            
        }

        private void OnEnable()
        {
            teleport.OnLightOn += HandleOnLightOn;
            teleport.OnLightOff += HandleOnLightOff;
        }

        private void OnDisable()
        {
            teleport.OnLightOn -= HandleOnLightOn;
            teleport.OnLightOff -= HandleOnLightOff;
        }

        private void HandleOnLightOff()
        {
        }

        private void HandleOnLightOn()
        {
            state = 2;
            
            // Block the right tunnel behind, so we can exit to come back to the car
            BlockRightTunnelBehind();
            // Black the left tunnel
            BlockLeftTunnel();
            // Wreckage the car
            DestroyCar();

        }

        void DestroyCar()
        {
            car.SetActive(false);
            wreckage.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;

            isInside = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;

            isInside = false;

            if (isOpen)
            {
                Vector3 dir = other.transform.position - transform.position;
                Vector3 rgt = transform.right;
                if (Vector3.Dot(dir, rgt) > 0)
                {
                    // You are inside the school, close the gate
                    state = 3;
                    CloseTheGate();
                    SaveManager.Instance.SaveGame();
                }
            }
        }

        

        void CloseTheGate()
        {
            if (!isOpen)
                return;
          
            isOpen = false;
            
            Vector3 leftEndValue = leftDoor.eulerAngles - Vector3.up * openAngle;
            leftDoor.DORotate(leftEndValue, closeTime, RotateMode.Fast);
            Vector3 rightEndValue = rightDoor.eulerAngles + Vector3.up * openAngle;
            rightDoor.DORotate(rightEndValue, closeTime, RotateMode.Fast);
            closeAudioSource.Play();

            // Block tunnels again
            BlockLeftTunnel();
            BlockRightTunnelFront();
            _collider.enabled = true;
        }

        void OpenTheGate()
        {
            if (isOpen)
                return;
          
            isOpen = true;
            Vector3 leftEndValue = leftDoor.eulerAngles + Vector3.up * openAngle;
            leftDoor.DORotate(leftEndValue, openTime, RotateMode.Fast);
            Vector3 rightEndValue = rightDoor.eulerAngles - Vector3.up * openAngle;
            rightDoor.DORotate(rightEndValue, openTime, RotateMode.Fast);
            _collider.enabled = false;
        }

        void DisableOthers(GameObject[] others)
        {
            foreach(GameObject other in others)
                other.SetActive(false);
        }

        void EnableOthers(GameObject[] others)
        {
            foreach (GameObject other in others)
                other.SetActive(true);
        }

        void DisableLights(Light[] lights)
        {
            foreach(Light light in lights)
            {
                light.enabled = false;
                light.GetComponent<AuraLight>().enabled = false;
            }
        }

        void EnableLights(Light[] lights)
        {
            foreach (Light light in lights)
            {
                light.enabled = true;
                light.GetComponent<AuraLight>().enabled = true;
            }
        }

        void BlockLeftTunnel()
        {
            leftBlock.SetActive(true);
            DisableLights(leftLights);
            DisableOthers(leftOthers); 
            
        }
        void FreeLeftTunnel()
        {
            leftBlock.SetActive(false);
            EnableLights(leftLights);
            EnableOthers(leftOthers);
        }

        void BlockRightTunnelBehind()
        {
            rightBlocks[1].SetActive(false);
            rightBlocks[0].SetActive(true);
            EnableLights(rightLights);
            EnableOthers(rightOthers);
            
        }
        void BlockRightTunnelFront()
        {
            rightBlocks[0].SetActive(false);
            rightBlocks[1].SetActive(true);
            DisableLights(rightLights);
            DisableOthers(rightOthers);
        }


        #region savable
        [Header("Save System")]
        [SerializeField]
        string code;
        public string GetCode()
        {
            return code;
        }

        public string GetData()
        {
            return state.ToString();
        }

        public void Init(string data)
        {
            leftEulerDefault = leftDoor.localEulerAngles.z;
            rightEulerDefault = rightDoor.localEulerAngles.z;
            BlockLeftTunnel();
            BlockRightTunnelFront();
            wreckage.SetActive(false);
            catActivator.SetActive(false);
            catDeactivator.SetActive(false);

            state = int.Parse(data);
            if(state == 3)
            {
                BlockRightTunnelFront();
                BlockLeftTunnel();
                car.SetActive(false);
                wreckage.SetActive(true);
            }
        }
        #endregion
    }

}

