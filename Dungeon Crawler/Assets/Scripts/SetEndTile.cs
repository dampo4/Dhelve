using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEndTile : MonoBehaviour
{
    /// <summary>
    /// A vector denoting the direction of the raycast.
    /// </summary>
    private Vector3 m_direction;
    /// <summary>
    /// A float value denoting the distance the raycast will check.
    /// </summary>
    [SerializeField] private float m_maxDistance = 0;
    void Start()
    {
        /// Used to set the raycast to be facing downwards.
        m_direction = transform.TransformDirection(Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, m_direction, out hit, m_maxDistance))
        {
            /// Sets the tile the end is on to include 'end' in its name.
            hit.collider.gameObject.transform.parent.name += "end";
        }
    }
}
