using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kidnapped.UI
{
    public class SliderSelector : MonoBehaviour
    {
        [SerializeField]
        TMP_Text valueField;
              

        [SerializeField]
        Slider slider;

        public void SetTextValue(string text)
        {
            valueField.text = text;
        }        

        public void SetSliderValue(float value)
        {
            slider.value = value;
        }


        public void RegisterOnValueChangedCallback(UnityAction<float> callback)
        {
            slider.onValueChanged.AddListener(callback);
        }

        
    }

}
