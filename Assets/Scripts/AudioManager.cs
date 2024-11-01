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
            if(currentAmbience >= 0)
                ambients[index].Stop();

            currentAmbience = index;
            ambients[index].Play();
        }

        public void PlayFlashlightFlicker(int index)
        {
            flashlightFlickers[index].Play();    
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
