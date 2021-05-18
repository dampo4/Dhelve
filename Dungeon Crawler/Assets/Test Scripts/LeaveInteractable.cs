using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveInteractable : Interactable
{
    [SerializeField] private GameObject m_player;
    public override void InteractedWith()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        SaveSystem.SavePlayer(m_player.GetComponentInChildren<PlayerHotbar>());
        SaveSystem.SavePlayer(m_player.GetComponentInChildren<PlayerStats>());
        SceneManager.LoadScene("Overworld");
    }
}
