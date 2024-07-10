using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    public const string SOUND_PREF_KEY = "soundOn";
    public const string VIBRATION_PREF_KEY = "vibrationOn";
    public const string MUSIC_PREF_KEY = "musicOn";
    public const int SETTING_ON = 1;
    public const float DEFAULT_SOUND_VALUE = 0.0f;
    public const float ATTENUATION_TO_MUTE = -80.0f;
    // Settings
    public float mixerTweenDuration = 1.0f;
    // Connections
    
    public SwitchToggle soundSwitch;
    public SwitchToggle vibrationSwitch;
    public SwitchToggle musicSwitch;
    public AudioMixer masterMixer;
    public UnityEvent<bool,bool,bool> OnClosed;

    // State variables
    float initialSoundVolume;
    float initialMusicVolume;
    void Awake(){
     
    }

    void Start()
    {
        InitState();
    }

    void InitConnections(){
       
    }
    void InitState()
    {
        masterMixer.GetFloat("soundVolume", out initialSoundVolume);
        masterMixer.GetFloat("musicVolume", out initialMusicVolume);

        LoadSettingsState();

    }

    public void LoadSettingsState()
    {
        bool soundOn = (PlayerPrefs.GetInt(SOUND_PREF_KEY, SETTING_ON)) == SETTING_ON;
        bool vibrationOn = (PlayerPrefs.GetInt(VIBRATION_PREF_KEY, SETTING_ON)) == SETTING_ON;
        bool musicOn = (PlayerPrefs.GetInt(MUSIC_PREF_KEY, SETTING_ON)) == SETTING_ON;

        //SoftwareKingdom.Static.GameSettingsBase.soundOn = soundOn;
        //SoftwareKingdom.Static.GameSettingsBase.vibrationOn = vibrationOn;
        //SoftwareKingdom.Static.GameSettingsBase.musicOn = musicOn;

        SetSoundSetting(soundOn);
        SetMusicSetting(musicOn);
        SetVibrationSetting(vibrationOn);

        soundSwitch.SetValue(soundOn);
        vibrationSwitch.SetValue(vibrationOn);
        musicSwitch.SetValue(musicOn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
        
        OnClosed.Invoke(
            soundSwitch.value,
            vibrationSwitch.value,
            musicSwitch.value
            );
    }

    public void SetSoundSetting(bool value)
    {
        float soundMixerValue = value ? DEFAULT_SOUND_VALUE : ATTENUATION_TO_MUTE;
      //  SoftwareKingdom.Static.GameSettingsBase.soundOn = value;
        PlayerPrefs.SetInt(SOUND_PREF_KEY, value ? SETTING_ON : 0);
        masterMixer.SetFloat("soundVolume", soundMixerValue);
    }

    public void SetVibrationSetting(bool value)
    {
        SoftwareKingdom.Static.GameSettingsBase.vibrationOn = value;
        PlayerPrefs.SetInt(VIBRATION_PREF_KEY, value ? SETTING_ON : 0);
    }

    public void SetMusicSetting(bool value)
    {

        float musicMixerValue = value ? DEFAULT_SOUND_VALUE : ATTENUATION_TO_MUTE;
        
        Debug.Log("musicMixerValue " + musicMixerValue);
        masterMixer.SetFloat("musicVolume", musicMixerValue);
        TweenToMixerSetting("musicVolume", musicMixerValue);
        SoftwareKingdom.Static.GameSettingsBase.musicOn = value;
        PlayerPrefs.SetInt(MUSIC_PREF_KEY, value ? SETTING_ON : 0);
    }

    public void TweenToMixerSetting(string mixerVariableName, float targetValue)
    {
        DOTween.To(
            ()=>GetMixerValue(mixerVariableName),
            x=>SetMixerValue(mixerVariableName,x),
            targetValue,
            mixerTweenDuration
            );
    }

    private float GetMixerValue(string mixerVariableName)
    {
        float value;
        masterMixer.GetFloat(mixerVariableName,out value);
        return value;
    }

    private void SetMixerValue(string mixerVariableName,float value)
    {        
        masterMixer.SetFloat(mixerVariableName, value);
    }

}
