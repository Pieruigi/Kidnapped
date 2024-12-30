using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class LightActivator : MonoBehaviour, ISavable
    {
        [SerializeField]
        Light _light;

        [SerializeField]
        bool active = false;

        private void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = active.ToString();
            Init(data);
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetActive(bool value)
        {
            Init(value.ToString());
        }

        [SerializeField]
        string code;
        public string GetCode()
        {
            return code;
        }

        public string GetData()
        {
            return active.ToString();
        }

        public void Init(string data)
        {
            active = bool.Parse(data);
            Utility.SwitchLightOn(_light, active);
        }
    }

}
