using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private GameObject m_weapon;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private CharacterStats m_myStats = null;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Animator m_playerAnimator = null;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private ParticleSystem m_bloodSystem = null;
    private CharacterStats m_enemyStats;
    private ItemBreak m_itemBreak;
    private Collider m_collider;
    public bool m_isAttacking = false;

    [SerializeField] private List<AudioSource> m_Sounds;
    private void Start()
    {
        m_collider = m_weapon.GetComponent<Collider>();
        m_collider.enabled = false;
        
    
    }
    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        Attack();
    }

    /// <summary>
    /// If the object connects with a gameobject, activates this
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
       
        ///Checks if the player is currently attacking, so the player cant just walk into an enemy
        if (m_playerAnimator.GetBool("Attack2") == true || m_playerAnimator.GetBool("Attack1") == true)
        {
            ///Checks if the gameobject is in fact an enemy
            if (other.tag == "Enemy")
            {
                ///Checks if the enemy has an enemystats script - Redundancy
                if (other.gameObject.GetComponent<EnemyStats>() != null)
                {
                    m_enemyStats = other.gameObject.GetComponent<EnemyStats>();
                    m_Sounds[1].Play();
                     m_enemyStats.TakeDamage((int)m_myStats.GetDamage() + (int)m_myStats.m_currentInsanity);
                    //other.gameObject.GetComponent<Rigidbody>().AddForce((transform.forward) * 500);
                    ///Checks if the enemy just died
                  if (m_enemyStats.m_currentHealth <= 0)
                    {
                        ///Gives gold and experience on kill
                        m_myStats.m_gold.SetValue(m_myStats.m_gold.GetValue() + m_enemyStats.m_gold.GetValue());
                        m_myStats.m_experience.SetValue(m_myStats.m_experience.GetValue() + m_enemyStats.m_experience.GetValue());
                        PlayerPrefs.SetInt("Gold", m_myStats.m_gold.GetValue());
                        Debug.Log("Got gold and experience!");
                    }
                  ///Creates a blood particle  effect
                    ParticleSystem Clone = Instantiate(m_bloodSystem, other.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
                    Destroy(Clone.gameObject, 1.0f);
                }
                else
                {
                    Debug.LogWarning("Enemy does not have an enemystats script");
                }
            }
            /// Checks if the gameobject is a breakable object
            if (other.tag == "Breakable")
            {
                ///Checks if the object has an ItemBreak script - Redundancy
                if (other.gameObject.GetComponent<ItemBreak>() != null)
                {
                    m_itemBreak = other.gameObject.GetComponent<ItemBreak>();

                    m_itemBreak.TakeDamage();
                }
                else
                {
                    Debug.LogWarning("Object does not have an ItemBreak script");
                }
            }
        }
    }

    /// <summary>
    /// Allows the player to attack by pressing the Left Mouse Button
    /// </summary>
    private void Attack()
    {
        //Main attack - Slash
        if (Input.GetButtonUp("Fire1"))
        {
            if (m_playerAnimator.GetBool("Attack1") == false && m_playerAnimator.GetBool("Attack2") == false)
            {
                m_collider.enabled = true;
                m_playerAnimator.SetBool("Attack1", true);
                m_Sounds[0].Play();
                StartCoroutine(AttackCooldown("Attack1", 0.5f));
            }
        }

        //Alt attack - Stab
        //if (Input.GetButtonUp("Fire2"))
        //{
        //    if (m_playerAnimator.GetBool("Attack1") == false && m_playerAnimator.GetBool("Attack2") == false)
        //    {
        //        m_collider.enabled = true;
        //        m_playerAnimator.SetBool("Attack2", true);
        //        StartCoroutine(AttackCooldown("Attack2", 0.5f));
        //    }
        //}
    }
    /// <summary>
    /// After a set delay, re-enable attack
    /// </summary>
    IEnumerator AttackCooldown(string animName, float time)
    {

        yield return new WaitForSeconds(time);
        m_playerAnimator.SetBool(animName, false);
        m_collider.enabled = false;
    }

}
