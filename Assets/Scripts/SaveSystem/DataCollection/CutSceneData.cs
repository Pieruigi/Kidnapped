using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.SaveSystem
{
    [System.Serializable]
    public class CutSceneData : Data
    {
        //public bool Played { get; set; }

        // If we want to open a new scene with a specific cut scene we can set the PlayOnEnter in the inspector to true and the simple fade in to false. Once the cut scene
        // has completed we can reset the PlayOnEnter to false and the simple fade in to true, both via script.
        public bool PlayOnEnter { get; set; }
    }

}
