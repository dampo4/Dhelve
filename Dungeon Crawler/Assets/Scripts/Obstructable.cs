using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstructable : MonoBehaviour
{
    /// <summary>
    /// A variable denoting the transparant colour of an obstruction.
    /// </summary>
    private Color m_transparentColour = Color.clear;
    /// <summary>
    /// A variable denoting the original colour of an obstruction.
    /// </summary>
    private Color m_originalColour;
    /// <summary>
    /// A variable to hold the renderer of an obstruction.
    /// </summary>
    private new Renderer renderer = null;
    /// <summary>
    /// A method which occurs when the application begins.
    /// </summary>
    private void Start()
    {
        renderer = GetComponent<Renderer>();
        m_originalColour = renderer.material.color;
    }
    /// <summary>
    /// Sets the obstructions colour to transparent.
    /// </summary>
    public void SetTransparent()
    {
        renderer.material.color = m_transparentColour;
    }
    /// <summary>
    /// Sets the obstructions colour to transparent.
    /// </summary>
    public void SetNormal()
    {
        renderer.material.color = m_originalColour;
    }
}
