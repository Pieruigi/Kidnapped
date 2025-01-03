using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Kidnapped
{
    public class GameSceneAudioManager : Singleton<GameSceneAudioManager>, ISavable
    {
        public static UnityAction<int> OnAmbienceCompleted;

        [SerializeField]
        List<AudioSource> stingers;

        [SerializeField]
        List<AudioSource> ambients;

        
        [SerializeField]
        List<AudioSource> flashlightFlickers;

        [SerializeField]
        List<AudioSource> killers;

    
        int currentAmbience = -1;
        //int currentMusic = -1;

        int oldAmbience = -1;
        bool switchingAmbience = false;

        float oldVolume = 0;
        float newVolume = 0;

        float switchTime = 3f;
        float switchElapsed = 0;

        float[] ambienceVolumes;



        protected override void Awake()
        {
            base.Awake();
            // Store ambience volumes
            ambienceVolumes = new float[ambients.Count];
            for(int i = 0; i < ambients.Count; i++)
            {
                ambienceVolumes[i] = ambients[i].volume;
            }

            ReadCacheAndInit();
        
        }

#if TRAILER
        private void Start()
        {
            var mixer = Resources.Load<AudioMixer>("AudioMixer");
            Debug.Log("Mixer:"+mixer.name);
            mixer.SetFloat("MusicVolume", -80);
            mixer.SetFloat("VoiceVolume", -80);
            mixer.SetFloat("AmbienceVolume", -80);
        }
#endif

        void Update()
        {
            if (switchingAmbience)
            {
                switchElapsed += Time.deltaTime;

                if(oldAmbience >= 0)
                {
                    ambients[oldAmbience].volume = Mathf.Lerp(oldVolume, 0, switchElapsed / switchTime);
                }
                
                ambients[currentAmbience].volume = Mathf.Lerp(0, newVolume, switchElapsed / switchTime);

                if(switchElapsed > switchTime)
                {
                    if(oldAmbience >= 0)
                    {
                        // Stop the old ambience
                        ambients[oldAmbience].Stop();
                        // Reset old ambience volume
                        ambients[oldAmbience].volume = oldVolume;
                        // Reset
                        oldAmbience = -1;
                    }
                    
                    // Stop switching
                    switchingAmbience = false;
                }
            }
        }

        
        void ReadCacheAndInit()
        {
            string data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = "-1";

            Init(data);
        }


        public void PlayStinger(int index, float delay = 0)
        {
            if(delay > 0)
                stingers[index].PlayDelayed(delay);
            else
                stingers[index].Play();
        }

        public void PlayAmbience(int index)
        {
         
            StopAmbience();

            currentAmbience = index;
            ambients[currentAmbience].Play();
        }

        public void StopAmbience()
        {
            if (currentAmbience < 0)
                return;

            ambients[currentAmbience].Stop();
            currentAmbience = -1;

        }

        public void FadeInAmbient(int newAmbience)
        {
            if (newAmbience == currentAmbience || newAmbience == -1)// || currentAmbience == -1)
                return;

            // The old ambience we must fade out
            if(currentAmbience >= 0)
            {
                oldAmbience = currentAmbience;
                oldVolume = ambienceVolumes[oldAmbience];
            }
            else
            {
                oldAmbience = -1;
            }

            // The new ambience we must fade in
            currentAmbience = newAmbience;
            newVolume = ambienceVolumes[currentAmbience];
            ambients[currentAmbience].volume = 0;
            if (!ambients[currentAmbience].isPlaying)
                ambients[currentAmbience].Play();

            // Enable switching
            switchingAmbience = true;
            switchElapsed = 0;
        }

        public void PlayFlashlightFlicker(int index)
        {
            flashlightFlickers[index].Play();    
        }

        public void PlayKiller(int index, float time = 0, float delay = 0)
        {
            killers[index].time = time;
            if (delay > 0)
                killers[index].PlayDelayed(delay);
            else 
                killers[index].Play();
        }

        #region save system
        [Header("SaveSystem")]
        [SerializeField]
        string code;
        public string GetCode()
        {
            return code;
        }

        public string GetData()
        {
            return $"{currentAmbience}";
        }

        public void Init(string data)
        {
            string[] s = data.Split(new char[] { ' ' });
            currentAmbience = int.Parse(s[0]);
            Debug.Log($"CurrentAmbience:{currentAmbience}");
            if (currentAmbience >= 0)
            {
                var newId = currentAmbience;
                currentAmbience = -1;
                FadeInAmbient(newId);
            }
                
                //PlayAmbience(currentAmbience);
        }
        #endregion

    }

}
