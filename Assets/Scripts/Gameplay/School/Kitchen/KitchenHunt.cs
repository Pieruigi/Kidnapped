using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Kidnapped
{
    public class KitchenHunt : MonoBehaviour, ISavable
    {
        [SerializeField]
        GameObject puckPrefab;

        [SerializeField]
        PlayerWalkInTrigger puckGetInTrigger;

        [SerializeField]
        Transform puckGetInTarget;


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

        private void HandleOnPuckGetInTrigger(PlayerWalkInTrigger trigger)
        {
            // Instantiate puck
            puck = Instantiate(puckPrefab, puckGetInTarget.position, puckGetInTarget.rotation);
            // Set the evil material
            puck.GetComponent<EvilMaterialSetter>().SetEvil();
            // Set first destination
            puck.GetComponent<ScaryBoyHunter>().ForceDestination(puckGetInTarget.position + puckGetInTarget.forward * 3f, true);
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
