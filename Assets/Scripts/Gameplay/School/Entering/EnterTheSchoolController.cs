using Aura2API;
using DG.Tweening;
using Kidnapped.SaveSystem;
using Kidnapped.UI;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Kidnapped
{
    public class EnterTheSchoolController : MonoBehaviour, ISavable
    {
        [SerializeField]
        GameObject lilithPrefab;

        [SerializeField]
        PlayerWalkInAndLookTrigger lilithFirstLookTrigger;

        [SerializeField]
        Transform lilithFirstLookTarget;
        
        [SerializeField]
        MMF_Player lockerPlayer;

        [SerializeField]
        AudioSource lockerAudioSource;

        [SerializeField]
        PlayerWalkInTrigger lockerWalkInTrigger;

        [SerializeField]
        PlayerWalkInAndLookTrigger lockerLookTrigger;

        [SerializeField]
        GameObject lockerJar;

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
        GameObject kitchenLight;

        [SerializeField]
        GameObject corridorBlock;

        [SerializeField]
        Transform scaryEvilTarget;

        [SerializeField]
        PlayerWalkInTrigger scaryEvilTrigger;

        [SerializeField]
        SimpleActivator scaryEvil;

        [SerializeField]
        AudioSource openingKitchenAudioSource;

        [SerializeField]
        AudioSource draggingTableAudioSource;

        [SerializeField]
        CrouchHint crouchHint;

        //[SerializeField]
        //LightActivator[] toDeactivateLights;

        //[SerializeField]
        //LightActivator[] toActivateLights;

        [SerializeField]
        Light internalCandle;

        //[SerializeField]
        //LightActivator lightActivator;

        int state = 0;

        float lockerStopAngle = -110f;

        bool playLocker = false;
        float nextLockerTime = 0;
        float nextLockerMinTime = 2;
        float nextLockerMaxTime = 5;
        bool lockerIsPlaying = false;
        GameObject lilithFirstLook;


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
                        {
                            lockerPlayer.PlayFeedbacks();
                            // Play audio
                            lockerAudioSource.PlayDelayed(0.25f);
                        }
                            
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
            tableTrigger.OnEnter += HandleOnTableTriggerEnter;
            scaryEvilTrigger.OnEnter += HandleOnScaryTriggerEnter;
            lilithFirstLookTrigger.OnEnter += HandleOnLilithFirstLookTriggerEnter;
            lockerLookTrigger.OnEnter += HandleOnLockerLookTrigger;
        }

        private void OnDisable()
        {
            lockerWalkInTrigger.OnEnter -= HandleOnLockerTriggerEnter;
            lockerWalkInTrigger.OnExit += HandleOnLockerTriggerExit;
            tableTrigger.OnEnter -= HandleOnTableTriggerEnter;
            scaryEvilTrigger.OnEnter -= HandleOnScaryTriggerEnter;
            lilithFirstLookTrigger.OnEnter -= HandleOnLilithFirstLookTriggerEnter;
            lockerLookTrigger.OnEnter -= HandleOnLockerLookTrigger;
        }

       

        private async void HandleOnLilithFirstLookTriggerEnter(PlayerWalkInAndLookTrigger t)
        {
           
            // Deactivate the trigger
            t.gameObject.SetActive(false);

            await Task.Delay(1000);

            // Play stinger
            GameSceneAudioManager.Instance.PlayStinger(0);

            // Lilith starts walking
            lilithFirstLook.GetComponentInChildren<Animator>().SetTrigger("Run");

            // Remove school main block
            mainBlock.gameObject.SetActive(false);

            //// Deactivate light
            //foreach(var l in toDeactivateLights)
            //    l.SetEnabled(false);

            //// Activate
            //foreach (var l in toActivateLights)
            //    l.SetEnabled(true);

            // We do it manually because we must detroy the object after a while
            //Utility.SwitchLightOn(internalCandle, false);
            // Remove candle
            Destroy(internalCandle.transform.parent.gameObject, .5f);

            // Add some delay
            await Task.Delay(5000);

            // Remove Lilith
            Destroy(lilithFirstLook);

            
        }

        

        private async void HandleOnScaryTriggerEnter(PlayerWalkInTrigger trigger)
        {
            state = 20;

            // Remove corridor block
            corridorBlock.SetActive(false);

            scaryEvilTrigger.gameObject.SetActive(false);
            //scaryEvil.GetComponent<EvilMaterialSetter>().SetNormal();
            scaryEvil.transform.position = scaryEvilTarget.transform.position;
            scaryEvil.transform.rotation = scaryEvilTarget.transform.rotation;
            scaryEvil.Init(true.ToString());
            scaryEvil.GetComponentInChildren<Animator>().SetTrigger("Walk");

            // Play stinger 
            GameSceneAudioManager.Instance.PlayStinger(1, 0.5f);

            await Task.Delay(3000);
            scaryEvil.Init(false.ToString());

           
        }

        private async void HandleOnTableTriggerEnter(PlayerWalkInTrigger trigger)
        {
            
            float time = 0.9f;
            tableObject.transform.DOMove(tableTarget.position, time);
            tableObject.transform.DORotate(tableTarget.eulerAngles, time);
            tableTrigger.gameObject.SetActive(false);
            // Play audio
            draggingTableAudioSource.Play();
            
            await Task.Delay(1000);
            //SubtitleUI.Instance.Show(LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationTables.Subtitles, "check_the_rules"));
            VoiceManager.Instance.Talk(Speaker.Lilith, 0);
            await Task.Delay(3000);
            //SubtitleUI.Instance.Show(LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationTables.Subtitles, "mmmph"));
            VoiceManager.Instance.Talk(Speaker.Puck, 0);
            await Task.Delay(1000);
            kitchenLight.SetActive(false);
            SubtitleUI.Instance.Hide();

            await Task.Delay(500);
            // Save state 20
            SaveManager.Instance.SaveGame();
        }

        private void HandleOnLockerTriggerExit(PlayerWalkInTrigger trigger)
        {
            playLocker=true;
        }

        private void HandleOnLockerTriggerEnter(PlayerWalkInTrigger trigger)
        {
            playLocker=false;
            
        }

        private void HandleOnLockerLookTrigger(PlayerWalkInAndLookTrigger t)
        {
            // Disable trigger
            t.gameObject.SetActive(false);
            // Flicker
            Flashlight.Instance.GetComponent<FlashlightFlickerController>().FlickerOnce(OnFlickerLightOff);

            // Enable crouch hint
            crouchHint.SetEnabled(true);
        }

        void OnFlickerLightOff()
        {
            if (state == 10)
                return;

            state = 10;
            playLocker = true;
            lockerWalkInTrigger.gameObject.SetActive(false);
            kitchenBlock.Init(false.ToString());
            kitchenFree.Init(true.ToString());
            mainBlock.Init(true.ToString());
            lockerJar.SetActive(false);
            kitchenLight.SetActive(true);
            openingKitchenAudioSource.Play();
            //lightActivator.SetEnabled(false);
        }

        void CreateLilithFirstLook()
        {
            // Instantiate
            lilithFirstLook = Instantiate(lilithPrefab);

            // Set position and rotation
            lilithFirstLook.transform.position = lilithFirstLookTarget.position;
            lilithFirstLook.transform.rotation = lilithFirstLookTarget.rotation;

            // Set evil
            lilithFirstLook.GetComponent<EvilMaterialSetter>().SetEvil();

            // Activate
            lilithFirstLook.SetActive(true);
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
            // Default
            playLocker = true;
            nextLockerTime = UnityEngine.Random.Range(nextLockerMinTime, nextLockerMaxTime);
            kitchenLight.SetActive(false);
            

            
            state = int.Parse(data);


            if(state == 20)
            {
                lockerWalkInTrigger.gameObject.SetActive(false);
                tableTrigger.gameObject.SetActive(false);
                scaryEvilTrigger.gameObject.SetActive(false);
                //corridorBlock.gameObject.SetActive(false);
                tableObject.transform.position = tableTarget.transform.position;
                tableObject.transform.rotation = tableTarget.transform.rotation;
                lilithFirstLookTrigger.gameObject.SetActive(false);
                lockerJar.SetActive(false);
                lockerLookTrigger.gameObject.SetActive(false);
                Destroy(internalCandle.transform.parent.gameObject);
            }
            else if(state == 0)
            {
                // Create Lilith in the gym
                CreateLilithFirstLook();
            }
        }
        #endregion
    }

}
