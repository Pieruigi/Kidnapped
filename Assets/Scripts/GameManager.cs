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



        protected override void Awake()
        {
            base.Awake();
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        }

        public async void FadeOutAndReloadAfterDeath()
        {
            PlayerController.Instance.PlayerInputEnabled = false;

            CameraFader.Instance.FadeOut(.25f);

            await Task.Delay(3000);

            SaveManager.Instance.LoadGame();

        }

    }

}
