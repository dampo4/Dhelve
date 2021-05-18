using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class DungeonGen : MonoBehaviour
{
    /// <summary>
    /// A reference to the navmeshsurface.
    /// </summary>
    [SerializeField] private NavMeshSurface m_surface = null;
    /// <summary>
    /// An array of game tiles consisting of every possible tile orientation.
    /// </summary>
    public GameObject[] m_mapTiles;
    public GameObject m_leave;
    /// <summary>
    /// The object which will trigger the end of the level.
    /// </summary>
    public GameObject m_end;
    /// <summary>
    /// List of tiles with a connection in the specified direction.
    /// </summary>
    private List<List<GameObject>> m_connectionList = new List<List<GameObject>>();
    private List<GameObject> m_northConnection = new List<GameObject>();
    private List<GameObject> m_eastConnection = new List<GameObject>();
    private List<GameObject> m_southConnection = new List<GameObject>();
    private List<GameObject> m_westConnection = new List<GameObject>();
    /// <summary>
    /// Where the player spawns.
    /// </summary>
    public tile m_startTile;
    /// <summary>
    /// The asset for the tile the player starts on.
    /// </summary>
    private GameObject m_start;
    /// <summary>
    /// Tiles which are only accessible via the specified direction.
    /// </summary>
    public GameObject m_justNorth;
    public GameObject m_justEast;
    public GameObject m_justSouth;
    public GameObject m_justWest;
    /// <summary>
    /// The minimum size of the dungeon.
    /// </summary>
    public int m_length;
    /// <summary>
    /// The temporary tile which will be added to the list of all tiles.
    /// </summary>
    private tile m_temp;
    /// <summary>
    /// The temporary game object version of the tile to be added.
    /// </summary>
    private GameObject m_tempTile;
    /// <summary>
    /// The temporary position of the tile to be added.
    /// </summary>
    private Vector3 m_tempPos;
    /// <summary>
    /// Specifies the maximum dimensions of the map.
    /// </summary>
    private int[,] m_map;
    /// <summary>
    /// The width of the tiles.
    /// </summary>
    private float width;
    /// <summary>
    /// The list of all tiles as they are added to the map.
    /// </summary>
    private List<tile> m_tileList = new List<tile>();
    /// <summary>
    /// A 2D grid specifying which future tiles will connect to a room.
    /// </summary>
    private tile[,] m_claimed;
    /// <summary>
    /// The list of tiles to be added to the map after each iteration.
    /// </summary>
    private List<tile> m_tempTileList = new List<tile>();
    /// <summary>
    /// A list of possible tiles to add which fit certain conditions. A tile will be randomly selected from this list.
    /// </summary>
    private List<GameObject> m_TempPossibleTileList = new List<GameObject>();
    /// <summary>
    /// A reference to the player GameObject that will be instantiated to the start position.
    /// </summary>
    [SerializeField] private GameObject m_player = null;

    // Start is called before the first frame update
    public void Start()
    {
        /// Sorts all the tiles into lists specifying which connections they have.
        StoreTiles();        
        width = m_northConnection[0].GetComponentInChildren<Renderer>().bounds.size.x;
        Debug.Log(width);
        /// Bool to make sure the program only adds tiles while new tiles can be added.
        bool changed = true;
        /// Loops until the dungeon has at least the minimum number of rooms.
        do {
            /// Clears the map.
            foreach (GameObject o in GameObject.FindGameObjectsWithTag("Tile"))
            {
                Destroy(o);
            }
            foreach (GameObject e in GameObject.FindGameObjectsWithTag("Enemy")) 
            {
                Destroy(e);
            }

            foreach  (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
            {
                Destroy(p);
            }
            foreach (GameObject p in GameObject.FindGameObjectsWithTag("Breakable"))
            {
                Destroy(p);
            }
            /// Resets all variables.
            m_tileList.Clear();
            m_tempTileList.Clear();
            m_connectionList.Clear();
            m_northConnection.Clear();
            m_eastConnection.Clear();
            m_southConnection.Clear();
            m_westConnection.Clear();
            m_map = new int[50, 50];
            m_claimed = new tile[50, 50];
            StoreTiles();
            /// Generates a start tile.
            StartTile();
            changed = true;
            /// Loops while the map size is less than the minimum number of desired tiles.
            while (m_tileList.Count < m_length && changed)
                {
                    /// No tile has been changed yet.
                    changed = false;
                    /// Loops through every tile in the current game.
                    foreach (tile itile in m_tileList)
                    {
                        /// Checks if every available exit of a room is connected to another room.
                        if (!itile.full)
                        {
                            /// Connects tiles to the room if it has exits which lead nowhere.
                            LoadSurroundingTiles(itile);
                            /// Tile has been added so loop again.
                            changed = true;
                        }
                    }
                    /// Adds the tiles which were just added to the list of all tiles.
                    foreach (tile itile in m_tempTileList)
                    {
                        m_tileList.Add(itile);
                    }
                    /// Resets the list of temp tiles.
                    m_tempTileList.Clear();
                }
        }while (m_tileList.Count < m_length);
        changed = true;
        while (changed)
        {
            changed = false;
            foreach (tile itile in m_tileList)
            {
                /// Checks if every available exit of a room is connected to another room.
                if (!itile.full)
                {
                    /// Adds dead end rooms to every available unconnected room left.
                    FinishTiles(itile);
                    /// Tile has been added so loop again.
                    changed = true;
                }
            }
        }
        Instantiate(m_leave, new Vector3(m_startTile.worldPosition.x, m_startTile.worldPosition.y + (m_end.GetComponent<Renderer>().bounds.size.y / 2), m_startTile.worldPosition.z-2), Quaternion.identity);
        /// Generates the end location in the last tile spawned.
        Instantiate(m_end,new Vector3(m_tileList[m_tileList.Count - 1].worldPosition.x, m_tileList[m_tileList.Count - 1].worldPosition.y + (m_end.GetComponent<Renderer>().bounds.size.y / 2), m_tileList[m_tileList.Count - 1].worldPosition.z), Quaternion.identity);
        //m_player.transform.position = new Vector3(m_startTile.worldPosition.x, (m_startTile.worldPosition.y + m_player.GetComponent<CharacterController>().bounds.size.y), m_startTile.worldPosition.z);
        m_surface.BuildNavMesh();

        Instantiate(m_player, new Vector3(m_start.transform.position.x, m_start.transform.position.y + 6, m_start.transform.position.z), Quaternion.identity);
        Debug.Log(m_player.GetComponent<Collider>().bounds.size.y);
    }
    /// <summary>
    /// Spawns a start tile and adds the tile to the list of tiles in the map. Also updates the 2D map arrays with info about the start tile.
    /// </summary>
    private void StartTile()
    {
        /// Selects a random tile from the list of all tiles.
        m_start = m_mapTiles[Random.Range(0, m_mapTiles.Length - 1)];
        /// Creates it as a tile.
        m_startTile = new tile(m_start, 25, 25, new Vector3(0, 0, 0), m_start.name);
        /// Adds the start tile to the list of tiles in the game.
        m_tileList.Add(m_startTile);
        /// The start tile is created in the centre of the 'map'.
        m_map[25, 25] = 1;
        /// Claims the surrounding accessible tiles for the room passed in.
        UpdateMap(m_startTile);
        /// Creates the start tile at the world origin.
        Instantiate(m_tileList[Random.Range(0, m_tileList.Count - 1)].type, m_tileList[0].worldPosition, m_start.transform.rotation);
    }
    /// <summary>
    /// Adds tiles which meet specific conditions to every available exit of the tile passed in.
    /// </summary>
    /// <param name="currentTile">The tile which needs rooms added to its exits.</param>
    private void LoadSurroundingTiles(tile currentTile)
    {
        /// Checks if the room has an exit to the north and whether instantiating a tile to the north would leave the map boundaries.
        if (currentTile.config[0] == '1' && m_claimed[currentTile.mapX,currentTile.mapY - 1] == currentTile && m_map[currentTile.mapX, currentTile.mapY - 1] != 1)
        {
            /// Checks if there are no rooms which would border the newly instantiated room.
            if (m_claimed[currentTile.mapX - 1, currentTile.mapY - 1] == null && m_map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && m_claimed[currentTile.mapX, currentTile.mapY - 2] == null && m_map[currentTile.mapX, currentTile.mapY - 2] == 0 && m_claimed[currentTile.mapX + 1, currentTile.mapY - 1] == null && m_map[currentTile.mapX + 1, currentTile.mapY - 1] == 0)
            {
                /// Selects a random room with a connection to the south.
                m_tempTile = m_connectionList[2][Random.Range(0, m_connectionList[2].Count - 1)];
                /// Positions the tile correctly.
                m_tempPos = new Vector3(currentTile.worldPosition.x, currentTile.worldPosition.y, currentTile.worldPosition.z + width);
                /// Spawns the new tile.
                Instantiate(m_tempTile, m_tempPos, m_tempTile.transform.rotation);
                
                /// Creates the tile to be added.
                m_temp = new tile(m_tempTile, currentTile.mapX, currentTile.mapY - 1, m_tempPos, m_tempTile.name);
                /// Updates the map to say that a tile now exists in the current coordinate
                m_map[m_temp.mapX, m_temp.mapY] = 1;
                /// Claims the surrounding accessible tiles for the room passed in.
                UpdateMap(m_temp);
                /// Adds the newly created tile to the array of temp tiles.
                m_tempTileList.Add(m_temp);
            }
            else
            {
                if ((m_claimed[currentTile.mapX - 1, currentTile.mapY - 1] != null || m_map[currentTile.mapX - 1, currentTile.mapY - 1] == 1) && m_claimed[currentTile.mapX, currentTile.mapY - 2] == null && m_map[currentTile.mapX, currentTile.mapY - 2] == 0 && m_claimed[currentTile.mapX + 1, currentTile.mapY - 1] == null && m_map[currentTile.mapX + 1, currentTile.mapY - 1] == 0)
                {
                    foreach (GameObject itile in m_connectionList[2])
                    {
                        if (itile.name[3] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (m_claimed[currentTile.mapX - 1, currentTile.mapY - 1] == null && m_map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && (m_claimed[currentTile.mapX, currentTile.mapY - 2] != null || m_map[currentTile.mapX, currentTile.mapY - 2] == 1) && m_claimed[currentTile.mapX + 1, currentTile.mapY - 1] == null && m_map[currentTile.mapX + 1, currentTile.mapY - 1] == 0)
                {
                    foreach (GameObject itile in m_connectionList[2])
                    {
                        if (itile.name[0] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (m_claimed[currentTile.mapX - 1, currentTile.mapY - 1] == null && m_map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && m_claimed[currentTile.mapX, currentTile.mapY - 2] == null && m_map[currentTile.mapX, currentTile.mapY - 2] == 0 && (m_claimed[currentTile.mapX + 1, currentTile.mapY - 1] != null || m_map[currentTile.mapX + 1, currentTile.mapY - 1] == 1))
                {
                    foreach (GameObject itile in m_connectionList[2])
                    {
                        if (itile.name[1] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((m_claimed[currentTile.mapX - 1, currentTile.mapY - 1] != null || m_map[currentTile.mapX - 1, currentTile.mapY - 1] == 1) && (m_claimed[currentTile.mapX, currentTile.mapY - 2] != null || m_map[currentTile.mapX, currentTile.mapY - 2] == 1) && m_claimed[currentTile.mapX + 1, currentTile.mapY - 1] == null && m_map[currentTile.mapX + 1, currentTile.mapY - 1] == 0)
                {
                    foreach (GameObject itile in m_connectionList[2])
                    {
                        if (itile.name[0] != '1' && itile.name[3] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((m_claimed[currentTile.mapX - 1, currentTile.mapY - 1] != null || m_map[currentTile.mapX - 1, currentTile.mapY - 1] == 1) && m_claimed[currentTile.mapX, currentTile.mapY - 2] == null && m_map[currentTile.mapX, currentTile.mapY - 2] == 0 && (m_claimed[currentTile.mapX + 1, currentTile.mapY - 1] != null || m_map[currentTile.mapX + 1, currentTile.mapY - 1] == 1))
                {
                    foreach (GameObject itile in m_connectionList[2])
                    {
                        if (itile.name[1] != '1' && itile.name[3] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (m_claimed[currentTile.mapX - 1, currentTile.mapY - 1] == null && m_map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && (m_claimed[currentTile.mapX, currentTile.mapY - 2] != null || m_map[currentTile.mapX, currentTile.mapY - 2] == 1) && (m_claimed[currentTile.mapX + 1, currentTile.mapY - 1] != null || m_map[currentTile.mapX + 1, currentTile.mapY - 1] == 1))
                {
                    foreach (GameObject itile in m_connectionList[2])
                    {
                        if (itile.name[0] != '1' && itile.name[1] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((m_claimed[currentTile.mapX - 1, currentTile.mapY - 1] != null || m_map[currentTile.mapX - 1, currentTile.mapY - 1] == 1) && (m_claimed[currentTile.mapX, currentTile.mapY - 2] != null || m_map[currentTile.mapX, currentTile.mapY - 2] == 1) && (m_claimed[currentTile.mapX + 1, currentTile.mapY - 1] != null || m_map[currentTile.mapX + 1, currentTile.mapY - 1] == 1))
                {
                    foreach (GameObject itile in m_connectionList[2])
                    {
                        if (itile.name[0] != '1' && itile.name[1] != '1' && itile.name[3] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                Debug.Log(m_TempPossibleTileList.Count);
                m_tempTile = m_TempPossibleTileList[Random.Range(0, m_TempPossibleTileList.Count - 1)];
                m_tempPos = new Vector3(currentTile.worldPosition.x, currentTile.worldPosition.y, currentTile.worldPosition.z + width);
                Instantiate(m_tempTile, m_tempPos, m_tempTile.transform.rotation);
                m_temp = new tile(m_tempTile, currentTile.mapX, currentTile.mapY - 1, m_tempPos, m_tempTile.name);
                m_map[m_temp.mapX, m_temp.mapY] = 1;
                UpdateMap(m_temp);
                m_tempTileList.Add(m_temp);
                m_TempPossibleTileList.Clear();
            }

        }
        /// Checks if the room has an exit to the east and whether instantiating a tile to the north would leave the map boundaries.
        if (currentTile.config[1] == '1' && m_claimed[currentTile.mapX + 1, currentTile.mapY] == currentTile && m_map[currentTile.mapX + 1, currentTile.mapY] != 1)
        {
            /// Checks if there are no rooms which would border the newly instantiated room.
            if (m_claimed[currentTile.mapX + 1, currentTile.mapY - 1] == null && m_map[currentTile.mapX + 1, currentTile.mapY - 1] == 0 && m_claimed[currentTile.mapX + 2, currentTile.mapY] == null && m_map[currentTile.mapX + 2, currentTile.mapY] == 0 && m_claimed[currentTile.mapX + 1, currentTile.mapY + 1] == null && m_map[currentTile.mapX + 1, currentTile.mapY + 1] == 0)
            {
                /// Selects a random room with a connection to the west.
                m_tempTile = m_connectionList[3][Random.Range(0, m_connectionList[3].Count - 1)];
                /// Positions the tile correctly.
                m_tempPos = new Vector3(currentTile.worldPosition.x + width, currentTile.worldPosition.y, currentTile.worldPosition.z);
                /// Spawns the new tile.
                Instantiate(m_tempTile, m_tempPos, m_tempTile.transform.rotation);
                /// Creates the tile to be added.
                m_temp = new tile(m_tempTile, currentTile.mapX + 1, currentTile.mapY, m_tempPos, m_tempTile.name);
                /// Updates the map to say that a tile now exists in the current coordinate
                m_map[m_temp.mapX, m_temp.mapY] = 1;
                /// Claims the surrounding accessible tiles for the room passed in.
                UpdateMap(m_temp);
                /// Adds the newly created tile to the array of temp tiles.
                m_tempTileList.Add(m_temp);
            }
            else
            {
                /// Loops through every possible arrangement of surrounding rooms of the tile to be instantiated which would block a connection.
                /// It then randomly selects a tile to be added which fits the conditions.
                if ((m_claimed[currentTile.mapX + 1, currentTile.mapY - 1] != null || m_map[currentTile.mapX + 1, currentTile.mapY - 1] == 1) && m_claimed[currentTile.mapX + 2, currentTile.mapY] == null && m_map[currentTile.mapX + 2, currentTile.mapY] == 0 && m_claimed[currentTile.mapX + 1, currentTile.mapY + 1] == null && m_map[currentTile.mapX + 1, currentTile.mapY + 1] == 0)
                {
                    Debug.Log("Gothere");
                    foreach (GameObject itile in m_connectionList[3])
                    {
                        if (itile.name[0] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (m_claimed[currentTile.mapX + 1, currentTile.mapY - 1] == null && m_map[currentTile.mapX + 1, currentTile.mapY - 1] == 0 && (m_claimed[currentTile.mapX + 2, currentTile.mapY] != null || m_map[currentTile.mapX + 2, currentTile.mapY] == 1) && m_claimed[currentTile.mapX + 1, currentTile.mapY + 1] == null && m_map[currentTile.mapX + 1, currentTile.mapY + 1] == 0)
                {
                    Debug.Log("Gothere");
                    foreach (GameObject itile in m_connectionList[3])
                    {
                        if (itile.name[1] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (m_claimed[currentTile.mapX + 1, currentTile.mapY - 1] == null && m_map[currentTile.mapX + 1, currentTile.mapY - 1] == 0 && m_claimed[currentTile.mapX + 2, currentTile.mapY] == null && m_map[currentTile.mapX + 2, currentTile.mapY] == 0 && (m_claimed[currentTile.mapX + 1, currentTile.mapY + 1] != null || m_map[currentTile.mapX + 1, currentTile.mapY + 1] == 1))
                {
                    Debug.Log("Gothere");
                    foreach (GameObject itile in m_connectionList[3])
                    {
                        if (itile.name[2] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((m_claimed[currentTile.mapX + 1, currentTile.mapY - 1] != null || m_map[currentTile.mapX + 1, currentTile.mapY - 1] == 1) && (m_claimed[currentTile.mapX + 2, currentTile.mapY] != null || m_map[currentTile.mapX + 2, currentTile.mapY] == 1) && m_claimed[currentTile.mapX + 1, currentTile.mapY + 1] == null && m_map[currentTile.mapX + 1, currentTile.mapY + 1] == 0)
                {
                    Debug.Log("Gothere");
                    foreach (GameObject itile in m_connectionList[3])
                    {
                        if (itile.name[0] != '1' && itile.name[1] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((m_claimed[currentTile.mapX + 1, currentTile.mapY - 1] != null || m_map[currentTile.mapX + 1, currentTile.mapY - 1] == 1) && m_claimed[currentTile.mapX + 2, currentTile.mapY] == null && m_map[currentTile.mapX + 2, currentTile.mapY] == 0 && (m_claimed[currentTile.mapX + 1, currentTile.mapY + 1] != null || m_map[currentTile.mapX + 1, currentTile.mapY + 1] == 1))
                {
                    Debug.Log("Gothere");
                    foreach (GameObject itile in m_connectionList[3])
                    {
                        if (itile.name[0] != '1' && itile.name[2] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (m_claimed[currentTile.mapX + 1, currentTile.mapY - 1] == null && m_map[currentTile.mapX + 1, currentTile.mapY - 1] == 0 && (m_claimed[currentTile.mapX + 2, currentTile.mapY] != null || m_map[currentTile.mapX + 2, currentTile.mapY] == 1) && (m_claimed[currentTile.mapX + 1, currentTile.mapY + 1] != null || m_map[currentTile.mapX + 1, currentTile.mapY + 1] == 1))
                {
                    Debug.Log("Gothere");
                    foreach (GameObject itile in m_connectionList[3])
                    {
                        if (itile.name[1] != '1' && itile.name[2] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((m_claimed[currentTile.mapX + 1, currentTile.mapY - 1] != null || m_map[currentTile.mapX + 1, currentTile.mapY - 1] == 1) && (m_claimed[currentTile.mapX + 2, currentTile.mapY] != null || m_map[currentTile.mapX + 2, currentTile.mapY] == 1) && (m_claimed[currentTile.mapX + 1, currentTile.mapY + 1] != null || m_map[currentTile.mapX + 1, currentTile.mapY + 1] == 1))
                {
                    Debug.Log("Gothere");
                    foreach (GameObject itile in m_connectionList[3])
                    {
                        if (itile.name[0] != '1' && itile.name[1] != '1' && itile.name[2] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                Debug.Log(m_TempPossibleTileList.Count);
                m_tempTile = m_TempPossibleTileList[Random.Range(0, m_TempPossibleTileList.Count - 1)];
                m_tempPos = new Vector3(currentTile.worldPosition.x + width, currentTile.worldPosition.y, currentTile.worldPosition.z);
                Instantiate(m_tempTile, m_tempPos, m_tempTile.transform.rotation);
                m_temp = new tile(m_tempTile, currentTile.mapX + 1, currentTile.mapY, m_tempPos, m_tempTile.name);
                m_map[m_temp.mapX, m_temp.mapY] = 1;
                UpdateMap(m_temp);
                m_tempTileList.Add(m_temp);
                m_TempPossibleTileList.Clear();
            }

        }
        /// Checks if the room has an exit to the south and whether instantiating a tile to the north would leave the map boundaries.
        if (currentTile.config[2] == '1' && m_claimed[currentTile.mapX, currentTile.mapY + 1] == currentTile && m_map[currentTile.mapX, currentTile.mapY + 1] != 1)
        {
            /// Checks if there are no rooms which would border the newly instantiated room.
            if (m_claimed[currentTile.mapX + 1, currentTile.mapY + 1] == null && m_map[currentTile.mapX + 1, currentTile.mapY + 1] == 0 && m_claimed[currentTile.mapX, currentTile.mapY + 2] == null && m_map[currentTile.mapX, currentTile.mapY + 2] == 0 && m_claimed[currentTile.mapX - 1, currentTile.mapY + 1] == null && m_map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
            {
                /// Selects a random room with a connection to the north.
                m_tempTile = m_connectionList[0][Random.Range(0, m_connectionList[0].Count - 1)];
                /// Positions the tile correctly.
                m_tempPos = new Vector3(currentTile.worldPosition.x, currentTile.worldPosition.y, currentTile.worldPosition.z - width);
                /// Spawns the new tile.
                Instantiate(m_tempTile, m_tempPos, m_tempTile.transform.rotation);
                
                /// Creates the tile to be added.
                m_temp = new tile(m_tempTile, currentTile.mapX, currentTile.mapY + 1, m_tempPos, m_tempTile.name);
                /// Updates the map to say that a tile now exists in the current coordinate
                m_map[m_temp.mapX, m_temp.mapY] = 1;
                /// Claims the surrounding accessible tiles for the room passed in.
                UpdateMap(m_temp);
                /// Adds the newly created tile to the array of temp tiles.
                m_tempTileList.Add(m_temp);
            }
            else
            {
                /// Loops through every possible arrangement of surrounding rooms of the tile to be instantiated which would block a connection.
                /// It then randomly selects a tile to be added which fits the conditions.
                if ((m_claimed[currentTile.mapX + 1, currentTile.mapY + 1] != null || m_map[currentTile.mapX + 1, currentTile.mapY + 1] == 1) && m_claimed[currentTile.mapX, currentTile.mapY + 2] == null && m_map[currentTile.mapX, currentTile.mapY + 2] == 0 && m_claimed[currentTile.mapX - 1, currentTile.mapY + 1] == null && m_map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in m_connectionList[0])
                    {
                        if (itile.name[1] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (m_claimed[currentTile.mapX + 1, currentTile.mapY + 1] == null && m_map[currentTile.mapX + 1, currentTile.mapY + 1] == 0 && (m_claimed[currentTile.mapX, currentTile.mapY + 2] != null || m_map[currentTile.mapX, currentTile.mapY + 2] == 1) && m_claimed[currentTile.mapX - 1, currentTile.mapY + 1] == null && m_map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in m_connectionList[0])
                    {
                        if (itile.name[2] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (m_claimed[currentTile.mapX + 1, currentTile.mapY + 1] == null && m_map[currentTile.mapX + 1, currentTile.mapY + 1] == 0 && m_claimed[currentTile.mapX, currentTile.mapY + 2] == null && m_map[currentTile.mapX, currentTile.mapY + 2] == 0 && (m_claimed[currentTile.mapX - 1, currentTile.mapY + 1] != null || m_map[currentTile.mapX - 1, currentTile.mapY + 1] == 1))
                {
                    foreach (GameObject itile in m_connectionList[0])
                    {
                        if (itile.name[3] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((m_claimed[currentTile.mapX + 1, currentTile.mapY + 1] != null || m_map[currentTile.mapX + 1, currentTile.mapY + 1] == 1) && (m_claimed[currentTile.mapX, currentTile.mapY + 2] != null || m_map[currentTile.mapX, currentTile.mapY + 2] == 1) && m_claimed[currentTile.mapX - 1, currentTile.mapY + 1] == null && m_map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in m_connectionList[0])
                    {
                        if (itile.name[1] != '1' && itile.name[2] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((m_claimed[currentTile.mapX + 1, currentTile.mapY + 1] != null || m_map[currentTile.mapX + 1, currentTile.mapY + 1] == 1) && m_claimed[currentTile.mapX, currentTile.mapY + 2] == null && m_map[currentTile.mapX, currentTile.mapY + 2] == 0 && (m_claimed[currentTile.mapX - 1, currentTile.mapY + 1] != null || m_map[currentTile.mapX - 1, currentTile.mapY + 1] == 1))
                {
                    foreach (GameObject itile in m_connectionList[0])
                    {
                        if (itile.name[1] != '1' && itile.name[3] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (m_claimed[currentTile.mapX + 1, currentTile.mapY + 1] == null && m_map[currentTile.mapX + 1, currentTile.mapY + 1] == 0 && (m_claimed[currentTile.mapX, currentTile.mapY + 2] != null || m_map[currentTile.mapX, currentTile.mapY + 2] == 1) && (m_claimed[currentTile.mapX - 1, currentTile.mapY + 1] != null || m_map[currentTile.mapX - 1, currentTile.mapY + 1] == 1))
                {
                    foreach (GameObject itile in m_connectionList[0])
                    {
                        if (itile.name[2] != '1' && itile.name[3] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((m_claimed[currentTile.mapX + 1, currentTile.mapY + 1] != null || m_map[currentTile.mapX + 1, currentTile.mapY + 1] == 1) && (m_claimed[currentTile.mapX, currentTile.mapY + 2] != null || m_map[currentTile.mapX, currentTile.mapY + 2] == 1) && (m_claimed[currentTile.mapX - 1, currentTile.mapY + 1] != null || m_map[currentTile.mapX - 1, currentTile.mapY + 1] == 1))
                {
                    foreach (GameObject itile in m_connectionList[0])
                    {
                        if (itile.name[1] != '1' && itile.name[2] != '1' && itile.name[3] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                Debug.Log(m_TempPossibleTileList.Count);
                m_tempTile = m_TempPossibleTileList[Random.Range(0, m_TempPossibleTileList.Count - 1)];
                m_tempPos = new Vector3(currentTile.worldPosition.x, currentTile.worldPosition.y, currentTile.worldPosition.z - width);
                Instantiate(m_tempTile, m_tempPos, m_tempTile.transform.rotation);
                m_temp = new tile(m_tempTile, currentTile.mapX, currentTile.mapY + 1, m_tempPos, m_tempTile.name);
                m_map[m_temp.mapX, m_temp.mapY] = 1;
                m_tempTileList.Add(m_temp);
                UpdateMap(m_temp);
                m_TempPossibleTileList.Clear();
            }
        }
        /// Checks if the room has an exit to the west and whether instantiating a tile to the north would leave the map boundaries.
        if (currentTile.config[3] == '1' && m_claimed[currentTile.mapX - 1, currentTile.mapY] == currentTile && m_map[currentTile.mapX - 1, currentTile.mapY] != 1)
        {
            /// Checks if there are no rooms which would border the newly instantiated room.
            if (m_claimed[currentTile.mapX - 1, currentTile.mapY - 1] == null && m_map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && m_claimed[currentTile.mapX - 2, currentTile.mapY] == null && m_map[currentTile.mapX - 2, currentTile.mapY] == 0 && m_claimed[currentTile.mapX - 1, currentTile.mapY + 1] == null && m_map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
            {
                /// Selects a random room with a connection to the east.
                m_tempTile = m_connectionList[1][Random.Range(0, m_connectionList[1].Count - 1)];
                /// Positions the tile correctly.
                m_tempPos = new Vector3(currentTile.worldPosition.x - width, currentTile.worldPosition.y, currentTile.worldPosition.z);
                /// Spawns the new tile.
                Instantiate(m_tempTile, m_tempPos, m_tempTile.transform.rotation);
                /// Creates the tile to be added.
                m_temp = new tile(m_tempTile, currentTile.mapX - 1, currentTile.mapY, m_tempPos, m_tempTile.name);
                /// Updates the map to say that a tile now exists in the current coordinate
                m_map[m_temp.mapX, m_temp.mapY] = 1;
                /// Claims the surrounding accessible tiles for the room passed in.
                UpdateMap(m_temp);
                /// Adds the newly created tile to the array of temp tiles.
                m_tempTileList.Add(m_temp);
            }
            else
            {
                /// Loops through every possible arrangement of surrounding rooms of the tile to be instantiated which would block a connection.
                /// It then randomly selects a tile to be added which fits the conditions.
                if ((m_claimed[currentTile.mapX - 1, currentTile.mapY - 1] != null || m_map[currentTile.mapX - 1, currentTile.mapY - 1] == 1) && m_claimed[currentTile.mapX - 2, currentTile.mapY] == null && m_map[currentTile.mapX - 2, currentTile.mapY] == 0 && m_claimed[currentTile.mapX - 1, currentTile.mapY + 1] == null && m_map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in m_connectionList[1])
                    {
                        if (itile.name[0] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (m_claimed[currentTile.mapX - 1, currentTile.mapY - 1] == null && m_map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && (m_claimed[currentTile.mapX - 2, currentTile.mapY] != null || m_map[currentTile.mapX - 2, currentTile.mapY] == 1) && m_claimed[currentTile.mapX - 1, currentTile.mapY + 1] == null && m_map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in m_connectionList[1])
                    {
                        if (itile.name[3] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (m_claimed[currentTile.mapX - 1, currentTile.mapY - 1] == null && m_map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && m_claimed[currentTile.mapX - 2, currentTile.mapY] == null && m_map[currentTile.mapX - 2, currentTile.mapY] == 0 && (m_claimed[currentTile.mapX - 1, currentTile.mapY + 1] != null || m_map[currentTile.mapX - 1, currentTile.mapY + 1] == 1))
                {
                    foreach (GameObject itile in m_connectionList[1])
                    {
                        if (itile.name[2] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((m_claimed[currentTile.mapX - 1, currentTile.mapY - 1] != null || m_map[currentTile.mapX - 1, currentTile.mapY - 1] == 1) && (m_claimed[currentTile.mapX - 2, currentTile.mapY] != null || m_map[currentTile.mapX - 2, currentTile.mapY] == 1) && m_claimed[currentTile.mapX - 1, currentTile.mapY + 1] == null && m_map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in m_connectionList[1])
                    {
                        if (itile.name[0] != '1' && itile.name[3] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((m_claimed[currentTile.mapX - 1, currentTile.mapY - 1] != null || m_map[currentTile.mapX - 1, currentTile.mapY - 1] == 1) && m_claimed[currentTile.mapX - 2, currentTile.mapY] == null && m_map[currentTile.mapX - 2, currentTile.mapY] == 0 && (m_claimed[currentTile.mapX - 1, currentTile.mapY + 1] != null || m_map[currentTile.mapX - 1, currentTile.mapY + 1] == 1))
                {
                    foreach (GameObject itile in m_connectionList[1])
                    {
                        if (itile.name[0] != '1' && itile.name[2] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (m_claimed[currentTile.mapX - 1, currentTile.mapY - 1] == null && m_map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && (m_claimed[currentTile.mapX - 2, currentTile.mapY] != null || m_map[currentTile.mapX - 2, currentTile.mapY] == 1) && (m_claimed[currentTile.mapX - 1, currentTile.mapY + 1] != null || m_map[currentTile.mapX - 1, currentTile.mapY + 1] == 1))
                {
                    foreach (GameObject itile in m_connectionList[1])
                    {
                        if (itile.name[3] != '1' && itile.name[2] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((m_claimed[currentTile.mapX - 1, currentTile.mapY - 1] != null || m_map[currentTile.mapX - 1, currentTile.mapY - 1] == 1) && (m_claimed[currentTile.mapX - 2, currentTile.mapY] != null || m_map[currentTile.mapX - 2, currentTile.mapY] == 1) && (m_claimed[currentTile.mapX - 1, currentTile.mapY + 1] != null || m_map[currentTile.mapX - 1, currentTile.mapY + 1] == 1))
                {
                    foreach (GameObject itile in m_connectionList[1])
                    {
                        if (itile.name[0] != '1' && itile.name[3] != '1' && itile.name[2] != '1')
                        {
                            m_TempPossibleTileList.Add(itile);
                        }
                    }
                }
                Debug.Log(m_TempPossibleTileList.Count);
                m_tempTile = m_TempPossibleTileList[Random.Range(0, m_TempPossibleTileList.Count - 1)];
                m_tempPos = new Vector3(currentTile.worldPosition.x - width, currentTile.worldPosition.y, currentTile.worldPosition.z);
                Instantiate(m_tempTile, m_tempPos, m_tempTile.transform.rotation);
                m_temp = new tile(m_tempTile, currentTile.mapX - 1, currentTile.mapY, m_tempPos, m_tempTile.name);
                m_map[m_temp.mapX, m_temp.mapY] = 1;
                m_tempTileList.Add(m_temp);
                UpdateMap(m_temp);
                m_TempPossibleTileList.Clear();
            }
        }
        /// After every possible situation is checked, the current tile is marked as full.
        currentTile.full = true;
    }
    /// <summary>
    /// Runs on the final iteration. Adds dead end rooms to any room which still has exits with no joining tile.
    /// </summary>
    /// <param name="currentTile">The tile which is being checked to see if it has unboardered exits.</param>
    private void FinishTiles(tile currentTile)
    {
        /// Checks if the tile has a north exit.
        if (currentTile.config[0] == '1' && m_map[currentTile.mapX,currentTile.mapY - 1] != 1)
        {
            /// Adds a dead end tile with a south connection.
            m_tempTile = m_justSouth;
            /// Positions the tile correctly.
            m_tempPos = new Vector3(currentTile.worldPosition.x, currentTile.worldPosition.y, currentTile.worldPosition.z + width);
            /// Spawns the new tile.
            Instantiate(m_tempTile, m_tempPos, m_tempTile.transform.rotation);
            m_temp = new tile(m_tempTile, currentTile.mapX, currentTile.mapY - 1, m_tempPos, m_tempTile.name);
        }
        /// Checks if the tile has a east exit.
        if (currentTile.config[1] == '1' && m_map[currentTile.mapX + 1, currentTile.mapY] != 1)
        {
            /// Adds a dead end tile with a west connection.
            m_tempTile = m_justWest;
            /// Positions the tile correctly.
            m_tempPos = new Vector3(currentTile.worldPosition.x + width, currentTile.worldPosition.y, currentTile.worldPosition.z);
            /// Spawns the new tile.
            Instantiate(m_tempTile, m_tempPos, m_tempTile.transform.rotation);
            m_temp = new tile(m_tempTile, currentTile.mapX + 1, currentTile.mapY, m_tempPos, m_tempTile.name);
        }
        /// Checks if the tile has a south exit.
        if (currentTile.config[2] == '1' && m_map[currentTile.mapX, currentTile.mapY + 1] != 1)
        {
            /// Adds a dead end tile with a north connection.
            m_tempTile = m_justNorth;
            /// Positions the tile correctly.
            m_tempPos = new Vector3(currentTile.worldPosition.x, currentTile.worldPosition.y, currentTile.worldPosition.z - width);
            /// Spawns the new tile.
            Instantiate(m_tempTile, m_tempPos, m_tempTile.transform.rotation);
            m_temp = new tile(m_tempTile, currentTile.mapX, currentTile.mapY + 1, m_tempPos, m_tempTile.name);
        }
        /// Checks if the tile has a west exit.
        if (currentTile.config[3] == '1' && m_map[currentTile.mapX - 1, currentTile.mapY ] != 1)
        {
            /// Adds a dead end tile with a east connection.
            m_tempTile = m_justEast;
            /// Positions the tile correctly.
            m_tempPos = new Vector3(currentTile.worldPosition.x - width, currentTile.worldPosition.y, currentTile.worldPosition.z);
            /// Spawns the new tile.
            Instantiate(m_tempTile, m_tempPos, m_tempTile.transform.rotation);
            m_temp = new tile(m_tempTile, currentTile.mapX - 1, currentTile.mapY, m_tempPos, m_tempTile.name);
        }
        /// After every possible situation is checked, the current tile is marked as full.
        currentTile.full = true;
    }
    /// <summary>
    /// Claims the surrounding accessible tiles for the room passed in.
    /// </summary>
    /// <param name="currentTile"> The room which will claim the surrounding accessible tiles.</param>
    private void UpdateMap(tile currentTile)
    {
        /// Checks if the tile to the north is accessible and whether it has already been claimed.
        if (currentTile.config[0] == '1' && m_claimed[currentTile.mapX, currentTile.mapY - 1] == null && m_map[currentTile.mapX, currentTile.mapY - 1] != 1)
        {
            m_claimed[currentTile.mapX, currentTile.mapY - 1] = currentTile;
        }
        /// Checks if the tile to the east is accessible and whether it has already been claimed.
        if (currentTile.config[1] == '1' && m_claimed[currentTile.mapX + 1, currentTile.mapY] == null && m_map[currentTile.mapX + 1, currentTile.mapY] != 1)
        {
            m_claimed[currentTile.mapX + 1, currentTile.mapY] = currentTile;
        }
        /// Checks if the tile to the south is accessible and whether it has already been claimed.
        if (currentTile.config[2] == '1' && m_claimed[currentTile.mapX, currentTile.mapY + 1] == null && m_map[currentTile.mapX, currentTile.mapY + 1] != 1)
        {
            m_claimed[currentTile.mapX, currentTile.mapY + 1] = currentTile;
        }
        /// Checks if the tile to the west is accessible and whether it has already been claimed.
        if (currentTile.config[3] == '1' && m_claimed[currentTile.mapX - 1, currentTile.mapY] == null && m_map[currentTile.mapX - 1, currentTile.mapY] != 1)
        {
            m_claimed[currentTile.mapX - 1, currentTile.mapY] = currentTile;
        }
    }
    /// <summary>
    /// Sorts the tiles by their binary representation of exits.
    /// </summary>
    private void StoreTiles()
    {
        foreach (GameObject section in m_mapTiles)
        {
            /// Checks if the tile has an exit to the north.
            if (section.name[0] == '1')
            {
                /// Adds the tile to the list of tiles which have a north exit.
                m_northConnection.Add(section);
            }
            /// Checks if the tile has an exit to the east.
            if (section.name[1] == '1')
            {
                /// Adds the tile to the list of tiles which have a east exit.
                m_eastConnection.Add(section);
            }
            /// Checks if the tile has an exit to the south.
            if (section.name[2] == '1')
            {
                /// Adds the tile to the list of tiles which have a south exit.
                m_southConnection.Add(section);
            }
            /// Checks if the tile has an exit to the west.
            if (section.name[3] == '1')
            {
                /// Adds the tile to the list of tiles which have a west exit.
                m_westConnection.Add(section);
            }
            /// Adds the list of north connections to the list of all collections.
            m_connectionList.Add(m_northConnection);
            /// Adds the list of east connections to the list of all collections.
            m_connectionList.Add(m_eastConnection);
            /// Adds the list of south connections to the list of all collections.
            m_connectionList.Add(m_southConnection);
            /// Adds the list of west connections to the list of all collections.
            m_connectionList.Add(m_westConnection);
        }
    }
    /// <summary>
    /// A tile made up of a visual asset, position, configuration of exits and position in the 2D array of the game map.
    /// </summary>
    public class tile
    {
        /// <summary>
        /// The tile prefab the tile consists of.
        /// </summary>
        public GameObject type;
        /// <summary>
        /// Where the tile is positioned in the game.
        /// </summary>
        public Vector3 worldPosition;
        /// <summary>
        /// binary representation of the configuration of exits the room has.
        /// </summary>
        public string config;
        /// <summary>
        /// The x coordinate of the position the tile has in the 2D array of the game map.
        /// </summary>
        public int mapX;
        /// <summary>
        /// The y coordinate of the position the tile has in the 2D array of the game map.
        /// </summary>
        public int mapY;
        /// <summary>
        /// Whether the room has exits which aren't connected.
        /// </summary>
        public bool full;
        public tile()
        {

        }
        public tile(GameObject typ, int X, int Y, Vector3 pos, string con)
        {
            type = typ;
            mapX = X;
            mapY = Y;
            worldPosition = pos;
            config = con;
            full = false;
        }
    }
}
