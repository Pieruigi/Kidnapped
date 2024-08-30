using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.SaveSystem
{
    [System.Serializable]
    public class DataCollection
    {
        [SerializeField]
        public List<Data> elements = new List<Data>();

        public void ClearAll()
        {
            elements.Clear();
        }
    }

}
