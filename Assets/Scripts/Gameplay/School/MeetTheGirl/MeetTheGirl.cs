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
        PlayerWalkInTrigger bellsTrigger;

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

        [SerializeField]
        PlayerWalkInTrigger dialogTrigger;

        [SerializeField]
        GameObject jinxPrefab;

        [SerializeField]
        Transform jinxTarget;

        GameObject jinx;

        int state = 0;

        int finalState = 100;
        
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
            dialogTrigger.OnEnter += HandleOnDialogTriggerEnter;
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
            dialogTrigger.OnEnter -= HandleOnDialogTriggerEnter;
        }

        private void HandleOnDialogTriggerEnter(PlayerWalkInTrigger arg0)
        {
            // Disable trigger
            dialogTrigger.gameObject.SetActive(false);

            // Talk
            dialogController.Play();
        }

        private async void HandleOnBallTriggerEnter(PlayerWalkInTrigger trigger)
        {
            // Deactivate trigger
            ballTrigger.gameObject.SetActive(false);

            // Play sound
            blockAudioSource.Play();

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
            await Task.Delay(3000);

            // Let the girl say something
            //SubtitleUI.Instance.Show(LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationTables.Subtitles, "so_stupid"), true);
            VoiceManager.Instance.Talk(Speaker.Lilith, 2);
        }

        private async void HandleOnPuzzleSolved()
        {
            // Deactivate all the bell triggers
            SetBellInteractorsEnable(false);

            // Activate the ball trigger
            ballTrigger.gameObject.SetActive(true);

            await Task.Delay(3000);

            // You are so smart
            VoiceManager.Instance.Talk(Speaker.Lilith, 1);

            await Task.Delay(7000);

            // I used to play better
            VoiceManager.Instance.Talk(Speaker.Puck, 3);
        }

        private async void HandleOnBoardTrigger(PlayerWalkInTrigger trigger)
        {
            // Deactivate the trigger
            bellsTrigger.gameObject.SetActive(false);
            // Enable interacors
            SetBellInteractorsEnable(true);
            // Reset false the big bell interactor because we play it by script the first time
            bellInteractors[0].SetActive(false); // The big bell
            // Play 
            bellInteractors[0].transform.parent.GetComponentInChildren<BellController>().Play();
            // Wait or the bell to complete
            await Task.Delay(13000);
            // Set interaction enable
            bellInteractors[0].SetActive(true); // The big bell

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

            FlashlightFlickerController.Instance.FlickerOnce(() => { Destroy(jinx); });
        }

        private async void HandleOnGirlToRoom(PlayerWalkInTrigger trigger)
        {
            // Deactivate the trigger
            girlToRoomTrigger.gameObject.SetActive(false);
            // Set material
            girl.GetComponent<EvilMaterialSetter>().SetNormal();
            // Set the girl transform
            girl.transform.position = girlToRoomTarget.position;
            girl.transform.rotation = girlToRoomTarget.rotation;
            // Play sound
            girlToRoomStingerSound.PlayDelayed(.25f);
            // Activate girl
            Debug.Log("Activate girl");
            girl.SetActive(true);
            // Play the run animation
            girl.GetComponentInChildren<Animator>().SetTrigger("Run");
            // Wait a while and eventually stop running
            //await Task.Delay(500);
            PlayerController.Instance.CanRunning = false;
            // Wait a few seconds
            await Task.Delay(3000);
            // Dectivate girl
            girl.GetComponentInChildren<Animator>().ResetTrigger("Run");
            girl.SetActive(false);
            // You can run again
            PlayerController.Instance.CanRunning = true;
        }

        void SetBellInteractorsEnable(bool value)
        {
            foreach(var interactor in bellInteractors)
            {
                interactor.SetActive(value);
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
            
            

            if (state == finalState)
            {
                girlToRoomTrigger.gameObject.SetActive(false);
                preblockTrigger.gameObject.SetActive(false);
                dialogTrigger.gameObject.SetActive(false);
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
