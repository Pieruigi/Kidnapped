using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class LightFlickerOff : MonoBehaviour
    {
        
        //public static UnityAction<LightFlickerOff> OnLightOff;
        //public static UnityAction<LightFlickerOff> OnLightOn;

        [SerializeField]
        Light _light;

        [SerializeField]
        Light handLight;

        [SerializeField]
        Light moonLight;

        [SerializeField]
        Material skybox;

        bool flickering = false;
        public bool Flickering {  get { return flickering; } }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if(Input.GetKeyDown(KeyCode.G)) 
            {
                Play();
            }
            
#endif
        }

       

        public void Play(UnityAction onLightOffCallback = null, UnityAction onLightOnCallback = null)
        {
            flickering = true;

            float startValue = _light.intensity;
            float minTime = 0.05f;
            float maxTime = 0.065f;
            float minInt = startValue * 0.3f;
            float maxInt = startValue * 0.4f;
            float minDef = startValue * 1.5f;// 0.9f;
            float maxDef = startValue * 2.1f;// 1.1f;

            float hStartValue = handLight.intensity;
            float hMinInt = hStartValue * 0.3f;
            float hMaxInt = hStartValue * 0.4f;
            float hMinDef = hStartValue * 1.5f;// 0.9f;
            float hMaxDef = hStartValue * 2.1f;// 1.1f;

            float moonIntensity = moonLight.intensity;
            Color skyboxColor = skybox.GetColor("_Tint");
            float skyboxExposure = skybox.GetFloat("_Exposure");
            float ambientIntensity = RenderSettings.ambientIntensity;
            float reflectionIntensity = RenderSettings.reflectionIntensity;


            Sequence seq = DOTween.Sequence();
            seq.OnStart(() => { flickering = true; });
            // Step 1
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minInt, maxInt), Random.Range(minTime, maxTime)));
            seq.Join(DOTween.To(() => handLight.intensity, x => handLight.intensity = x, Random.Range(hMinInt, hMaxInt), Random.Range(minTime, maxTime)));
            seq.Join(DOTween.To(() => moonLight.intensity, x => moonLight.intensity = x, moonIntensity*0.8f, minTime));
            seq.Join(skybox.DOColor(skyboxColor*0.8f, "_Tint", minTime));
            seq.Join(DOTween.To(() => skybox.GetFloat("_Exposure"), x => skybox.SetFloat("_Exposure", x), skyboxExposure*0.8f, minTime));

            // Step 2
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minDef, maxDef), Random.Range(minTime, maxTime)));
            seq.Join(DOTween.To(() => handLight.intensity, x => handLight.intensity = x, Random.Range(hMinDef, hMaxDef), Random.Range(minTime, maxTime)));
            seq.Join(DOTween.To(() => moonLight.intensity, x => moonLight.intensity = x, moonIntensity * 1.4f, minTime));
            seq.Join(skybox.DOColor(skyboxColor * 1.4f, "_Tint", minTime));
            seq.Join(DOTween.To(() => skybox.GetFloat("_Exposure"), x => skybox.SetFloat("_Exposure", x), skyboxExposure * 1.4f, minTime));

            // Step 3
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minInt, maxInt), Random.Range(minTime, maxTime)));
            seq.Join(DOTween.To(() => handLight.intensity, x => handLight.intensity = x, Random.Range(hMinInt, hMaxInt), Random.Range(minTime, maxTime)));
            seq.Join(DOTween.To(() => moonLight.intensity, x => moonLight.intensity = x, moonIntensity * 0.8f, minTime));
            seq.Join(skybox.DOColor(skyboxColor * 0.8f, "_Tint", minTime));
            seq.Join(DOTween.To(() => skybox.GetFloat("_Exposure"), x => skybox.SetFloat("_Exposure", x), skyboxExposure * 0.8f, minTime));

            // Step 4
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minDef, maxDef), Random.Range(minTime, maxTime)));
            seq.Join(DOTween.To(() => handLight.intensity, x => handLight.intensity = x, Random.Range(hMinDef, hMaxDef), Random.Range(minTime, maxTime)));
            seq.Join(DOTween.To(() => moonLight.intensity, x => moonLight.intensity = x, moonIntensity * 1.4f, minTime));
            seq.Join(skybox.DOColor(skyboxColor * 1.4f, "_Tint", minTime));
            seq.Join(DOTween.To(() => skybox.GetFloat("_Exposure"), x => skybox.SetFloat("_Exposure", x), skyboxExposure * 1.4f, minTime));

            // Step 5
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, 0, Random.Range(minTime, maxTime)).OnComplete(() => { onLightOffCallback?.Invoke(); }));
            seq.Join(DOTween.To(() => handLight.intensity, x => handLight.intensity = x, 0, Random.Range(minTime, maxTime)));
            seq.Join(DOTween.To(()=>moonLight.intensity, x=>moonLight.intensity = x, 0, minTime));
            seq.Join(skybox.DOColor(Color.black, "_Tint", minTime));
            seq.Join(DOTween.To(() => skybox.GetFloat("_Exposure"), x => skybox.SetFloat("_Exposure",x), 0, minTime));
            seq.Join(DOTween.To(() => RenderSettings.ambientIntensity, x => RenderSettings.ambientIntensity = x, 0, minTime));
            seq.Join(DOTween.To(() => RenderSettings.reflectionIntensity, x => RenderSettings.reflectionIntensity = x, 0, minTime));

            // Step 6
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, startValue, 2f * Random.Range(minTime, maxTime)).SetDelay(Random.Range(minTime, maxTime) * 4).OnStart(() => { onLightOnCallback?.Invoke(); }));
            seq.Join(DOTween.To(() => handLight.intensity, x => handLight.intensity = x, hStartValue, 2f * Random.Range(minTime, maxTime))/*.SetDelay(Random.Range(minTime, maxTime) * 10)*/);
            seq.Join(DOTween.To(() => moonLight.intensity, x => moonLight.intensity = x, moonIntensity, minTime));
            seq.Join(skybox.DOColor(skyboxColor, "_Tint", minTime));
            seq.Join(DOTween.To(() => skybox.GetFloat("_Exposure"), x => skybox.SetFloat("_Exposure", x), skyboxExposure, minTime));
            seq.Join(DOTween.To(() => RenderSettings.ambientIntensity, x => RenderSettings.ambientIntensity = x, ambientIntensity, minTime));
            seq.Join(DOTween.To(() => RenderSettings.reflectionIntensity, x => RenderSettings.reflectionIntensity = x, reflectionIntensity, minTime));

            seq.OnComplete(() => { flickering = false; });

        }
    }

}

