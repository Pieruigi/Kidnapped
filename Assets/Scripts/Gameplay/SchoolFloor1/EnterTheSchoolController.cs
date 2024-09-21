using Aura2API;
using DG.Tweening;
using Kidnapped.SaveSystem;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Android.Types;
using UnityEngine;

namespace Kidnapped
{
    public class EnterTheSchoolController : MonoBehaviour, ISavable
    {
        [SerializeField]
        MMF_Player lockerPlayer;

        [SerializeField]
        PlayerWalkInTrigger lockerWalkInTrigger;

        [SerializeField]
        PlayerCloseLook lockerLook;

        [SerializeField]
        SimpleActivator kitchenFree;

        [SerializeField]
        SimpleActivator kitchenBlock;

        [SerializeField]
        SimpleActivator mainBlock;

        [SerializeField]
        PlayerWalkInTrigger tableTrigger;

        [SerializeField]
        GameObject tableObject;

        [SerializeField]
        Transform tableTarget;

        int state = 0;

        float lockerStopAngle = -110f;

        bool playLocker = false;
        float nextLockerTime = 0;
        float nextLockerMinTime = 2;
        float nextLockerMaxTime = 5;
        bool lockerIsPlaying = false;



        private void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = state.ToString();
            Init(data);

        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Y))
            {
                //lockerPlayer.ShouldRevertOnNextPlay = true;

                //lockerPlayer.ForceInitialValues();
                //StopLockerFeedback();
                playLocker = false;
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                //lockerPlayer.ShouldRevertOnNextPlay = true;

                //lockerPlayer.ForceInitialValues();
                //lockerPlayer.PlayFeedbacks();
                playLocker = true;
            }
#endif

            if(playLocker)
            {
                if (!lockerPlayer.IsPlaying)
                {
                    if(lockerIsPlaying)
                    {
                        lockerIsPlaying = false;
                        nextLockerTime = UnityEngine.Random.Range(nextLockerMinTime, nextLockerMaxTime);
                    }
                    else
                    {
                        nextLockerTime -= Time.deltaTime;
                        if (nextLockerTime < 0)
                            lockerPlayer.PlayFeedbacks();
                    }
                    

                }
                else
                {
                    if(!lockerIsPlaying)
                        lockerIsPlaying=true;
                }
            }
           
        }



        private void OnEnable()
        {
            lockerWalkInTrigger.OnEnter += HandleOnLockerTriggerEnter;
            lockerWalkInTrigger.OnExit += HandleOnLockerTriggerExit;
            lockerLook.OnPlayerLook += HandleOnLockerLook;
            tableTrigger.OnEnter += HandleOnTableTriggerEnter;
        }

        private void OnDisable()
        {
            lockerWalkInTrigger.OnEnter -= HandleOnLockerTriggerEnter;
            lockerWalkInTrigger.OnExit += HandleOnLockerTriggerExit;
            lockerLook.OnPlayerLook -= HandleOnLockerLook;
            tableTrigger.OnEnter -= HandleOnTableTriggerEnter;
        }

        private void HandleOnTableTriggerEnter()
        {
            float time = 0.25f;
            tableObject.transform.DOMove(tableTarget.position, time);
            tableObject.transform.DORotate(tableTarget.eulerAngles, time);
            tableTrigger.gameObject.SetActive(false);
        }

        private void HandleOnLockerTriggerExit()
        {
            playLocker=true;
        }

        private void HandleOnLockerTriggerEnter()
        {
            playLocker=false;
            
        }

        void HandleOnLockerLook()
        {
            state = 10; 
            lockerWalkInTrigger.gameObject.SetActive(false);
            lockerLook.gameObject.SetActive(false);
            kitchenBlock.Init(false.ToString());
            kitchenFree.Init(true.ToString());
            mainBlock.Init(true.ToString());
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

            playLocker = true;
            nextLockerTime = UnityEngine.Random.Range(nextLockerMinTime, nextLockerMaxTime);
            
            
        }
        #endregion
    }

}
