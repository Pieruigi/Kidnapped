using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VLB;

namespace Kidnapped
{
    public class LightBeam : MonoBehaviour
    {
        [SerializeField]
        Light _light;

        [SerializeField]
        float multiplier;

        float intensity = 0;
        VolumetricLightBeamHD beam;

        private void Awake()
        {
            beam = GetComponent<VolumetricLightBeamHD>();
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
            if(!_light.enabled)
            {
                intensity = 0;
            }
            else
            {
                intensity = _light.intensity * multiplier;
            }

            beam.intensity = intensity;
        }
    }

}
