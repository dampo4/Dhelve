using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    /// <summary>
    /// List of possible items to drop.
    /// </summary>
    [SerializeField]
    private List<GameObject> itemList;
    /// <summary>
    /// Chance of an item actually dropping.
    /// </summary>
    [Range(0.0f, 100.0f)]
    [SerializeField] private float m_ChanceOfItemSpawn = 0;
    /// <summary>
    /// Drops an item if it passes the random chance check.
    /// </summary>
    /// <param name="position">The position of the object dropping the item</param>
    /// <param name="source">The object dropping the item</param>
    public void Drop(Vector3 position, GameObject source)
    {
        if (Random.Range(0, 100) < m_ChanceOfItemSpawn)
        {
            /// Randomly chooses an item to drop.
            int dropItem = Random.Range(0, itemList.Count);
            /// Moves the spawn position up so the item doesnt spawn in the ground.
            float moveUp = itemList[dropItem].GetComponent<Renderer>().bounds.size.y;
            position.y += moveUp;
            /// Spawns the item.
            Instantiate(itemList[dropItem], position, Quaternion.identity);
        }
    }
}
