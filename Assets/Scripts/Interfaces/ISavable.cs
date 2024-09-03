using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.SaveSystem
{
    public interface ISavable
    {
        public static char Separator = ' ';
        
        public string GetCode();

        public string GetData();

        public void Init(string data);
    }

}
