using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class FilterManager : MonoBehaviour
    {
        [SerializeField]
        CameraFilterPack_Pixel_Pixelisation pixelFilter;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                pixelFilter.enabled = !pixelFilter.enabled;
            }
        }
    }

}
