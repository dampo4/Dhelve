                                        using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    /// <summary>
    /// A vector denoting the direction of the raycast.
    /// </summary>
    private Vector3 m_direction;
    /// <summary>
    /// A float value denoting the distance the raycast will check.
    /// </summary>
    [SerializeField] private float m_maxDistance = 0;

    /// <summary>
    /// Checks in a direction for  an interactable object.
    /// </summary>
    private void Update()
    {
        ///This is being checked during FixedUpdate as without this, the forward direction of the player is only counted at start time.
        m_direction = transform.TransformDirection(Vector3.forward);
        ///Used to get information back using the raycast.
        RaycastHit hit;

        ///If the raycast hits something within the max distance in the direction of m_direction
        if (Physics.Raycast(transform.position, m_direction, out hit, m_maxDistance))
        {
            ///Check if the hit object has an interactable script
            if(hit.collider.gameObject.GetComponent<Interactable>() != null)
            {
                ///If it does and the player hits interact
                if (Input.GetButtonDown("Interact"))
                {
                    ///Call the interactedwith method associated with the gameobject.
                    hit.collider.gameObject.GetComponent<Interactable>().InteractedWith();
                }
            }
            
        }
    }
    /// <summary>
    /// Draws a wireframe line from the character to easily see its 
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position,new Vector3(0,0,m_maxDistance));
    }
}
