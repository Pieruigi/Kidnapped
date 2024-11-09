using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


namespace Kidnapped.UI
{
    public class AudioMenu : MonoBehaviour
    {
        [SerializeField]
        Button applyButton;

        #region global volume
        [SerializeField]
        SliderSelector globalVolumeSelector;

        int globalVolume = 100;
        int globalVolumeNew = 100;
        #endregion

        string volumeTextFormat = "{0}%";

        private void Awake()
        {
            RegisterCallbacks();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        private void OnEnable()
        {
            if (!SettingsManager.Instance)
                return;

            // Set local global volume
            globalVolume = SettingsManager.Instance.GlobalVolume;
            globalVolumeNew = globalVolume;
            // Set global volume slider
            globalVolumeSelector.SetSliderValue(globalVolumeNew);
            // Set text
            SetVolumeText(globalVolumeSelector, globalVolumeNew);
            // Update the apply button
            UpdateApplyButton();
        }
 

        void RegisterCallbacks()
        {
            // Global volume
            globalVolumeSelector.RegisterOnValueChangedCallback(HandleOnGlobalVolumeValueChanged);
        }

        
        private void HandleOnGlobalVolumeValueChanged(float value)
        {
            globalVolumeNew = (int)value;
            SetVolumeText(globalVolumeSelector, value);
            // Update settings
            SettingsManager.Instance.UpdateGlobalVolume(globalVolumeNew);
            // Update apply button
            UpdateApplyButton();
        }


        void SetVolumeText(SliderSelector selector, float value)
        {
            selector.SetTextValue(string.Format(volumeTextFormat, value));
        }
        
        bool NothingChanged()
        {
            return (globalVolume == globalVolumeNew);
        }

        void RevertChanges()
        {
            ///
            /// Global volume
            /// 
            globalVolumeNew = globalVolume;
            SettingsManager.Instance.UpdateGlobalVolume(globalVolumeNew);
            //SetVolumeText(globalVolumeSelector, globalVolumeNew);
            //globalVolumeSelector.SetSliderValue(globalVolumeNew);
            
            // Update apply button
            UpdateApplyButton();
        }

        void UpdateApplyButton()
        {
            if (NothingChanged())
                applyButton.interactable = false;
            else
                applyButton.interactable = true;

        }

        void ApplyChanges()
        {
            if(globalVolume != globalVolumeNew)
            {
                globalVolume = globalVolumeNew;
                SettingsManager.Instance.UpdateGlobalVolume(globalVolumeNew);
            }

            // Save all
            PlayerPrefs.Save();
        }

        public void Back()
        {
            if (!NothingChanged())
                PopUpManager.Instance.ShowPopUp("unapplied_changes", () => 
                {
                    // We don't apply changes
                    RevertChanges(); 
                    // Back
                    GetComponentInParent<PageManager>().Back(); 
                }, 
                () => { });
            else
                GetComponentInParent<PageManager>().Back();
        }

        public void Apply()
        {
            ApplyChanges();
            UpdateApplyButton();
        }
    }
}


