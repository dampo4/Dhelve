using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    /// <summary>
    /// A float value denoting the duration of a potion.
    /// </summary>
    public float m_potionTimer;
    /// <summary>
    /// The potency, I.E the strength of a potion.
    /// </summary>
    public float m_potency;

    /// <summary>
    /// An override for the Item classes ActivateEffect.
    /// </summary>
    public override void ActivateEffect()
    {
        Debug.Log("Base potion activated.");
    }

}
