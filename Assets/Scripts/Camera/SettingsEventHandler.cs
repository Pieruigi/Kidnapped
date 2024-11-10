using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Kidnapped
{
    public class SettingsEventHandler : MonoBehaviour
    {
        private void Awake()
        {
            SettingsManager.OnAntialiasingChanged += HandleOnAntialiasingChanged;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            SettingsManager.OnAntialiasingChanged -= HandleOnAntialiasingChanged;
        }

        private void HandleOnAntialiasingChanged(int antialiasing)
        {
            GetComponent<PostProcessLayer>().antialiasingMode = (PostProcessLayer.Antialiasing)antialiasing;
        }
    }

}
