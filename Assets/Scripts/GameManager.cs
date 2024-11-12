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
        public static UnityAction OnSceneLoadingStarted;
        public static UnityAction<float> OnSceneLoadingProgress;
        public static UnityAction OnSceneLoadingCompleted;

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

        public async void StartNewGame()
        {
            Debug.Log("Starting a new game...");
            OnSceneLoadingStarted?.Invoke();

            if(SaveManager.Instance.SaveGameExists())
                SaveManager.Instance.DeleteSaveGame();

            float progress = 0;
            var op = SceneManager.LoadSceneAsync(gameSceneIndex);
            op.allowSceneActivation = false;

            while (op.progress < .9f)
            {
                if (progress != op.progress)
                {
                    progress = op.progress;
                    OnSceneLoadingProgress?.Invoke(progress);
                }
                await Task.Delay(100);
            }

            OnSceneLoadingCompleted?.Invoke();

            await Task.Delay(3000);
            op.allowSceneActivation = true;
        }

        public bool IsGameScene()
        {
            return SceneManager.GetActiveScene().buildIndex == gameSceneIndex;
        }

        public void ReturnToMainScene()
        {
            SceneManager.LoadScene(mainSceneIndex);
        }

    }

}
