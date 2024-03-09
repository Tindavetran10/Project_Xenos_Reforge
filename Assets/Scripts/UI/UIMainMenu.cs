using System.Collections;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private string sceneName = "MainScene";
        [SerializeField] private GameObject continueButton;
        [SerializeField] private UIFadeScreen fadeScreen;

        [SerializeField] private TMP_Text volumeTextValue = null;
        [SerializeField] private Slider volumeSlider = null;
        [SerializeField] private GameObject confirmationPromt = null;
        [SerializeField] private float defaultVolume = 1.0f;
        
        private void Start()
        {
            if(SaveManager.Instance.HasSaveData() == false)
                continueButton.SetActive(false);
        }

        public void ContinueGame()
        {
            StartCoroutine(LoadScreenWithAfterEffect(1.5f));
        }

        public void NewGame()
        {
            SaveManager.Instance.DeleteSaveData();
            StartCoroutine(LoadScreenWithAfterEffect(1.5f));
            
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
            SceneManager.LoadScene(sceneName);
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


        public void ResetButton(string Menutype)
        {
            if (Menutype == "Audio")
            {
                AudioListener.volume = defaultVolume;
                volumeSlider.value = defaultVolume;
                volumeTextValue.text = defaultVolume.ToString("0.0");
                VolumeApply();
            }
        }


        public IEnumerator ConfirmationBox()
        {
            confirmationPromt.SetActive(true);
            yield return new WaitForSeconds(2);
            confirmationPromt.SetActive(false);
        }
    }
}
