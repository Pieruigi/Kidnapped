using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //private void OnEnable()
        //{
        //    pageManager.OnClosed += HandleOnMenuClosed;
        //}

        //private void OnDisable()
        //{
        //    pageManager.OnClosed -= HandleOnMenuClosed;
        //}

        //private void HandleOnMenuClosed()
        //{
            
        //}

        public void SetMenuAvailable(bool value)
        {
            menuUnavailable = !value;
        }
    }

}
