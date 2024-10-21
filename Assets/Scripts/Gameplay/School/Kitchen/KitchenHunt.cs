using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class KitchenHunt : MonoBehaviour, ISavable
    {

        [SerializeField]
        PlayerWalkInTrigger puckGetInTrigger;


        const int notReadyState = 0;
        const int readyState = 100;
        const int completedState = 200;

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
