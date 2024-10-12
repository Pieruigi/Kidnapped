using System.Collections;
using System.Collections.Generic;
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
            currentIntensity = _light.intensity;
            UpdateEmissive();
        }

        // Update is called once per frame
        void Update()
        {
            if (currentIntensity != _light.intensity)
            {
                currentIntensity = _light.intensity;
                UpdateEmissive();
            }
        }

        void UpdateEmissive()
        {
            float power = intensityMul * currentIntensity;
            material.SetColor("_EmissionColor", baseColor * power);
        }
    }

}
