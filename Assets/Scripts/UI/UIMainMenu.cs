using System.Collections;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using NUnit.Framework;
using System.Collections.Generic;

namespace UI
{
    public class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private string sceneName = "MainScene";
        [SerializeField] private GameObject continueButton;
        [SerializeField] private UIFadeScreen fadeScreen;


        /*[SerializeField] private GameObject comfirmationPrompt;


        [SerializeField] private TMP_Text volumeTextValue = null;
        [SerializeField] private Slider volumeSlider = null;
        [SerializeField] private float defaultVolume = 1.0f;


        [SerializeField] private Slider brightnessSlider = null;
        [SerializeField] private TMP_Text brightnessTextValue = null;
        [SerializeField] private float defaultBrightness = 1;
        [SerializeField] PostProcessProfile _brightness;
        [SerializeField] private PostProcessLayer layer;
        [SerializeField] private TMP_Dropdown qualityDropdown;
        [SerializeField] private Toggle fullScreenToggle;

        private int _qualityLevel;
        private bool _isFullScreen;
        private float _brightnessLevel;
        AutoExposure exposure;


        public TMP_Dropdown resolutionDropdown;
        private Resolution[] resolutions;
*/

        
        private void Start()
        {
            AudioManager.Instance.Play("MainMenu");

            /*_brightness.TryGetSettings(out exposure);

            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;

            for(int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + "x" + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();*/

            if(SaveManager.Instance.HasSaveData() == false)
                continueButton.SetActive(false);
        }

        public void ContinueGame() => StartCoroutine(LoadScreenWithAfterEffect(1.5f));

        public void NewGame()
        {
            SaveManager.Instance.DeleteSaveData();
            StartCoroutine(LoadScreenWithAfterEffect(1.5f));
        }


        /*public void SetVolume(float volume){
            AudioListener.volume = volume;
            volumeTextValue.text = volume.ToString("0.0");
        }


        public void VolumeApply()
        {
            PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
            StartCoroutine(ComfirmationBox());
        }


        public void SetBrightness(float brightness){
            //_brightnessLevel = brightness;
            if(brightness != 0)
            {
                exposure.keyValue.value = brightness;
            }
            else
            {
                exposure.keyValue.value = .05f;
            }
            _brightnessLevel = exposure.keyValue.value;
            brightnessTextValue.text = brightness.ToString("0.0");
        }


        public void SetFullScreen(bool isFullScreen){
            _isFullScreen = isFullScreen;
        }


        public void SetQuality(int qualityIndex){
            _qualityLevel = qualityIndex;
        }


        public void GraphicsApply(){
            PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
            

            PlayerPrefs.SetInt("masterQuality", _qualityLevel);
            QualitySettings.SetQualityLevel(_qualityLevel);

            PlayerPrefs.SetInt("masterFullScreen", (_isFullScreen ? 1 : 0));
            Screen.fullScreen = _isFullScreen;

            StartCoroutine(ComfirmationBox());
        }


        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }


        public IEnumerator ComfirmationBox()
        {
            comfirmationPrompt.SetActive(true);
            yield return new WaitForSeconds(2);
            comfirmationPrompt.SetActive(false);
        }


        public void ResetButton(string MenuType){
            if(MenuType == "Audio"){
                AudioListener.volume = defaultVolume;
                volumeSlider.value = defaultVolume;
                volumeTextValue.text = defaultVolume.ToString("0.0");
                VolumeApply();
            }

            if(MenuType == "Graphics")
            {
                brightnessSlider.value = defaultBrightness;
                brightnessTextValue.text = defaultBrightness.ToString("0.0");

                qualityDropdown.value = 1;
                QualitySettings.SetQualityLevel(1);

                fullScreenToggle.isOn = true;
                Screen.fullScreen = true;

                Resolution currentResolution = Screen.currentResolution;
                Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
                resolutionDropdown.value = resolutions.Length;

                GraphicsApply();
            }
        }*/

        public void NumberOfLevel(int index){
            SaveManager.Instance.DeleteSaveData();
            SceneManager.LoadScene(index);
        }

        public void ExitGame()
        {
            Debug.Log("Exit Game");
            Application.Quit();
        }

        private IEnumerator LoadScreenWithAfterEffect(float delay)
        {
            fadeScreen.FadeOut();
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
