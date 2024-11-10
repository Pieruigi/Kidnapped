using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kidnapped.UI
{
    public class ControlsMenu : MonoBehaviour
    {
        [SerializeField]
        Button applyButton;

        /// <summary>
        /// Mouse sensitivity
        /// </summary>
        [SerializeField]
        SliderSelector mouseSensitivitySelector;
        float mouseSensitivity = 0;
        float mouseSensitivityNew = 0;

        /// <summary>
        /// Y axis
        /// </summary>
        [SerializeField]
        ToggleSelector yAxisInvertedSelector;
        bool yAxisInverted = false;
        bool yAxisInvertedNew = false;

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
            if (!SettingsManager.Instance) return;

            ///
            /// Mouse sensitivity
            /// 
            // Get value
            mouseSensitivity = SettingsManager.Instance.MouseSensitivity;
            mouseSensitivityNew = mouseSensitivity;
            mouseSensitivitySelector.SetSliderValue(mouseSensitivity*2);
            mouseSensitivitySelector.SetTextValue(mouseSensitivity.ToString());

            ///
            /// Inverted Y
            /// 
            // Get value
            yAxisInverted = SettingsManager.Instance.MouseInvertedY;
            yAxisInvertedNew = yAxisInverted;
            yAxisInvertedSelector.SetIsOn(yAxisInverted);

            // Update apply button
            UpdateApplyButton();
        }

        void RegisterCallbacks()
        {
            mouseSensitivitySelector.RegisterOnValueChangedCallback(HandleOnMouseSensitivityChanged);
            yAxisInvertedSelector.RegisterCallback(HandleOnYAxisInvertedChanged);
        }

        bool NothingChanged()
        {
            return (mouseSensitivity == mouseSensitivityNew && yAxisInverted == yAxisInvertedNew);
        }

        void RevertChanges()
        {
            if(mouseSensitivity != mouseSensitivityNew)
            {
                mouseSensitivityNew = mouseSensitivity;
                // Update selector text
                mouseSensitivitySelector.SetTextValue(mouseSensitivityNew.ToString());
            }
                
            if(yAxisInverted != yAxisInvertedNew)
            {
                yAxisInvertedNew = yAxisInverted;
            }

            UpdateApplyButton();
        }

        void ApplyChanges()
        {
            if(mouseSensitivity != mouseSensitivityNew)
            {
                mouseSensitivity = mouseSensitivityNew;
                SettingsManager.Instance.UpdateMouseSensitivity(mouseSensitivity);
            }

            if(yAxisInverted != yAxisInvertedNew)
            {
                yAxisInverted = yAxisInvertedNew;
                SettingsManager.Instance.UpdateInvertedAxisY(yAxisInverted);
            }
        }

        void UpdateApplyButton()
        {
            if (NothingChanged())
                applyButton.interactable = false;
            else
                applyButton.interactable = true;

        }

        public void Apply()
        {
            ApplyChanges();
            UpdateApplyButton();
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

        #region callbacks
        private void HandleOnMouseSensitivityChanged(float value)
        {
            mouseSensitivityNew = (int)value * .5f;
            // Update selector text
            mouseSensitivitySelector.SetTextValue(mouseSensitivityNew.ToString());

            UpdateApplyButton();
        }
        private void HandleOnYAxisInvertedChanged(bool inverted)
        {
            yAxisInvertedNew = inverted;

            // Update apply button
            UpdateApplyButton();
        }

        #endregion
    }

}
