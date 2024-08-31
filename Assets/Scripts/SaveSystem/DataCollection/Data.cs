using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.SaveSystem
{
    [System.Serializable]
    public class Data
    {
        [SerializeField]
        string code;

        public string Code { get { return code; } set {  code = value; } }
    }
}
