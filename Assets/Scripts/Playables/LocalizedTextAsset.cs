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
    public class LocalizedTextAsset : PlayableAsset
    {
        [SerializeField]
        ExposedReference<SubtitleUI> subtitleUI;
        [SerializeField]
        string tableName;
        
        [SerializeField] 
        string textKey;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<LocalizedTextBehaviour>.Create(graph);

            var behaviour = playable.GetBehaviour();

            behaviour.subtitleUI = subtitleUI.Resolve(graph.GetResolver());
            behaviour.textKey = textKey;
            behaviour.tableName = tableName; 
            //Debug.Log("AAAAAAAAAA:"+LocalizationSettings.StringDatabase.GetLocalizedString("PlayerText", "engine_stall"));
            return playable;


        }
    }

}
