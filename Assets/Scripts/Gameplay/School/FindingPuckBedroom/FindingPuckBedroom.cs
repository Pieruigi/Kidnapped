using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Kidnapped
{
    public class FindingPuckBedroom : MonoBehaviour, ISavable
    {
        int state = 0;

        const int notReadyState = 0;
        const int readyState = 100;
        const int completedState = 200;

        private void Awake()
        {
            var data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = notReadyState.ToString();

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

        public void SetReadyState()
        {
            
            Init(readyState.ToString());
        }

        #region save system
        [Header("SaveSystem")]
        [SerializeField]
        string code;
        public string GetCode()
        {
            return code;
        }

        public string GetData()
        {
            return state.ToString();
        }

        public void Init(string data)
        {
            // Set state
            state = int.Parse(data);

            // Default
            
            // Set default values
            if (state == readyState)
            {
                // Spawn the first jar    
                

            }
        }

        #endregion
    }

}
