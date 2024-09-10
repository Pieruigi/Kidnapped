using CSA;
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

        bool isOn = false;
        bool notAvailable = false;

        protected override void Awake()
        {
            base.Awake();
            flashLight.enabled = false;
            handsLight.enabled = false;
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

            if(Input.GetKeyDown(KeyBindings.FlashlightKey))
            {
                isOn = !isOn;
                if(isOn)
                    SwitchOn();
                else
                    SwitchOff();
            }
        }

        void DisableLights()
        {
            flashLight.enabled = false;
            handsLight.enabled = false;
        }

        void EnableLights()
        {
            flashLight.enabled = true;
            handsLight.enabled = true;
        }

        public void SwitchOn()
        {
            EnableLights();
            clickAudioSource.Play();
        }
        public void SwitchOff()
        {
            DisableLights();
            clickAudioSource.Play();
        }
    }

}
