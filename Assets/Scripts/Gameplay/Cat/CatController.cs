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

        [SerializeField]
        CatScaredAndRunWay scaredAndRunAway;

        int state = 0;

        protected override void Awake()
        {
            base.Awake();
            string data = SaveManager.GetCachedValue(code);
            Init(data);
        }



        public void ResetAll()
        {
            cat.SetActive(false);
            standAndPlayRandom.enabled = false;
            scaredAndRunAway.enabled = false;
        }

        public void StandAndPlayRandom(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;

            if(!cat.activeSelf)
                cat.SetActive(true);

            scaredAndRunAway.enabled = false;

            standAndPlayRandom.enabled = true;
                
        }

        public void ScaredAndRunAway(Vector3 destination)
        {
            if(!cat.activeSelf)
                cat.SetActive(true);

            standAndPlayRandom.enabled=false;
            scaredAndRunAway.Destination = destination;
            scaredAndRunAway.enabled=true;
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
                    else if(state == 2)
                    {
                        scaredAndRunAway.enabled = true;
                    }
                }

            }
        }
    #endregion


    }

}
