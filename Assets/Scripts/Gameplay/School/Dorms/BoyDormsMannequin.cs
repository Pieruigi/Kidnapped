using DG.Tweening;
using EvolveGames;
using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kidnapped
{
    public class BoyDormsMannequin : MonoBehaviour, ISavable
    {

        [SerializeField]
        GameObject scaryMannequin;

        [SerializeField]
        GameObject scaryMannequinHead;

        [SerializeField]
        GameObject scaryMannequinTrunk;

        [SerializeField]
        GameObject scaryMannequinArm;

        [SerializeField]
        GameObject lockerBlock;

        [SerializeField]
        AudioSource scaryMannequinAudioSource;

        [SerializeField]
        AudioSource scaryMannequinDestroyAudioSource;

        [SerializeField]
        GameObject puck;

        [SerializeField]
        PlayerWalkInAndLookTrigger scaryMannequinTrigger;

        [SerializeField]
        GameObject ventriloquistPrefab;

        [SerializeField]
        Transform ventriloquistTarget;

        [SerializeField]
        ObjectInteractor jarInteractor;

        [SerializeField]
        GameObject jarPrefab;

        [SerializeField]
        GameObject openJarPrefab;

        [SerializeField]
        Transform jarTarget;

        [SerializeField]
        AudioSource jarAudioSource;

        [SerializeField]
        GameObject bloodyFloorPrefab;

        [SerializeField]
        Transform bloodyFloorTarget;

        [SerializeField]
        GameObject hookedPartsPrefab;

        [SerializeField]
        Transform hookedPartsTarget;

        [SerializeField]
        SimpleActivator abandonedSchoolKitchen;

        [SerializeField]
        SimpleActivator originalSchoolKitchen;

        [SerializeField]
        Transform schoolKitchenPlayerTarget;

        [SerializeField]
        KitchenHunt kitchenHunt;

        [SerializeField]
        GameObject schoolEntranceBlock;

        const int notReadyState = 0;
        const int readyState = 100;
        const int completedState = 200;
        GameObject ventriloquist;
        GameObject bloodyFloor;
        GameObject hookedParts;

        int state = 0;

        GameObject jar;

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
     
            scaryMannequinTrigger.OnEnter += HandleOnLookTriggerEnter;
            jarInteractor.OnInteraction += HandleOnJarInteractor;
        }

        private void OnDisable()
        {
            scaryMannequinTrigger.OnEnter -= HandleOnLookTriggerEnter;
            jarInteractor.OnInteraction -= HandleOnJarInteractor;
        }

        private void HandleOnJarInteractor(ObjectInteractor arg0)
        {
            // Play sound
            jarAudioSource.Play();
            // Flicker
            FlashlightFlickerController.Instance.FlickerToDarkeness(OnJarInteractionFlicker);
            // Start rotating the jar
            jar.transform.DOLocalRotate(Vector3.right * 90, 0.5f, RotateMode.LocalAxisAdd);
        }

        private void OnJarInteractionFlicker(float duration)
        {
            // Change ambient
            GameSceneAudioManager.Instance.PlayAmbience(2);

            // Destroy the jar
            Destroy(jar);

            // Instantiate the open jar
            jar = Instantiate(openJarPrefab);

            // Set position and rotation
            jar.transform.position = jarTarget.position;
            jar.transform.rotation = jarTarget.rotation;

            // Create the bloody rising water
            bloodyFloor = Instantiate(bloodyFloorPrefab);
            // Set position and rotation
            bloodyFloor.transform.position = bloodyFloorTarget.position;
            bloodyFloor.transform.rotation = bloodyFloorTarget.rotation;
            // Register callback
            bloodyFloor.GetComponent<BloodyFloor>().OnHeightReached += HandleOnBloodyFloor;
            // Remove the locker room block and the mannequin
            lockerBlock.SetActive(false);
            scaryMannequin.SetActive(false);
            // Spawn hooked body parts
            hookedParts = Instantiate(hookedPartsPrefab);
            // Set position and rotation
            hookedParts.transform.position = hookedPartsTarget.position;
            hookedParts.transform.rotation = hookedPartsTarget.rotation;
            // Prevent the player from crouching
            PlayerController.Instance.CanCrouch = false;
        }

        private void HandleOnBloodyFloor()
        {
            // Flicker
            FlashlightFlickerController.Instance.FlickerToDarkeness(OnBloodyFloorFlickerCallback);

        }

        private async void OnBloodyFloorFlickerCallback(float duration)
        {
            // Change ambient
            GameSceneAudioManager.Instance.PlayAmbience(3);

            // Set completed state
            Init(completedState.ToString());

            // Disable the school abandoned kitchen
            abandonedSchoolKitchen.Init(false.ToString());

            // Enable the school original kitchen
            originalSchoolKitchen.Init(true.ToString());

            // Block school entrance
            schoolEntranceBlock.SetActive(true);

            // Init the new gameplay part
            kitchenHunt.SetReady();

            // Set this one completed
            Init(completedState.ToString());

            // Move the player in the school kitchen
            PlayerController.Instance.ForcePositionAndRotation(schoolKitchenPlayerTarget.position, schoolKitchenPlayerTarget.rotation);

            // Player can crouch again
            PlayerController.Instance.CanCrouch = true;

            await Task.Delay(1000);

            // Save game
            SaveManager.Instance.SaveGame();

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

            // Play destroy clip
            scaryMannequinDestroyAudioSource.Play();

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

            // Instantiate the jar object
            jar = Instantiate(jarPrefab);
            // Set position and rotation
            jar.transform.position = jarTarget.position;
            jar.transform.rotation = jarTarget.rotation;

            // Enable jar interactor
            jarInteractor.gameObject.SetActive(true);
        }

        private void OnFlickerOffCallback()
        {
            puck.SetActive(false);
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
            // Disable scary mannequin
            scaryMannequin.SetActive(false);
            scaryMannequinHead.GetComponent<Rigidbody>().isKinematic = true;
            scaryMannequinTrunk.GetComponent<Rigidbody>().isKinematic = true;
            scaryMannequinArm.GetComponent<Rigidbody>().isKinematic = true;
            // Disable block
            lockerBlock.SetActive(false);
            // Disable mannequin trigger
            scaryMannequinTrigger.gameObject.SetActive(false);
            // Disable jar interactor
            jarInteractor.gameObject.SetActive(false);
            

            switch (state)
            {
                case readyState:
                    // Enable Puck destroying mannequin trigger
                    scaryMannequinTrigger.gameObject.SetActive(true);
                    // Enable mannequin
                    scaryMannequin.SetActive(true);
                    // Activate locker block
                    lockerBlock.SetActive(true);
                    // Enable the ventriloquist
                    ventriloquist = Instantiate(ventriloquistPrefab);
                    // Set position and rotation
                    ventriloquist.transform.position = ventriloquistTarget.position;
                    ventriloquist.transform.rotation = ventriloquistTarget.rotation;
                    // Set scripted eyes
                    ventriloquist.GetComponent<VentriloquistEyes>().UseScriptedEyes = true;
                    
                    break;
                case completedState: 
                    break;

            }

            //if (state == completedState)
            //{
            //    // Disable scary mannequin
            //    scaryMannequin.SetActive(false);
            //    // Disable scary mannequin look trigger
            //    scaryMannequinTrigger.gameObject.SetActive(false);
            //}
        }
        #endregion
    }

}
