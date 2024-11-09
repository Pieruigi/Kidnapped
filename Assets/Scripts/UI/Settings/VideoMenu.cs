using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        private void Awake()
        {
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
            var resolutions = SettingsManager.Instance.GetAvailableResolutionsWithTheCurrentRefreshRate();
            List<string> options = new List<string>();
            foreach (var resolution in resolutions)
            {
                options.Add(ResolutionToString(resolution));
            }
            resolutionSelector.InitializeOptionList(options);
            // Get the current resolution
            var currentResolution = SettingsManager.Instance.GetCurrentResolution();
            resolutionOptionId = options.FindIndex(r => r.Equals(ResolutionToString(currentResolution)));
            resolutionOptionIdNew = resolutionOptionId;
            // Set the selector value
            resolutionSelector.SetCurrentOptionId(resolutionOptionIdNew);


            // Update apply button
            UpdateApplyButton();
        }

        #region common
        void RegisterCallbacks()
        {
            resolutionSelector.RegisterCallback(HandleOnResolutionChanged);
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
            if(resolutionOptionId != resolutionOptionIdNew) 
            {
                // Apply resolution
                var res = StringToResolution(resolutionSelector.GetCurrentOptionValue());
                SettingsManager.Instance.ApplyResolution(res.width, res.height, FullScreenMode.MaximizedWindow);
            }

            // Update the apply button
            UpdateApplyButton();

            // Save all
            PlayerPrefs.Save();
        }

        bool NothingChanged()
        {
            return (resolutionOptionId == resolutionOptionIdNew);
        }

        void RevertChanges()
        {
            if(resolutionOptionId != resolutionOptionIdNew)
            {
                resolutionOptionIdNew = resolutionOptionId;
            }
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

        #region resolution
        private void HandleOnResolutionChanged(int optionId)
        {
            Debug.Log($"Resolution changed, optionId:{optionId}");
            resolutionOptionIdNew = optionId;


            // Update apply button
            UpdateApplyButton() ;
        }
        #endregion
    }

}
