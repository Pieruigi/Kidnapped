using Aura2API;
using DG.Tweening;
using Kidnapped.SaveSystem;
using Kidnapped.UI;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Android.Types;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Kidnapped
{
    public class EnterTheSchoolController : MonoBehaviour, ISavable
    {
        [SerializeField]
        MMF_Player lockerPlayer;

        [SerializeField]
        PlayerWalkInTrigger lockerWalkInTrigger;

        [SerializeField]
        ObjectInteractor chipsInteractor;

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

        [SerializeField]
        GameObject jar;

        [SerializeField]
        GameObject brokenJar;

        [SerializeField]
        GameObject kitchenLight;

        [SerializeField]
        GameObject corridorBlock;

        [SerializeField]
        Transform scaryEvilTarget;

        [SerializeField]
        PlayerWalkInTrigger scaryEvilTrigger;

        [SerializeField]
        SimpleActivator scaryEvil;

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
            chipsInteractor.OnInteraction += HandleOnChipsInteraction;
            tableTrigger.OnEnter += HandleOnTableTriggerEnter;
            scaryEvilTrigger.OnEnter += HandleOnScaryTriggerEnter;
        }

        private void OnDisable()
        {
            lockerWalkInTrigger.OnEnter -= HandleOnLockerTriggerEnter;
            lockerWalkInTrigger.OnExit += HandleOnLockerTriggerExit;
            chipsInteractor.OnInteraction -= HandleOnChipsInteraction;
            tableTrigger.OnEnter -= HandleOnTableTriggerEnter;
            scaryEvilTrigger.OnEnter -= HandleOnScaryTriggerEnter;
        }

        private async void HandleOnScaryTriggerEnter()
        {
            state = 20;
            
            scaryEvilTrigger.gameObject.SetActive(false);
            scaryEvil.transform.position = scaryEvilTarget.transform.position;
            scaryEvil.transform.rotation = scaryEvilTarget.transform.rotation;
            scaryEvil.Init(true.ToString());
            scaryEvil.GetComponentInChildren<Animator>().SetTrigger("Walk");

            await Task.Delay(3000);
            scaryEvil.Init(false.ToString());

            // Save state 20
            SaveManager.Instance.SaveGame();
        }

        private async void HandleOnTableTriggerEnter()
        {
            
            float time = 0.15f;
            tableObject.transform.DOMove(tableTarget.position, time);
            tableObject.transform.DORotate(tableTarget.eulerAngles, time);
            tableTrigger.gameObject.SetActive(false);

            await Task.Delay(1000);
            SubtitleUI.Instance.Show(LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationTables.Subtitles, "check_the_rules"));
            await Task.Delay(3000);
            SubtitleUI.Instance.Show(LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationTables.Subtitles, "mmmph"));
            await Task.Delay(1000);
            kitchenLight.SetActive(false);
            SubtitleUI.Instance.Hide();
            corridorBlock.SetActive(false);
        }

        private void HandleOnLockerTriggerExit()
        {
            playLocker=true;
        }

        private void HandleOnLockerTriggerEnter()
        {
            playLocker=false;
            
        }

        void HandleOnChipsInteraction()
        {
           
            state = 10; 
            lockerWalkInTrigger.gameObject.SetActive(false);
            chipsInteractor.gameObject.SetActive(false);
            kitchenBlock.Init(false.ToString());
            kitchenFree.Init(true.ToString());
            mainBlock.Init(true.ToString());
            jar.SetActive(false);
            brokenJar.SetActive(true);
            kitchenLight.SetActive(true);
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
            Debug.Log("Init entrance:"+state);
            playLocker = true;
            nextLockerTime = UnityEngine.Random.Range(nextLockerMinTime, nextLockerMaxTime);
            brokenJar.SetActive(false);
            kitchenLight.SetActive(false);

            state = int.Parse(data);

            if(state == 20)
            {
                lockerWalkInTrigger.gameObject.SetActive(false);
                tableTrigger.gameObject.SetActive(false);
                scaryEvilTrigger.gameObject.SetActive(false);
                jar.gameObject.SetActive(false);
                brokenJar.gameObject.SetActive(true);
                corridorBlock.gameObject.SetActive(false);
                tableObject.transform.position = tableTarget.transform.position;
                tableObject.transform.rotation = tableTarget.transform.rotation;
            }
        }
        #endregion
    }

}
