using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// A slider for Ambient volume
    /// </summary>
    [SerializeField] private Slider m_ambient_Slider;
    /// <summary>
    ///  A slider for SFX volume
    /// </summary>
    [SerializeField] private Slider m_sfx_Slider;
    /// <summary>
    /// a Slider for voice volume
    /// </summary>
    [SerializeField] private Slider m_voice_Slider;

    /// <summary>
    /// a Toggle for global mute
    /// </summary>
    [SerializeField] private Toggle m_globalMute;

    /// <summary>
    /// Initialises variables and checks playerprefs
    /// </summary>
    private void Start()
    {
        m_ambient_Slider.value = PlayerPrefs.GetFloat("Ambient_Sound", 1f);
        m_sfx_Slider.value = PlayerPrefs.GetFloat("SFX_Sound", 1f);
        m_voice_Slider.value = PlayerPrefs.GetFloat("Voice_Sound", 1f);
        PlayerPrefs.GetString("Global_Mute", "Off");

        if (PlayerPrefs.GetString("Global_Mute") == "On")
        {
            m_globalMute.isOn = true;
        }

        else
        {
            m_globalMute.isOn = false;
        }
    }
    /// <summary>
    /// Occurs every frame, checks for sound updates
    /// </summary>
    private void Update()
    {
        UpdateMusicSounds();
    }
    /// <summary>
    /// Updates all music volumes based on slider values.
    /// </summary>
    private void UpdateMusicSounds()
    {
        ///If sound is not globalled muted, continue
        if (!m_globalMute.isOn)
        {
            PlayerPrefs.SetString("Global_Mute", "Off");
            ///redundancy check - Make sure audiolistener is active.
            AudioListener.volume = 1;
            AudioListener.pause = false;
            ///Check all objects marked SFX and set their volume to the slider value
            foreach (GameObject audiosource in GameObject.FindGameObjectsWithTag("SFX"))
            {   
                ///Redundancy check - Check if there are any components
                if (audiosource.GetComponent<AudioSource>() != null)
                {
                    audiosource.GetComponent<AudioSource>().volume = m_sfx_Slider.value;
                    PlayerPrefs.SetFloat("SFX_Sound", m_sfx_Slider.value);

                }
            }
            ///Check all objects marked Ambient and set their volume to the slider value
            foreach (GameObject audiosource in GameObject.FindGameObjectsWithTag("Ambient"))
            {
                ///Redundancy check - Check if there are any components
                if (audiosource.GetComponent<AudioSource>() != null)
                {
                    audiosource.GetComponent<AudioSource>().volume = m_ambient_Slider.value;
                    PlayerPrefs.SetFloat("Ambient_Sound", m_ambient_Slider.value);

                }
            }
            ///Check all objects marked Voices and set their volume to the slider value
            foreach (GameObject audiosource in GameObject.FindGameObjectsWithTag("Voices"))
            {
                ///Redundancy check - Check if there are any components
                if (audiosource.GetComponent<AudioSource>() != null)
                {
                    audiosource.GetComponent<AudioSource>().volume = m_voice_Slider.value;
                    PlayerPrefs.SetFloat("Voice_Sound", m_voice_Slider.value);

                }
            }
        }
        ///Else mutes all audio because global mute is enabled.
        else
        {
            AudioListener.pause = true;
            AudioListener.volume = 0;
            PlayerPrefs.SetString("Global_Mute", "On");
        }
    }

}
