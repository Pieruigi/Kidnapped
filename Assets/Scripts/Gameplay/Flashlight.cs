using CSA;
using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class Flashlight : Singleton<Flashlight>
    {
        [SerializeField]
        Light flashLight;

        [SerializeField]
        Light handsLight;

        [SerializeField]
        AudioSource clickAudioSource;

        [SerializeField]
        float flashIntensity = 4.5f;
       
        bool isOn = false;
        bool notAvailable = false;

        Animation anims;
        LightFlickerOff flickerOff;

        protected override void Awake()
        {
            base.Awake();
            DisableLights();
            anims = GetComponent<Animation>();
            flickerOff = GetComponent<LightFlickerOff>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {


            if (notAvailable)
            {
                if(isOn)
                {
                    isOn = false;
                    DisableLights();
                }
                return;
            }

            if(PlayerController.Instance.PlayerInputEnabled && !flickerOff.Flickering)
            {
                if (Input.GetKeyDown(KeyBindings.FlashlightKey))
                {
                    if (!isOn)
                        SwitchOn();
                    else
                        SwitchOff();
                    
                }
            }
            
        }

        
        void DisableLights()
        {
            flashLight.enabled = false;
            handsLight.enabled = false;
            flashLight.intensity = 0;
        }

        void EnableLights()
        {
            flashLight.enabled = true;
            handsLight.enabled = true;
            flashLight.intensity = flashIntensity;
        }

        public void SwitchOn()
        {
            if (isOn) return;
            isOn = true;
            EnableLights();
            clickAudioSource.Play();
        }
        public void SwitchOff()
        {
            if(!isOn) return;
            isOn = false;
            DisableLights();
            clickAudioSource.Play();
        }

        
    }

}
