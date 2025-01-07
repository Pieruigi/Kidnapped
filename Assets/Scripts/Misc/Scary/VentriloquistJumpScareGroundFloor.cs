using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Kidnapped
{
    public class VentriloquistJumpScareGroundFloor : MultiStateSaver
    {
        [SerializeField]
        GameObject furniturePrefab;

        [SerializeField]
        Transform notReadyTarget;

        [SerializeField]
        Transform readyTarget;

        [SerializeField]
        GameObject ventriloquistPrefab;

        [SerializeField]
        Transform ventriloquistTarget;

        [SerializeField]
        Transform ventriloquistEndTarget;

        [SerializeField]
        AudioSource laughAudioSource;

        [SerializeField]
        PlayerWalkInAndLookTrigger trigger;

        //[SerializeField]
        //GameObject jinxPrefab;

        const int notActive = 0;
        const int notReady = 1;
        const int ready = 2;
        const int complete = 3;

        GameObject furniture;
        GameObject ventriloquist;
        //GameObject jinx;

        protected override void Awake()
        {
            base.Awake();

            Initialize(GetState());
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
//#if UNITY_EDITOR
//            if (Input.GetKeyDown(KeyCode.E))
//            {
//                SpawnVentriloquist();
//            }
//#endif
        }

        private void OnEnable()
        {
            trigger.OnEnter += HandleOnTriggerEnter;
        }

        private void OnDisable()
        {
            trigger.OnEnter -= HandleOnTriggerEnter;
        }

        private async void HandleOnTriggerEnter(PlayerWalkInAndLookTrigger trigger)
        {
            trigger.gameObject.SetActive(false);
            await Task.Delay(250);
            SpawnVentriloquist();
        }

        public void SetReady()
        {
            SetState(ready);
            Initialize(GetState());
        }

        public void SetNotReady()
        {
            SetState(notReady); 
            Initialize(GetState());
        }

        async void SpawnVentriloquist()
        {
            ventriloquist = Instantiate(ventriloquistPrefab);
            ventriloquist.transform.position = ventriloquistTarget.position;
            ventriloquist.transform.rotation = ventriloquistTarget.rotation;
            ventriloquist.SetActive(true);
            var anim = ventriloquist.GetComponent<Animator>();
            anim.SetInteger("Type", 7);
            anim.SetTrigger("Pose");
            // Use scripted eyes to look at the player
            var eyes = ventriloquist.GetComponent<VentriloquistEyes>(); 
            eyes.ScriptedEyesTarget = Camera.main.transform;
            eyes.UseScriptedEyes = true;
            // Play stinger
            GameSceneAudioManager.Instance.PlayStinger(1);

            // Move
            ventriloquist.transform.DOMove(ventriloquistEndTarget.position, 0.2f, false);
            
            laughAudioSource.Play();

            await Task.Delay(1300);
            FlashlightFlickerController.Instance.FlickerOnce(() =>
            {
                SetState(complete);
                Initialize(GetState());
            });
        }

        void Initialize(int state)
        {
            switch (state)
            {
                case notActive:
                    if(furniture)
                        Destroy(furniture);
                    trigger.gameObject.SetActive(false);
                    break;
                case notReady:
                    if (!furniture)
                        furniture = Instantiate(furniturePrefab);
                    furniture.transform.position = notReadyTarget.position;
                    furniture.transform.rotation = notReadyTarget.rotation;
                    trigger.gameObject.SetActive(false);
                    break;
                case ready:
                    if (!furniture)
                        furniture = Instantiate(furniturePrefab);
                    furniture.transform.position = readyTarget.position;
                    furniture.transform.rotation = readyTarget.rotation;
                    trigger.gameObject.SetActive(true);

                    break;
                case complete:
                    if (!furniture)
                        furniture = Instantiate(furniturePrefab);
                    furniture.transform.position = readyTarget.position;
                    furniture.transform.rotation = readyTarget.rotation;
                    if(ventriloquist)
                        Destroy(ventriloquist);
                    trigger.gameObject.SetActive(false);
                    break;
            }
        }
    }

}
