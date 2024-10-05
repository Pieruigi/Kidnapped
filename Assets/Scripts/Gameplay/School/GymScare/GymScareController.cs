using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Kidnapped
{
    public class GymScareController : MonoBehaviour, ISavable
    {
        [Header("Ball")]
        [SerializeField]
        GameObject ball;

        [SerializeField]
        Collider ballTrigger;

        [SerializeField]
        Transform ballTarget;

        [SerializeField]
        Transform ballEnd;

        [Header("Doors")]
        [SerializeField]
        Collider doorTrigger;

        [SerializeField]
        ScaryDoor door;

        [SerializeField]
        PlayerWalkInTwoWayTrigger doorStateTrigger;

        [SerializeField]
        GameObject doorUnblock;

        [SerializeField]
        GameObject doorBlock;

        [Header("Cat")]
        [SerializeField]
        Transform catTarget;

        [SerializeField]
        Transform catDestination;

        [SerializeField]
        GameObject catDeactivator;

        [SerializeField]
        GameObject[] gyms;

        [SerializeField]
        ScaryGroup[] scaryGroups;

        [SerializeField]
        InTheFog inTheFogController;


        int scaryIndex = 0;
        int state = 0;

        GameObject brokenMannequin;
        
        private void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = "0";
                
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
            ballTrigger.GetComponent<PlayerWalkInTrigger>().OnEnter += HandleOnBallTriggerEnter;
            doorTrigger.GetComponent<PlayerWalkInTrigger>().OnEnter += HandleOnDoorTriggerEnter;
            doorStateTrigger.OnExit += HandleOnDoorStateTriggerExit;
        }

        private void OnDisable()
        {
            ballTrigger.GetComponent<PlayerWalkInTrigger>().OnEnter -= HandleOnBallTriggerEnter;
            doorTrigger.GetComponent<PlayerWalkInTrigger>().OnEnter -= HandleOnDoorTriggerEnter;
            doorStateTrigger.OnExit -= HandleOnDoorStateTriggerExit;
        }

        private void HandleOnDoorStateTriggerExit(bool fromBehind)
        {
            if (state > 0)
                return;

            if (fromBehind)
                door.Close();
            else
                door.Open();

        }

        private void HandleOnDoorTriggerEnter()
        {
            SlamTheDoor();
        }

        private void HandleOnBallTriggerEnter()
        {
           
#pragma warning disable CS4014
            LaunchTheBall();
#pragma warning restore CS4014
        }

        void SetDoorBlock()
        {
            doorBlock.GetComponent<ISavable>().Init(true.ToString());
            doorUnblock.GetComponent<ISavable>().Init(false.ToString());
            //doorBlock.SetActive(true);
            //doorUnblock.SetActive(false);
            
        }

        void ResetDoorBlock()
        {
            doorBlock.GetComponent<ISavable>().Init(false.ToString());
            doorUnblock.GetComponent<ISavable>().Init(true.ToString());
            //doorBlock.SetActive(false);
            //doorUnblock.SetActive(true);
        }

        public void SlamTheDoor()
        {
            door.Close();

            if (state > 0)
                return;

            state = 10;

            SetDoorBlock();

            ballTrigger.enabled = true;

            // Activate the black cat
            CatController.Instance.StandAndPlayRandom(catTarget.position, catTarget.rotation);
        }

     

        public async Task LaunchTheBall()
        {
            // Update state
            state = 20;

            // Launch the ball against the wall
            ballTrigger.enabled = false;

            // Activate the ball rigidbody
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            // Launch the ball
            Vector3 dir = ballTarget.position - ball.transform.position;
            rb.AddForce(dir.normalized * 15, ForceMode.VelocityChange);

            // Activate the cat deactivation trigger
            catDeactivator.SetActive(true);

            await Task.Delay(500);

            CatController.Instance.ScaredAndRunAway(catDestination.position);

            // Just wait for a while and then flicker the flashlight out and start the new section
            await Task.Delay(2500);

            // Flashlight
            Flashlight.Instance.GetComponent<FlashlightFlickerController>().FlickerToDarkeness(HandleOnLightOff, HandleOnLightComplete);


            
        }

        void HandleOnInteraction()
        {
            //state = 40;
            scaryGroups[scaryIndex].GetComponentInChildren<ObjectInteractor>().OnInteraction -= HandleOnInteraction;

            // Flashlight
            Flashlight.Instance.GetComponent<FlashlightFlickerController>().FlickerToDarkeness(HandleOnLightOff, HandleOnLightComplete);
        }

        private void HandleOnLightOff(float duration)
        {
            switch (state)
            {
                case 20:
                    // Switch to modern school
                    gyms[0].GetComponent<SimpleActivator>().Init(false.ToString());
                    gyms[1].GetComponent<SimpleActivator>().Init(true.ToString());

                    // Create first scary group
                    scaryIndex = 0;
                    scaryGroups[scaryIndex].Create();
                    // Get the target and set the callback
                    scaryGroups[scaryIndex].GetComponentInChildren<ObjectInteractor>().OnInteraction += HandleOnInteraction;
                    break;
                case 30:
                    // Switch to abandoned school
                    gyms[0].GetComponent<SimpleActivator>().Init(true.ToString());
                    gyms[1].GetComponent<SimpleActivator>().Init(false.ToString());
                    // remove scary group
                    scaryIndex = 0;
                    scaryGroups[scaryIndex].Release();
                    // Reset the door block
                    ResetDoorBlock();
                    break;
            }

            
        }

        

        private void HandleOnLightComplete()
        {
            switch(state) 
            { 
                case 20:
                    // Set the new state
                    state = 30;

                    // Save state
                    SaveManager.Instance.SaveGame();
                    break;
                case 30:
                    // Update state
                    state = 40; // Complete
                    // Set in the fog controller ready
                    inTheFogController.SetReady();
                    // Save
                    SaveManager.Instance.SaveGame();
                    break;
            
            }

           
        }

        void DisableScaryGroupAll()
        {
            foreach(var group in scaryGroups)
            {
                group.Release();
            }
        }

        #region save system
        [Header("Save System")]
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

            //ResetDoorBlock();
            ballTrigger.enabled = false;
            catDeactivator.SetActive(false);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            DisableScaryGroupAll();

            if (state == 30) // Step to save ( the cat freaking out )
            {
                //doorTrigger.enabled = false;
                rb.isKinematic = false;
                rb.position = ballEnd.position;
                scaryIndex = 0;
                scaryGroups[scaryIndex].Create();
                scaryGroups[scaryIndex].GetComponentInChildren<ObjectInteractor>().OnInteraction += HandleOnInteraction;
            }
            
            if(state == 40) // Dummy target touched
            {
                //doorTrigger.enabled = false;
                rb.isKinematic = false;
                rb.position = ballEnd.position;
                //ResetDoorBlock();
            }
        }
        #endregion
    }

}
