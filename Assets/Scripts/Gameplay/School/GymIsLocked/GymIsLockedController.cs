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
        BouncingBallController theBouncingBall;

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
        }

        private void OnDisable()
        {
            ballTrigger.OnEnter -= HandleOnBallTriggerEnter;
            slamTrigger.OnEnter -= HandleOnSlamTriggerEnter;
            gymDoor.OnLocked -= HandleOnDoorLocked;
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
            theBouncingBall.gameObject.SetActive(true);
            // Set first step
            theBouncingBall.MoveToNextStep();
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
            theBouncingBall.gameObject.SetActive(false);

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
