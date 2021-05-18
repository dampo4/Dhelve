using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopCheckout : MonoBehaviour
{
    /// <summary>
    /// A reference to the player GameObject.
    /// </summary>
    public GameObject m_player;
    /// <summary>
    /// A TMPRO GUI variable that shows the player's gold.
    /// </summary>
    public TextMeshProUGUI m_playerGold;

    /// <summary>
    /// Sets the players gold.
    /// </summary>
    private void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        SetGold();
    }
    /// <summary>
    /// Buys the item and attaches it to the players hotbar if the player has enough gold, otherwise does nothing.
    /// </summary>
    /// <param name="item"></param>
    public void Checkout(Item item)
    {


        PlayerHotbar m_playerHotbar = m_player.GetComponentInChildren<PlayerHotbar>();
        PlayerStats m_playerStats = m_player.GetComponent<PlayerStats>();

        if (m_playerStats.m_gold.GetValue() > item.m_itemCost)
        {
            item.OnPickUp();
            m_playerStats.m_gold.SetValue(m_playerStats.m_gold.GetValue() - (int)item.m_itemCost);
            SaveSystem.SavePlayer(m_playerHotbar);
            SaveSystem.SavePlayer(m_playerStats);
        }
        else
        {
            Debug.Log("Not enough gold to purchase.");
        }

        SetGold();
    }
    /// <summary>
    /// Displays the players gold as a part of the GUI.
    /// </summary>
    public void SetGold()
    {
        m_playerGold.text = m_player.GetComponent<PlayerStats>().GetGold().ToString();
    }
}
