using Kidnapped.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Kidnapped
{
    public class Launcher : MonoBehaviour
    {
        [SerializeField]
        PageManager pageManager;

        [SerializeField]
        GameObject mainMenu;

        [SerializeField]
        GameObject splashPage;

        [SerializeField]
        GameObject autoSaveHintPage;

        [SerializeField]
        GameObject languageSelectionPage;

        static bool skip = false;

        bool languageSelected = false;
       
        // Start is called before the first frame update
        void Start()
        {
            HidePageAll();
            if (!skip)
            {
                // Set skip true
                skip = true;
                // Show splash scene 
                ShowPages();
            }
            else
            {
                // Show the main menu
                pageManager.Open(mainMenu.gameObject);
                //mainMenu.Open();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            SettingsManager.OnLanguageSelected += HandleOnLaguageSelected;
        }

        private void OnDisable()
        {
            SettingsManager.OnLanguageSelected -= HandleOnLaguageSelected;
        }

        private async void HandleOnLaguageSelected(int language)
        {
            // We activate the autosave hint page to let the system load text in the selected language
            ShowAutoSaveHintPage();
            // Add some delay
            await Task.Delay(500);
            // Hide language selection page
            HideLanguageSelectionPage();
            
            await Task.Delay(4000);
            HideAutoSaveHintPage();
            // Show main menu
            pageManager.Open(mainMenu.gameObject);
        }

        void HidePageAll()
        {
            HideSplashPage();
            HideAutoSaveHintPage();
            HideLanguageSelectionPage();
        }

        void HideAutoSaveHintPage()
        {
            autoSaveHintPage.SetActive(false);
        }

        void HideSplashPage()
        {
            splashPage.SetActive(false);
        }

        void ShowAutoSaveHintPage()
        {
            autoSaveHintPage.SetActive(true);
        }

        void ShowSplashPage()
        {
            splashPage.SetActive(true);
        }

        void ShowLanguageSelectionPage()
        {
            languageSelectionPage.SetActive(true);
        }

        void HideLanguageSelectionPage()
        {
            languageSelectionPage.SetActive(false);
        }

        async void ShowPages()
        {
            ShowSplashPage();
            await Task.Delay(3000);
            // Hide splash page
            HideSplashPage();

            // Show language selection
            ShowLanguageSelectionPage();
           
        }
    }

}
