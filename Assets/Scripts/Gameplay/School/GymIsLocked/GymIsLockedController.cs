using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
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
                slamAudio.Play();
        }

        private void OnEnable()
        {
            ballTrigger.OnEnter += HandleOnBallTriggerEnter;
            slamTrigger.OnEnter += HandleOnSlamTriggerEnter;
        }

        private void OnDisable()
        {
            ballTrigger.OnEnter -= HandleOnBallTriggerEnter;
            slamTrigger.OnEnter -= HandleOnSlamTriggerEnter;
        }

        private void HandleOnSlamTriggerEnter()
        {
            // Deacivate trigger
            slamTrigger.gameObject.SetActive(false);

            // Play audio
            slamAudio.Play();
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
