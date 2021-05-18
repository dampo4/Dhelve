using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : CharacterMovement
{
    /// <summary>
    /// A variable to store the look radius of a character. Within this radius, the character will search for the target.
    /// </summary>
    [SerializeField] private float m_lookRadius = 0;
    /// <summary>
    /// The target the current character is attempting to follow.
    /// </summary>
    [SerializeField] private GameObject m_target = null;
    /// <summary>
    /// A float value to denote when the character last attacked/
    /// </summary>
    private float m_lastAttacked;
    /// <summary>
    /// A reference to the NavMeshAgent GameObject.
    /// </summary>
    private NavMeshAgent m_navMeshAgent;
    /// <summary>
    /// A reference to the Animator Component
    /// </summary>
    private Animator m_Animator;
    /// <summary>
    /// attackSpeed is an integer derived from the enemyStats script.
    /// </summary>
    private int m_attackSpeed = 0;
    /// <summary>
    /// A reference to the current GameObjects EnemyStats script, used to get variable values.
    /// </summary>
    CharacterStats m_myStats;
   
    /// <summary>
    /// A boolean to see if the model can attack
    /// </summary>
    private bool m_CanAttack = true;
    /// <summary>
    /// In this method variables are initialised.
    /// </summary>
    void Start()
    {
       
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_myStats = GetComponent<EnemyStats>();
        m_Animator = GetComponent<Animator>();
        m_attackSpeed = (int)m_myStats.GetAttackSpeed();
        m_target = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Update occurs during every frame, so input is taken using the Move method every frame.
    /// </summary>
    public void Update()
    {
        Move();
    }
    /// <summary>
    /// Moves the character if the distance from the target is less than the look radius. Uses navmesh to avoid obstacles.
    /// </summary>
    public override void Move()
    {
        ///A variable denoting the distance between the current character and its target.
        float distance = Vector3.Distance(m_target.transform.position, transform.position);
        if (distance <= m_lookRadius)
        {
            m_Animator.SetBool("Walking", true);
            m_navMeshAgent.SetDestination(m_target.transform.position);
            if (distance <= m_navMeshAgent.stoppingDistance)
            {
                ///Do attack
                Attack();
                m_Animator.SetBool("Walking", false);
                FaceTarget();
            }
            ///This is reset when the player leaves stopping distance so the enemy instantly attacks whenever the player moves. This may not be required when actual combat is implemented due to knockback, but is a reasonable failsafe.
            else
            {
             
                m_lastAttacked = m_attackSpeed;
                m_Animator.SetBool("Attacking", false);
            }
        }
        else
        {
            m_Animator.SetBool("Walking", false);
        }
    }


    void FaceTarget()
    {
        Vector3 direction = (m_target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    /// <summary>
    /// Draws a wireframe sphere  around the character within the editor to easily see its look radius.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_lookRadius);
    }

    /// <summary>
    /// Attacks the target when within the stopping distance of the character. 
    /// </summary>
    private void Attack()
    {

        /// A reference to the target GameObjects PlayerStats script, used to get variable values.
        CharacterStats targetStats = m_target.GetComponent<PlayerStats>();

        if (m_lastAttacked >= m_attackSpeed)
        {
            ///Clamps the value of the armour between 0 and the value of the targets armour
            targetStats.TakeDamage((int)(m_myStats.GetDamage() + targetStats.m_currentInsanity - (Mathf.Clamp(m_myStats.GetArmour(), 0, m_myStats.GetArmour()))));
            m_lastAttacked = 0;
            //Do attack animation
                m_Animator.SetBool("Attacking", true);
                m_CanAttack = false;
            Invoke("ResetAttack", 1f);

        }
            //Debug.Log("Attacked - " + m_lastAttacked);
        
        else
        {
            m_lastAttacked += Time.deltaTime;
            m_Animator.SetBool("Attacking", false);
            //Debug.Log("Did not attack - " + m_lastAttacked);
        }
    }
    private void ResetAttack()
    {
        m_CanAttack = true;
        m_Animator.SetBool("Attacking", false);
    }
}
