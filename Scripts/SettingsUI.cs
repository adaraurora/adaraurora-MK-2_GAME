using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public Slider musicSlider;

    void Start()
    {
        float volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicSlider.value = volume;
    }
}