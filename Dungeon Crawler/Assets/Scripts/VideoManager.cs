using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Made using Brackeys tutorial: https://www.youtube.com/watch?v=YOaYQrN1oYQ
/// </summary>
public class VideoManager : MonoBehaviour
{
    /// <summary>
    /// Reference to a Dropdown UI component.
    /// </summary>
    [SerializeField] private TMP_Dropdown m_resolutionDropdown;
    
    /// <summary>
    /// An array of viable resolutions.
    /// </summary>
    private Resolution[] m_resolutions;

    // Start is called before the first frame update
    void Start()
    {
        m_resolutions = Screen.resolutions;
        m_resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResIndex = 0;
        for (int i = 0; i < m_resolutions.Length; i++)
        {
           
            string option = m_resolutions[i].width + " x " + m_resolutions[i].height;
            options.Add(option);
            if (m_resolutions[i].width == Screen.currentResolution.width && m_resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }
        m_resolutionDropdown.AddOptions(options);
        m_resolutionDropdown.value = currentResIndex;
        m_resolutionDropdown.RefreshShownValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetResolution(int index)
    {
        Resolution res = m_resolutions[index];
        Screen.SetResolution(res.width, res.height, FullScreenMode.FullScreenWindow);
    }
}
