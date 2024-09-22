using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class TransformSaver : MonoBehaviour, ISavable
    {
        private void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = GetData();
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
            return $"{SaveManager.ParseVector3ToString(transform.position)} {SaveManager.ParseQuaternionToString(transform.rotation)} {SaveManager.ParseVector3ToString(transform.localScale)}";
        }

        public void Init(string data)
        {
            string[] splits = data.Split(' ');
            transform.position = SaveManager.ParseStringToVector3(splits[0]);
            transform.rotation = SaveManager.ParseStringToQuaternion(splits[1]);
            transform.localScale = SaveManager.ParseStringToVector3(splits[2]);
        }
    }

}
