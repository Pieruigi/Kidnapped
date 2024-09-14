using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class CatController : Singleton<CatController>, ISavable
    {
        [SerializeField]
        GameObject cat;

        [SerializeField]
        CatStandAndPlayRandom standAndPlayRandom;

        int state = 0;

        protected override void Awake()
        {
            base.Awake();
            string data = SaveManager.GetCachedValue(code);
            Init(data);
        }

       
        void ResetAll()
        {
            cat.SetActive(false);
            standAndPlayRandom.enabled = false;
        }

        public void StandAndPlayRandom(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;

            if(!cat.activeSelf)
                cat.SetActive(true);

            if(!standAndPlayRandom.enabled)
                standAndPlayRandom.enabled = true;
        }

        #region save system
        [Header("Save System")]
        [SerializeField]
        string code;
        public string GetCode()
        {
            return code;
        }

        public string GetData()
        {
            return "";
        }

        public void Init(string data)
        {
            ResetAll();

            if (!string.IsNullOrEmpty(data))
            {
                string[] values = data.Split(ISavable.Separator);
                state = int.Parse(values[0]);
                if (state > 0)
                {
                    cat.SetActive(true);
                    transform.position = SaveManager.ParseStringToVector3(values[1]);
                    transform.rotation = SaveManager.ParseStringToQuaternion(values[2]);

                    if(state == 1)
                    {
                        standAndPlayRandom.enabled = true;
                    }
                }

            }
        }
    #endregion


    }

}
