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



        int state = 0;

        const int notReadyState = 0;
        const int readyState = 100;
        const int completedState = 200;

        bool doorLocked = false;
        int currentTeleportIndex = 0;

        string playerTargetChildName = "PlayerTarget";
        string boyTargetChildName = "BoyTarget";

        private void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if(string.IsNullOrEmpty(data))
                data = notReadyState.ToString();
            Init(data);
        }

        private void OnEnable()
        {
            kitchenDoor.GetComponentInChildren<ScaryDoor>().OnLocked += HandleOnKitchenDoorLocked;

        }

        private void OnDisable()
        {
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
            PlayerWalkInTwoWayTrigger trigger = g.GetComponentInChildren<PlayerWalkInTwoWayTrigger>();
            if(trigger)
                trigger.OnExit += HandleOnTeleportTriggerOnExit;
            
        }

        void DeactivateTeleportGroup(int index)
        {
            GameObject g = teleportGroups[index];
            PlayerWalkInTwoWayTrigger trigger = g.GetComponentInChildren<PlayerWalkInTwoWayTrigger>();
            if (trigger)
                trigger.OnExit -= HandleOnTeleportTriggerOnExit;
            
            
            g.SetActive(false);
            
        }

        private void HandleOnTeleportTriggerOnExit(bool fromBehind)
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
                    // Set completed and save the game
                    state = completedState;
                }

                // Eventually reactivate player input
                if(currentTeleportIndex == 2)
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
                    Transform target = teleportGroups[teleportIndex].transform.Find(boyTargetChildName);
                    boy.transform.position = target.position;
                    boy.transform.rotation = target.rotation;
                    boy.GetComponent<EvilMaterialSetter>().SetEvil();
                    boy.SetActive(true);
                    break;
            }
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
