using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTestScript : MonoBehaviour
{
    /// <summary>
    /// A reference to the Animator Component
    /// </summary>
    private Animator m_Animator;
    /// <summary>
    /// A boolean to see if the model can attack
    /// </summary>
    private bool m_CanAttack = true;

    // Start is called before the first frame update
    public void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {

        /// <summary>
        /// 
        /// 
        /// </summary>
        if (Input.GetKeyDown("space"))
        {
            if (m_CanAttack)
            {
                m_Animator.SetBool("Attacking", true);
                m_CanAttack = false;
                Invoke("resetAttack", 1);
                
            }
            
        }
        if (Input.GetKeyDown("left shift"))
        {
            m_Animator.SetBool("Walking", true);
            Debug.Log("Walking");
        }
        else if (Input.GetKeyUp("left shift"))
        {
            m_Animator.SetBool("Walking", false);
        }

        if (Input.GetKeyDown("left ctrl")){
            m_Animator.SetBool("Dead", true);
            Debug.Log("Dead");
        }
        else if (Input.GetKeyUp("left ctrl"))
        {
            m_Animator.SetBool("Dead", false);
            Debug.Log("NotDead");

        }
    }

    private void resetAttack()
    {
        m_CanAttack = true;
        m_Animator.SetBool("Attacking", false);
    }
}
