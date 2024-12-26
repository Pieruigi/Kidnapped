using EvolveGames;
using JetBrains.Annotations;
using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Kidnapped
{
    public class KitchenHunt : MonoBehaviour, ISavable
    {
        [SerializeField]
        Transform killingPlane;

        [SerializeField]
        GameObject puckKillerPrefab;
        
        [SerializeField]
        GameObject puckPrefab;

        [SerializeField]
        List<Transform> puckTargets;

        [SerializeField]
        List<Transform> patrolPoints;

        [SerializeField]
        PlayerWalkInTrigger puckGetInTrigger;

        [SerializeField]
        Transform puckFirstDestination;

        [SerializeField]
        ScaryDoor externalDoor;

        [SerializeField]
        GameObject jarPrefab;

        [SerializeField]
        List<Transform> jarTargets;

        [SerializeField]
        GameObject jarInteractorPrefab;

        [SerializeField]
        AudioSource squishEyesAudioSource;

        [SerializeField]
        PlayerWalkInTrigger exitTrigger;

        [SerializeField]
        GameObject originalKitchen;

        [SerializeField]
        GameObject abandonedKitchen;

        [SerializeField]
        GameplayGroup findingPuckBedroom;

        const int notReadyState = 0;
        const int readyState = 100;
        const int completedState = 200;

        List<GameObject> pucks = new List<GameObject>();
        GameObject jar;
        GameObject jarInteractor;
        int jarIndex = 0;
        int puckIndex = 0;

        int state;


       
        private void Awake()
        {
            var data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = notReadyState.ToString();
            
            Init(data);
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
            puckGetInTrigger.OnEnter += HandleOnPuckGetInTrigger;
            exitTrigger.OnEnter += HandleOnExitTrigger;
        }

        private void OnDisable()
        {
            puckGetInTrigger.OnEnter -= HandleOnPuckGetInTrigger;
            exitTrigger.OnEnter -= HandleOnExitTrigger;
        }

        private void HandleOnExitTrigger(PlayerWalkInTrigger arg0)
        {
            // Flicker
            FlashlightFlickerController.Instance.FlickerOnce(OnExitFlicker);
        }

        private async void OnExitFlicker()
        {
            // Disable the trigger
            exitTrigger.gameObject.SetActive(false);
            // Destroy all killers
            foreach (var puck in pucks)
                Destroy(puck);
            // Clear list
            pucks.Clear();
            // Hide the original kitchen
            originalKitchen.SetActive(false);
            // Show the old abandoned 
            abandonedKitchen.SetActive(true);

            // Reset ambience sound
            GameSceneAudioManager.Instance.PlayAmbience(0);

            // Set the completed state
            Init(completedState.ToString());

            // Set next gameplay step ready
            findingPuckBedroom.SetReadyState();

            // Set demo block
            DemoManager.Instance.ActivateBlock();

            await Task.Delay(1000);

            // Save game
            SaveManager.Instance.SaveGame();
        }

        void SpawnJar(int index)
        {
            // Get target position
            Transform target = jarTargets[index];
            // Instantiate the jar
            jar = Instantiate(jarPrefab);
            jar.transform.position = target.position;
            jar.transform.rotation = target.rotation;
            
            // Instantiate interactor
            jarInteractor = Instantiate(jarInteractorPrefab);
            jarInteractor.transform.position = target.position;
            jarInteractor.transform.rotation = target.rotation;
            // Register callback
            jarInteractor.GetComponent<ObjectInteractor>().OnInteraction += HandleOnJarInteraction;

        }

        private void HandleOnJarInteraction(ObjectInteractor intractor)
        {
            // Squish
            squishEyesAudioSource.Play();

            // Flicker
            FlashlightFlickerController.Instance.FlickerOnce(OnJarFlicker);
        }

        private async void OnJarFlicker()
        {
            // Destroy the interactor
            Destroy(jarInteractor);

            // Destroy the jar object
            Destroy(jar);


           

            // Update index 
            jarIndex++;

            if(jarIndex < jarTargets.Count)
            {
                // Spawn the next jar
                SpawnJar(jarIndex);

                if (jarIndex % 2 == 0)
                {
                    puckIndex++;

                    if (puckIndex == 2)
                    {
                        externalDoor.Open();
                    }

                    await Task.Delay(1000);

                    // Spawn new killer
                    SpawnKiller(puckIndex);

                    if (puckIndex == 2)
                    {
                        await Task.Delay(1000);
                        externalDoor.Close();
                    }

                }
                    
                
            }
            else
            {
                // Completed
                // Open the door
                externalDoor.Open();
                // Enable the exit trigger
                exitTrigger.gameObject.SetActive(true);

            }

        }

        private async void HandleOnPuckGetInTrigger(PlayerWalkInTrigger trigger)
        {
            // Disable trigger
            trigger.gameObject.SetActive(false);

            // Instantiate puck
            SpawnKiller(puckIndex);
            // Add some delay
            await Task.Delay(1000);
            // Slam the door
            externalDoor.Close();

        }

        void SpawnKiller(int index)
        {
            Vector3 position = puckTargets[puckIndex].position;
            Quaternion rotation = puckTargets[puckIndex].rotation;

            // Instantiate puck
            var puck = Instantiate(puckPrefab, position, rotation);
            // Set the evil material
            puck.GetComponent<EvilMaterialSetter>().SetEvil();
            // Get script controller
            var ctrl = puck.GetComponent<ScaryBoyHunter>();
            // Register callbacks
            ctrl.OnKillingPlayer += HandleOnKillingPlayer;
            // Set patrol points
            ctrl.SetPatrolPoints(patrolPoints);
            // Force the destination to reach the first time we spawn a killer
            if(puckIndex == 0)
                ctrl.ForceDestination(puckFirstDestination.position, false);
            else
                ctrl.ForceDestination(PlayerController.Instance.transform.position, false);
            // Add to the list
            pucks.Add(puck);
        }

        void HandleOnKillingPlayer(ScaryBoyHunter killer)
        {
            // Set player dying 
            PlayerController.Instance.IsDying = true;

            // Disable interaction
            PlayerController.Instance.InteractionDisabled = true;

            // Start flickering
            FlashlightFlickerController.Instance.FlickerToDarkeness(OnPlayerDead);
        }

        private async void OnPlayerDead(float duration)
        {
            // Stop player input
            //PlayerController.Instance.PlayerInputEnabled = false;

            // Move player to the killing plane
            PlayerController.Instance.ForcePositionAndRotation(killingPlane);

            await Task.Delay(2000);

            // Instantiate puck the killer
            var killer = Instantiate(puckKillerPrefab);
            killer.transform.position = PlayerController.Instance.transform.position + PlayerController.Instance.transform.forward * 30;

        }

        public void SetReady()
        {
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
            // Set state
            state = int.Parse(data);

            // Default
            // Disable the exit trigger
            exitTrigger.gameObject.SetActive(false);

            // Set default values
            if(state == readyState)
            {
                // Spawn the first jar    
                SpawnJar(0);

            }
        }

        #endregion
    }

}
