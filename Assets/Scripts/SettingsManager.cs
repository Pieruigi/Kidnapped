using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering.PostProcessing;

namespace Kidnapped
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        public static UnityAction<int> OnAntialiasingChanged;
        public static UnityAction<float> OnMouseSensitivityChanged;
        public static UnityAction<bool> OnMouseYAxisInvertedChanged;

        #region audio
        [SerializeField]
        AudioMixer audioMixer;

        //
        // Global volume
        //
        int globalVolume = 100;
        public int GlobalVolume
        {
            get { return globalVolume; }
        }
        string globalVolumeKeyName = "GlobalVolume";
        string globalVolumeParamName = "GlobalVolume";

        //
        // Subtitles on/off
        //
        int subtitlesOnOff;
        string subtitlesOnOffKeyName = "SubtitlesOn";
        public bool SubtitlesOn
        {
            get { return subtitlesOnOff > 0; }
        }

        ////
        //// Language
        ////
        //int localeId;
        //string localeIdKeyName = "Locale";
        //int LocaleId
        //{
        //    get { return localeId; }
        //}

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

        /// <summary>
        /// Antialiasing
        /// 0: None
        /// 1: FXAA
        /// 2: SMAA
        /// 3: TXAA
        /// </summary>
        int antialiasing = 3; // TAA
        public int Antialiasing
        {
            get { return antialiasing; }
        }
        string antialiasingKeyName = "Antialiasing";
        #endregion

        #region controls
        /// <summary>
        /// Mouse sensitivity
        /// </summary>
        float mouseSensitivity = 5.5f;
        public float MouseSensitivity
        {
            get { return mouseSensitivity; }
        }
        string mouseSensitivityKeyName = "MouseSensitivity";

        /// <summary>
        /// Invert Y axis
        /// </summary>
        int yAxisInverted = 0; 
        public bool MouseInvertedY
        {
            get { return yAxisInverted != 0; }
        }
        string yAxisInvertedKeyName = "InvertedY";
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            InitAudio();
            InitGraphics();
            InitControls();
            //DebugResolutions();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                //QualitySettings.SetQualityLevel(0);
                UpdateAntialiasing(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //QualitySettings.SetQualityLevel(5);
                UpdateAntialiasing(3);
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

            // Subtitles on/off
            if(PlayerPrefs.HasKey(subtitlesOnOffKeyName))
                subtitlesOnOff = PlayerPrefs.GetInt(subtitlesOnOffKeyName);

            // Language
            //if(PlayerPrefs.HasKey(localeIdKeyName))
            //    localeId = PlayerPrefs.GetInt(localeIdKeyName);
            //LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];

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

        public void UpdateSubtitlesOnOff(int newValue)
        {
            subtitlesOnOff = newValue;
            PlayerPrefs.SetInt(subtitlesOnOffKeyName, subtitlesOnOff);
            PlayerPrefs.Save();
        }

        //public void UpdateLocale(int newValue)
        //{
        //    localeId = newValue;
        //    LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
        //    PlayerPrefs.SetInt(localeIdKeyName, localeId);
        //    PlayerPrefs.Save();
        //}
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

            // 
            // Antialiasing
            //
            if(PlayerPrefs.HasKey(antialiasingKeyName))
                antialiasing = PlayerPrefs.GetInt(antialiasingKeyName);
            UpdateAntialiasing(antialiasing);
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

        public void UpdateAntialiasing(int antialiasing)
        {
            this.antialiasing = antialiasing;
            PlayerPrefs.SetInt(antialiasingKeyName, antialiasing);
            PlayerPrefs.Save();
            OnAntialiasingChanged?.Invoke(antialiasing);
        }

        #endregion

        #region controls
        void InitControls()
        {
            ///
            /// Mouse sensitivity
            /// 
            if(PlayerPrefs.HasKey(mouseSensitivityKeyName))
                mouseSensitivity = PlayerPrefs.GetFloat(mouseSensitivityKeyName);

            ///
            /// Y axis
            /// 
            if(PlayerPrefs.HasKey(yAxisInvertedKeyName))
                yAxisInverted = PlayerPrefs.GetInt(yAxisInvertedKeyName);
        }

        public void UpdateMouseSensitivity(float sensitivity)
        {
            mouseSensitivity = sensitivity;
            PlayerPrefs.SetFloat(mouseSensitivityKeyName, mouseSensitivity);
            PlayerPrefs.Save();
            OnMouseSensitivityChanged?.Invoke(sensitivity);
        }

        public void UpdateInvertedAxisY(bool inverted)
        {
            yAxisInverted = inverted ? 1 : 0;
            PlayerPrefs.SetInt(yAxisInvertedKeyName, yAxisInverted);
            PlayerPrefs.Save();
            OnMouseYAxisInvertedChanged?.Invoke(inverted);
            
        }
        #endregion
    }

}
