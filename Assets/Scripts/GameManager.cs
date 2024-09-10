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

    }

}
