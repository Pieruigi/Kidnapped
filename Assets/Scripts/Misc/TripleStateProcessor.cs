using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class TripleStateProcessor : MonoBehaviour, ISavable
    {
        public enum State { NotReady, Ready, Completed }

        [SerializeField]
        State initialState = (int)State.NotReady;

        int state;

        protected virtual void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = initialState.ToString();
            Init(data);
        }

        public void SetState(State state)
        {
            Init(((int)state).ToString());
        }

        public State GetState()
        {
            return (State)state;
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
            state = int.Parse(data);
        }
    }

}
