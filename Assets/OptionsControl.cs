using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsControl : MonoBehaviour
{
    [SerializeField] private AudioMixer mainAudio;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMasterVolume()
    {
        float finalValue = masterSlider.value;
        if (finalValue == -20f) finalValue = -80f;
        mainAudio.SetFloat("MasterVolume", finalValue);
    }
    public void ChangeBGMVolume()
    {
        float finalValue = bgmSlider.value;
        if (finalValue == -20f) finalValue = -80f;
        mainAudio.SetFloat("BGMVolume", finalValue);
    }

    public void ChangeSFXVolume()
    {
        float finalValue = sfxSlider.value;
        if (finalValue == -20f) finalValue = -80f;
        mainAudio.SetFloat("SFXVolume", finalValue);
    }
}
