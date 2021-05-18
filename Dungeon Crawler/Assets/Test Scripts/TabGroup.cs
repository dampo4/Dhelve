using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Created using Game Dev Guide's 'Creating a Custom Tab System in Unity' https://www.youtube.com/watch?v=211t6r12XPQ
/// </summary>
public class TabGroup : MonoBehaviour
{
    /// <summary>
    /// A list where all the buttons in the group are stored.
    /// </summary>
    public List<TabButton> m_tabButtons;
    /// <summary>
    /// The idle sprite of the button.
    /// </summary>
    public Color m_tabIdle;
    /// <summary>
    /// The hover sprite of a button.
    /// </summary>
    public Color m_tabHover;
    /// <summary>
    /// The active sprite of a button.
    /// </summary>
    public Color m_tabActive;
    /// <summary>
    /// The currently selected tab.
    /// </summary>
    public TabButton m_selectedTab;
    /// <summary>
    /// The objects that are swapped between, when a button is pressed.
    /// </summary>
    public List<GameObject> m_objectsToSwap;
    /// <summary>
    /// Subscribes a button to a tab group.
    /// </summary>
    /// <param name="button"></param>
    public void Subscribe(TabButton button)
    {
        if(m_tabButtons == null)
        {
            m_tabButtons = new List<TabButton>();
        }
        m_tabButtons.Add(button);
    }
    /// <summary>
    /// When a tab is hovered over, activate the hover sprite.
    /// </summary>
    /// <param name="button"></param>
    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        button.m_background.color = m_tabHover;
    }
    /// <summary>
    /// Resets all tabs when a tab is exited.
    /// </summary>
    /// <param name="button"></param>
    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }
    /// <summary>
    /// Activates the object attached to the selected tab and disables all other objects in m_selectedTab.
    /// </summary>
    /// <param name="button"></param>
    public void OnTabSelected(TabButton button)
    {
        m_selectedTab = button;
        ResetTabs();
        button.m_background.color = m_tabActive;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < m_objectsToSwap.Count; i++)
        {
            if(i == index)
            {
                m_objectsToSwap[i].SetActive(true);
            }
            else
            {
                m_objectsToSwap[i].SetActive(false);
            }
        }
    }
    /// <summary>
    /// Resets all tabs to their default state.
    /// </summary>
    public void ResetTabs()
    {
        foreach (TabButton button in m_tabButtons)
        {
            if (m_selectedTab != null && button == m_selectedTab)
            { continue; }
            button.m_background.color = m_tabIdle;
        }
    }
}
