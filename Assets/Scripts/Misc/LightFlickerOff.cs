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
            float minDef = startValue * 0.9f;
            float maxDef = startValue * 1.1f;

            float hStartValue = handLight.intensity;
            float hMinInt = hStartValue * 0.3f;
            float hMaxInt = hStartValue * 0.4f;
            float hMinDef = hStartValue * 0.9f;
            float hMaxDef = hStartValue * 1.1f;

            Sequence seq = DOTween.Sequence();
            seq.OnStart(() => { flickering = true; });
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minInt, maxInt), Random.Range(minTime, maxTime)));
            seq.Join(DOTween.To(() => handLight.intensity, x => handLight.intensity = x, Random.Range(hMinInt, hMaxInt), Random.Range(minTime, maxTime)));
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minDef, maxDef), Random.Range(minTime, maxTime)));
            seq.Join(DOTween.To(() => handLight.intensity, x => handLight.intensity = x, Random.Range(hMinDef, hMaxDef), Random.Range(minTime, maxTime)));
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minInt, maxInt), Random.Range(minTime, maxTime)));
            seq.Join(DOTween.To(() => handLight.intensity, x => handLight.intensity = x, Random.Range(hMinInt, hMaxInt), Random.Range(minTime, maxTime)));
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minDef, maxDef), Random.Range(minTime, maxTime)));
            seq.Join(DOTween.To(() => handLight.intensity, x => handLight.intensity = x, Random.Range(hMinDef, hMaxDef), Random.Range(minTime, maxTime)));
            //seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, startValue, Random.Range(minTime, maxTime)));

            //seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minInt, maxInt), Random.Range(minTime, maxTime)).SetDelay(Random.Range(minTime, maxTime) * 2));
            //seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minDef, maxDef), Random.Range(minTime, maxTime)));
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, 0, Random.Range(minTime, maxTime)).OnComplete(() => { onLightOffCallback?.Invoke(); }));
            seq.Join(DOTween.To(() => handLight.intensity, x => handLight.intensity = x, 0, Random.Range(minTime, maxTime)));

            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, startValue, 2f * Random.Range(minTime, maxTime)).SetDelay(Random.Range(minTime, maxTime) * 10).OnStart(() => { onLightOnCallback?.Invoke(); }));
            seq.Join(DOTween.To(() => handLight.intensity, x => handLight.intensity = x, hStartValue, 2f * Random.Range(minTime, maxTime)).SetDelay(Random.Range(minTime, maxTime) * 10));
            seq.OnComplete(() => { flickering = false; });

        }
    }

}

