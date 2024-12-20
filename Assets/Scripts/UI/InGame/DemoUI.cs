using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.UI
{
    public class DemoUI : Singleton<DemoUI>
    {
        [SerializeField]
        GameObject panel;
        
        string steamUrl = "https://store.steampowered.com/app/3383710/Beneath_The_Bell/";

        // Start is called before the first frame update
        void Start()
        {
            panel.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.B))
                Show();
#endif
        }

        public void Show()
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            PlayerController.Instance.PlayerInputEnabled = false;
            panel.SetActive(true);
        }

        public void OpenSteamPage()
        {
            Application.OpenURL(steamUrl);
        }
    }

}
