using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Kidnapped.UI
{
    public class DoorInteractionClueUI : MonoBehaviour
    {
        [SerializeField]
        TMP_Text textField;

        string openClue = "Press 'E' to open the door";
        //string lockedClue = "The door is locked";
        

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
            textField.text = "";
            DoorController.OnCameraOver += HandleOnCameraOver;
            DoorController.OnCameraExit += HandleOnCameraExit;
            //DoorController.OnDoorLocked += HandleOnDoorLocked;
        }

        private void OnDisable()
        {
            DoorController.OnCameraOver -= HandleOnCameraOver;
            DoorController.OnCameraExit -= HandleOnCameraExit;
            //DoorController.OnDoorLocked -= HandleOnDoorLocked;
        }

        private void HandleOnDoorLocked(DoorController arg0)
        {
            throw new NotImplementedException();
        }

        private void HandleOnCameraExit(DoorController arg0)
        {
            textField.text = "";
        }

        private void HandleOnCameraOver(DoorController arg0)
        {
            textField.text = openClue;
            
        }
    }

}
