using Aura2API;
using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class MoonController : Singleton<MoonController>, ISavable
    {
        public static float InternalStrength = 18f;
        public static float ExternalStrength = 1.5f;

        AuraLight auraLight;

        float targetStrength;
        public float TargetStrength 
        {  
            get 
            { 
                return targetStrength; 
            } 
            set 
            { 
                if(value != targetStrength)
                {
                    targetStrength = value;
                    strengthSpeed = Mathf.Abs(auraLight.strength - targetStrength) / strengthTime;
                }
                
            }
        }

        float strengthSpeed = 1f;
        float strengthTime = 1f;

        protected override void Awake()
        {
            base.Awake();
            auraLight = GetComponent<AuraLight>();
            targetStrength = auraLight.strength;
            string data = SaveManager.GetCachedValue(code);
            if (!string.IsNullOrEmpty(data))
                Init(data);
        }

        void Update()
        {
            auraLight.strength = Mathf.MoveTowards(auraLight.strength, targetStrength, strengthSpeed * Time.deltaTime);

        }

        [SerializeField]
        string code;
        public string GetCode()
        {
            return code;
        }

        public string GetData()
        {
            return targetStrength.ToString();
        }

        public void Init(string data)
        {
            targetStrength = float.Parse(data);
            auraLight.strength = targetStrength;
        }


    }

}
