using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    /// <summary>
    /// The rigidbody associated with this GameObject.
    /// </summary>
    private Rigidbody m_rb;
    /// <summary>
    /// The speed the GameObject travels at.
    /// </summary>
    [SerializeField] private float m_speed = 15;
    /// <summary>
    /// A float variable denoting the damage this GameObject deals.
    /// </summary>
    [SerializeField] private int m_damage = 5;
    /// <summary>
    /// The lifetime of the object.
    /// </summary>
    [SerializeField] private int m_lifeTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        Destroy(this.gameObject, m_lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        m_rb.velocity = new Vector3(0,0,m_speed);
    }

    /// <summary>
    /// Activates when the object collides with something.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerStats m_targetStats = other.GetComponent<PlayerStats>();
            m_targetStats.TakeDamage(m_damage);
            Destroy(this.gameObject);
        }

    }

    public int GetLifetime()
    {
        return m_lifeTime;
    }
}
