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

        private void OnEnable()
        {
            //SaveManager.OnGameSaved += HandleOnGameSaved; // TODO: remove comment
        }

        private void OnDisable()
        {
            //SaveManager.OnGameSaved -= HandleOnGameSaved; // TODO: remove comment
        }

        private void HandleOnGameSaved()
        {
            saveIcon.SetActive(true);
            elapsed = 0;
        }
    }

}
