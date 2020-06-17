using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroScreen : MonoBehaviour
{

    public Slider m_MasterVolume;
    public Slider m_SFXVolume;
    public Slider m_MusicVolume;

    public Text m_MasterText;
    public Text m_MusicText;
    public Text m_SFXText;

    public AudioMixer m_Mixer;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetVolume(GameObject goSlider)
    {
        if (goSlider.name == "Master")
        {
            m_Mixer.SetFloat("Master", m_MasterVolume.value);
            m_MasterText.text = (m_MasterVolume.value + 100).ToString();
        }
        else if (goSlider.name == "SFX")
        {
            m_Mixer.SetFloat("SFX", m_SFXVolume.value);
            m_SFXText.text = (m_SFXVolume.value + 100).ToString();
        }
        else if (goSlider.name == "Music")
        {
            m_Mixer.SetFloat("Music", m_MusicVolume.value);
            m_MusicText.text = (m_MusicVolume.value + 100).ToString();
        }
    }

    public void Begin()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Scenes/test_scene");
    }
}
