using DG.Tweening;
using EvolveGames;
using Kidnapped.SaveSystem;
using Kidnapped.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Kidnapped
{
    public class MeetTheGirl : MonoBehaviour, ISavable
    {
        [SerializeField]
        PlayerWalkInTrigger girlToRoomTrigger;

        [SerializeField]
        GameObject girlPrefab;

        //[SerializeField]
        GameObject girl;

        [SerializeField]
        Transform girlToRoomTarget;

        [SerializeField]
        AudioSource girlToRoomStingerSound;

        [SerializeField]
        PlayerWalkInTrigger preblockTrigger;

        [SerializeField]
        PlayerWalkInTrigger blockTrigger;

        [SerializeField]
        GameObject roomBlock;

        [SerializeField]
        Transform blockFreeTransform;

        [SerializeField]
        Transform blockTransform;

        [SerializeField]
        PlayerWalkInAndLookTrigger bellsTrigger;

        [SerializeField]
        List<GameObject> bellInteractors;

        [SerializeField]
        BellPuzzleController puzzleController;

        [SerializeField]
        PlayerWalkInTrigger ballTrigger;

        [SerializeField]
        GymIsLockedController gymIsLockedController;

        [SerializeField]
        AudioSource blockAudioSource;

        [SerializeField]
        DialogController dialogController;

        //[SerializeField]
        //PlayerWalkInTrigger dialogTrigger;

        [SerializeField]
        GameObject jinxPrefab;

        [SerializeField]
        Transform jinxTarget;

        [SerializeField]
        Transform jinxTarget2;

        [SerializeField]
        GameObject bellsContainer;

        [SerializeField]
        GameObject toHideOnBells;

        [SerializeField]
        GameObject internalCandles;

        [SerializeField]
        GameObject corridorPumpkin;

        GameObject jinx;



        int state = 0;

        int finalState = 100;
        bool puckTalked = false;
        int failed = 0;
        
        private void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if(string.IsNullOrEmpty(data))
                data = state.ToString();
            Init(data);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            //if (Input.GetKeyDown(KeyCode.T))
            //    PlayerController.Instance.ForcePositionAndRotation(blockTrigger.transform.position, Quaternion.identity);
            if (Input.GetKeyDown(KeyCode.T))
                Destroy(internalCandles);
#endif
        }

        private void OnEnable()
        {
            girlToRoomTrigger.OnEnter += HandleOnGirlToRoom;
            preblockTrigger.OnEnter += HandleOnPreblockTrigger;
            blockTrigger.OnEnter += HandleOnBlockTrigger;
            bellsTrigger.OnEnter += HandleOnBoardTrigger;
            puzzleController.OnSolved += HandleOnPuzzleSolved;
            puzzleController.OnFailed += HandleOnPuzzleFailed;
            ballTrigger.OnEnter += HandleOnBallTriggerEnter;
            //dialogTrigger.OnEnter += HandleOnDialogTriggerEnter;
        }

        

        private void OnDisable()
        {
            girlToRoomTrigger.OnEnter -= HandleOnGirlToRoom;
            preblockTrigger.OnEnter -= HandleOnPreblockTrigger;
            blockTrigger.OnEnter -= HandleOnBlockTrigger;
            bellsTrigger.OnEnter -= HandleOnBoardTrigger;
            puzzleController.OnSolved -= HandleOnPuzzleSolved;
            puzzleController.OnFailed -= HandleOnPuzzleFailed;
            ballTrigger.OnEnter -= HandleOnBallTriggerEnter;
            //dialogTrigger.OnEnter -= HandleOnDialogTriggerEnter;
        }

        //private void HandleOnDialogTriggerEnter(PlayerWalkInTrigger arg0)
        //{
        //    // Disable trigger
        //    dialogTrigger.gameObject.SetActive(false);

        //    // Talk
        //    dialogController.Play();
        //}

        private async void HandleOnBallTriggerEnter(PlayerWalkInTrigger trigger)
        {
            // Deactivate trigger
            ballTrigger.gameObject.SetActive(false);

            // Play sound
            blockAudioSource.Play();

            FlashlightFlickerController.Instance.FlickerOnce(() => { Destroy(internalCandles); });
           
            await Task.Delay(200);

            // Remove entrance block
            float time = .5f;
            roomBlock.transform.DOMove(blockFreeTransform.position, time);
            roomBlock.transform.DORotate(blockFreeTransform.eulerAngles, time);

            // Wait 
            await Task.Delay(500);
            
            // Set the next gameplay controller
            gymIsLockedController.SetWorkingState();

            // Update current state
            state = finalState;

            // Save game
            SaveManager.Instance.SaveGame();
        }

        private async void HandleOnPuzzleFailed()
        {
            // Disable interacors soon
            SetBellInteractorsEnable(false);

            await Task.Delay(5000);

            FlashlightFlickerController.Instance.FlickerToDarkeness(ResetPuzzle);

            failed++;

            if (failed == 2)
            {
                await Task.Delay(1000);

                // Let the girl say something
                //SubtitleUI.Instance.Show(LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationTables.Subtitles, "so_stupid"), true);
                VoiceManager.Instance.Talk(Speaker.Lilith, 2);
            }
        }

        void ResetPuzzle(float duration)
        {
            Debug.Log("Reset puzzle");
            // Spawn Jinx
            jinx = Instantiate(jinxPrefab, jinxTarget2.position, jinxTarget2.rotation);

            jinx.GetComponent<SimpleCatController>().Lick();

            bellsContainer.SetActive(false);
            // Activate trigger
            bellsTrigger.gameObject.SetActive(true);
            // Disable interacors
            //SetBellInteractorsEnable(false); 
            toHideOnBells.SetActive(true);
        }

        private async void HandleOnPuzzleSolved()
        {
            // Deactivate all the bell triggers
            SetBellInteractorsEnable(false);

            // Activate the ball trigger
            ballTrigger.gameObject.SetActive(true);

            

            await Task.Delay(3000);

            FlashlightFlickerController.Instance.FlickerToDarkeness((duration) => {
                bellsContainer.SetActive(false);
                // Activate trigger
                bellsTrigger.gameObject.SetActive(true);

                // Remove the pumpkin in the corridor
                corridorPumpkin.gameObject.SetActive(false);

                // Disable interacors
                //SetBellInteractorsEnable(false); 
                toHideOnBells.SetActive(true);
            });

            // You are so smart
            VoiceManager.Instance.Talk(Speaker.Lilith, 1);

            await Task.Delay(7000);

            // I used to play better
            //VoiceManager.Instance.Talk(Speaker.Puck, 3);
        }

        private void HandleOnBoardTrigger(PlayerWalkInAndLookTrigger trigger)
        {

            FlashlightFlickerController.Instance.FlickerToDarkeness(HandleOnFlickerToBells);

        }

        private void HandleOnFlickerToBells(float arg0)
        {
            toHideOnBells.SetActive(false);

            if (!puckTalked)
            {
                // Talk
                puckTalked = true;
                dialogController.Play();
            }
            

            // Unspawn Jinx
            Destroy(jinx);

            bellsContainer.SetActive(true);
            // Deactivate the trigger
            bellsTrigger.gameObject.SetActive(false);
            // Enable interacors
            SetBellInteractorsEnable(true);

            //bellInteractors[0].SetActive(true); // The big bell
        }

        private async void HandleOnBlockTrigger(PlayerWalkInTrigger trigger)
        {
            // Deactivate the trigger
            blockTrigger.gameObject.SetActive(false);

            // Play sound
            blockAudioSource.Play();

            // Add some delay 
            await Task.Delay(200);

            // Move the furniture to block the path
            float time = .5f;
            roomBlock.transform.DOMove(blockTransform.position, time);
            roomBlock.transform.DORotate(blockTransform.eulerAngles, time);
            
            // Activate the board trigger
            bellsTrigger.gameObject.SetActive(true);
        }

        private void HandleOnPreblockTrigger(PlayerWalkInTrigger trigger)
        {
            // Deactivate the preblock trigger
            preblockTrigger.gameObject.SetActive(false);
            // Just activate the block trigger
            blockTrigger.gameObject.SetActive(true);

            FlashlightFlickerController.Instance.FlickerOnce(() => { jinx.transform.position = jinxTarget2.position; jinx.transform.rotation = jinxTarget2.rotation; });
        }

        private async void HandleOnGirlToRoom(PlayerWalkInTrigger trigger)
        {
            // Deactivate the trigger
            girlToRoomTrigger.gameObject.SetActive(false);
            // Spawn Lilith
            girl = Instantiate(girlPrefab, girlToRoomTarget.position, girlToRoomTarget.rotation);
            // Set material
            girl.GetComponent<EvilMaterialSetter>().SetEvil();
            // Set the girl transform
            //girl.transform.position = girlToRoomTarget.position;
            //girl.transform.rotation = girlToRoomTarget.rotation;
            // Play sound
            girlToRoomStingerSound.PlayDelayed(.25f);
            // Activate girl
            Debug.Log("Activate girl");
            girl.SetActive(true);
            // Play the run animation
            girl.GetComponentInChildren<Animator>().SetTrigger("Run");
            
            await Task.Delay(2000);

            FlashlightFlickerController.Instance.FlickerOnce(() =>
            {
                // Destroy Lilith
                Destroy(girl);
                //girl.GetComponentInChildren<Animator>().ResetTrigger("Run");
                //girl.SetActive(false);
            });

            // Wait a few seconds
            
            // Dectivate girl
            
            // You can run again
            //PlayerController.Instance.CanRunning = true;
        }

        void SetBellInteractorsEnable(bool value)
        {
            foreach(var interactor in bellInteractors)
            {
                //interactor.SetActive(value);
                interactor.GetComponent<ObjectInteractor>().SetInteractionEnabled(value);
            }
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

            // Default
            blockTrigger.gameObject.SetActive(false);
            bellsTrigger.gameObject.SetActive(false); // Commented only for test
            SetBellInteractorsEnable(false);
            ballTrigger.gameObject.SetActive(false);
            bellsContainer.SetActive(false);


            if (state == finalState)
            {
                corridorPumpkin.gameObject.SetActive(false);
                girlToRoomTrigger.gameObject.SetActive(false);
                preblockTrigger.gameObject.SetActive(false);
                //dialogTrigger.gameObject.SetActive(false);
                Destroy(internalCandles);
            }
            else
            {
                // Jinx
                jinx = Instantiate(jinxPrefab, jinxTarget.position, jinxTarget.rotation);
                jinx.gameObject.SetActive(true);
                jinx.GetComponent<SimpleCatController>().Lick();
            }
        }
        #endregion
    }

}
