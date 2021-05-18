using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Minimap : MonoBehaviour
{
    /// <summary>
    /// A vector denoting the direction of the raycast.
    /// </summary>
    private Vector3 m_direction;
    /// <summary>
    /// The map UI panel.
    /// </summary>
    private GameObject m_map;
    /// <summary>
    /// The map UI overlay which tracks player position.
    /// </summary>
    private GameObject m_playerPositionMap;
    /// <summary>
    /// The position of the tile the player is in.
    /// </summary>
    private Vector3 m_tempVec;
    /// <summary>
    /// The tile the player is in.
    /// </summary>
    private GameObject m_tempTile;
    /// <summary>
    /// An array the size of the map UI which tracks whether the player has already visited a room.
    /// </summary>
    private int[,] m_grid = new int[9,9];
    /// <summary>
    /// Array of UI tile assets.
    /// </summary>
    public Sprite[] m_imgs;
    /// <summary>
    /// Sprite which represents the players position.
    /// </summary>
    public Sprite m_marker;
    /// <summary>
    /// Sprite used to clear the player position once they leave a room.
    /// </summary>
    public Sprite m_clear;
    /// <summary>
    /// Sprite which represents the end position.
    /// </summary>
    public Sprite m_end;
    /// <summary>
    /// Map overlay displaying the end room.
    /// </summary>
    private GameObject m_endMap;
    /// <summary>
    /// A float value denoting the distance the raycast will check.
    /// </summary>
    [SerializeField] private float m_maxDistance = 0;
    /// <summary>
    /// A GameObject which contains all minimap tiles.
    /// </summary>
    [SerializeField] private CanvasGroup m_mapPanel;
    private GameObject currentTile;
    public GameObject player;
    void Update()
    {
        UpdateMap();
        if (SceneManager.GetActiveScene().name == "Dungeon" && player.transform.position.y < 0)
        {
            player.transform.position = currentTile.transform.position + new Vector3(0,4,0);
        }
    }
    /// <summary>
    /// Updates the minimap UI.
    /// </summary>
    private void UpdateMap()
    {
        if (Input.GetButtonUp("MapToggle"))
        {
         if (m_mapPanel.alpha == 1)
            {
                m_mapPanel.alpha = 0;
            }
            else
            {
                m_mapPanel.alpha = 1;
            }
        }


        /// Used to set the raycast to be facing downwards.
        m_direction = transform.TransformDirection(Vector3.down);
        /// Used to get the UI component for the map
        m_map = GameObject.FindGameObjectWithTag("MapTiles");
        /// Used to get the UI component for the player position.
        m_playerPositionMap = GameObject.FindGameObjectWithTag("MapPlayer");
        /// Used to get the UI component for the map end position
        m_endMap = GameObject.FindGameObjectWithTag("MapEnd");
        RaycastHit hit;

        ///If the raycast hits something within the max distance in the direction of m_direction
        if (Physics.Raycast(transform.position, m_direction, out hit, m_maxDistance))
        {
            ///Check if the hit object has an interactable script
            if (hit.collider.gameObject.transform.parent.tag == "Tile")
            {
                currentTile = hit.collider.gameObject.transform.parent.gameObject;
                /// Gets the tile the player is standing on.
                m_tempTile = hit.collider.gameObject.transform.parent.gameObject;
                /// Gets the world position of the tile the player is standing on.
                m_tempVec = m_tempTile.transform.position;
                /// Gets the binary representation of the tile.
                string str = m_tempTile.name.Substring(0, 4);
                /// Clears the UI representing the player position.
                for (int i = 0; i < 81; i++)
                {
                    m_playerPositionMap.transform.GetChild(i).GetComponent<Image>().sprite = m_clear;
                }
                /// Places the player marker on the UI which represents the room the player is in.
                m_playerPositionMap.transform.GetChild((40 - (Mathf.RoundToInt(m_tempVec.z / 40) * 9)) + (Mathf.RoundToInt(m_tempVec.x / 40))).GetComponent<Image>().sprite = m_marker;
                /// Updates the UI which represents the map with the asset which represents the tile the player is on.
                m_map.transform.GetChild((40 - (Mathf.RoundToInt(m_tempVec.z / 40) * 9)) + (Mathf.RoundToInt(m_tempVec.x / 40))).GetComponent<Image>().sprite = m_imgs[(Convert.ToInt32(str, 2)) - 1];
                /// Updates the 2D array to say that the player has entered the room and the graphic no longer needs to be changed.
                m_grid[Mathf.RoundToInt(m_tempVec.x / 40) + 4, 4 - Mathf.RoundToInt(m_tempVec.z / 40)] = 1;
                /// If the tile contains the end, the UI is updated to show this.
                if (m_tempTile.name.Contains("end"))
                {
                    Debug.Log("test");
                    m_endMap.transform.GetChild((40 - (Mathf.RoundToInt(m_tempVec.z / 40) * 9)) + (Mathf.RoundToInt(m_tempVec.x / 40))).GetComponent<Image>().sprite = m_end;
                }
                /// Checks if the room has a north connection.
                if (m_tempTile.name[0] == '1')
                {
                    /// Checks if the UI square to the north has already been populated.
                    if (m_grid[Mathf.RoundToInt(m_tempVec.x / 40) + 4, 3 - Mathf.RoundToInt(m_tempVec.z / 40)] != 1)
                    {
                        /// Updates the UI with an unknown tile to the north.
                        m_map.transform.GetChild((40 - (Mathf.RoundToInt(m_tempVec.z / 40) * 9) + (Mathf.RoundToInt(m_tempVec.x / 40))) - 9).GetComponent<Image>().sprite = m_imgs[16];
                    }
                }
                /// Checks if the room has a east connection.
                if (m_tempTile.name[1] == '1')
                {
                    /// Checks if the UI square to the east has already been populated.
                    if (m_grid[Mathf.RoundToInt(m_tempVec.x / 40) + 5, 4 - Mathf.RoundToInt(m_tempVec.z / 40)] != 1)
                    {
                        /// Updates the UI with an unknown tile to the east.
                        m_map.transform.GetChild((40 - (Mathf.RoundToInt(m_tempVec.z / 40) * 9) + (Mathf.RoundToInt(m_tempVec.x / 40))) + 1).GetComponent<Image>().sprite = m_imgs[15];
                    }
                }
                /// Checks if the room has a south connection.
                if (m_tempTile.name[2] == '1')
                {
                    /// Checks if the UI square to the south has already been populated.
                    if (m_grid[Mathf.RoundToInt(m_tempVec.x / 40) + 4, 5 - Mathf.RoundToInt(m_tempVec.z / 40)] != 1)
                    {
                        /// Updates the UI with an unknown tile to the south.
                        m_map.transform.GetChild((40 - (Mathf.RoundToInt(m_tempVec.z / 40) * 9) + (Mathf.RoundToInt(m_tempVec.x / 40))) + 9).GetComponent<Image>().sprite = m_imgs[17];
                    }
                }
                /// Checks if the room has a west connection.
                if (m_tempTile.name[3] == '1')
                {
                    /// Checks if the UI square to the west has already been populated.
                    if (m_grid[3 + Mathf.RoundToInt(m_tempVec.x / 40), 4 - Mathf.RoundToInt(m_tempVec.z / 40)] != 1)
                    {
                        /// Updates the UI with an unknown tile to the west.
                        m_map.transform.GetChild((40 - (Mathf.RoundToInt(m_tempVec.z / 40) * 9) + (Mathf.RoundToInt(m_tempVec.x / 40))) - 1).GetComponent<Image>().sprite = m_imgs[18];
                    }
                }
            }
        }
    }
}
