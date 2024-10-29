using Kidnapped.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Kidnapped.Playables
{
    public class PlayVoiceAsset : PlayableAsset
    {
        [SerializeField]
        Speaker speaker;

        [SerializeField]
        int clipIndex;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<PlayVoiceBehaviour>.Create(graph);
            var behaviour = playable.GetBehaviour();

            behaviour.speaker = speaker;
            behaviour.clipIndex = clipIndex;
            
            return playable;


        }
    }

}
