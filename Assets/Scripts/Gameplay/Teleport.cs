using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class Teleport : MonoBehaviour
    {
        [SerializeField]
        Light[] lights;

        [SerializeField]
        GameObject[] objects; 

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
            LightFlickerOff.OnLightOn += HandleOnLightOn;
            LightFlickerOff.OnLightOff += HandleOnLightOff;
        }

        private void OnDisable()
        {
            LightFlickerOff.OnLightOn -= HandleOnLightOn;
            LightFlickerOff.OnLightOff -= HandleOnLightOff;
        }

        private void HandleOnLightOff(LightFlickerOff arg0)
        {
            foreach(Light light in lights)
                light.enabled = false;
            foreach(GameObject obj in objects)
                obj.SetActive(false);
        }

        private void HandleOnLightOn(LightFlickerOff arg0)
        {
            foreach (Light light in lights)
                light.enabled = true;
            foreach (GameObject obj in objects)
                obj.SetActive(true);
        }
    }

}
