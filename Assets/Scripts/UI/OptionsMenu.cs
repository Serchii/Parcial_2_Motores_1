using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    void Start()
    {
        float value;

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float saved = PlayerPrefs.GetFloat("MusicVolume");
            musicSlider.value = saved;
            audioMixer.SetFloat("MusicVolume", SliderToDb(saved));
        }
        else if (audioMixer.GetFloat("MusicVolume", out value))
        {
            musicSlider.value = DbToSlider(value);
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float saved = PlayerPrefs.GetFloat("SFXVolume");
            sfxSlider.value = saved;
            audioMixer.SetFloat("SFXVolume", SliderToDb(saved));
        }
        else if (audioMixer.GetFloat("SFXVolume", out value))
        {
            sfxSlider.value = DbToSlider(value);
        }
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", SliderToDb(value));
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", SliderToDb(value));
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    float SliderToDb(float sliderValue)
    {
        return Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
    }

    float DbToSlider(float dbValue)
    {
        return Mathf.Pow(10f, dbValue / 20f);
    }
}