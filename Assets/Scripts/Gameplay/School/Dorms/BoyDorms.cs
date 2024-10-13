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
        GameObject ventriloquist;

        [SerializeField]
        List<Transform> ventriloquistTargets;

        [SerializeField]
        PlayerWalkInAndLookTrigger ventriloquistLockerRoomTrigger;

        const int notReadyState = 0;
        const int readyState = 100;
        const int completedState = 200;

        int state = 0;

        Animator ventriloquistAnimator;

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
            ventriloquistLockerRoomTrigger.OnEnter += HandleOnVentriLockerRoomTrigger;
        }

        private void OnDisable()
        {
            entranceCloseTrigger.OnEnter -= HandleOnEntranceCloseTriggerEnter;
            ventriloquistLockerRoomTrigger.OnEnter -= HandleOnVentriLockerRoomTrigger;
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
            ventriloquist.SetActive(false);
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

            // Default settings
            // Activate the ventriloquist
            ventriloquist.transform.position = ventriloquistTargets[0].position;
            ventriloquist.transform.rotation = ventriloquistTargets[0].rotation;
            ventriloquist.SetActive(true);
            ventriloquistAnimator = ventriloquist.GetComponentInChildren<Animator>();
            ventriloquistAnimator.SetTrigger("Hanged");
            
            
            if (state == completedState)
            {
                // Reset the entrance trigger
                entranceCloseTrigger.gameObject.SetActive(false);
            }
        }
        #endregion
    }

}
