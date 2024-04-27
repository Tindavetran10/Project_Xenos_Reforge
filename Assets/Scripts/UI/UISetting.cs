using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace UI
{
    public class UISetting : MonoBehaviour
    {
        private GameObject _confirmationPrompt;

        [Header("Volume Setting")]
        [SerializeField] private TMP_Text volumeTextValue;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private float defaultVolume = 1.0f;

        [Header("Brightness Setting")]
        [SerializeField] private Slider brightnessSlider;
        [SerializeField] private TMP_Text brightnessTextValue;
        [SerializeField] private float defaultBrightness = 1;
        [SerializeField] private PostProcessProfile brightness;
        [SerializeField] private PostProcessLayer layer;

        [Header("Quality of Screen")]
        [SerializeField] private TMP_Dropdown qualityDropdown;
        [SerializeField] private Toggle fullScreenToggle;

        private int _qualityLevel;
        private bool _isFullScreen;
        static float _brightnessLevel = 1f;
        private AutoExposure _exposure;


        public TMP_Dropdown resolutionDropdown;
        private Resolution[] _resolutions;

        //static float _v=5f;

        // Start is called before the first frame update
        void Start()
        {
            brightness.TryGetSettings(out _exposure);

            _resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            var options = new List<string>();

            var currentResolutionIndex = 0;

            for (var i = 0; i < _resolutions.Length; i++)
            {
                var option = _resolutions[i].width + "x" + _resolutions[i].height;
                options.Add(option);

                if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
                    currentResolutionIndex = i;
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void SetVolume(float volume)
        {
            AudioListener.volume = volume;
            volumeTextValue.text = volume.ToString("0.0");
        }


        public void VolumeApply()
        {
            PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
            StartCoroutine(ConfirmationBox());
        }


        public void SetBrightness(float brightnessValue)
        {
            //_brightnessLevel = brightness;
            _exposure.keyValue.value = brightnessValue != 0 ? brightnessValue : .05f;
            _brightnessLevel = _exposure.keyValue.value;
            brightnessTextValue.text = brightnessValue.ToString("0.0");
        }


        public void SetFullScreen(bool isFullScreen) => _isFullScreen = isFullScreen;


        public void SetQuality(int qualityIndex) => _qualityLevel = qualityIndex;


        public void GraphicsApply()
        {
            PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
            
            PlayerPrefs.SetInt("masterQuality", _qualityLevel);
            QualitySettings.SetQualityLevel(_qualityLevel);

            PlayerPrefs.SetInt("masterFullScreen", (_isFullScreen ? 1 : 0));
            Screen.fullScreen = _isFullScreen;

            StartCoroutine(ConfirmationBox());
        }


        public void SetResolution(int resolutionIndex)
        {
            var resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }


        private IEnumerator ConfirmationBox()
        {
            _confirmationPrompt.SetActive(true);
            yield return new WaitForSeconds(2);
            _confirmationPrompt.SetActive(false);
        }


        public void ResetButton(string menuType)
        {
            switch (menuType)
            {
                case "Audio":
                    AudioListener.volume = defaultVolume;
                    volumeSlider.value = defaultVolume;
                    volumeTextValue.text = defaultVolume.ToString("0.0");
                    VolumeApply();
                    break;
                case "Graphics":
                {
                    brightnessSlider.value = defaultBrightness;
                    brightnessTextValue.text = defaultBrightness.ToString("0.0");

                    qualityDropdown.value = 1;
                    QualitySettings.SetQualityLevel(1);

                    fullScreenToggle.isOn = true;
                    Screen.fullScreen = true;

                    var currentResolution = Screen.currentResolution;
                    Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
                    resolutionDropdown.value = _resolutions.Length;

                    GraphicsApply();
                    break;
                }
            }
        }

        private void Update()
        {
            if (Mathf.Approximately(_exposure.keyValue.value, _brightnessLevel))
                _exposure.keyValue.value = _brightnessLevel;
        }
    }
}
