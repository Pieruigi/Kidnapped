using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CarPreviewPackage
{
    public class DemoUI : MonoBehaviour
    {
        public GameObject helpPanel;

        private static DemoUI instance;
        public static DemoUI Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<DemoUI>();
                }

                return instance;
            }
        }

        private void Awake()
        {
            instance = this;
        }

    }
}
