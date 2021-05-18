using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseManager : MonoBehaviour
{
    /// <summary>
    /// Bool that checks if the game is paused or not.
    /// </summary>
    private bool m_isPaused = false;
    /// <summary>
    /// A reference to the pause menu gameobject.
    /// </summary>
    [SerializeField] private GameObject m_pauseMenu = null;
    /// <summary>
    /// A reference to the controls menu gameobject.
    /// </summary>
    [SerializeField] private GameObject m_ControlsMenu = null;
    /// <summary>
    /// A reference to the options menu gameobject.
    /// </summary>
    [SerializeField] private GameObject m_OptionsMenu = null;
    /// <summary>
    /// Occurs before the first update loop.
    /// </summary>
    private void Start()
    {
        m_isPaused = false;
        m_pauseMenu.SetActive(false);
        m_ControlsMenu.SetActive(false);
        m_OptionsMenu.SetActive(false);
    }
    /// <summary>
    /// Checks for pause menu input, opens or closes pause menu and pauses the game if its found.
    /// </summary>
    /// 
    private void Update()
    {
        CheckForInput();
    }

    /// <summary>
    /// Checks if there's any input, if so pauses/unpaused the game
    /// </summary>
    public void CheckForInput()
    {
        if (m_isPaused == false)
        {
            if (Input.GetButtonDown("Pause"))
            {
                Time.timeScale = 0f;
                m_isPaused = true;
                m_pauseMenu.SetActive(true);
                m_ControlsMenu.SetActive(false);
                m_OptionsMenu.SetActive(false);
            }
        }
        else
        {
            if (Input.GetButtonDown("Pause"))
            {
                Resume();
            }
        }
    }

    /// <summary>
    /// A method used for the button click event - Resumes the game
    /// </summary>
    public void Resume()
    {
        Time.timeScale = 1f;
        m_isPaused = false;
        m_pauseMenu.SetActive(false);
        m_ControlsMenu.SetActive(false);
        m_OptionsMenu.SetActive(false);
    }
    
    /// <summary>
    /// Quits the game in runtime, if in editor it debugs to log.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        Debug.Log("Application quit.");
#endif
    }
    public void QuitSemi()
    {
        SceneManager.LoadScene("Menu Screen");
    }
    /// <summary>
    /// Opens the help menu.
    /// </summary>
    public void Help()
    {
        if (m_ControlsMenu.activeInHierarchy)
        {
            m_ControlsMenu.SetActive(false);
        }
        else
        {
            m_ControlsMenu.SetActive(true);
        }
    }
    /// <summary>
    /// Opens the option menu.
    /// </summary>
    public void Options()
    {
        if (m_OptionsMenu.activeInHierarchy)
        {
            m_OptionsMenu.SetActive(false);
        }
        else
        {
            m_OptionsMenu.SetActive(true);
        }
    }
}
