using DG.Tweening;
using EvolveGames;
using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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
        PlayerWalkInTrigger boardTrigger;

        [SerializeField]
        GameObject boardCover;

        int state = 0;

        int finalState = 10;
        float coverRotationAngle = -152;

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
            boardTrigger.OnEnter += HandleOnBoardTrigger;
        }

        

        private void OnDisable()
        {
            girlToRoomTrigger.OnEnter -= HandleOnGirlToRoom;
            preblockTrigger.OnEnter -= HandleOnPreblockTrigger;
            blockTrigger.OnEnter -= HandleOnBlockTrigger;
            boardTrigger.OnEnter -= HandleOnBoardTrigger;
        }

        private void HandleOnBoardTrigger()
        {
            // Deactivate the trigger
            boardTrigger.gameObject.SetActive(false);
            // Rotate the cover
            boardCover.transform.DORotate(Vector3.right * coverRotationAngle, 1f);
        }

        private void HandleOnBlockTrigger()
        {
            // Deactivate the trigger
            blockTrigger.gameObject.SetActive(false);

            // Move the furniture to block the path
            float time = .5f;
            roomBlock.transform.DOMove(blockTransform.position, time);
            roomBlock.transform.DORotate(blockTransform.eulerAngles, time);
            // Activate the board trigger
            boardTrigger.gameObject.SetActive(true);
        }

        private void HandleOnPreblockTrigger()
        {
            // Deactivate the preblock trigger
            preblockTrigger.gameObject.SetActive(false);
            // Just activate the block trigger
            blockTrigger.gameObject.SetActive(true);
        }

        private async void HandleOnGirlToRoom()
        {
            // Deactivate the trigger
            girlToRoomTrigger.gameObject.SetActive(false);
            // Set the girl transform
            girl.transform.position = girlToRoomTarget.position;
            girl.transform.rotation = girlToRoomTarget.rotation;
            // Activate girl
            Debug.Log("Activte girl");
            girl.SetActive(true);
            // Play the run animation
            girl.GetComponentInChildren<Animator>().SetTrigger("Run");
            // Wait a while and eventually stop running
            //await Task.Delay(500);
            PlayerController.Instance.CanRunning = false;
            // Wait a few seconds
            await Task.Delay(3000);
            // Dectivate girl
            girl.SetActive(false);
            // You can run again
            PlayerController.Instance.CanRunning = true;
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
            //boardTrigger.gameObject.SetActive(false);

            if (state == finalState)
            {
                girlToRoomTrigger.gameObject.SetActive(false);
                preblockTrigger.gameObject.SetActive(false);
            }
        }
        #endregion
    }

}
