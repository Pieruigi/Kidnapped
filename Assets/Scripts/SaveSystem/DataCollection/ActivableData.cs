using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.SaveSystem
{
    [System.Serializable]
    public class ActivableData : Data
    {
        [SerializeField]
        bool active = false;

        public bool Active { get { return active; } set {  active = value; } }
    }

}
