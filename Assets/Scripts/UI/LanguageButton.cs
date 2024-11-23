using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Kidnapped.UI
{
    public class LanguageButton : MonoBehaviour
    {
        enum Language { English, Italian }

        [SerializeField]
        Language _language = Language.English;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => { SettingsManager.Instance.SetLanguage((int)_language); });
        }

        private void OnEnable()
        {
            SettingsManager.OnLanguageSelected += HandleOnLanguageSelected;
        }

        private void OnDisable()
        {
            SettingsManager.OnLanguageSelected -= HandleOnLanguageSelected;
        }

        private void HandleOnLanguageSelected(int language)
        {
            GetComponent<Button>().interactable = false;
        }
    }

}
