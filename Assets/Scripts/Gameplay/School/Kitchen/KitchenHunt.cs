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

        const int notReadyState = 0;
        const int readyState = 100;
        const int completedState = 200;

        GameObject puck;

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
            // Set patrol points
            ctrl.SetPatrolPoints(patrolPoints);
            // Force the first destination to reach
            ctrl.ForceDestination(puckFirstDestination.position, false);
            // Add some delay
            await Task.Delay(500);
            // Slam the door
            externalDoor.Close();
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

        }

        #endregion
    }

}
