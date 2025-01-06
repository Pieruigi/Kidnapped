using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class SimpleActivator : MonoBehaviour, ISavable
    {
        [SerializeField]
        bool activate = false;

        protected virtual void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if(string.IsNullOrEmpty(data))
                data = activate.ToString();
            Init(data);
        }

        
        [SerializeField]
        string code;
        public string GetCode()
        {
            return code;
        }

        public string GetData()
        {
            return gameObject.activeSelf.ToString();

        }

        public void Init(string data)
        {
            Debug.Log($"Init - {gameObject.name}:{data}");
            //if (!string.IsNullOrEmpty(data))
            //{
            //    activate = bool.Parse(data);
            //}
            
            gameObject.SetActive(bool.Parse(data));
        }
    }

}
