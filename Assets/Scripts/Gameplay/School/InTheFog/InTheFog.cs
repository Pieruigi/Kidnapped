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

        [SerializeField]
        DialogController kitchenDialogController;

        [SerializeField]
        DialogController mannequinsDialogController;

        [SerializeField]
        PlayerWalkInTrigger brothersDialogTrigger;

        [SerializeField]
        DialogController brothersDialogController;

        [SerializeField]
        PlayerWalkInTrigger catScreamingTrigger;

        [SerializeField]
        AudioSource catScreamingAudioSource;

        [SerializeField]
        AudioSource leavesCrunchAudioSource;

        //[SerializeField]
        //GameObject kitchenHintGroup;

        [SerializeField]
        GameObject entranceBlock;

        [SerializeField]
        GameObject corridorBlock;
        
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
            brothersDialogTrigger.OnEnter += HandleOnBrothersDialogTrigger;
            catScreamingTrigger.OnEnter += HandleOnCatScreamingTrigger;
        }

        private void OnDisable()
        {
            if(kitchenDoor)
                kitchenDoor.GetComponentInChildren<ScaryDoor>().OnLocked -= HandleOnKitchenDoorLocked;

            brothersDialogTrigger.OnEnter -= HandleOnBrothersDialogTrigger;
            catScreamingTrigger.OnEnter -= HandleOnCatScreamingTrigger;
        }

        private void HandleOnCatScreamingTrigger(PlayerWalkInTrigger arg0)
        {
            // Disable trigger
            catScreamingTrigger.gameObject.SetActive(false);
            // Play audio
            catScreamingAudioSource.Play();

            leavesCrunchAudioSource.PlayDelayed(.8f);

            VoiceManager.Instance.Talk(Speaker.Lilith, 12, delay: 3f);
        }

        private void HandleOnBrothersDialogTrigger(PlayerWalkInTrigger arg0)
        {
            // Disable trigger
            brothersDialogTrigger.gameObject.SetActive(false);

            // Play dialog
            brothersDialogController.Play();
        }

        private async void HandleOnKitchenDoorLocked(ScaryDoor door)
        {
            if (state != readyState)
                return;

            if (!doorLocked)
            {
                doorLocked = true;

                //FlashlightFlickerController.Instance.FlickerOnce(() => { /*kitchenHintGroup.SetActive(false);*/ entranceBlock.SetActive(false); });

                entranceBlock.SetActive(false);
                corridorBlock.SetActive(true);

                // Puck says something here
                //SubtitleUI.Instance.Show(LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationTables.Subtitles, "kitchen_pass_needed"), true);

                // Activate some trigger group
                ActivateTeleportGroup(currentTeleportIndex);
                //teleportGroups[currentTeleportIndex].SetActive(true);

                // Add some delay
                await Task.Delay(2000);

                // Talk
                kitchenDialogController.Play();

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
                    trigger.OnEnter += HandleOnTeleportTwoWaysTriggerOnEnter;
            }
            

            
        }

       

        void DeactivateTeleportGroup(int index)
        {
            GameObject g = teleportGroups[index];
            PlayerWalkInTwoWayTrigger trigger = g.GetComponentInChildren<PlayerWalkInTwoWayTrigger>();
            if (trigger)
                trigger.OnEnter -= HandleOnTeleportTwoWaysTriggerOnEnter;
            
            
            g.SetActive(false);
            
        }

        private void HandleOnTeleportTriggerOnExit(PlayerWalkInTrigger trigger)
        {
            // Flashlight flickering
            FlashlightFlickerController.Instance.FlickerOnce(HandleOnLightOffCallback);
        }

        private void HandleOnTeleportTwoWaysTriggerOnEnter(bool fromBehind)
        {
            Debug.Log($"Getting out trigger");

            // We want the player to follow a specific path
            if (!fromBehind)
                return;

          
            VoiceManager.Instance.Talk(Speaker.Lilith, 11, HandleOnLilithFenceTalk, 1f);

        }

        private void HandleOnLilithFenceTalk(Speaker speaker)
        {
            VoiceManager.Instance.Talk(Speaker.Puck, 11, volumeMultiplier: 1.5f);
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
                    //VoiceManager.Instance.Talk(Speaker.Puck, 11);

                    // Deactivate player input
                    PlayerController.Instance.PlayerInputEnabled = false;

                    
                    // Reset wreckage position and rotation
                    carWreckage.transform.position = carWreckagePositionDefault;
                    carWreckage.transform.rotation = carWreckageRotationDefault;
                    
                }
                else if (currentTeleportIndex == 0)
                {
                    // Start next dialog 
                    mannequinsDialogController.Play(delay:2);

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
                    // Set completed and save the game
                    state = completedState;
                    // Set the next gameplay controller
                    dormsGameplay.SetReady();
                    // Disable the kitchen door activator containing the abandoned kitchen door ( it's easier to do it here )
                    kitchenDoor.SetActive(false);
                    // Enable brothers dialog trigger
                    brothersDialogTrigger.gameObject.SetActive(true);

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

        async void HandleOnFlickerToDarkness(float duration)
        {
            // Disable Puck
            boy.SetActive(false);

            // Add some delay
            await Task.Delay(1500);

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
            
            // Disable kitchen hint group
            //kitchenHintGroup.SetActive(false);

            //if(state == readyState)
            //{
            //    kitchenHintGroup.SetActive(true);
            //}
        }
        #endregion
    }

}
