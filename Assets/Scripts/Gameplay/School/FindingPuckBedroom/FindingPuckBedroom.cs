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
        PlayerWalkInTrigger jinxInTrigger;

        [SerializeField]
        PlayerWalkInTrigger jinxOutTrigger;

        [SerializeField]
        GameObject jinxPrefab;

        [SerializeField]
        Transform jinxTarget;

        [SerializeField]
        GameObject lilithPrefab;

        [SerializeField]
        Transform lilithTarget;

        int state = 0;

        const int notReadyState = 0;
        const int readyState = 100;
        const int completedState = 200;

        GameObject jinx;
        GameObject lilith;

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
            jinxInTrigger.OnEnter += HandleOnJinxInEnter;
            jinxOutTrigger.OnEnter += HandleOnJinxOutEnter;
        }

        private void OnDisable()
        {
            jinxInTrigger.OnEnter -= HandleOnJinxInEnter;
            jinxOutTrigger.OnEnter -= HandleOnJinxOutEnter;
        }

        private void HandleOnJinxOutEnter(PlayerWalkInTrigger arg0)
        {
            // Disable trigger
            arg0.gameObject.SetActive(false);

            // Flicker
            FlashlightFlickerController.Instance.FlickerAndWatch(OnFlickerJinxOutBefore, OnFlickerJinxOutAfter, onDuration: 0.5f);
        }

        private void OnFlickerJinxOutBefore()
        {
            // Hide Jinx and show Lilith
            jinx.gameObject.SetActive(false);

            // Spawn Lilith
            lilith = Instantiate(lilithPrefab, lilithTarget.position, lilithTarget.rotation);

            lilith.SetActive(true);

            lilith.GetComponentInChildren<Animator>().SetTrigger("Run");

            // Stinger
            GameSceneAudioManager.Instance.PlayStinger(0);
        }

        private void OnFlickerJinxOutAfter()
        {
            // Unspawn Lilith
            Destroy(lilith);
        }

        private void HandleOnJinxInEnter(PlayerWalkInTrigger trigger)
        {
            // Disable trigger
            trigger.gameObject.SetActive(false);

            // Flicker once
            FlashlightFlickerController.Instance.FlickerOnce(onLightOffCallback: HandleOnJinxInLightOff);

            

            
        }

        private void HandleOnJinxInLightOff()
        {
            // Show Jinx
            jinx = Instantiate(jinxPrefab, jinxTarget.position, jinxTarget.rotation);
                    
            var jinxController = jinx.GetComponentInChildren<SimpleCatController>();

            // Play stinger
            GameSceneAudioManager.Instance.PlayStinger(2);

            // Move Jinx 
            jinxController.Run(3);
                        
                    
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
            jinxInTrigger.gameObject.SetActive(false);
            jinxOutTrigger.gameObject.SetActive(false);

            // Set default values
            if (state == readyState)
            {
                // Spawn the first jar    
                jinxInTrigger.gameObject.SetActive(true);
                jinxOutTrigger.gameObject.SetActive(true);
            }
        }

        #endregion
    }

}
