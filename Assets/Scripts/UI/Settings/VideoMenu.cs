using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.UI
{
    public class VideoMenu : MonoBehaviour
    {
        #region resolution
        int resolutionIndex = 0;
        List<Resolution> resolutions = new List<Resolution>();
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            RegisterCallbacks();
            Init();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void Init()
        {

        }

        void RegisterCallbacks()
        {

        }
    }

}
