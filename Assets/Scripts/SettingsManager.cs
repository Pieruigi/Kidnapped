using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Kidnapped
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        #region audio
        [SerializeField]
        AudioMixer audioMixer;

        int globalVolume = 100;
        public int GlobalVolume
        {
            get { return globalVolume; }
        }
        string globalVolumeKeyName = "GlobalVolume";
        string globalVolumeParamName = "GlobalVolume";
        #endregion

        #region graphics
        int resolutionIndex = -1; // The current resolution 
        List<Resolution> resolutions; // All the available resolutions

        string resolutionKeyName = "ResolutionIndex";
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            InitAudio();
        }

        // Update is called once per frame
        void Update()
        {

        }

      

        #region audio
        void InitAudio()
        {
            // Read stored value if any
            if (PlayerPrefs.HasKey(globalVolumeKeyName))
                globalVolume = PlayerPrefs.GetInt(globalVolumeKeyName);
            SetAudioMixerVolume(globalVolumeParamName, globalVolume);
        }

        void SetAudioMixerVolume(string paramName, float value)
        {
            audioMixer.SetFloat(paramName, Mathf.Log10(value / 100) * 20);
        }

        public void UpdateGlobalVolume(int newValue)
        {
            globalVolume = newValue;
            SetAudioMixerVolume(globalVolumeParamName, globalVolume);
            PlayerPrefs.SetInt(globalVolumeKeyName, globalVolume);
            //PlayerPrefs.Save();
        }
        #endregion

        #region graphics
        void InitGraphics()
        {
            // 
            // Resolution
            //
            // Get all the available resolutions
            resolutions = Screen.resolutions.ToList();
            // Load from player prefs
            if (PlayerPrefs.HasKey(resolutionKeyName))
            {
                resolutionIndex = PlayerPrefs.GetInt(resolutionKeyName);
            }
            else
            {
                // Set the current screen resolution as default
                var resolution = Screen.currentResolution;
                resolutionIndex = resolutions.FindIndex(r=>r.width == resolution.width && r.height == resolution.height && r.refreshRateRatio.value == resolution.refreshRateRatio.value);
                if(resolutionIndex < 0)
                    resolutionIndex = resolutions.FindIndex(r=>r.width == 1024 && r.height == 768 && r.refreshRateRatio.value == resolution.refreshRateRatio.value);
            }

            // Apply resolution
            Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, FullScreenMode.FullScreenWindow, resolutions[resolutionIndex].refreshRateRatio); 

            

        }
        #endregion
    }

}
