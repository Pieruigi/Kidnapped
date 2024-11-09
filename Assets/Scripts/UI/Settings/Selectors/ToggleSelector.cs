using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kidnapped.UI
{
    public class ToggleSelector : MonoBehaviour
    {
        [SerializeField]
        Toggle toggle;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void RegisterCallback(UnityAction<bool> callback)
        {
            toggle.onValueChanged.AddListener(callback);
        }

        public void SetIsOn(bool value)
        {
            toggle.isOn = value;
        }
    }

}
