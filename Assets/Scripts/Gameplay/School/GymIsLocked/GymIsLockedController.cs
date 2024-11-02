using DG.Tweening;
using EvolveGames;
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

        [SerializeField]
        PlayerWalkInTrigger bouncingBallMovingStep3;

        [SerializeField]
        ScaryDoor scaryDoorStep4;

        [SerializeField]
        PlayerWalkInTrigger scaryDoorStep4Trigger;

        [SerializeField]
        PlayerWalkInTrigger lockerConjuringTrigger;

        [SerializeField]
        GameObject showerDoor;

        [SerializeField]
        Transform playerBathroomTarget;

     
        [SerializeField]
        Transform lockerRoomGirlTarget;

        [SerializeField]
        GameObject schoolBlock;

        [SerializeField]
        GameObject gymBlock;

        [SerializeField]
        PlayerWalkInTrigger dialogTrigger;

        [SerializeField]
        DialogController dialogController;

        int lightOffCount = 0;
       

        const int deactivatedState = 0;

        const int workingState = 1;

        const int finalState = 100;

        int state = 0;
        Vector3 showerDoorLocalEulers;
        Vector3 playerOldPosition;
        Quaternion playeroldRotation;

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
            
        }

        private void OnEnable()
        {
            ballTrigger.OnEnter += HandleOnBallTriggerEnter;
            slamTrigger.OnEnter += HandleOnSlamTriggerEnter;
            gymDoor.OnLocked += HandleOnDoorLocked;
            kitchenConjuringTrigger.OnEnter += HandleOnKitchenTriggerEnter;
            bouncingBallController.OnStepCompleted += HandleOnBouncingBallStepCompleted;
            bouncingBallActivationStep1.OnEnter += HandleOnBouncingBallActivationTriggerEnter;
            bouncingBallMovingStep1.OnEnter += HandleOnBouncingBallMovingStepOneTriggerEnter;
            scaryDoorStep3Trigger.OnEnter += HandleOnScaryDoorStep3TriggerEnter;
            bouncingBallMovingStep3.OnEnter += HandleOnBouncingBallMovingTriggerEnter;
            scaryDoorStep4Trigger.OnEnter += HandleOnScaryDoorStep4TriggerEnter;
            lockerConjuringTrigger.OnEnter += HandleOnLockerConjuringTrigger;
            dialogTrigger.OnEnter += HandleOnDialogTriggerEnter;
        }

        private void OnDisable()
        {
            ballTrigger.OnEnter -= HandleOnBallTriggerEnter;
            slamTrigger.OnEnter -= HandleOnSlamTriggerEnter;
            gymDoor.OnLocked -= HandleOnDoorLocked;
            kitchenConjuringTrigger.OnEnter -= HandleOnKitchenTriggerEnter;
            bouncingBallController.OnStepCompleted -= HandleOnBouncingBallStepCompleted;
            bouncingBallActivationStep1.OnEnter -= HandleOnBouncingBallActivationTriggerEnter;
            bouncingBallMovingStep1.OnEnter -= HandleOnBouncingBallMovingStepOneTriggerEnter;
            scaryDoorStep3Trigger.OnEnter -= HandleOnScaryDoorStep3TriggerEnter;
            bouncingBallMovingStep3.OnEnter -= HandleOnBouncingBallMovingTriggerEnter;
            scaryDoorStep4Trigger.OnEnter -= HandleOnScaryDoorStep4TriggerEnter;
            lockerConjuringTrigger.OnEnter -= HandleOnLockerConjuringTrigger;
            dialogTrigger.OnEnter -= HandleOnDialogTriggerEnter;
        }

        private void HandleOnDialogTriggerEnter(PlayerWalkInTrigger arg0)
        {
            // Disable trigger
            dialogTrigger.gameObject.SetActive(false);

            // Play dialog
            dialogController.Play();
        }

        // When the player enter the bathroom and walk towards the shower
        private async void HandleOnLockerConjuringTrigger(PlayerWalkInTrigger trigger)
        {
            // Disable trigger
            lockerConjuringTrigger.gameObject.SetActive(false);

            // Slam the shower door

            Vector3 localEulers = showerDoor.transform.localEulerAngles;
            showerDoorLocalEulers = localEulers;
            localEulers.y = 90;
            showerDoor.transform.DOLocalRotate(localEulers, 0.25f).SetEase(Ease.OutBounce);

            // Add some delay
            await Task.Delay(500);

            // Hide the bouncing ball
            bouncingBallController.ForceStopMoving();
            bouncingBallController.gameObject.SetActive(false);

            // Add some delay
            await Task.Delay(200);
            // Play stinger
            AudioManager.Instance.PlayStinger(1);
            // Activate Lilith
            girl.transform.position = lockerRoomGirlTarget.transform.position;
            girl.transform.rotation = lockerRoomGirlTarget.transform.rotation;
            girl.SetActive(true);
            girl.GetComponent<EvilMaterialSetter>().SetEvil();
            girl.GetComponentInChildren<Animator>().SetTrigger("Walk");
            // Open the door
            showerDoor.transform.DOLocalRotate(showerDoorLocalEulers, 0.1f).SetEase(Ease.OutBounce);
            // Add some delay
            await Task.Delay(200);
            // Start flickering
            FlashlightFlickerController.Instance.FlickerToDarkeness(HandleOnFlickerToDarkness);

        }

        void HandleOnFlickerToDarkness(float duration)
        {
            // Deactivate Lilith
            girl.SetActive(false);
            // Remove blocks
            schoolBlock.GetComponent<SimpleActivator>().Init(false.ToString());
            gymBlock.GetComponent<SimpleActivator>().Init(false.ToString());
            // Set final state
            state = finalState;

            // Activate the dialog trigger
            dialogTrigger.gameObject.SetActive(true);

            // Save game
            SaveManager.Instance.SaveGame();
         
        }

       
        private async void HandleOnScaryDoorStep4TriggerEnter(PlayerWalkInTrigger trigger)
        {
            // Disable trigger
            scaryDoorStep4Trigger.gameObject.SetActive(false);

            // Open the door
            scaryDoorStep4.Open();

            // Activate the conjuring trigger
            lockerConjuringTrigger.gameObject.SetActive(true);

            // Await 
            await Task.Delay(3000);

            scaryDoorStep4.Init(false.ToString());
        }

        private async void HandleOnScaryDoorStep3TriggerEnter(PlayerWalkInTrigger trigger)
        {
            // Disable trigger
            scaryDoorStep3Trigger.gameObject.SetActive(false);

            // Open the door
            scaryDoorStep3.Open();

            // Await 
            await Task.Delay(3000);

            scaryDoorStep3.Init(false.ToString());

        }

        private void HandleOnBouncingBallMovingStepOneTriggerEnter()
        {
            switch (bouncingBallController.Step)
            {
                case 1:
                // Disable trigger
                bouncingBallMovingStep1.gameObject.SetActive(false);
                // Move ball
                bouncingBallController.Move();
                break;
            }
        }

        private async void HandleOnBouncingBallMovingTriggerEnter(PlayerWalkInTrigger trigger)
        {
            switch (bouncingBallController.Step)
            {
                //case 1:
                //    // Disable trigger
                //    bouncingBallMovingStep1.gameObject.SetActive(false);
                //    // Move ball
                //    bouncingBallController.Move();
                //    break;
                case 3:
                    // Disable trigger
                    bouncingBallMovingStep3.gameObject.SetActive(false);
                    // Move ball
                    bouncingBallController.Move();
                    // Play stinger
                    AudioManager.Instance.PlayStinger(2);
                    // Add some delay
                    await Task.Delay(350);
                    // Flicker
                    FlashlightFlickerController.Instance.FlickerOnce();
                    // Wait
                    await Task.Delay(TimeSpan.FromSeconds(FlashlightFlickerController.FlickerDuration / 2f));
                    // Disable ball 
                    bouncingBallController.ForceStopMoving();
                    // Add delay
                    await Task.Delay(1000);
                    // Activate the step 4
                    bouncingBallController.MoveToNextStep();
                    // Activate the step 4 door trigger
                    scaryDoorStep4Trigger.gameObject.SetActive(true);

                    break;
            }


            
            
        }

        private async void HandleOnBouncingBallActivationTriggerEnter(PlayerWalkInTrigger trigger)
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

                    // Girls don't play basketball
                    //VoiceManager.Instance.Talk(Speaker.Puck, 5);
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

            
            // Set character transform
            girl.transform.position = girlKitchenTarget.position;
            girl.transform.rotation = girlKitchenTarget.rotation;
            // Activate character
            girl.SetActive(true);
            // Set material
            girl.GetComponent<EvilMaterialSetter>().SetNormal();
            // Set animation
            girl.GetComponentInChildren<Animator>().SetTrigger("RunScary");

            // Play stinger
            AudioManager.Instance.PlayStinger(1);

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
            Debug.Log($"Setting position:{girlTarget.transform.position}");
            girl.transform.position = girlTarget.transform.position;
            girl.transform.rotation = girlTarget.transform.rotation;
            girl.SetActive(true);
            girl.GetComponent<EvilMaterialSetter>().SetNormal();
            //girl.GetComponentInChildren<Animator>().SetTrigger("Pose1");
            girl.transform.DOMoveY(0.140f, 0.05f, false);

            // Play stinger
            AudioManager.Instance.PlayStinger(1);

            // Start the flickering
            await Task.Delay(400);
            FlashlightFlickerController.Instance.FlickerAndWatch(HandleOnLightOffBefore, HandleOnLightOffAfter);

            // Some delay
            await Task.Delay(3);
            // Activate the bouncing ball
            bouncingBallController.gameObject.SetActive(true);
            // Set first step
            bouncingBallController.MoveToNextStep();
        }

        

        private void HandleOnLightOffBefore()
        {
            girl.transform.position = girlTarget2.position;
            girl.transform.rotation = girlTarget2.rotation;
            girl.GetComponentInChildren<Animator>().SetTrigger("Walk");
           
        }

        private async void HandleOnLightOffAfter()
        {
            girl.SetActive(false);

            await Task.Delay(1000);

            VoiceManager.Instance.Talk(Speaker.Puck, 4);
           
        }

        private void HandleOnSlamTriggerEnter(PlayerWalkInTrigger trigger)
        {
            // Deacivate trigger
            slamTrigger.gameObject.SetActive(false);

            // Play audio
            slamAudio.Play();

            // Set the door trigger on ( now if we try to open the door something will be triggered )
            doorTriggerOn = true;


        }

        private async void HandleOnBallTriggerEnter(PlayerWalkInTrigger trigger)
        {
            // Disable trigger
            ballTrigger.gameObject.SetActive(false);

            // Launch the ball
            ball.SetActive(true);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            Vector3 dir = ballTarget.position - rb.position;
            rb.AddForce(dir.normalized * 10, ForceMode.VelocityChange);

            // Play audio
            var source = rb.GetComponentInChildren<AudioSource>();
            source.Play();

            await Task.Delay(500);
            source.Play();

            await Task.Delay(200);
            source.Play();
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
            scaryDoorStep4Trigger.gameObject.SetActive(false);
            lockerConjuringTrigger.gameObject.SetActive(false);
            
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
