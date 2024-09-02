using Kidnapped.OldSaveSystem;
using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace Kidnapped
{
    public class BurningBlockRemover : MonoBehaviour, ISavable
    {
        [SerializeField]
        DoorController doorController;

        [SerializeField]
        GameObject block;

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
            DoorController.OnDoorOpenFailed += HandleOnOpenFailed;
        }

        private void OnDisable()
        {
            DoorController.OnDoorOpenFailed -= HandleOnOpenFailed;
        }

        private void HandleOnOpenFailed(DoorController arg0)
        {
            block.SetActive(false);
            doorController.GetComponentInParent<BurningController>().StartBurning();

            SaveManager.Instance.SaveGame();
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
            return block.activeSelf.ToString();
        }

        public void Init(string data)
        {
            bool active = bool.Parse(data);
            if(active && !gameObject.activeSelf)
            {
                block.SetActive(true);
            }
            else if(!active && gameObject.activeSelf)
            {
                block.SetActive(false);
                doorController.GetComponentInParent<BurningController>().StartBurning();
            }
        }

        #endregion
    }

}
