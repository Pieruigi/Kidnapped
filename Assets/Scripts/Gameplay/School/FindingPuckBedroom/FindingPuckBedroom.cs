using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Kidnapped
{
    public class FindingPuckBedroom : MonoBehaviour//, ISavable
    {
        [SerializeField]
        PlayerWalkInTrigger jinxInTrigger;

        [SerializeField]
        PlayerWalkInTrigger jinxOutTrigger;

        [SerializeField]
        GameObject jinxPrefab;

        [SerializeField]
        Transform jinxTarget;

        
        GameObject jinx;
        
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
            FlashlightFlickerController.Instance.FlickerOnce(OnFlickerJinxOut);
        }

        private async void OnFlickerJinxOut()
        {
            // Hide Jinx and show Lilith
            jinx.gameObject.SetActive(false);

            // Stinger
            GameSceneAudioManager.Instance.PlayStinger(2);

            await Task.Delay(TimeSpan.FromSeconds(2));

            // Next gameplay element
            GetComponentInParent<GameplayGroup>().MoveToNextElement();
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
            jinxController.Trot();
                        
                    
        }

    }

}
