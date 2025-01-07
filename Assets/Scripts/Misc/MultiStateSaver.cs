using Kidnapped.SaveSystem;
using MoreMountains.Feel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    
    public class MultiStateSaver : MonoBehaviour, ISavable
    {
        
        [SerializeField]
        int initialState;

        int state;

        protected virtual void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = initialState.ToString();
            Init(data);
        }

        public virtual void SetState(int state)
        {
            Init(state.ToString());
        }

        public int GetState()
        {
            return state;
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
            state = int.Parse(data);

            //gameObject.SetActive(bool.Parse(data));
        }
    }

}
