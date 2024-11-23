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
            GetComponent<Button>().onClick.AddListener(() => { LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)_language]; });
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
