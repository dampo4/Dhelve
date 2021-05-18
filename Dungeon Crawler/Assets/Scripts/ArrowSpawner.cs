using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{

    [SerializeField] private GameObject m_Arrow;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnArrow", 5f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnArrow()
    {
        Instantiate(m_Arrow, this.transform.position, m_Arrow.transform.rotation);
        Debug.Log("spawned arrow");
    }
}
