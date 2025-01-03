using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Kidnapped.UI
{
    public class SaveUI : MonoBehaviour
    {
        [SerializeField]
        GameObject saveIcon;

        float time = 3;
        float elapsed = 0;

        private void Awake()
        {
            saveIcon.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            elapsed += Time.deltaTime;
            if(elapsed > time)
                saveIcon.SetActive(false);
        }

#if !TRAILER
        private void OnEnable()
        {

            SaveManager.OnGameSaved += HandleOnGameSaved; 
        }

        private void OnDisable()
        {
            SaveManager.OnGameSaved -= HandleOnGameSaved; 
        }
#endif

        private void HandleOnGameSaved()
        {
            saveIcon.SetActive(true);
            elapsed = 0;
        }
    }

}
