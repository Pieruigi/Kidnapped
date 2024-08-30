using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.SaveSystem
{
    [System.Serializable]
    public class DoorData : Data
    {
        public bool IsLocked { get; set; }
        public bool IsOpen { get; set; }

        public bool InteractionDisabled { get; set; }
    }

}
