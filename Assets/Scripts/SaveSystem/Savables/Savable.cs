using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.SaveSystem
{
    

    public abstract class Savable: MonoBehaviour
    {
        public abstract object GetData();
        public abstract void SetData(object data);

        [SerializeField]
        string code;
        public string Code
        {
            get { return code; }
            protected set { code = value; }
        }
        
        protected virtual void Awake()
        {

        }

        protected virtual void OnEnable()
        {
            SaveManager.Instance.RegisterSavable(this);
        }

        protected virtual void OnDisable()
        {
            SaveManager.Instance.UnregisterSavable(this);
        }


    }

}
