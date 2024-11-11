using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kidnapped.UI
{
    public class SaveIcon : MonoBehaviour
    {
        [SerializeField]
        float angle = 30;

        [SerializeField]
        float time = .25f;

        float elapsed = 0;

       
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            elapsed += Time.deltaTime;
            if (elapsed > time)
            {
                elapsed -= time;
                transform.localEulerAngles += Vector3.forward * angle;
            }
                
        }

        private void OnEnable()
        {
            transform.localRotation = Quaternion.identity;
            elapsed = 0;
        }

       
    }

}
