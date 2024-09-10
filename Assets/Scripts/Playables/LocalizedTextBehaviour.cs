using Kidnapped.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Playables;

namespace Kidnapped.Playables
{
    public class LocalizedTextBehaviour : PlayableBehaviour
    {
        public SubtitleUI subtitleUI;

        public string tableName;

        public string textKey;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            if(!string.IsNullOrEmpty(textKey))
                subtitleUI.Show(LocalizationSettings.StringDatabase.GetLocalizedString(tableName, textKey));
            
        }

        
    }

}
