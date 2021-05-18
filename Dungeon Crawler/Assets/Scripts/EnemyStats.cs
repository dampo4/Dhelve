using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{


    private Animator m_Animator;
    /// <summary>
    /// Awake is called when a script is  loaded for the first time.
    /// </summary>
    private void Awake()
    {
        CalculateStats();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        this.GetComponent<Rigidbody>().AddForce(transform.forward * -10, ForceMode.Impulse);
        //Play audio clip
        Debug.Log("OOF");
    }
    /// <summary>
    /// Calculates the stats of the enemy. Can be ignored and set in the editor for special cases/mobs, otherwise standard formula will take precedence.
    /// </summary>
    public override void CalculateStats()
    {
        //10 is the base value for these three variables. That is to say that at level 1, these are your levels. If m_isLinked is set to true, enemies will follow this pattern. Untick m_isLinked for unique, or to set up enemies manually.

        if (m_isLinked)
        {
            m_movementSpeed.SetValue(10 + m_level);
            m_vitality.SetValue(10 + m_level);
            m_strength.SetValue(5 + m_level);

            m_maxHealth = m_vitality.GetValue() * (3);

            m_damage.SetValue(m_strength.GetValue() * (2));

            m_currentInsanity = 5;

            m_gold.SetValue(m_level);
            m_experience.SetValue(m_level);
        }
    }

    /// <summary>
    /// The  enemy death event. //TODO Add an animation/shader to show death, instead of destroying the gameobject.
    /// </summary>
    public override void Die()
    {
        base.Die();
        Vector3 pos = gameObject.transform.position;
        pos.y += gameObject.GetComponentInChildren<SkinnedMeshRenderer>().bounds.size.y / 2;
        gameObject.GetComponent<DropItem>().Drop(pos, gameObject);
    }

}
