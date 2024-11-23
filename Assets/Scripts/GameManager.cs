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
            SceneManager.sceneLoaded += HandleOnSceneLoaded;

#if UNITY_EDITOR
            //LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0]; // en

            //foreach(var loc in LocalizationSettings.AvailableLocales.Locales)
            //{
            //    Debug.Log($"Locale - {loc.LocaleName}");
            //}
            //Utility.GetSupportedLanguageCodes();
#endif
        }

        void HandleOnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //if (scene.buildIndex == gameSceneIndex) 
            //{ 
            //    // Activate the game scene
            //    SceneManager.SetActiveScene(scene); 
            //    // Unload the main scene
            //    SceneManager.UnloadSceneAsync(mainSceneIndex); 
            //}
        }

        async void LoadGameSceneAsync()
        {
            // Call loading event
            OnSceneLoadingStarted?.Invoke();

            await Task.Delay(500);

            // Load scene async
            float progress = 0;
            var op = SceneManager.LoadSceneAsync(gameSceneIndex/*, LoadSceneMode.Additive*/);
            // We take some time to fade music out when scene is almost loaded, so we don't activate it
            op.allowSceneActivation = false;

            // When allowSceneActivation loading stops at 90% and wait for the allowSceneActivation to be true
            while (op.progress < .9f)
            {
                if (progress != op.progress)
                {
                    progress = op.progress;
                    OnSceneLoadingProgress?.Invoke(progress); // Send progress event
                }
                await Task.Delay(100);
            }

            // At this point we consider loading completed, even if we are still at 90%.
            OnSceneLoadingCompleted?.Invoke();

            // Just give some time to do some loading stuff
            await Task.Delay(2000);

            // Ok, complete loading
            op.allowSceneActivation = true;
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
            //SceneManager.LoadScene(gameSceneIndex);

            LoadGameSceneAsync();
        }

        public void StartNewGame()
        {
            // Remove old save game if any
            if(SaveManager.Instance.SaveGameExists())
                SaveManager.Instance.DeleteSaveGame();

            // Clear cache
            SaveManager.Instance.ClearCache();

            LoadGameSceneAsync();
            

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
