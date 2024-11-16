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

        List<float> defaultVolumes = new List<float>();

        protected override void Awake()
        {
            base.Awake();
            // Init dictionary
            for (int i = 0; i < 2; i++)
            {
                callbacks.Add((Speaker)i, (false, null));
                // Set default volume for each source
                defaultVolumes.Add(sources[i].volume);
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

                if (callbacks[key].Item1)
                    Debug.Log($"TEST - {key} is talking");

                // Is talking
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
                // We must store the callback to call it after the reset. If we call it before we reset the tuple, then the upcoming reset
                // will erase the new callback eventually.
                var callback = callbacks[key].Item2;
// Reset
                callbacks[key] = (false, null);

                // Callback
                callback?.Invoke(key);
                Debug.Log($"TEST - Update key {key}, callbacks:{callbacks[key]}");
            }

            
        }


        public async void Talk(Speaker speaker, int index, UnityAction<Speaker> OnCompleteCallback = null, float delay = 0, float volumeMultiplier = 1f)
        {
            if(delay > 0) 
                await Task.Delay(TimeSpan.FromSeconds(delay));
          
            ClipData clipData = clipCollections.Find(c => c.speaker == speaker).clips[index];
            AudioClip clip = clipData.clip;
            AudioSource source = sources[(int)speaker];

            // Adjust volume
            source.volume = defaultVolumes[(int)speaker] * volumeMultiplier;

            callbacks[speaker] = (true, OnCompleteCallback);

            Debug.Log($"TEST - {speaker} start talkingkey, callbacks:{callbacks[speaker]}");

            source.clip = clip;
            source.Play();

            await Task.Delay(800);
            // Call subtitle manager
            SubtitleUI.Instance.Show(LocalizationSettings.StringDatabase.GetLocalizedString(clipData.subtitleTableName, clipData.subtitleTextKey));
        }
    }

}
