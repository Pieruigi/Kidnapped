using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarPreviewPackage
{
    [RequireComponent(typeof(CarSettings))]
    public class CarManager : MonoBehaviour
    {
        /// <summary>
        /// Mostly to check if Editor lost reference to dynamicParts and staticParts dictionaries
        /// </summary>
        public bool IsInitilized
        {
            get
            {
                return dynamicPartsDic.Count > 0 && staticPartsDic.Count > 0;
            }
        }

        private DynamicPart[] dynamicParts;
        private StaticPart[] staticParts;
        private Dictionary<CarParts, DynamicPart> dynamicPartsDic = new Dictionary<CarParts, DynamicPart>();
        private Dictionary<CarParts, StaticPart> staticPartsDic = new Dictionary<CarParts, StaticPart>();
        private CarSettings carSettings;
        
        /// <summary>
        /// Call any CarManager's methods after Awake function. Start is a pretty good bet
        /// </summary>
        public void Awake()
        {
            carSettings = this.GetComponent<CarSettings>();
            dynamicParts = carSettings.DynamicParts.Clone() as DynamicPart[];
            staticParts = carSettings.StaticParts.Clone() as StaticPart[];

            for (int i = 0; i < dynamicParts.Length; i++)
            {
                dynamicPartsDic.Add(dynamicParts[i].carPartType, dynamicParts[i]);
            }

            for (int i = 0; i < staticParts.Length; i++)
            {
                staticPartsDic.Add(staticParts[i].carPartType, staticParts[i]);
            }
        }

        /// <summary>
        /// Set rotation (Open/Close state) for dynamic car part
        /// </summary>
        /// <param name="amount">[0, 100]. 0 - fully closed. 100 - fully openned</param>
        /// <param name="carPart">dynamic car part</param>
        public void SetCarPartRotation(float amount, CarParts carPart)
        {
            if (dynamicPartsDic.ContainsKey(carPart))
                dynamicPartsDic[carPart].SetRotation(amount);
            else
                Debug.LogError($"Key {carPart} doesn't exist in dynamic parts", this.gameObject);
        }

        /// <summary>
        /// Instantly change open/close state of dynamic car part
        /// </summary>
        /// <param name="carPart">dynamic car part</param>
        public void ToggleDynamicCarPartState(CarParts carPart)
        {
            if (dynamicPartsDic.ContainsKey(carPart))
                dynamicPartsDic[carPart].SetRotation(dynamicPartsDic[carPart].currentAngleStep < 50.0f ? 100.0f : 0.0f);
            else
                Debug.LogError($"Key {carPart} doesn't exist in dynamic parts", this.gameObject);
        }

        /// <summary>
        /// Animate open/close state of dynamic carPart
        /// </summary>
        /// <param name="carPart">dynamic car part</param>
        public void AnimateDynamicCarPart(CarParts carPart)
        {
            if (dynamicPartsDic.ContainsKey(carPart))
            {
                if (dynamicPartsDic[carPart].currentAnim != null)
                {
                    StopCoroutine(dynamicPartsDic[carPart].currentAnim);
                }

                dynamicPartsDic[carPart].currentAnim = StartCoroutine("PlayCarPartAnim", carPart);
            }
            else
            {
                Debug.LogError($"Key {carPart} doesn't exist in dynamic parts", this.gameObject);
            }
        }

        /// <summary>
        /// Animation coroutine
        /// </summary>
        /// <param name="carPart">dynamic car part</param>
        /// <returns>Coroutine</returns>
        private IEnumerator PlayCarPartAnim(CarParts carPart)
        {
            float startRotation = dynamicPartsDic[carPart].currentAngleStep;
            dynamicPartsDic[carPart].isOpenned = !dynamicPartsDic[carPart].isOpenned;

            if (dynamicPartsDic[carPart].currentAngleStep > 90)
                dynamicPartsDic[carPart].isOpenned = true;
            else if (dynamicPartsDic[carPart].currentAngleStep < 10)
                dynamicPartsDic[carPart].isOpenned = false;

            float endRotation = dynamicPartsDic[carPart].isOpenned ? 0.0f : 100.0f;
            float timer = 0.0f;

            while (timer <= 1.0f)
            {
                dynamicPartsDic[carPart].SetRotation(Mathf.Lerp(startRotation, endRotation, timer));
                timer += Time.deltaTime;
                yield return null;
            }

            dynamicPartsDic[carPart].currentAnim = null;
        }

        /// <summary>
        /// Set static car part visibility
        /// </summary>
        /// <param name="state">visibility state</param>
        /// <param name="carPart">static car part</param>
        public void SetCarPartState(bool state, CarParts carPart)
        {
            if (staticPartsDic.ContainsKey(carPart))
                staticPartsDic[carPart].SetState(state);
            else
                Debug.LogError($"Key {carPart} doesn't exist in static parts", this.gameObject);
        }

        /// <summary>
        /// Toggle static car part visibility
        /// </summary>
        /// <param name="carPart">static car part</param>
        public void ToggleStaticCarPartState(CarParts carPart)
        {
            if (staticPartsDic.ContainsKey(carPart))
                staticPartsDic[carPart].ToggleState();
            else
                Debug.LogError($"Key {carPart} doesn't exist in static parts", this.gameObject);
        }
    }
}