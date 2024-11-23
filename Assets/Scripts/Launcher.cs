using Kidnapped.UI;
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
        GameObject languageSelection;

        static bool skip = false;

       
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


        void HidePageAll()
        {
            HideSplashPage();
            HideAutoSaveHintPage();
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

        void ShowLanguageSelection()
        {
            languageSelection.SetActive(true);
        }

        void HideLanguageSelection()
        {
            languageSelection.SetActive(false);
        }

        async void ShowPages()
        {
            ShowSplashPage();
            await Task.Delay(3000);
            // Hide splash page
            HideSplashPage();

            // Show language selection
            ShowLanguageSelection();

            return;

            // Show auto save page
            ShowAutoSaveHintPage();
            await Task.Delay(4000);
            HideAutoSaveHintPage();
            // Show main menu
            //mainMenu.Open();
            pageManager.Open(mainMenu.gameObject);
        }
    }

}
