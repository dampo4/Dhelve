using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourPotion : Potion
{
    private void Start()
    {
        m_itemCooldown = 5;
    }
    public override void ActivateItem()
    {
        base.ActivateItem();
        Debug.Log(m_isAvailable);
    }
    /// <summary>
    /// An override for the ActivateEffect method in the Item class. Activates armour potion specific code,
    /// I.E a coroutine which sets the players armour to a higher value for a set amount of time.
    /// </summary>
    /// 
    public override void ActivateEffect()
    {
            StartCoroutine("ArmourUp");
    }
    /// <summary>
    /// A coroutine that sets the players armour to a higher value for a set amount of time.
    /// </summary>
    /// <returns></returns>
    IEnumerator ArmourUp()
    {
        CharacterStats myStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        int originalArmour = (int)myStats.GetArmour();
        myStats.SetArmour((int)(myStats.GetArmour() * m_potency));
        yield return new WaitForSeconds(m_potionTimer);
        myStats.SetArmour(originalArmour);
        Debug.Log(m_isAvailable);
    }
}
