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
        float refreshRate = -1; // Unlimited
        public float RefreshRate
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

            DebugResolutions();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                QualitySettings.SetQualityLevel(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                QualitySettings.SetQualityLevel(5);
            }
        }

        void DebugResolutions()
        {
            foreach(var r in Screen.resolutions)
            {
                Debug.Log($"{r.width}x{r.height} @ {r.refreshRateRatio.value}({r.refreshRateRatio.numerator}/{r.refreshRateRatio.denominator})");
            }
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
            PlayerPrefs.Save();
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
            SaveRefreshRatePlayerPrefs(refreshRate);

            //
            // VSync
            //
            if(PlayerPrefs.HasKey(vSyncKeyName))
                vSync = PlayerPrefs.GetInt(vSyncKeyName);
            // Update vSync
            UpdateVSync(vSync);
        }
        
        public void SaveRefreshRatePlayerPrefs(float value)
        {
            this.refreshRate = value;
            // Save player prefs only
            PlayerPrefs.SetFloat(refreshRateKeyName, value);
            PlayerPrefs.Save();
        }
      
        public void UpdateVSync(int value)
        {
            vSync = value;
            QualitySettings.vSyncCount = value;
            PlayerPrefs.SetInt(vSyncKeyName, value);
            PlayerPrefs.Save();
        }

        public void UpdateResolution(int width, int height, FullScreenMode fullScreenMode, float refreshRate)
        {
            Screen.SetResolution(width, height, fullScreenMode, new UnityEngine.RefreshRate() { numerator = (uint)refreshRate, denominator = 1});    
        }

        

        #endregion
    }

}
