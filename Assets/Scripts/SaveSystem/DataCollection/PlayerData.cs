using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.SaveSystem
{
    [System.Serializable]
    public class PlayerData : Data
    {
        [SerializeField]
        bool hasFlashlight;

        public bool HasFlashlight
        {
            get { return hasFlashlight; }
            set { hasFlashlight = value; }
        }

        [SerializeField]
        bool canRun;

        public bool CanRun { get { return canRun; } set { canRun = value; } }

        [SerializeField]
        bool canCrouch;
        public bool CanCrouch { get { return canCrouch; } set { canCrouch = value; } }
    }
}
