using Aura2API;
using EvolveGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Events;

namespace Kidnapped
{
    public class Teleport : MonoBehaviour
    {
        public UnityAction OnLightOff;
        public UnityAction OnLightOn;

        [SerializeField]
        Light[] lights;

        [SerializeField]
        GameObject[] objects;

        [SerializeField]
        Transform target;

        [SerializeField]
        bool keepRotation;

       
        private void OnTriggerEnter(Collider other)
        {
            Flashlight.Instance.GetComponent<FlashlightFlickerController>().FlickerToDarkeness(HandleOnLightOff/*, HandleOnLightOn*/);
        }

        void MovePlayer()
        {
            PlayerController.Instance.characterController.enabled = false;
            PlayerController.Instance.transform.position = target.position;
            if(!keepRotation)
                PlayerController.Instance.transform.rotation = target.rotation;
            PlayerController.Instance.characterController.enabled = true;
        }

        private async void HandleOnLightOff(float duration)
        {
            foreach(Light light in lights)
                DisableLight(light);
            foreach(GameObject obj in objects)
                obj.SetActive(false);
            MovePlayer();
            Debug.Log(OnLightOff);
            OnLightOff?.Invoke();

            await Task.Delay(TimeSpan.FromSeconds(duration));

            foreach (Light light in lights)
                EnableLight(light);
            foreach (GameObject obj in objects)
                obj.SetActive(true);
            OnLightOn?.Invoke();
        }

        //private void HandleOnLightOn()
        //{
        //    foreach (Light light in lights)
        //        EnableLight(light);
        //    foreach (GameObject obj in objects)
        //        obj.SetActive(true);
        //    OnLightOn?.Invoke();
        //}

        void DisableLight(Light light)
        {
            light.enabled = false;
            light.GetComponent<AuraLight>().enabled = false;
        }

        void EnableLight(Light light)
        {
            light.enabled = true;
            light.GetComponent<AuraLight>().enabled = true;
        }

        //public void Activate()
        //{
        //    Flashlight.Instance.GetComponent<LightFlickerOff>().Play();
        //}
    }

}
