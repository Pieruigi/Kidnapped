using CSA;
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
        bool active = false;

        Collider coll;

        private void Awake()
        {
            coll = GetComponent<Collider>();
            if(!active)
                coll.enabled = false;
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
            active = true;
            coll.enabled = active;
            doorController.GetComponentInParent<BurningController>().StartBurning();

            SaveManager.Instance.SaveGame();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;

            Debug.Log("Player is inside");
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
            return active.ToString();
        }

        public void Init(string data)
        {
            active = bool.Parse(data);
            if(active && !coll.enabled)
            {
                coll.enabled = true;
            }
            else if(!active && coll.enabled)
            {
                coll.enabled = false;
                doorController.GetComponentInParent<BurningController>().StartBurning();
            }
        }

        #endregion
    }

}
