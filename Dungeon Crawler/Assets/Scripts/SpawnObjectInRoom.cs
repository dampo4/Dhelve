using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectInRoom : MonoBehaviour
{
    /// <summary>
    /// Enemy asset.
    /// </summary>
    public GameObject m_enemy;
    /// <summary>
    /// Percentage chance of enemy spawning.
    /// </summary>
    [Range(0.0f, 100.0f)]
    [SerializeField] private float m_ChanceOfEnemySpawn = 0;
    /// <summary>
    /// Percentage chance of first barrel spawning.
    /// </summary>
    [Range(0.0f, 100.0f)]
    [SerializeField] private float m_ChanceOfBarrelSpawn = 0;
    /// <summary>
    /// Percentage chance of second barrel spawning.
    /// </summary>
    [Range(0.0f, 100.0f)]
    [SerializeField] private float m_ChanceOfSecondBarrelSpawn = 0;
    /// <summary>
    /// Percentage chance of third barrel spawning.
    /// </summary>
    [Range(0.0f, 100.0f)]
    [SerializeField] private float m_ChanceOfThirdBarrelSpawn = 0;
    /// <summary>
    /// the width of the tile.
    /// </summary>
    private float m_tileWidth;
    /// <summary>
    /// The width of the enemy.
    /// </summary>
    private float m_enemyWidth;
    /// <summary>
    /// The barrel to be spawned.
    /// </summary>
    public GameObject m_barrel;
    /// <summary>
    /// the barrel width.
    /// </summary>
    private float m_barrelWidth;
    /// <summary>
    /// The barrel height.
    /// </summary>
    private float m_barrelHeight;
    // Start is called before the first frame update
    void Start()
    {
        /// variable used to track which quadrant has barrels spawned in it. This is later used to prevent additional enemies from spawning in the same quadrant.
        int hasBarrel = -1;
        m_tileWidth = gameObject.GetComponentInChildren<Renderer>().bounds.size.x;
        m_enemyWidth = m_enemy.GetComponentInChildren<SkinnedMeshRenderer>().bounds.size.x;
        m_barrelWidth = m_barrel.GetComponentInChildren<Renderer>().bounds.size.x;
        m_barrelHeight = m_barrel.GetComponentInChildren<Renderer>().bounds.size.y;
        float halfTileWidth = m_tileWidth / 2;
        float halfEnemyWidth = m_enemyWidth / 2;
        /// Gets the coordinate position of the tile the script is attached to.
        Vector3 centre = gameObject.transform.position;
        if (Random.Range(0, 100) < m_ChanceOfBarrelSpawn)
        {
            /// Picks a random quadrant to spawn barrels in.
            int quadrant = Random.Range(0, 3);
            if (quadrant == 0)
            {
                hasBarrel = 0;
                Vector3 tempPos = new Vector3((centre.x + halfTileWidth) - (m_tileWidth/13), centre.y + m_barrelHeight / 2, (centre.z + halfTileWidth) - (m_tileWidth / 13));
                Instantiate(m_barrel, tempPos, Quaternion.identity);
                if (Random.Range(0, 100) < m_ChanceOfSecondBarrelSpawn)
                {
                    Vector3 newTempPos = tempPos;
                    newTempPos.x -= m_barrelWidth;
                    Instantiate(m_barrel, newTempPos, Quaternion.identity);
                }
                if (Random.Range(0, 100) < m_ChanceOfThirdBarrelSpawn)
                {
                    Vector3 newTempPos = tempPos;
                    newTempPos.z -= m_barrelWidth;
                    Instantiate(m_barrel, newTempPos, Quaternion.identity);
                }
            }
            if (quadrant == 1)
            {
                hasBarrel = 1;
                Vector3 tempPos = new Vector3((centre.x + halfTileWidth) - (m_tileWidth / 13), centre.y + m_barrelHeight / 2, (centre.z - halfTileWidth) + (m_tileWidth / 13));
                Instantiate(m_barrel, tempPos, Quaternion.identity);
                if (Random.Range(0, 100) < m_ChanceOfSecondBarrelSpawn)
                {
                    Vector3 newTempPos = tempPos;
                    newTempPos.x -= m_barrelWidth;
                    Instantiate(m_barrel, newTempPos, Quaternion.identity);
                }
                if (Random.Range(0, 100) < m_ChanceOfThirdBarrelSpawn)
                {
                    Vector3 newTempPos = tempPos;
                    newTempPos.z += m_barrelWidth;
                    Instantiate(m_barrel, newTempPos, Quaternion.identity);
                }
            }
            if (quadrant == 2)
            {
                hasBarrel = 2;
                Vector3 tempPos = new Vector3((centre.x - halfTileWidth) + (m_tileWidth / 13), centre.y + m_barrelHeight / 2, (centre.z - halfTileWidth) + (m_tileWidth / 13));
                Instantiate(m_barrel, tempPos, Quaternion.identity);
                if (Random.Range(0, 100) < m_ChanceOfSecondBarrelSpawn)
                {
                    Vector3 newTempPos = tempPos;
                    newTempPos.x += m_barrelWidth;
                    Instantiate(m_barrel, newTempPos, Quaternion.identity);
                }
                if (Random.Range(0, 100) < m_ChanceOfThirdBarrelSpawn)
                {
                    Vector3 newTempPos = tempPos;
                    newTempPos.z += m_barrelWidth;
                    Instantiate(m_barrel, newTempPos, Quaternion.identity);
                }
            }
            if (quadrant == 3)
            {
                hasBarrel = 3;
                Vector3 tempPos = new Vector3((centre.x - halfTileWidth) + (m_tileWidth / 13), centre.y + m_barrelHeight / 2, (centre.z + halfTileWidth) - (m_tileWidth / 13));
                Instantiate(m_barrel, tempPos, Quaternion.identity);
                if (Random.Range(0, 100) < m_ChanceOfSecondBarrelSpawn)
                {
                    Vector3 newTempPos = tempPos;
                    newTempPos.x += m_barrelWidth;
                    Instantiate(m_barrel, newTempPos, Quaternion.identity);
                }
                if (Random.Range(0, 100) < m_ChanceOfThirdBarrelSpawn)
                {
                    Vector3 newTempPos = tempPos;
                    newTempPos.z -= m_barrelWidth;
                    Instantiate(m_barrel, newTempPos, Quaternion.identity);
                }
            }

        }
        /// Calculates (based on probability) if an enemy will spawn.
        if (Random.Range(0, 100) < m_ChanceOfEnemySpawn && hasBarrel != 0)
        {
            /// Generates a random coordinate in first quadrant of the tile.
            Vector3 tempPos = new Vector3(centre.x + Random.Range(halfEnemyWidth + (m_tileWidth / 16), (halfTileWidth) - (halfEnemyWidth) - (m_tileWidth / 20)), centre.y, centre.z + Random.Range((halfEnemyWidth) + (m_tileWidth / 16), (halfTileWidth) - halfEnemyWidth - (m_tileWidth / 20)));
            /// Spawns enemy in a random point in first quadrant of the tile.
            Instantiate(m_enemy, tempPos, Quaternion.identity);
        }
        /// Calculates (based on probability) if an enemy will spawn.
        if (Random.Range(0, 100) < m_ChanceOfEnemySpawn && hasBarrel != 2)
        {
            /// Generates a random coordinate in first quadrant of the tile.
            Vector3 tempPos = new Vector3(centre.x - Random.Range(halfEnemyWidth + (m_tileWidth / 16), (halfTileWidth) - (halfEnemyWidth) - (m_tileWidth / 20)), centre.y, centre.z - Random.Range(halfEnemyWidth + (m_tileWidth / 16), (halfTileWidth) - (halfEnemyWidth) - (m_tileWidth / 20)));
            /// Spawns enemy in a random point in first quadrant of the tile.
            Instantiate(m_enemy, tempPos, Quaternion.identity);
        }
        /// Calculates (based on probability) if an enemy will spawn.
        if (Random.Range(0, 100) < m_ChanceOfEnemySpawn && hasBarrel != 3)
        {
            /// Generates a random coordinate in first quadrant of the tile.
            Vector3 tempPos = new Vector3(centre.x - Random.Range(halfEnemyWidth + (m_tileWidth / 16), (halfTileWidth) - (halfEnemyWidth) - (m_tileWidth / 20)), centre.y, centre.z + Random.Range(halfEnemyWidth + (m_tileWidth / 16), (halfTileWidth) - (halfEnemyWidth) - (m_tileWidth / 20)));
            /// Spawns enemy in a random point in first quadrant of the tile.
            Instantiate(m_enemy, tempPos, Quaternion.identity);
        }
        /// Calculates (based on probability) if an enemy will spawn.
        if (Random.Range(0, 100) < m_ChanceOfEnemySpawn && hasBarrel != 1)
        {
            /// Generates a random coordinate in first quadrant of the tile.
            Vector3 tempPos = new Vector3(centre.x + Random.Range(halfEnemyWidth + (m_tileWidth / 16), (halfTileWidth) -(halfEnemyWidth) - (m_tileWidth / 20)), centre.y, centre.z - Random.Range(halfEnemyWidth + (m_tileWidth / 16), (halfTileWidth) - (halfEnemyWidth) - (m_tileWidth / 20)));
            /// Spawns enemy in a random point in first quadrant of the tile.
            Instantiate(m_enemy, tempPos, Quaternion.identity);
        }
    }
}
