using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// Was getting annoyed with testing bugs that occured after a time interval - This allows you to change the time simply within the editor.
/// </summary>
public class TimeChanger : EditorWindow
{
    /// <summary>
    /// The timescale to set the timescale to.
    /// </summary>
    private float m_TimeScale = 1;
    [MenuItem("Window/TimeChanger/Change TimeScale")]
    /// <summary>
    /// Shows the window.
    /// </summary>
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TimeChanger));
        
        
    }

    /// <summary>
    /// Creates the editor GUI.
    /// </summary>
    private void OnGUI()
    {
        // The actual window code goes here
        GUILayout.Label("Time Scale");
        m_TimeScale = EditorGUILayout.Slider(m_TimeScale, 0, 10);
   
        if (GUILayout.Button("Set Time"))
        {
            SetTime(m_TimeScale);
        }
    }
    /// <summary>
    /// Sets the timescale to the passed integer.
    /// </summary>
    /// <param name="TimeScale">The timescale to set.</param>
    private void SetTime(float TimeScale)
    {
        Time.timeScale = TimeScale;
    }
}
