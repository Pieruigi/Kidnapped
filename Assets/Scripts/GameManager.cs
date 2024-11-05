using EvolveGames;
using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

namespace Kidnapped
{
    public class GameManager : Singleton<GameManager>
    {
        int mainSceneIndex = 0;
        int gameSceneIndex = 1;


        protected override void Awake()
        {
            base.Awake();
            
        }

        public async void FadeOutAndReloadAfterDeath()
        {
            PlayerController.Instance.PlayerInputEnabled = false;

            CameraFader.Instance.FadeOut(.25f);

            await Task.Delay(3000);

            SaveManager.Instance.LoadGame();
            SceneManager.LoadScene(gameSceneIndex);
        }

        public void LoadSavedGame()
        {
            SaveManager.Instance.LoadGame();
            SceneManager.LoadScene(gameSceneIndex);
        }

        public void StartNewGame()
        {
            Debug.Log("Starting a new game...");
            if(SaveManager.Instance.SaveGameExists())
                SaveManager.Instance.DeleteSaveGame();

            SceneManager.LoadScene(gameSceneIndex);
        }

        public bool IsGameScene()
        {
            return SceneManager.GetActiveScene().buildIndex == gameSceneIndex;
        }

    }

}
