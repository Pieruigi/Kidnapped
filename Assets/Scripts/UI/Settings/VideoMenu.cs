using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Kidnapped.UI
{
    public class VideoMenu : MonoBehaviour
    {
        [SerializeField]
        Button applyButton;

        #region resolution
        [SerializeField]
        DropSelector resolutionSelector;

        int resolutionOptionId;
        int resolutionOptionIdNew;
        string resolutionFormat = "{0}x{1}";
        #endregion

        #region full screen mode
        [SerializeField]
        DropSelector fullScreenModeSelector;

        int fullScreenModeOptionId;
        int fullScreenModeOptionIdNew;
        #endregion

        #region refresh rate
        [SerializeField]
        DropSelector refreshRateSelector;
        int refreshRateOptionId;
        int refreshRateOptionIdNew;
        #endregion

        #region vSync
        [SerializeField]
        ToggleSelector vSyncSelector;
        int vSync;
        int vSyncNew;
        #endregion

        #region quality
        [SerializeField]
        DropSelector qualitySelector;
        int qualityOptionId;
        int qualityOptionIdNew;
        #endregion

        private void Awake()
        {
            Debug.Log(59.9468.ToString("F1", CultureInfo.InvariantCulture));
            RegisterCallbacks();
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
            if (!SettingsManager.Instance)
                return;

            //
            // Resolution
            //
            // Update selector option list
            // Get all the resolutions
            List<Resolution> resolutions = Screen.resolutions.ToList();
            // Create the resolution option list
            List<string> options = new List<string>();
            foreach (var resolution in resolutions)
            {
                if(!options.Contains(ResolutionToString(resolution)))
                    options.Add(ResolutionToString(resolution));
            }
            // Initialize the selector
            resolutionSelector.InitializeOptionList(options);
            // Get the current resolution string
            string currentResolutionString = ResolutionToString(new Resolution() { width = Screen.width, height = Screen.height });
            // Get the current option id
            resolutionOptionId = options.FindIndex(r=>currentResolutionString.Equals(r));
            resolutionOptionIdNew = resolutionOptionId;
            // Set the current option id
            resolutionSelector.SetCurrentOptionId(resolutionOptionIdNew);

            //
            // Full screen mode
            //
            // Get the current full screen mode from the settings manager
            fullScreenModeOptionId = (int)Screen.fullScreenMode;
            if (fullScreenModeOptionId == 3)
                fullScreenModeOptionId = 2; // The option 2 (windowedFullscreen) is only available on macOS, so there is one fewer option in the list
            fullScreenModeOptionIdNew = fullScreenModeOptionId;
            // Create the option list
            options.Clear();
            options.Add(FullScreenMode.ExclusiveFullScreen.ToString());
            options.Add(FullScreenMode.FullScreenWindow.ToString());
            //options.Add(FullScreenMode.MaximizedWindow.ToString());
            options.Add(FullScreenMode.Windowed.ToString());
            // Initialize selector
            fullScreenModeSelector.InitializeOptionList(options);
            // Set current value
            fullScreenModeSelector.SetCurrentOptionId(fullScreenModeOptionId);

            ///
            /// Refresh rate ratio
            /// 
            //refreshRateRatio = Screen.currentResolution.refreshRateRatio;
            //refreshRateRatioNew = refreshRateRatio;
            // Init the refresh rate data
            options.Clear();
            options.Add(30.ToString());
            options.Add(60.ToString());
            options.Add(120.ToString());
            options.Add(LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationTables.Menu, "rr_unlimited"));
            // Init the selector options
            refreshRateSelector.InitializeOptionList(options);
            // Get current refresh rate option id 
            string v = SettingsManager.Instance.RefreshRate.ToString();
            if ("-1".Equals(v))
                v = LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationTables.Menu, "rr_unlimited");
            refreshRateSelector.TryGetOptionIdByValue(v, out refreshRateOptionId);
            refreshRateOptionIdNew = refreshRateOptionId;
            // Init the selector value
            refreshRateSelector.SetCurrentOptionId(refreshRateOptionId);
            // Set interactivity
            if(fullScreenModeOptionId != (int)FullScreenMode.ExclusiveFullScreen)
            {
                refreshRateSelector.SetInteractable(false);
                // Reset value to unlimited
                refreshRateOptionId = refreshRateSelector.OptionCount - 1;
                refreshRateOptionIdNew = refreshRateOptionId;
                refreshRateSelector.SetCurrentOptionId(refreshRateOptionId);

            }
            else
            {
                refreshRateSelector.SetInteractable(true);
            }
                

            ///
            /// VSync
            /// 
            vSync = SettingsManager.Instance.VSync;
            vSyncNew = vSync;
            // Init the selector
            vSyncSelector.SetIsOn(vSync == 0 ? false : true);

            ///
            /// Quality
            /// 
            // Create the option list
            options.Clear();
            for(int i=0; i<QualitySettings.names.Length; i++)
                options.Add(LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationTables.Menu, GetQualityKeyName(i)));
            // Init the selector option list
            qualitySelector.InitializeOptionList(options);
            // Set the current quality id
            qualitySelector.SetCurrentOptionId(QualitySettings.GetQualityLevel());
            Debug.Log($"Current quality level:{QualitySettings.GetQualityLevel()}");

            // Update apply button
            UpdateApplyButton();
        }

        #region common
        void RegisterCallbacks()
        {
            resolutionSelector.RegisterCallback(HandleOnResolutionChanged);
            fullScreenModeSelector.RegisterCallback(HandleOnFullScreenModeChanged);
            refreshRateSelector.RegisterCallback(HandleOnRefreshRateChanged);
            vSyncSelector.RegisterCallback(HandleOnVSyncChanged);
        }
               
        string GetQualityKeyName(int qualityId)
        {
            string ret = "";
            switch(qualityId)
            {
                case 0:
                    ret = "very_low";
                    break;
                case 1:
                    ret = "low";
                    break;
                case 2:
                    ret = "medium";
                    break;
                case 3:
                    ret = "high";
                    break;
                case 4:
                    ret = "very_high";
                    break;
                case 5:
                    ret = "ultra";
                    break;
            }
            return ret;
        }
       
        string ResolutionToString(Resolution resolution)
        {
            return string.Format(resolutionFormat, resolution.width, resolution.height);
        }

        Resolution StringToResolution(string resolutionString)
        {
            string[] split = resolutionString.Split('x');
            return new Resolution() { width = int.Parse(split[0]), height = int.Parse(split[1]) };
        }

        void ApplyChanges()
        {
            if(resolutionOptionId != resolutionOptionIdNew || fullScreenModeOptionId != fullScreenModeOptionIdNew || refreshRateOptionId != refreshRateOptionIdNew) 
            {
                // Get the refresh rate
                string rrStr = refreshRateSelector.GetCurrentOptionValue();
                int rr = -1;
                if (!int.TryParse(rrStr, out rr))
                    rr = -1; // Unlimited

                // Create a resolution object
                var r = StringToResolution(resolutionSelector.GetCurrentOptionValue());
                
                // Set resolution
                //Screen.SetResolution(r.width, r.height, (FullScreenMode)fullScreenModeOptionIdNew);
                SettingsManager.Instance.SaveRefreshRatePlayerPrefs(rr);
                int fs = fullScreenModeOptionIdNew;
                if (fs == 2) fs = 3; // The fullscreen mode 2 (for macOS only) is not in the list and the windowed mode (windows) has id 3
                SettingsManager.Instance.UpdateResolution(r.width, r.height, (FullScreenMode)fs, rr);

                // Update current values
                resolutionOptionId = resolutionOptionIdNew;
                fullScreenModeOptionId = fullScreenModeOptionIdNew;
                refreshRateOptionId = refreshRateOptionIdNew;
            }

            if(vSync != vSyncNew)
            {
                vSync = vSyncNew;
                SettingsManager.Instance.UpdateVSync(vSync);
            }

            // Update the apply button
            UpdateApplyButton();

        }

        bool NothingChanged()
        {
            return (resolutionOptionId == resolutionOptionIdNew && fullScreenModeOptionId == fullScreenModeOptionIdNew && 
                    (refreshRateOptionId == refreshRateOptionIdNew || fullScreenModeOptionId != (int)FullScreenMode.ExclusiveFullScreen) && 
                    vSync == vSyncNew && qualityOptionId == qualityOptionIdNew);

        }

        void RevertChanges()
        {
            if(resolutionOptionId != resolutionOptionIdNew)
                resolutionOptionIdNew = resolutionOptionId;
            
            if (fullScreenModeOptionId != fullScreenModeOptionIdNew)
                fullScreenModeOptionIdNew = fullScreenModeOptionId;

            if(refreshRateOptionId != refreshRateOptionIdNew)
                refreshRateOptionIdNew = refreshRateOptionId;

            if (vSync != vSyncNew)
                vSyncNew = vSync;
        }

        void UpdateApplyButton()
        {
            if (NothingChanged())
                applyButton.interactable = false;
            else
                applyButton.interactable = true;
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

        #endregion

        #region callbacks
        private void HandleOnResolutionChanged(int optionId)
        {
            Debug.Log($"Resolution changed, optionId:{optionId}");
            resolutionOptionIdNew = optionId;

            // Update apply button
            UpdateApplyButton() ;
        }

        private void HandleOnFullScreenModeChanged(int optionId)
        {
            fullScreenModeOptionIdNew = optionId;

            if (fullScreenModeOptionIdNew != (int)FullScreenMode.ExclusiveFullScreen)
            {
                refreshRateSelector.SetInteractable(false);
                // Set unlimited refresh rate
                refreshRateOptionId = refreshRateSelector.OptionCount-1; 
                refreshRateOptionIdNew = refreshRateOptionId;
                refreshRateSelector.SetCurrentOptionId(refreshRateOptionId);
            }
            else
            {
                // Set the refresh rate selector interactable
                refreshRateSelector.SetInteractable(true);
                // Set unlimited
                refreshRateOptionId = refreshRateSelector.OptionCount-1; 
                refreshRateOptionIdNew = refreshRateOptionId;
                refreshRateSelector.SetCurrentOptionId(refreshRateOptionId);
                
            }
                

            // Update apply button
            UpdateApplyButton();
        }

        private void HandleOnRefreshRateChanged(int optionId)
        {
            refreshRateOptionIdNew = optionId;

            // Update apply button
            UpdateApplyButton();
        }

        private void HandleOnVSyncChanged(bool isOn)
        {
            vSyncNew = !isOn ? 0 : 1;

            UpdateApplyButton();
        }
        #endregion


    }

}
