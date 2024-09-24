using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kidnapped
{
    public class GymIsLockedController : MonoBehaviour, ISavable
    {
        const int deactivatedState = 0;

        const int workingState = 1;

        const int finalState = 100;

        int state = 0;

        private void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = deactivatedState.ToString();
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


        public void SetWorkingState()
        {
            Init(workingState.ToString());
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
            state = int.Parse(data);

            switch (state)
            {
                case deactivatedState:

                    break;
                case workingState:

                    break;

                case finalState:

                    break;

            }
            
        }
        #endregion
    }

}
