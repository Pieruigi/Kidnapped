using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class AudioManager : Singleton<AudioManager>, ISavable
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

        protected override void Awake()
        {
            base.Awake();
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
            //if(currentAmbience >= 0)
            //    ambients[currentAmbience].Stop();
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
            if (currentAmbience >= 0)
                PlayAmbience(currentAmbience);
        }
        #endregion

    }

}
