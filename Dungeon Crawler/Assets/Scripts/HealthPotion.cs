using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Potion
{
    /// <summary>
    /// An override for the ActivateEffect method in Item class. Increases the players health by adding m_potency to current health, but clamps it to the maximum health value.
    /// </summary>
    public override void ActivateEffect()
    {
        CharacterStats myStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        myStats.m_currentHealth = Mathf.Clamp(myStats.m_currentHealth + (int)m_potency, 0, myStats.m_maxHealth);
    }

}
