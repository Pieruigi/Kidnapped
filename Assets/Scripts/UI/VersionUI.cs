using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Kidnapped.UI
{
    public class VersionUI : MonoBehaviour
    {
        [SerializeField]
        TMP_Text textField;

        // Start is called before the first frame update
        void Start()
        {
            textField.text = $"v {Application.version}";
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
