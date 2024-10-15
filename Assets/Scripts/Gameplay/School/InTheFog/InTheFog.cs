using EvolveGames;
using Kidnapped.SaveSystem;
using Kidnapped.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Kidnapped
{
    public class InTheFog : MonoBehaviour, ISavable
    {
        [SerializeField]
        GameObject brokenKitchenDoor;

        [SerializeField]
        GameObject kitchenDoor;

        [SerializeField]
        GameObject[] teleportGroups;

        [SerializeField]
        ScaryDoor boothDoor;

        [SerializeField]
        GameObject boy;

        [SerializeField]
        BoyDorms dormsGameplay;

        [SerializeField]
        GameObject carWreckage;

        [SerializeField]
        GameObject carWreckageTarget;

        
        int state = 0;

        const int notReadyState = 0;
        const int readyState = 100;
        const int completedState = 200;

        bool doorLocked = false;
        int currentTeleportIndex = 0;

        string playerTargetChildName = "PlayerTarget";
        string boyTargetChildName = "BoyTarget";

        Vector3 carWreckagePositionDefault;
        Quaternion carWreckageRotationDefault;

        private void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if(string.IsNullOrEmpty(data))
                data = notReadyState.ToString();
            Init(data);
        }

        private void Update()
        {


        }

        private void OnEnable()
        {
            kitchenDoor.GetComponentInChildren<ScaryDoor>().OnLocked += HandleOnKitchenDoorLocked;

        }

        private void OnDisable()
        {
            if(kitchenDoor)
                kitchenDoor.GetComponentInChildren<ScaryDoor>().OnLocked -= HandleOnKitchenDoorLocked;
        }

        private void HandleOnKitchenDoorLocked(ScaryDoor door)
        {
            if (state != readyState)
                return;

            if (!doorLocked)
            {
                doorLocked = true;

                // Puck says something here
                SubtitleUI.Instance.Show(LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationTables.Subtitles, "kitchen_pass_needed"), true);

                // Activate some trigger group
                ActivateTeleportGroup(currentTeleportIndex);
                //teleportGroups[currentTeleportIndex].SetActive(true);


            }
        }

        void DisableTeleportGroupAll()
        {
            foreach(var trigger in teleportGroups)
                trigger.SetActive(false);
        }

        void ActivateTeleportGroup(int index)
        {
            GameObject g = teleportGroups[index];
            g.SetActive(true);
            if(index == 0 || index == 2)
            {
                PlayerWalkInTrigger trigger = g.GetComponentInChildren<PlayerWalkInTrigger>();
                if (trigger)
                    trigger.OnExit += HandleOnTeleportTriggerOnExit;
            }
            else
            {
                PlayerWalkInTwoWayTrigger trigger = g.GetComponentInChildren<PlayerWalkInTwoWayTrigger>();
                
                if (trigger)
                    trigger.OnExit += HandleOnTeleportTwoWaysTriggerOnExit;
            }
            

            
        }

       

        void DeactivateTeleportGroup(int index)
        {
            GameObject g = teleportGroups[index];
            PlayerWalkInTwoWayTrigger trigger = g.GetComponentInChildren<PlayerWalkInTwoWayTrigger>();
            if (trigger)
                trigger.OnExit -= HandleOnTeleportTwoWaysTriggerOnExit;
            
            
            g.SetActive(false);
            
        }

        private void HandleOnTeleportTriggerOnExit()
        {
            // Flashlight flickering
            FlashlightFlickerController.Instance.FlickerOnce(HandleOnLightOffCallback);
        }

        private void HandleOnTeleportTwoWaysTriggerOnExit(bool fromBehind)
        {
            Debug.Log($"Getting out trigger");

            // We want the player to follow a specific path
            if (!fromBehind)
                return;



            // Flashlight flickering
            FlashlightFlickerController.Instance.FlickerOnce(HandleOnLightOffCallback);


        }

        private async void HandleOnLightOffCallback()
        {
            if (currentTeleportIndex < teleportGroups.Length)
            {
                // Get the player target 
                GameObject currentGroup = teleportGroups[currentTeleportIndex];
                Transform t = currentGroup.transform.Find(playerTargetChildName);

                if (currentTeleportIndex == 1)
                {
                    // Deactivate player input
                    PlayerController.Instance.PlayerInputEnabled = false;

                    
                    // Reset wreckage position and rotation
                    carWreckage.transform.position = carWreckagePositionDefault;
                    carWreckage.transform.rotation = carWreckageRotationDefault;
                    
                }
                else if (currentTeleportIndex == 0)
                {

                    // Save the old position and rotation
                    carWreckagePositionDefault = carWreckage.transform.position;
                    carWreckageRotationDefault = carWreckage.transform.rotation;
                    // Move the wreckage
                    carWreckage.transform.position = carWreckageTarget.transform.position;
                    carWreckage.transform.rotation = carWreckageTarget.transform.rotation;
                }

                // Move the player
                PlayerController.Instance.ForcePositionAndRotation(t);
                // Deactivate the current group
                DeactivateTeleportGroup(currentTeleportIndex);
                // Do some specific action here
                DoSomethingSpecial(currentTeleportIndex);
               
                // Update the index
                currentTeleportIndex++;
                // Activate the next group if any, otherwise set completed
                if(currentTeleportIndex < teleportGroups.Length)
                {
                    // Activate the next group
                    ActivateTeleportGroup(currentTeleportIndex);
                }
                else
                {
                    Debug.Log("AAAAAAAAAAAAAAAAAAAA");
                    // Set completed and save the game
                    state = completedState;
                    // Set the next gameplay controller
                    dormsGameplay.SetReady();
                    // Save game
                    SaveManager.Instance.SaveGame();
                }

                // Eventually reactivate player input
                if(!PlayerController.Instance.PlayerInputEnabled)
                {
                    // Add some delay
                    await Task.Delay(250);

                    // Activate player input
                    PlayerController.Instance.PlayerInputEnabled = true;
                }
                
                
            }
        }

        void DoSomethingSpecial(int teleportIndex)
        {
            switch(teleportIndex)
            {
                case 1: // External metal fence
                    // We are in the booth, activate Puck
                    // Set position and rotation
                    Transform target = teleportGroups[teleportIndex].transform.Find(boyTargetChildName);
                    boy.transform.position = target.position;
                    boy.transform.rotation = target.rotation;
                    // Register the animation event handler
                    boy.GetComponentInChildren<AnimationEventDispatcher>().OnAnimationEvent += HandleOnAnimationEvent;
                    // Set evil material
                    boy.GetComponent<EvilMaterialSetter>().SetEvil();
                    // Activate character
                    boy.SetActive(true);
                    // Start animation
                    boy.GetComponentInChildren<Animator>().SetTrigger("Scary1");
                    break;
            }
        }

        void HandleOnAnimationEvent(int id)
        {
            // Unregister callback
            boy.GetComponentInChildren<AnimationEventDispatcher>().OnAnimationEvent -= HandleOnAnimationEvent;
            // Flicker
            FlashlightFlickerController.Instance.FlickerToDarkeness(HandleOnFlickerToDarkness);
        }

        void HandleOnFlickerToDarkness(float duration)
        {
            // Disable Puck
            boy.SetActive(false);
            // Open the booth door
            boothDoor.Open();
        }

        #region logic


        #endregion

        #region public methods
        public void SetReady()
        {
            // Disable the broken kitchen door 
            brokenKitchenDoor.SetActive(false);
            // Enable the kitchen door
            kitchenDoor.SetActive(true);

            Init(readyState.ToString());
        }
        #endregion

        #region save system
        [Header("SaveSystem")]
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
            state = int.Parse(data);

            // Not ready state
            DisableTeleportGroupAll();
            
        }
        #endregion
    }

}
