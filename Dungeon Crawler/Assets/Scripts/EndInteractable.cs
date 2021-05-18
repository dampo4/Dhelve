using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndInteractable : Interactable
{
    /// <summary>
    /// The player.
    /// </summary>
    [SerializeField] private GameObject m_player;
    /// <summary>
    /// The dungeon generator script.
    /// </summary>
    [SerializeField] private DungeonGen m_dungeon;
    [SerializeField] private PlayerMovement m_movement;

    /// <summary>
    /// Checks if the player has interacted with the end.
    /// </summary>
    public override void InteractedWith()
    {
        /// Gets the dungeonGen script from the dungeon gen game object.
        m_dungeon = GameObject.FindGameObjectWithTag("Dungeon").GetComponent<DungeonGen>();
        m_movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        /// Gets the player object.
        m_player = GameObject.FindGameObjectWithTag("Player");
        /// Debug.
        base.InteractedWith();
        SaveSystem.SavePlayer(m_player.GetComponentInChildren<PlayerHotbar>());
        SaveSystem.SavePlayer(m_player.GetComponentInChildren<PlayerStats>());
        /// Rebuilds a new dungeon.
        m_dungeon.Start();
        /// Moves the player to the start position of the new dungeon.
        

    }

}
