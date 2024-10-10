using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class BoyDorms : MonoBehaviour, ISavable
    {

        [SerializeField]
        PlayerWalkInTrigger entranceCloseTrigger;

        [SerializeField]
        ScaryDoor[] externalDoors;

        const int notReadyState = 0;
        const int readyState = 100;
        const int completedState = 200;

        int state = 0;

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

        }

        private void OnEnable()
        {
            entranceCloseTrigger.OnEnter += HandleOnEntranceCloseTriggerEnter;
        }

        private void OnDisable()
        {
            entranceCloseTrigger.OnEnter -= HandleOnEntranceCloseTriggerEnter;
        }

        private void HandleOnEntranceCloseTriggerEnter()
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
        }
        #endregion
    }

}
