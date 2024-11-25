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
        CameraFilterPack_Film_Grain filmGrainFx;

        [SerializeField]
        CameraFilterPack_TV_Artefact artefactFx;

       
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
                //EnableHorrorFx();
            }
#endif
        }

        void DisableFxAll()
        {
            filmGrainFx.enabled = false;
            artefactFx.enabled = false;
        }


        public void ShowFilmGrainFx()
        {
            DisableFxAll();
            filmGrainFx.enabled = true;
        }
    }

}
