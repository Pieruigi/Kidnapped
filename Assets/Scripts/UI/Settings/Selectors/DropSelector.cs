using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped.UI
{
    public class DropSelector : MonoBehaviour
    {
        [SerializeField]
        TMP_Dropdown dropdown;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void RegisterCallback(UnityAction<int> callback)
        {
            dropdown.onValueChanged.AddListener(callback);
        }

        public void InitializeOptionList(List<string> options)
        {
            dropdown.options.Clear();
            foreach (string option in options)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(option));
            }
        }

        public void SetCurrentOptionId(int id)
        {
            dropdown.value = id;
        }

        public string GetCurrentOptionValue()
        {
            return dropdown.options[dropdown.value].text;
        }

        public bool TryGetOptionIdByValue(string value, out int id)
        {
            id = dropdown.options.ToList().FindIndex(o => o.text.Equals(value));
            if (id < 0)
                return false;

            return true;
        }
    }

}
