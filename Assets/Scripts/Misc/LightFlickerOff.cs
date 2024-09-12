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

       

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

       

        public void Play(UnityAction onLightOffCallback = null, UnityAction onLightOnCallback = null)
        {
            

            float startValue = _light.intensity;
            float minTime = 0.05f;
            float maxTime = 0.065f;
            float minInt = startValue * 0.3f;
            float maxInt = startValue * 0.4f;
            float minDef = startValue * 0.9f;
            float maxDef = startValue * 1.1f;
            Sequence seq = DOTween.Sequence();
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minInt, maxInt), Random.Range(minTime, maxTime)));
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minDef, maxDef), Random.Range(minTime, maxTime)));
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minInt, maxInt), Random.Range(minTime, maxTime)));
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minDef, maxDef), Random.Range(minTime, maxTime)));
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, startValue, Random.Range(minTime, maxTime)));

            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minInt, maxInt), Random.Range(minTime, maxTime)).SetDelay(Random.Range(minTime, maxTime) * 2));
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, Random.Range(minDef, maxDef), Random.Range(minTime, maxTime)));
            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, 0, Random.Range(minTime, maxTime)).OnComplete(() => { onLightOffCallback?.Invoke(); }));

            seq.Append(DOTween.To(() => _light.intensity, x => _light.intensity = x, startValue, 2f * Random.Range(minTime, maxTime)).SetDelay(Random.Range(minTime, maxTime) * 10).OnStart(() => { onLightOnCallback?.Invoke(); }));


        }
    }

}

