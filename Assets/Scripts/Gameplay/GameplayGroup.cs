using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Kidnapped
{
    public class GameplayGroup : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> elements;

        int state = 0;

        const int notReadyState = 0;
        const int readyState = 100;
        const int completedState = 200;

        int current = 0;

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

        public void MoveToNextElement()
        {
            // Deactivate the current element
            elements[current].SetActive(false);

            current++;
            if(current < elements.Count)
            {
                // Activate next
                elements[current].SetActive(true);
            }
            else
            {
                // Set completed
                Init(completedState.ToString());
                // Save game
                //SaveManager.Instance.SaveGame();
            }
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

            // Deacivate all
            foreach (var element in elements)
                element.SetActive(false);

            if(state == readyState)
            {
                // Activate the first element
                current = 0;
                elements[current].SetActive(true);
            }
            
        }

        #endregion

    }

}
