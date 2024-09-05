using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class DreamDoor : MonoBehaviour, ISavable
    {
        [SerializeField]
        BurningController burningController;

        [SerializeField]
        DoorController doorController;

        bool activated = false;

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
            DoorController.OnDoorOpened += HandleOnDoorOpened;
        }

        private void OnDisable()
        {
            DoorController.OnDoorOpened -= HandleOnDoorOpened;
        }

        private void HandleOnDoorOpened(DoorController arg0)
        {
            activated = true;
            burningController.StartBurning();
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
            return activated.ToString();
        }

        public void Init(string data)
        {
            activated = bool.Parse(data);
            if (activated)
            {
                burningController.StartBurning();
            }
        }
        #endregion
    }

}
