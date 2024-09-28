using DG.Tweening;
using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kidnapped
{
    public class GymIsLockedController : MonoBehaviour, ISavable
    {
        [SerializeField]
        PlayerWalkInTrigger ballTrigger;

        [SerializeField]
        GameObject ball;

        [SerializeField]
        Transform ballTarget;

        [SerializeField]
        PlayerWalkInTrigger slamTrigger;

        [SerializeField]
        AudioSource slamAudio;

        [SerializeField]
        ScaryDoor gymDoor;

        [SerializeField]
        GameObject girl;

        [SerializeField]
        Transform girlTarget;

        [SerializeField]
        Transform girlTarget2;

        [SerializeField]
        bool doorTriggerOn = false;

        [SerializeField]
        BouncingBallController bouncingBallController;

        [SerializeField]
        PlayerWalkInAndLookTrigger kitchenConjuringTrigger;

        [SerializeField]
        Transform girlKitchenTarget;

        [SerializeField]
        PlayerWalkInTrigger bouncingBallActivationStep1;

        [SerializeField]
        PlayerWalkInAndLookTrigger bouncingBallMovingStep1;

        [SerializeField]
        Transform ballKitchenTarget;

        [SerializeField]
        ScaryDoor scaryDoorStep3;

        [SerializeField]
        PlayerWalkInTrigger scaryDoorStep3Trigger;

        int lightOffCount = 0;
       

        const int deactivatedState = 0;

        const int workingState = 1;

        const int finalState = 100;

        int state = 0;

        private void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = deactivatedState.ToString();
            Init(data);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.H))
            {
                girl.SetActive(true);
                girl.transform.position = girlTarget.transform.position;
                girl.transform.rotation = girlTarget.transform.rotation;
                girl.GetComponent<EvilMaterialSetter>().SetNormal();
                girl.GetComponentInChildren<Animator>().SetTrigger("Pose1");
                girl.transform.DOMoveY(0.140f, 0.05f, false);
                // 
            }
        }

        private void OnEnable()
        {
            ballTrigger.OnEnter += HandleOnBallTriggerEnter;
            slamTrigger.OnEnter += HandleOnSlamTriggerEnter;
            gymDoor.OnLocked += HandleOnDoorLocked;
            kitchenConjuringTrigger.OnEnter += HandleOnKitchenTriggerEnter;
            bouncingBallController.OnStepCompleted += HandleOnBouncingBallStepCompleted;
            bouncingBallActivationStep1.OnEnter += HandleOnBouncingBallActivationTriggerEnter;
            bouncingBallMovingStep1.OnEnter += HandleOnBouncingBallMovingTriggerEnter;
            scaryDoorStep3Trigger.OnEnter += HandleOnScaryDoorStep3TriggerEnter;
        }

        private void OnDisable()
        {
            ballTrigger.OnEnter -= HandleOnBallTriggerEnter;
            slamTrigger.OnEnter -= HandleOnSlamTriggerEnter;
            gymDoor.OnLocked -= HandleOnDoorLocked;
            kitchenConjuringTrigger.OnEnter -= HandleOnKitchenTriggerEnter;
            bouncingBallController.OnStepCompleted -= HandleOnBouncingBallStepCompleted;
            bouncingBallActivationStep1.OnEnter -= HandleOnBouncingBallActivationTriggerEnter;
            bouncingBallMovingStep1.OnEnter -= HandleOnBouncingBallMovingTriggerEnter;
            scaryDoorStep3Trigger.OnEnter -= HandleOnScaryDoorStep3TriggerEnter;
        }

        private async void HandleOnScaryDoorStep3TriggerEnter()
        {
            // Disable trigger
            scaryDoorStep3Trigger.gameObject.SetActive(false);

            // Open the door
            scaryDoorStep3.Open();

            // Await 
            await Task.Delay(3000);


        }

        private void HandleOnBouncingBallMovingTriggerEnter()
        {
            if(bouncingBallController.Step == 1)
                bouncingBallMovingStep1.gameObject.SetActive(false);

            bouncingBallController.Move();
            
        }

        private async void HandleOnBouncingBallActivationTriggerEnter()
        {   
            int step = bouncingBallController.Step;
            switch(step)
            {
                case 0:
                    // Flicker
                    FlashlightFlickerController.Instance.FlickerOnce();
                    // Wait
                    await Task.Delay(TimeSpan.FromSeconds(FlashlightFlickerController.FlickerDuration / 2f));
                    // Remove ball 
                    ball.SetActive(false);
                    // Wait a little more
                    await Task.Delay(2000);
                    // Activate next bouncing ball event
                    bouncingBallActivationStep1.gameObject.SetActive(false);
                    bouncingBallMovingStep1.gameObject.SetActive(true);
                    bouncingBallController.MoveToNextStep();
                    break;
            }
        }

        private void HandleOnBouncingBallStepCompleted(int stepIndex)
        {
            Debug.Log($"OnBallStepCompleted:{stepIndex}");
            switch(stepIndex)
            {
                case 0:
                    kitchenConjuringTrigger.gameObject.SetActive(true);
                    break;
                case 1:
                    bouncingBallController.MoveToNextStep();
                    break;
                case 2:
                    // Move the bouncing ball to next step
                    bouncingBallController.MoveToNextStep();
                    // Activate the scary door trigger
                    scaryDoorStep3Trigger.gameObject.SetActive(true);
                    break;
            }
        }

        private async void HandleOnKitchenTriggerEnter()
        {
            // Disable trigger
            kitchenConjuringTrigger.gameObject.SetActive(false);

            // Activate the ball in the kitchen
            ball.SetActive(true);
            Rigidbody ballRB = ball.GetComponent<Rigidbody>();
            ballRB.isKinematic = true;
            ballRB.velocity = Vector3.zero;
            ballRB.angularVelocity = Vector3.zero;
            ball.transform.position = ballKitchenTarget.transform.position;
            //ballRB.isKinematic = false;

            // Activate character
            girl.SetActive(true);
            // Set transform
            girl.transform.position = girlKitchenTarget.position;
            girl.transform.rotation = girlKitchenTarget.rotation;
            // Set material
            girl.GetComponent<EvilMaterialSetter>().SetNormal();
            // Set animation
            girl.GetComponentInChildren<Animator>().SetTrigger("RunScary");

            // Add some delay
            await Task.Delay(1200);

            // Deactivate character
            girl.SetActive(false);

            // Lilith conjuring
            Debug.Log("Lilith conjuring in the kitchen");
            bouncingBallActivationStep1.gameObject.SetActive(true);
        }

        private async void HandleOnDoorLocked(ScaryDoor arg0)
        {
            if (!doorTriggerOn)
                return;

            // Deactivate trigger
            doorTriggerOn = false;

            // Add some delay
            await Task.Delay(500);

            FlashlightFlickerController.Instance.FlickerOnce();
            await Task.Delay(TimeSpan.FromSeconds(FlashlightFlickerController.FlickerDuration / 2));

            // Hide ball 
            ball.SetActive(false);

            // Show Lilith
            girl.SetActive(true);
            girl.GetComponent<EvilMaterialSetter>().SetNormal();
            girl.transform.position = girlTarget.transform.position;
            girl.transform.rotation = girlTarget.transform.rotation;
            //girl.GetComponentInChildren<Animator>().SetTrigger("Pose1");
            girl.transform.DOMoveY(0.140f, 0.05f, false);

            // Start the flickering
            await Task.Delay(400);
            FlashlightFlickerController.Instance.FlickerAndWatch(HandleOnLightOff);

            // Some delay
            await Task.Delay(3);
            // Activate the bouncing ball
            bouncingBallController.gameObject.SetActive(true);
            // Set first step
            bouncingBallController.MoveToNextStep();
        }

        

        private void HandleOnLightOff()
        {
            if(lightOffCount == 0)
            {
                girl.transform.position = girlTarget2.position;
                girl.transform.rotation = girlTarget2.rotation;
                girl.GetComponentInChildren<Animator>().SetTrigger("Walk");
            }
            else if(lightOffCount == 1)
            {
                girl.SetActive(false);
            }

            lightOffCount++;
        }

        private void HandleOnSlamTriggerEnter()
        {
            // Deacivate trigger
            slamTrigger.gameObject.SetActive(false);

            // Play audio
            slamAudio.Play();

            // Set the door trigger on ( now if we try to open the door something will be triggered )
            doorTriggerOn = true;


        }

        private void HandleOnBallTriggerEnter()
        {
            // Disable trigger
            ballTrigger.gameObject.SetActive(false);

            // Launch the ball
            ball.SetActive(true);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            Vector3 dir = ballTarget.position - rb.position;
            rb.AddForce(dir.normalized * 10, ForceMode.VelocityChange);

        }

        public void SetWorkingState()
        {
            Init(workingState.ToString());
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

            // Deactivated is the default state
            ballTrigger.gameObject.SetActive(false);
            ball.SetActive(false);
            slamTrigger.gameObject.SetActive(false);
            bouncingBallController.gameObject.SetActive(false);
            kitchenConjuringTrigger.gameObject.SetActive(false);
            bouncingBallActivationStep1.gameObject.SetActive(false);
            bouncingBallMovingStep1.gameObject.SetActive(false);
            scaryDoorStep3Trigger.gameObject.SetActive(false);
            switch (state)
            {
                case workingState:
                    ballTrigger.gameObject.SetActive(true);
                    slamTrigger.gameObject.SetActive(true);
                    break;

                case finalState:
                    break;

            }
            
        }
        #endregion
    }

}
