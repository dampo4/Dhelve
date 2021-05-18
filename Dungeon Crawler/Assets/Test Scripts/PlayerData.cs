using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    /// <summary>
    /// An int denoting the players level.
    /// </summary>
    public int m_level;
    /// <summary>
    /// An int denoting the players current gold.
    /// </summary>
    public int m_gold;
    /// <summary>
    /// An int denoting the players current experience.
    /// </summary>
    public int m_experience;
    /// <summary>
    /// A string array, holding the current items within the players hotbar as names.
    /// </summary>
    public string[] m_hotbarItems = new string[4];

    /// <summary>
    /// Takes in a playerhotbar, and sets m_hotBarItems.
    /// </summary>
    /// <param name="playerHotbar"></param>
    public PlayerData(PlayerHotbar playerHotbar)
    {
        for (int i = 0; i < playerHotbar.m_hotBarItems.Length; i++)
        {
            if (playerHotbar.m_hotBarItems[i] != null)
            {
                m_hotbarItems[i] = playerHotbar.m_hotBarItems[i].name;
            }
        }
    }
    /// <summary>
    /// Stores the players current stats, ready to be saved.
    /// </summary>
    /// <param name="playerStats"></param>
    public PlayerData(PlayerStats playerStats)
    {
        m_level = playerStats.m_level;
        m_gold = playerStats.m_gold.GetValue();
        m_experience = playerStats.m_experience.GetValue();
    }
}
