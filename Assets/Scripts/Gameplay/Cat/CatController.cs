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

        [SerializeField]
        AudioSource screamAudioSource;

        [SerializeField]
        AudioSource meowAudioSource;

        [SerializeField]
        List<AudioClip> meowAudioClips;

        int state = 0;

        protected override void Awake()
        {
            base.Awake();
            string data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = 0.ToString();
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

        public void ScaredAndRunAway(Vector3 destination, bool jumpDisabled = false)
        {
            if(!cat.activeSelf)
                cat.SetActive(true);

            standAndPlayRandom.enabled=false;
            scaredAndRunAway.Destination = destination;
            scaredAndRunAway.JumpDisabled = jumpDisabled;
            scaredAndRunAway.enabled=true;
        }

        public void ScaredAndRunAway(Vector3 destination, Vector3 position, Quaternion rotation, bool jumpDisabled = false)
        {
            if (!cat.activeSelf)
                cat.SetActive(true);

            transform.position = position;
            transform.rotation = rotation;



            ScaredAndRunAway(destination, jumpDisabled);
        }

        public void Scream(float delay = 0)
        {
            if(delay > 0)
                screamAudioSource.PlayDelayed(delay);
            else
                screamAudioSource.Play();
        }

        public void Meow()
        {
            meowAudioSource.clip = meowAudioClips[Random.Range(0, meowAudioClips.Count)];
            meowAudioSource.Play();
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

                    if(state == 1) // Save only the state 1 ( other states must be triggered )
                    {
                        standAndPlayRandom.enabled = true;
                    }
                    //else if(state == 2)
                    //{
                    //    scaredAndRunAway.enabled = true;
                    //}

                }

            }
        }
    #endregion


    }

}
