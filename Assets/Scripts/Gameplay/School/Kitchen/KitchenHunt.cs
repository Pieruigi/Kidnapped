using EvolveGames;
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
        List<Transform> patrolPoints;

        [SerializeField]
        PlayerWalkInTrigger puckGetInTrigger;

        [SerializeField]
        Transform puckGetInTarget;

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

        const int notReadyState = 0;
        const int readyState = 100;
        const int completedState = 200;

        GameObject puck;
        GameObject jar;
        GameObject jarInteractor;
        int jarIndex = 0;

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
        }

        private void OnDisable()
        {
            puckGetInTrigger.OnEnter -= HandleOnPuckGetInTrigger;
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
            // Flicker
            FlashlightFlickerController.Instance.FlickerOnce(OnJarFlicker);
        }

        private void OnJarFlicker()
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
            }
            else
            {
                // Completed

            }
        }

        private async void HandleOnPuckGetInTrigger(PlayerWalkInTrigger trigger)
        {
            // Disable trigger
            trigger.gameObject.SetActive(false);

            // Instantiate puck
            puck = Instantiate(puckPrefab, puckGetInTarget.position, puckGetInTarget.rotation);
            // Set the evil material
            puck.GetComponent<EvilMaterialSetter>().SetEvil();
            // Get script controller
            var ctrl = puck.GetComponent<ScaryBoyHunter>();
            // Register callbacks
            ctrl.OnKillingPlayer += HandleOnKillingPlayer;
            // Set patrol points
            ctrl.SetPatrolPoints(patrolPoints);
            // Force the first destination to reach
            ctrl.ForceDestination(puckFirstDestination.position, false);
            // Add some delay
            await Task.Delay(500);
            // Slam the door
            externalDoor.Close();

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
