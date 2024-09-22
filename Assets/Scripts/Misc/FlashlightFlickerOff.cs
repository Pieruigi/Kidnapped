using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class FlashlightFlickerOff : MonoBehaviour
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

        float defaultIntensity;
        float handsLightDefaultIntensity;

        private void Awake()
        {
            defaultIntensity = GetComponent<Flashlight>().LightIntensity;
            handsLightDefaultIntensity = handLight.intensity;
        }

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



        public void Play(UnityAction onLightOffCallback = null, UnityAction onLightOnCallback = null, UnityAction onCompleteCallback = null)
        {
            flickering = true;

            float flickerDuration = 0.1f;
            float offDuration = .2f;
            Sequence flickerSequence = DOTween.Sequence();

            // Primo flicker veloce prima dello spegnimento
            flickerSequence.Append(_light.DOIntensity(0, flickerDuration / 2))  // Abbassiamo l'intensità della torcia
                           .Join(handLight.DOIntensity(0, flickerDuration / 2))
                           .Append(_light.DOIntensity(defaultIntensity, flickerDuration / 2)) // Riaccendiamo velocemente
                           .Join(handLight.DOIntensity(handsLightDefaultIntensity, flickerDuration / 2)) // Riaccendiamo velocemente
                           .Append(_light.DOIntensity(0, flickerDuration / 2)) // Di nuovo spegnimento
                           .Join(handLight.DOIntensity(0, flickerDuration / 2)) // Di nuovo spegnimento
                           .Append(_light.DOIntensity(defaultIntensity, flickerDuration / 2))
                           .Join(handLight.DOIntensity(handsLightDefaultIntensity, flickerDuration / 2)); // Riaccensione veloce


            // Spegniamo la torcia per 0.2 o 0.3 secondi
            flickerSequence.Append(_light.DOIntensity(0, 0.01f).OnComplete(() => { onLightOffCallback?.Invoke(); })) // Spegni subito la torcia
                           .Join(handLight.DOIntensity(0, 0.01f))
                           .AppendInterval(offDuration); // Rimane spenta per offDuration

            // Un altro flicker veloce per la riaccensione
            flickerSequence.Append(_light.DOIntensity(defaultIntensity, flickerDuration / 2).OnStart(() => { onLightOnCallback?.Invoke(); }))  // Riaccendi con flicker
                           .Join(handLight.DOIntensity(handsLightDefaultIntensity, flickerDuration / 2))
                           .Append(_light.DOIntensity(0, flickerDuration / 2)) // Spegnimento rapido
                           .Join(handLight.DOIntensity(0, flickerDuration / 2)) // Spegnimento rapido
                           .Append(_light.DOIntensity(defaultIntensity, flickerDuration / 2)) // Riaccensione finale
                           .Join(handLight.DOIntensity(handsLightDefaultIntensity, flickerDuration / 2)); // Riaccensione finale          

            flickerSequence.OnComplete(() => { flickering = false; onCompleteCallback?.Invoke(); });


        }

        public void Play_BKP(UnityAction onLightOffCallback = null, UnityAction onLightOnCallback = null, UnityAction onCompleteCallback = null)
        {
            flickering = true;

            float startValue = defaultIntensity;
            float minTime = 0.05f;
            float maxTime = 0.065f;
            float minInt = startValue * 0.3f;
            float maxInt = startValue * 0.4f;
            float minDef = startValue * 1.5f;// 0.9f;
            float maxDef = startValue * 2.1f;// 1.1f;

            float hStartValue = handsLightDefaultIntensity;
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
            seq.Join(DOTween.To(() => moonLight.intensity, x => moonLight.intensity = x, moonIntensity * 0.8f, minTime));
            seq.Join(skybox.DOColor(skyboxColor * 0.8f, "_Tint", minTime));
            seq.Join(DOTween.To(() => skybox.GetFloat("_Exposure"), x => skybox.SetFloat("_Exposure", x), skyboxExposure * 0.8f, minTime));

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

            // Step 5 ( darkness )
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, 0, Random.Range(minTime, maxTime)).OnComplete(() => { onLightOffCallback?.Invoke(); }));
            seq.Join(DOTween.To(() => handLight.intensity, x => handLight.intensity = x, 0, Random.Range(minTime, maxTime)));
            seq.Join(DOTween.To(() => moonLight.intensity, x => moonLight.intensity = x, 0, minTime));
            seq.Join(skybox.DOColor(Color.black, "_Tint", minTime));
            seq.Join(DOTween.To(() => skybox.GetFloat("_Exposure"), x => skybox.SetFloat("_Exposure", x), 0, minTime));
            seq.Join(DOTween.To(() => RenderSettings.ambientIntensity, x => RenderSettings.ambientIntensity = x, 0, minTime));
            seq.Join(DOTween.To(() => RenderSettings.reflectionIntensity, x => RenderSettings.reflectionIntensity = x, 0, minTime));

            // Step 6 ( start on )
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minDef, maxDef), Random.Range(minTime, maxTime)).SetDelay(Random.Range(minTime, maxTime) * 4).OnStart(() => { onLightOnCallback?.Invoke(); }));
            seq.Join(DOTween.To(() => handLight.intensity, x => handLight.intensity = x, Random.Range(hMinDef, hMaxDef), Random.Range(minTime, maxTime)));

            // Step 7
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minInt, maxInt), Random.Range(minTime, maxTime)));
            seq.Join(DOTween.To(() => handLight.intensity, x => handLight.intensity = x, Random.Range(hMinInt, hMaxInt), Random.Range(minTime, maxTime)));


            // Step 8
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, startValue, Random.Range(minTime, maxTime)));
            seq.Join(DOTween.To(() => handLight.intensity, x => handLight.intensity = x, hStartValue, Random.Range(minTime, maxTime))/*.SetDelay(Random.Range(minTime, maxTime) * 10)*/);
            seq.Join(DOTween.To(() => moonLight.intensity, x => moonLight.intensity = x, moonIntensity, minTime));
            seq.Join(skybox.DOColor(skyboxColor, "_Tint", minTime));
            seq.Join(DOTween.To(() => skybox.GetFloat("_Exposure"), x => skybox.SetFloat("_Exposure", x), skyboxExposure, minTime));
            seq.Join(DOTween.To(() => RenderSettings.ambientIntensity, x => RenderSettings.ambientIntensity = x, ambientIntensity, minTime));
            seq.Join(DOTween.To(() => RenderSettings.reflectionIntensity, x => RenderSettings.reflectionIntensity = x, reflectionIntensity, minTime));


            // Step 6
            //seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, startValue, 2f * Random.Range(minTime, maxTime)).SetDelay(Random.Range(minTime, maxTime) * 4).OnStart(() => { onLightOnCallback?.Invoke(); }));
            //seq.Join(DOTween.To(() => handLight.intensity, x => handLight.intensity = x, hStartValue, 2f * Random.Range(minTime, maxTime))/*.SetDelay(Random.Range(minTime, maxTime) * 10)*/);
            //seq.Join(DOTween.To(() => moonLight.intensity, x => moonLight.intensity = x, moonIntensity, minTime));
            //seq.Join(skybox.DOColor(skyboxColor, "_Tint", minTime));
            //seq.Join(DOTween.To(() => skybox.GetFloat("_Exposure"), x => skybox.SetFloat("_Exposure", x), skyboxExposure, minTime));
            //seq.Join(DOTween.To(() => RenderSettings.ambientIntensity, x => RenderSettings.ambientIntensity = x, ambientIntensity, minTime));
            //seq.Join(DOTween.To(() => RenderSettings.reflectionIntensity, x => RenderSettings.reflectionIntensity = x, reflectionIntensity, minTime));



            seq.OnComplete(() => { flickering = false; onCompleteCallback?.Invoke(); });

        }
    }

}

