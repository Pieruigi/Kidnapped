using Kidnapped.SaveSystem;
using Kidnapped.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kidnapped
{
    public class CrouchHint : MonoBehaviour, ISavable
    {
        [SerializeField]
        PlayerWalkInTrigger trigger;

        const int stateDisabled = 100;
        const int stateEnabled = 0;

        int state = 100;

        bool inside = false;

        private void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = stateDisabled.ToString();
            Init(data);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (state == stateDisabled || !inside)
                return;

            if (Input.GetKey(KeyBindings.CrouchKey))
            {
                SetEnabled(false);
                // Disable text
                GameplayHintUI.Instance.ForceHideHint();
            }
                
        }

        private void OnEnable()
        {
            trigger.OnEnter += HandleOnTriggerEnter;
            trigger.OnExit += HandleOnTriggerExit;
        }

        private void OnDisable()
        {
            trigger.OnEnter -= HandleOnTriggerEnter;
            trigger.OnExit -= HandleOnTriggerExit;
        }

        private void HandleOnTriggerExit(PlayerWalkInTrigger arg0)
        {
            inside = false;
        }

        private void HandleOnTriggerEnter(PlayerWalkInTrigger arg0)
        {
            inside = true;
            GameplayHintUI.Instance.ShowHint(0);
        }

        public void SetEnabled(bool value)
        {
            Init(value ? stateEnabled.ToString() : stateDisabled.ToString());
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

            if(state == stateDisabled)
            {
                trigger.gameObject.SetActive(false);
            }
            else
            {
                trigger.gameObject.SetActive(true);
            }
        }
        #endregion
    }

}
