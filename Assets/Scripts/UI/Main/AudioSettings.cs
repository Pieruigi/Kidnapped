using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


namespace Kidnapped.UI
{
    public class AudioSettings : MonoBehaviour
    {
        [SerializeField]
        Button applyButton;

        #region global volume
        [SerializeField]
        SliderSelector globalVolumeSelector;

        string globalVolumeKeyName = "GlobalVolume";
        string globalVolumeParamName = "GlobalVolume";

        int globalVolume = 100;
        int globalVolumeNew = 100;
        #endregion

        [SerializeField]
        AudioMixer audioMixer;

        string volumeTextFormat = "{0}%";

        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            RegisterCallbacks();
            Init();
        }
                
      
        private void Init()
        {
            //
            // Set up global volume
            //
            // Read stored value if any
            if (PlayerPrefs.HasKey(globalVolumeKeyName))
                globalVolume = PlayerPrefs.GetInt(globalVolumeKeyName);
            // Store the current global volume
            globalVolumeNew = globalVolume;
            // Set selector
            globalVolumeSelector.SetSliderValue(globalVolume);
            SetVolumeText(globalVolumeSelector, globalVolume);
            // Set the audio mixer
            Debug.Log("Setting mixer");
            SetAudioMixerVolume(globalVolumeParamName, globalVolumeNew);


            // Update apply button
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
            // Set audio mixer
            SetAudioMixerVolume(globalVolumeParamName, globalVolumeNew);
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

        void SetAudioMixerVolume(string paramName, float value)
        {
            audioMixer.SetFloat(paramName, Mathf.Log10(value/100) * 20);
        }

        void RevertChanges()
        {
            ///
            /// Global volume
            /// 
            globalVolumeNew = globalVolume;
            SetAudioMixerVolume(globalVolumeParamName, globalVolumeNew);
            SetVolumeText(globalVolumeSelector, globalVolumeNew);
            globalVolumeSelector.SetSliderValue(globalVolumeNew);
            
            
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
                PlayerPrefs.SetInt(globalVolumeParamName, globalVolumeNew);
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


