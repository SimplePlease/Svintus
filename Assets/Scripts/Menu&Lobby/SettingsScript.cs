using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public Toggle isMusicOn;
    public AudioSource source;
    public Slider slider;
    public AudioMixer mixer;
    public Toggle isFullScreen;

    public void SetUp()
    {
        isFullScreen.isOn = PlayerPrefs.GetInt("ScreenMode") == 0;
        isMusicOn.isOn = PlayerPrefs.GetInt("MusicMode") == 0;
        slider.value = PlayerPrefs.GetFloat("SliderValue");
        mixer.SetFloat("MusicVolume", slider.value);
        source.mute = !isMusicOn.isOn;
    }


 
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) { gameObject.SetActive(false); };
        
    }

    public void ToggleFullScreen()
    {
        Screen.fullScreen = isFullScreen.isOn;
    }

    public void ToggleMusic()
    {
        source.mute = !isMusicOn.isOn;
    }
    
    
    public void SetVolume()
    {
        mixer.SetFloat("MusicVolume", slider.value);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("ScreenMode", isFullScreen.isOn ?  0:1);
        PlayerPrefs.SetInt("MusicMode", isMusicOn.isOn ? 0 : 1);
        PlayerPrefs.SetFloat("SliderValue", slider.value);
    }
}