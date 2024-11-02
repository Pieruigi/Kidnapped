using Kidnapped.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;

namespace Kidnapped
{
    
    public enum Speaker { Lilith, Puck }

    public class VoiceManager : Singleton<VoiceManager>
    {
        [System.Serializable]
        class ClipData
        {
            [SerializeField]
            public AudioClip clip;

            [SerializeField]
            public string subtitleTableName;

            [SerializeField]
            public string subtitleTextKey;
        }
        
        [System.Serializable]
        class ClipCollection
        {
            [SerializeField]
            public Speaker speaker;

            [SerializeField]
            public List<ClipData> clips;
        }



        [SerializeField]
        List<ClipCollection> clipCollections;

        [SerializeField]
        List<AudioSource> sources;

        Dictionary<Speaker, (bool, UnityAction<Speaker>)> callbacks = new Dictionary<Speaker, (bool, UnityAction<Speaker>)>();

        protected override void Awake()
        {
            base.Awake();
            // Init dictionary
            for (int i = 0; i < 2; i++)
            {
                callbacks.Add((Speaker)i, (false, null));
            }
        }
        
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            List<Speaker> toUpdateKeys = new List<Speaker>();
            
            
            foreach(var key in callbacks.Keys)
            {
                // Skip the current source if is not playing
                if (callbacks[key].Item1 == false)
                    continue;

                
                
                if (!sources[(int)key].isPlaying)
                {

                    toUpdateKeys.Add(key); 

                    // Reset
                    //callbacks[key] = (false, null);

                    // Hide subtitle
                    SubtitleUI.Instance.Hide();
                }

            }

            // Collections can't be modified in the foreach loop
            foreach (var key in toUpdateKeys)
            {
                //if (callbacks[key].Item2 != null)
                callbacks[key].Item2?.Invoke(key);
                // Reset
                callbacks[key] = (false, null);
                //Debug.Log($"TEST - Update key {key}");
            }

            
        }

        
        public async void Talk(Speaker speaker, int index, UnityAction<Speaker> OnCompleteCallback = null)
        {
          
            ClipData clipData = clipCollections.Find(c => c.speaker == speaker).clips[index];
            AudioClip clip = clipData.clip;
            AudioSource source = sources[(int)speaker];

            callbacks[speaker] = (true, OnCompleteCallback);

            source.clip = clip;
            source.Play();

            await Task.Delay(800);
            // Call subtitle manager
            SubtitleUI.Instance.Show(LocalizationSettings.StringDatabase.GetLocalizedString(clipData.subtitleTableName, clipData.subtitleTextKey));
        }
    }

}
