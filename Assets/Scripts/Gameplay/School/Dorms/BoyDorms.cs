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
        GameObject scaryMannequin;

        [SerializeField]
        GameObject scaryMannequinHead;

        [SerializeField]
        GameObject scaryMannequinTrunk;

        [SerializeField]
        GameObject scaryMannequinArm;

        [SerializeField]
        AudioSource scaryMannequinAudioSource;

        [SerializeField]
        GameObject puck;

        [SerializeField]
        PlayerWalkInAndLookTrigger scaryMannequinTrigger;

        const int notReadyState = 0;
        const int readyState = 100;
        const int completedState = 200;

        int state = 0;

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
            scaryMannequinTrigger.OnEnter += HandleOnLookTriggerEnter;
        }

        private void OnDisable()
        {
            entranceCloseTrigger.OnEnter -= HandleOnEntranceCloseTriggerEnter;
            scaryMannequinTrigger.OnEnter -= HandleOnLookTriggerEnter;
        }

        
        private async void HandleOnLookTriggerEnter()
        {
            // Play stinger
            scaryMannequinAudioSource.Play();
            // Deactivate the trigger
            scaryMannequinTrigger.gameObject.SetActive(false);
            // Set Puck position and rotation
            Vector3 pos = scaryMannequin.transform.position - PlayerController.Instance.transform.position;
            pos = pos.normalized;
            pos = scaryMannequin.transform.position + pos * .5f;
            puck.transform.position = pos;
            puck.transform.forward = Vector3.ProjectOnPlane(PlayerController.Instance.transform.position - puck.transform.position, Vector3.up);
            // Set material
            puck.GetComponent<EvilMaterialSetter>().SetEvil();
            // Activate Puck
            puck.gameObject.SetActive(true);
            // Set Puck animation
            puck.GetComponentInChildren<Animator>().SetTrigger("AttackHigh");

            // Add some delay
            await Task.Delay(500);

            // Activate physics
            Rigidbody rb = scaryMannequinHead.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            // Toss the head
            //Vector3 dir = PlayerController.Instance.transform.position - headRB.position;
            //dir = Vector3.ProjectOnPlane(dir, Vector3.up).normalized;
            //dir += Vector3.up * .1f;
            rb.AddForce(new Vector3(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3)), ForceMode.VelocityChange);
            rb.AddTorque(new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10)));
            rb = scaryMannequinTrunk.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(new Vector3(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3)), ForceMode.VelocityChange);
            rb.AddTorque(new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10)));
            rb = scaryMannequinArm.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(new Vector3(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3)), ForceMode.VelocityChange);
            rb.AddTorque(new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10)));

            await Task.Delay(2000);
            FlashlightFlickerController.Instance.FlickerOnce(OnFlickerOffCallback);

        }

        private void OnFlickerOffCallback()
        {
            puck.SetActive(false);
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
            // Disable the scary mannequin head rigidbody
            scaryMannequinHead.GetComponent<Rigidbody>().isKinematic = true;
            scaryMannequinTrunk.GetComponent<Rigidbody>().isKinematic = true;
            scaryMannequinArm.GetComponent<Rigidbody>().isKinematic = true;
            if (state == completedState)
            {
                // Disable scary mannequin
                scaryMannequin.SetActive(false);
                // Disable scary mannequin look trigger
                scaryMannequinTrigger.gameObject.SetActive(false);
            }
        }
        #endregion
    }

}
