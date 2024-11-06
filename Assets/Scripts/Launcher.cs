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
        BaseMenu mainMenu;

        [SerializeField]
        GameObject splashPage;

        [SerializeField]
        GameObject autoSaveHintPage;

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
                mainMenu.Open();
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

        async void ShowPages()
        {
            ShowSplashPage();
            await Task.Delay(1);
            // Hide splash page
            HideSplashPage();
            // Show auto save page
            ShowAutoSaveHintPage();
            await Task.Delay(4000);
            HideAutoSaveHintPage();
            // Show main menu
            mainMenu.Open();
        }
    }

}
