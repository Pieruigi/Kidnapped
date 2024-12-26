using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Kidnapped
{
    public class FindingPuckBedroom : MonoBehaviour, ISavable
    {
        [SerializeField]
        PlayerWalkInTrigger jinxMiaoTrigger;

        [SerializeField]
        GameObject jinxPrefab;

        [SerializeField]
        Transform[] jinxTargets;

        int state = 0;

        const int notReadyState = 0;
        const int readyState = 100;
        const int completedState = 200;

        GameObject jinx;
        SimpleCatController jinxController;
        int jinxStep = 0;

        

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
            jinxMiaoTrigger.OnEnter += HandleOnJinxMiaoEnter;
        }

        private void OnDisable()
        {
            jinxMiaoTrigger.OnEnter -= HandleOnJinxMiaoEnter;
        }

        private void HandleOnJinxMiaoEnter(PlayerWalkInTrigger trigger)
        {
            // Disable trigger
            trigger.gameObject.SetActive(false);

            // Flicker once
            FlashlightFlickerController.Instance.FlickerOnce(onLightOffCallback: HandleOnJinxMeowLightOff);

            

            
        }

        private async void HandleOnJinxMeowLightOff()
        {
            switch (jinxStep)
            {
                case 0: // We spawn Jinx on the corridor
                    // Show Jinx
                    SpawnJinx(jinxTargets[0]);

                    // Play Jinx audio
                    jinxController.PlayMeow(0, delay:.5f);

                    // Move Jinx 
                    jinxController.Walk();

                    // Await some seconds
                    await Task.Delay(TimeSpan.FromSeconds(222.5f));

                    // Update step
                    jinxStep++; // 1

                    FlashlightFlickerController.Instance.FlickerOnce(HandleOnJinxMeowLightOff);
                    break;
                case 1: // Hide Jinx in the corridor after a while
                    jinx.gameObject.SetActive(false);

                    // Play Lilith/Puck dialog
                    Debug.LogError("Play Lilith Puck dialog");
                    break;
            }

            

            

            // 
        }

        void SpawnJinx(Transform target)
        {
            if (jinx)
            {
                // It only disabled
                jinx.SetActive(true);
            }
            else
            {
                // Spawn jinx
                jinx = Instantiate(jinxPrefab);
            }
          
            jinxController = jinx.GetComponent<SimpleCatController>();

            jinx.transform.position = target.position;
            jinx.transform.rotation = target.rotation;  
        }

        public void SetReadyState()
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
            jinxMiaoTrigger.gameObject.SetActive(false);
            
            // Set default values
            if (state == readyState)
            {
                // Spawn the first jar    
                jinxMiaoTrigger.gameObject.SetActive(true);

            }
        }

        #endregion
    }

}
