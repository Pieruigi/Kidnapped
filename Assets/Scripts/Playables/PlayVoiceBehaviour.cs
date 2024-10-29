using Kidnapped.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Playables;

namespace Kidnapped.Playables
{
    public class PlayVoiceBehaviour : PlayableBehaviour
    {
        public Speaker speaker;

        public int clipIndex;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);
            VoiceManager.Instance.Talk(speaker, clipIndex);
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            //if (!string.IsNullOrEmpty(textKey))
            //    subtitleUI.Show(LocalizationSettings.StringDatabase.GetLocalizedString(tableName, textKey));
            //VoiceManager.Instance.Speak(speaker, clipIndex);

        }
    }

}
