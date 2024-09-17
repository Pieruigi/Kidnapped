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

        private void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            Init(data);
        }

        // Start is called before the first frame update
        void Start()
        {
            //gameObject.SetActive(activate);
        }

        // Update is called once per frame
        void Update()
        {

        }

      
        [SerializeField]
        string code;
        public string GetCode()
        {
            return code;
        }

        public string GetData()
        {
            return activate.ToString();

        }

        public void Init(string data)
        {
            Debug.Log($"Init - {gameObject.name}:{data}");
            if (!string.IsNullOrEmpty(data))
            {
                activate = bool.Parse(data);
            }
            
            gameObject.SetActive(activate);
        }
    }

}
