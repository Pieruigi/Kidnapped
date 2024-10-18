using Kidnapped;
using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class Flashlight : Singleton<Flashlight>
    {

        public UnityAction OnSwitchedOn;
        public UnityAction OnSwitchedOff;

        [SerializeField]
        Light flashLight;

        [SerializeField]
        Light handsLight;

        [SerializeField]
        AudioSource clickAudioSource;

        [SerializeField]
        float flashIntensity = 4.5f;
        public float LightIntensity { get { return flashIntensity; } }
       
        bool isOn = false;
        public bool IsOn
        {
            get { return isOn; }
        }
        bool notAvailable = false;

        Animation anims;
        FlashlightFlickerController flickerOff;
 
        
        protected override void Awake()
        {
            base.Awake();
            DisableLights();
            anims = GetComponent<Animation>();
            flickerOff = GetComponent<FlashlightFlickerController>();
              
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
            OnSwitchedOn?.Invoke();
        }
        public void SwitchOff()
        {
            if(!isOn) return;
            isOn = false;
            DisableLights();
            clickAudioSource.Play();
            OnSwitchedOff?.Invoke();
        }

        
    }

}
