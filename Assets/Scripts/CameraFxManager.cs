using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Kidnapped
{
    public class CameraFxManager : MonoBehaviour
    {
        [SerializeField]
        CameraFilterPack_TV_Horror horrorFx;

       
        private void Awake()
        {
            DisableFxAll();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if(Input.GetKeyDown(KeyCode.Alpha1)) 
            {
                EnableHorrorFx();
            }
#endif
        }

        void DisableFxAll()
        {
            horrorFx.enabled = false;
        }

        void EnableHorrorFx()
        {
            DisableFxAll();
            horrorFx.enabled = true;
        }
    }

}
