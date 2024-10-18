using EvolveGames;
using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Kidnapped
{
    public class BoyDorms : MonoBehaviour, ISavable
    {

        [SerializeField]
        PlayerWalkInTrigger entranceCloseTrigger;

        [SerializeField]
        ScaryDoor[] externalDoors;

        [SerializeField]
        ScaryDoor[] internalDoors;

        [SerializeField]
        GameObject ventriloquistPrefab;

        [SerializeField]
        List<Transform> ventriloquistTargets;

        [SerializeField]
        PlayerWalkInAndLookTrigger ventriloquistLockerRoomTrigger;

        [SerializeField]
        GameObject sportRoomMannequinGroup;

        [SerializeField]
        PlayerWalkInAndLookTrigger ventriloquistSportRoomTrigger;

        [SerializeField]
        VentriloquistPuzzle ventriloquistPuzzle;

        [SerializeField]
        PlayerWalkInTrigger bellTrigger;

        [SerializeField]
        AudioSource bellAudioSource;

        [SerializeField]
        GameObject hookedVentriloquistPrefab;

        [SerializeField]
        List<Transform> hookedVentriloquistTargets;

        [SerializeField]
        List<Collider> hookedTriggers;

        [SerializeField]
        GameObject kitchenRoomClearGroup;

        [SerializeField]
        GameObject mannequinGroupPrefab;

        [SerializeField]
        Transform mannequinGroupTarget;

        [SerializeField]
        Light kitchenLight;

        [SerializeField]
        DormsKitchenPuzzle kitchenPuzzle;
        

        const int notReadyState = 0;
        const int readyState = 100;
        const int completedState = 200;
        const int firstPuzzleCompletedState = 110;

        int state = 0;

        GameObject ventriloquist;
        Animator ventriloquistAnimator;
        List<GameObject> hookedVentriloquists = new List<GameObject>();
        int hookStep = 0;
        GameObject mannequinGroup;
        bool spawnMoreHookedDummies = false;

        private void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = notReadyState.ToString();
            Init(data);
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //bellTrigger.gameObject.SetActive(true);


                // Start kitchen puzzle
                mannequinGroup = Instantiate(mannequinGroupPrefab);
                mannequinGroup.transform.position = mannequinGroupTarget.position;
                mannequinGroup.transform.rotation = mannequinGroupTarget.rotation;
                mannequinGroup.transform.Find("Female").gameObject.SetActive(false);
                kitchenLight.enabled = true;
                PlayerController.Instance.ForcePositionAndRotation(mannequinGroupTarget);
                kitchenPuzzle.Init(mannequinGroup, kitchenLight);
                kitchenPuzzle.gameObject.SetActive(true);
            }
#endif

            if (spawnMoreHookedDummies)
            {
                // Check distance between the player and each target ( we don't reuse the first one, so we skip it )
                for(int i=1; i<hookedVentriloquistTargets.Count; i++)
                {
                    float distance = Vector3.ProjectOnPlane(PlayerController.Instance.transform.position - hookedVentriloquistTargets[i].position, Vector3.up).magnitude;
                    if (distance < 1f)
                        return;
                }

                spawnMoreHookedDummies = false;

                // Flicker
                FlashlightFlickerController.Instance.FlickerToDarkeness(OnHookedFlicker);
            }
        }

        private void OnEnable()
        {
            entranceCloseTrigger.OnEnter += HandleOnEntranceCloseTriggerEnter;
            ventriloquistLockerRoomTrigger.OnEnter += HandleOnVentriLockerRoomTrigger;
            ventriloquistSportRoomTrigger.OnEnter += HandleOnVentriloquistSportRoomTrigger;
            ventriloquistPuzzle.OnPuzzleSolved += HandleOnVentriloquistPuzzleSolved;
            bellTrigger.OnExit += HandleOnBellTrigger;
        }

        private void OnDisable()
        {
            entranceCloseTrigger.OnEnter -= HandleOnEntranceCloseTriggerEnter;
            ventriloquistLockerRoomTrigger.OnEnter -= HandleOnVentriLockerRoomTrigger;
            ventriloquistSportRoomTrigger.OnEnter -= HandleOnVentriloquistSportRoomTrigger;
            ventriloquistPuzzle.OnPuzzleSolved -= HandleOnVentriloquistPuzzleSolved;
            bellTrigger.OnExit -= HandleOnBellTrigger;
        }

        private async void HandleOnBellTrigger(PlayerWalkInTrigger trigger)
        {
            // Disable trigger
            bellTrigger.gameObject.SetActive(false);

            // Play audio
            bellAudioSource.Play();

            // Spawn hooked ventriloquist
            var hv = Instantiate(hookedVentriloquistPrefab);
            hv.transform.position = hookedVentriloquistTargets[0].transform.position;
            hv.transform.rotation = hookedVentriloquistTargets[0].transform.rotation;
            hookedVentriloquists.Add(hv);

            // Enable triggers
            hookedTriggers[0].gameObject.SetActive(true);

            // Set trigger callback
            hookedTriggers[0].GetComponent<PlayerWalkInTrigger>().OnEnter += HandleOnHookTriggerEnter;

            // Set the first step
            hookStep = 0;

            // Add some delay
            await Task.Delay(1500);

            // Open the kitchen door
            internalDoors[2].Open();
        }


        async void HandleOnHookTriggerEnter(PlayerWalkInTrigger trigger)
        {
            // Get trigger index
            int index = hookedTriggers.FindIndex(t=>t.GetComponent<PlayerWalkInTrigger>() == trigger);

            // Remove handle
            hookedTriggers[index].GetComponent<PlayerWalkInTrigger>().OnEnter -= HandleOnHookTriggerEnter;

            // Disable the trigger
            hookedTriggers[index].gameObject.SetActive(false);

            switch (hookStep)
            {
                case 0: // The trigger close to the first hooked dummy
                    // Flicker
                    FlashlightFlickerController.Instance.FlickerToDarkeness(OnHookedFlicker);
                    break;
                case 1: // The internal kitchen door trigger to slam the door closed
                    // Slam kitchen door
                    internalDoors[2].Close();

                    // Add some delay
                    await Task.Delay(12000);

                    // Since the player is free to move around the room, before we spawn the hooked dummies, we need to make sure they don't collide. For this reason
                    // we let the update to choose when to spawn the dummies.
                    spawnMoreHookedDummies = true;

                    // Update step
                    hookStep++;

                    break;
            }
        }

        private async void OnHookedFlicker(float duration)
        {
            switch (hookStep)
            {
                case 0:
                    // Destroy ventriloquist
                    foreach (var o in hookedVentriloquists)
                        Destroy(o);

                    // Clear list
                    hookedVentriloquists.Clear();

                    // Clear room
                    kitchenRoomClearGroup.gameObject.SetActive(false);

                    // Create the mannequin group
                    mannequinGroup = Instantiate(mannequinGroupPrefab);
                    // Set position and rotation
                    mannequinGroup.transform.position = mannequinGroupTarget.transform.position;
                    mannequinGroup.transform.rotation = mannequinGroupTarget.transform.rotation;

                    // Switch the light on
                    kitchenLight.enabled = true;

                    // Update step
                    hookStep++;

                    // Activate door trigger
                    hookedTriggers[1].gameObject.SetActive(true);

                    // Register callback
                    hookedTriggers[1].GetComponent<PlayerWalkInTrigger>().OnEnter += HandleOnHookTriggerEnter;

                    break;

                case 1:
                    // Disable mannequin group
                    mannequinGroup.SetActive(false);

                    // Switch the light off
                    kitchenLight.enabled = false;

                    break;
                case 2:
                    // Spawn more hooked dummies
                    for(int i=1; i<hookedVentriloquistTargets.Count; i++)
                    {
                        // Spawn the next dummy
                        var dummy = Instantiate(hookedVentriloquistPrefab);
                        // Set position and rotation
                        dummy.transform.position = hookedVentriloquistTargets[i].transform.position;
                        dummy.transform.rotation = hookedVentriloquistTargets[i].transform.rotation;
                        // Add to the list
                        hookedVentriloquists.Add(dummy);
                    }

                    // Hide mannequins
                    mannequinGroup.SetActive(false);

                    // Switch the light off
                    kitchenLight.enabled = false;

                    // Add some delay
                    await Task.Delay(14000);

                    // Update step
                    hookStep++;

                    // Flicker
                    FlashlightFlickerController.Instance.FlickerToDarkeness(OnHookedFlicker);
                    break;
                case 3:
                    // Remove all hooked dummies
                    foreach (var d in hookedVentriloquists)
                        Destroy(d);
                    // Clear list
                    hookedVentriloquists.Clear();
                    // Move the player in the middle of the mannequins group
                    PlayerController.Instance.ForcePositionAndRotation(mannequinGroupTarget);
                    // Disable the female child in the mannequin group
                    mannequinGroup.transform.Find("Female").gameObject.SetActive(false);
                    // Activate the group
                    mannequinGroup.SetActive(true);

                    // Switch the light on
                    kitchenLight.enabled = true;

                    // Init the kitchen puzzle
                    kitchenPuzzle.Init(mannequinGroup, kitchenLight);

                    // Start the kitchen puzzle
                    kitchenPuzzle.gameObject.SetActive(true);

                    break;
            }
        }

        async void HandleOnVentriloquistPuzzleSolved()
        {
            // Add some delay
            await Task.Delay(2000);

            // Flicker
            FlashlightFlickerController.Instance.FlickerToDarkeness(OnBathroomFlicker);
        }

        private void OnBathroomFlicker(float duration)
        {
            // Disable puzzle
            ventriloquistPuzzle.StopPuzzle();

            // Activate the bell trigger
            bellTrigger.gameObject.SetActive(true);

            // Update state
            Init(firstPuzzleCompletedState.ToString());

            // Save game
            SaveManager.Instance.SaveGame();
        }

        private void HandleOnVentriloquistSportRoomTrigger()
        {
            // Disable trigger
            ventriloquistSportRoomTrigger.gameObject.SetActive(false);
           
            // Flicker
            FlashlightFlickerController.Instance.FlickerToDarkeness(OnSportRoomFlicker);
            // Open sports room door
            internalDoors[1].Open();
        }

        private void HandleOnVentriLockerRoomTrigger()
        {
            // Disable trigger
            ventriloquistLockerRoomTrigger.gameObject.SetActive(false);
            // Set animation
            ventriloquistAnimator.SetTrigger("HangedEnd");
            // Flicker
            FlashlightFlickerController.Instance.FlickerToDarkeness(OnLockerRoomFlicker);
            
        }

        private void OnLockerRoomFlicker(float arg0)
        {
            // Disable the ventriloquist
            ventriloquist.SetActive(false);
            
            // Open the sport room door
            internalDoors[0].Open();

            // Enable the ventriloquist
            ventriloquist.SetActive(true);
            // Set position and rotation
            ventriloquist.transform.position = ventriloquistTargets[1].transform.position;
            ventriloquist.transform.rotation = ventriloquistTargets[1].transform.rotation;
            // Set animation
            ventriloquistAnimator.SetTrigger("Basketed");
            
        }

        private void OnSportRoomFlicker(float arg0)
        {
            // Destroy the ventriloquist
            Destroy(ventriloquist);
            //ventriloquist.SetActive(false);

            // Activate mannequins
            sportRoomMannequinGroup.SetActive(true);

            // Open the bathroom door
            internalDoors[1].Open();

            // Enable the ventriloquist puzzle
            ventriloquistPuzzle.StartPuzzle();
            //// Set position and rotation
            //ventriloquist.transform.position = ventriloquistTargets[1].transform.position;
            //ventriloquist.transform.rotation = ventriloquistTargets[1].transform.rotation;
            //// Set animation
            //ventriloquistAnimator.SetTrigger("Basketed");

        }

        private void HandleOnEntranceCloseTriggerEnter(PlayerWalkInTrigger trigger)
        {
            // Deactivate the trigger
            entranceCloseTrigger.gameObject.SetActive(false);
            // Close all external doors
            foreach (var door in externalDoors)
            {
                door.Close();
            }
        }

        public void SetReady()
        {
            Debug.Log("Setting ready");
            Init(readyState.ToString());
        }

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
            Debug.Log($"Dorms new state = {state}");

            // Default settings
            // Disable sport room mannequin group
            sportRoomMannequinGroup.SetActive(false);
            // Disable the bell trigger
            bellTrigger.gameObject.SetActive(false);
            // Disable all hook triggers
            foreach (var t in hookedTriggers)
                t.gameObject.SetActive(false);
            // Disable kitchen light
            kitchenLight.enabled = false;
            // Disable the kitchen puzzle
            kitchenPuzzle.gameObject.SetActive(false);


            if(state == readyState)
            {
                // Activate the ventriloquist
                ventriloquist = Instantiate(ventriloquistPrefab);
                ventriloquist.transform.position = ventriloquistTargets[0].position;
                ventriloquist.transform.rotation = ventriloquistTargets[0].rotation;
                ventriloquist.SetActive(true);
                ventriloquistAnimator = ventriloquist.GetComponentInChildren<Animator>();
                ventriloquistAnimator.SetTrigger("Hanged");
            }
            else if(state == firstPuzzleCompletedState)
            {
                // enable the bell trigger
                bellTrigger.gameObject.SetActive(true);
            }
            else if (state == completedState)
            {
                // Reset the entrance trigger
                entranceCloseTrigger.gameObject.SetActive(false);
            }
        }
        #endregion
    }

}
