using CSA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class Flashlight : MonoBehaviour
    {
        [SerializeField]
        Light flashLight;

        [SerializeField]
        Light handsLight;

        bool isOn = false;
        bool notAvailable = false;

        private void Awake()
        {
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
                    EnableLights();
                else
                    DisableLights();
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
    }

}
