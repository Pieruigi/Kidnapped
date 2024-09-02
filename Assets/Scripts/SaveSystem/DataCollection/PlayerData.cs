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

        //[SerializeField]
        //Vector3 position;
        //public Vector3 Position { get { return position; } set {  position = value; } }

        //[SerializeField]
        //Quaternion rotation;
        //public Quaternion Rotation { get { return rotation; } set { rotation = value; } }
    }
}
