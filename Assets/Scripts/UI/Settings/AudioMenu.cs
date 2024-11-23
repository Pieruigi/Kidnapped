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

        #region subtitles on/off
        [SerializeField]
        ToggleSelector subtitlesOnOffSelector;

        bool subtitlesOnOff = false;
        bool subtitlesOnOffNew = false;

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

            //
            // Set global volume
            //
            globalVolume = SettingsManager.Instance.GlobalVolume;
            globalVolumeNew = globalVolume;
            // Set global volume slider
            globalVolumeSelector.SetSliderValue(globalVolumeNew);
            // Set text
            SetVolumeText(globalVolumeSelector, globalVolumeNew);

            //
            // Set subtitles on/off
            //
            subtitlesOnOff = SettingsManager.Instance.SubtitlesOn;
            subtitlesOnOffNew = subtitlesOnOff;
            // Set selector
            subtitlesOnOffSelector.SetIsOn(subtitlesOnOff);

            // Update the apply button
            UpdateApplyButton();
        }
 

        void RegisterCallbacks()
        {
            // Global volume
            globalVolumeSelector.RegisterOnValueChangedCallback(HandleOnGlobalVolumeValueChanged);
            // Subtitles on/off
            subtitlesOnOffSelector.RegisterCallback(HandleOnSubtitlesOnOffChanged);
        }

        private void HandleOnSubtitlesOnOffChanged(bool isOn)
        {
            // Set new value
            subtitlesOnOffNew = isOn;
            // Update apply button
            UpdateApplyButton();
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
            return (globalVolume == globalVolumeNew && subtitlesOnOff == subtitlesOnOffNew);
        }

        void RevertChanges()
        {
            ///
            /// Global volume
            /// 
            globalVolumeNew = globalVolume;
            SettingsManager.Instance.UpdateGlobalVolume(globalVolumeNew); // We want to hear volume changes ( we can always reset them back )
            
            //
            // Subtitles on/off
            //
            subtitlesOnOffNew = subtitlesOnOff;
           
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

            if(subtitlesOnOff != subtitlesOnOffNew)
            {
                subtitlesOnOff = subtitlesOnOffNew;
                SettingsManager.Instance.UpdateSubtitlesOnOff(subtitlesOnOffNew ? 1 : 0);
            }

        }

        public void Back()
        {
            if (!NothingChanged())
                PopUpManager.Instance.ShowActionPopUp("unapplied_changes", () => 
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


