using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped.UI
{
    public class InGameUIManager : Singleton<InGameUIManager>
    {
        
        [SerializeField]
        PageManager pageManager;

        [SerializeField]
        GameObject mainMenu;
        
        bool menuUnavailable = false;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(!menuUnavailable && !pageManager.IsOpen) 
                {
                    
                    pageManager.Open(mainMenu);
                }
            }
        }

        private void OnEnable()
        {
            pageManager.OnClosed += HandleOnMenuClosed;
            pageManager.OnOpened += HandleOnMenuOpened;
        }

        private void OnDisable()
        {
            pageManager.OnClosed -= HandleOnMenuClosed;
            pageManager.OnOpened -= HandleOnMenuOpened;
        }

        private void HandleOnMenuClosed()
        {
            Time.timeScale = 1f;
            Utility.SetCursorVisible(false);
        }

        private void HandleOnMenuOpened()
        {
            Time.timeScale = 0f;
            Utility.SetCursorVisible(true);
        }

        public void SetMenuAvailable(bool value)
        {
            menuUnavailable = !value;
        }
    }

}
