using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBreak : MonoBehaviour
{
    /// <summary>
    /// Objects take 2 hits to break.
    /// </summary>
    int m_health = 2;
    /// <summary>
    /// Reduces the item's health by 1 for every hit and possibly spawns an item.
    /// </summary>
    public void TakeDamage()
    {
        /// Reduces health by 1.
        m_health -= 1;
        /// Checks if the health is less than 0.
        if (m_health <= 0)
        {
            /// Calls the function to drop an item.
            gameObject.GetComponent<DropItem>().Drop(gameObject.transform.position, gameObject);
            /// Destroys the item.
            Destroy(gameObject);
        }
    }
}
