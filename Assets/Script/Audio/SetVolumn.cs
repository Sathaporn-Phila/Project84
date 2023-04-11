using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolumn : MonoBehaviour
{
    public AudioMixer music_mixer;
    public AudioMixer sound_mixer;
    public void SetMasterLevel (float sliderValue)
    {
        music_mixer.SetFloat("MusicVolumn", Mathf.Log10(sliderValue) * 20);
        sound_mixer.SetFloat("SoundVolumn", Mathf.Log10(sliderValue) * 20);
    }
    public void SetMusicLevel (float sliderValue)
    {
        music_mixer.SetFloat("MusicVolumn", Mathf.Log10(sliderValue) * 20);
    }
    public void SetSoundLevel (float sliderValue)
    {
        sound_mixer.SetFloat("SoundVolumn", Mathf.Log10(sliderValue) * 20);
    }
}
