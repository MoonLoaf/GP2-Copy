using System;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    private AudioMixer _mixer;

    private float _masterVolume;
    private float _uiVolume;
    private float _sfxVolume;
    private float _dialogVolume;
    private float _ambientVolume;
    private float _musicVolume;

    private void Awake() {
        if (_mixer == null) {
            _mixer = Resources.Load<AudioMixer>("Mixer/Master");
        }
        _mixer.GetFloat("MasterVolume", out _masterVolume);
        _mixer.GetFloat("UiVolume", out _uiVolume);
        _mixer.GetFloat("AmbientVolume", out _ambientVolume);
        _mixer.GetFloat("MusicVolume", out _musicVolume);
        _mixer.GetFloat("DialogVolume", out _dialogVolume);
        _mixer.GetFloat("SfxVolume", out _sfxVolume);
    }
}
