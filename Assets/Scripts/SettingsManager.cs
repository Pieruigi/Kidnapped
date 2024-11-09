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
        /// <summary>
        /// Resolution
        /// </summary>
        int resolutionIndex = -1; // The current resolution 
        public int ResolutionIndex
        {
            get { return resolutionIndex; }
        }
        List<Resolution> resolutions; // All the available resolutions
        string resolutionKeyName = "ResolutionIndex";

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            InitAudio();
            InitGraphics();
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
        void DebugResolutions()
        {
            foreach (var resolution in resolutions)
                Debug.Log($"[SettingsManager - Resolution:{resolution}]");
        }
        void InitGraphics()
        {
            // 
            // Resolution
            //
            // Get all the available resolutions
            resolutions = Screen.resolutions.ToList();
            DebugResolutions();
            // Load from player prefs
            if (PlayerPrefs.HasKey(resolutionKeyName))
            {
                resolutionIndex = PlayerPrefs.GetInt(resolutionKeyName);
            }
            else
            {
                // Set the current screen resolution as default
                var resolution = Screen.currentResolution;
                Debug.Log($"[SettingsManager - Current resolution:{resolution}");
                resolutionIndex = resolutions.FindIndex(r=>r.width == resolution.width && r.height == resolution.height && 
                                                        r.refreshRateRatio.numerator == resolution.refreshRateRatio.numerator && 
                                                        r.refreshRateRatio.denominator == resolution.refreshRateRatio.denominator);
                if(resolutionIndex < 0)
                    resolutionIndex = resolutions.FindIndex(r=>r.width == 1024 && r.height == 768);
            }

            // Apply resolution
            Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, FullScreenMode.FullScreenWindow, resolutions[resolutionIndex].refreshRateRatio); 
        }

        public Resolution GetCurrentResolution()
        {
            return resolutions[resolutionIndex];
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Resolution> GetAvailableResolutionsWithTheCurrentRefreshRate()
        {
            
            var refreshRateRatio = resolutions[resolutionIndex].refreshRateRatio;

            return resolutions.FindAll(r=>r.refreshRateRatio.Equals(refreshRateRatio));
        }

        /// <summary>
        /// We only apply resolution
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void ApplyResolution(int width, int height, FullScreenMode fullScreenMode)
        {
            Debug.Log($"[SettingsManager - Applying new resolution:{width}x{height}");
            // Try to keep the same frame rate if possible
            var index = resolutions.FindIndex(r=>r.width == width && r.height == height && r.refreshRateRatio.Equals(resolutions[resolutionIndex].refreshRateRatio));
            if(index < 0)
            {
                // Probably the current refresh rate doesn't support the resolution we are trying to set
                index = resolutions.FindLastIndex(r => r.width == width && r.height == height); // This should be the one with the highest refresh rate
            }
            if(index < 0)
            {
                Debug.LogWarning($"[SettingsManager - No resolution found:{width}x{height}");
                return;
            }
            // Set the new resolution index
            resolutionIndex = index;
            Screen.SetResolution(width, height, FullScreenMode.MaximizedWindow, resolutions[index].refreshRateRatio);
        }

        #endregion
    }

}
