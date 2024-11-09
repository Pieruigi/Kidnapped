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
        /// Refresh rate
        /// </summary>
        int refreshRate = -1; // Unlimited
        public int RefreshRate
        {
            get { return refreshRate; }
        }
        string refreshRateKeyName = "RefreshRate";

        /// <summary>
        /// VSync
        /// </summary>
        int vSync = 0; // Disabled
        public int VSync
        {
            get { return vSync; }
        }
        string vSyncKeyName = "VSync";
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            InitAudio();
            InitGraphics();
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
            // Refresh rate
            //
            if(PlayerPrefs.HasKey(refreshRateKeyName))
                refreshRate = PlayerPrefs.GetInt(refreshRateKeyName);
            // Set the refresh rate
            UpdateRefreshRate(refreshRate);

            //
            // VSync
            //
            if(PlayerPrefs.HasKey(vSyncKeyName))
                vSync = PlayerPrefs.GetInt(vSyncKeyName);
            // Update vSync
            UpdateVSync(vSync);
        }
        
        public void UpdateRefreshRate(int value)
        {
            this.refreshRate = value;
            // Set the refresh rate
            Application.targetFrameRate = value;
            // Save value
            PlayerPrefs.SetInt(refreshRateKeyName, value);
        }
      
        public void UpdateVSync(int value)
        {
            vSync = value;
            QualitySettings.vSyncCount = value;
            PlayerPrefs.SetInt(vSyncKeyName, value);
        }

        #endregion
    }

}
