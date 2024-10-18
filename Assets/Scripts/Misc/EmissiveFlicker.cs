using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.PropertyVariants.TrackedProperties;

namespace Kidnapped
{
    public class EmissiveFlicker : MonoBehaviour
    {

        [SerializeField]
        int materialId;

        [SerializeField]
        Light _light;

        [SerializeField]
        Color baseColor;

        [SerializeField]
        float intensityMul;

        Material material;
        float currentIntensity;

        

        private void Awake()
        {
            material = GetComponent<Renderer>().materials[materialId];
        }

        // Start is called before the first frame update
        void Start()
        {
            currentIntensity = GetLightIntensity();
            UpdateEmissive();
        }

        // Update is called once per frame
        void Update()
        {
            float lightIntensity = GetLightIntensity();

            if (currentIntensity != lightIntensity)
            {
                currentIntensity = lightIntensity;
                UpdateEmissive();
            }
        }

        void UpdateEmissive()
        {
            float power = intensityMul * currentIntensity;
            material.SetColor("_EmissionColor", baseColor * power);
        }

        float GetLightIntensity()
        {
            if (_light.enabled)
                return _light.intensity;

            return 0;
        }
    }

}
