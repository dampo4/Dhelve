using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstructionDetector : MonoBehaviour
{
    /// <summary>
    /// A variable to store a reference to the player.
    /// </summary>
    [SerializeField] private GameObject player = null;
    /// <summary>
    /// A variable to store the last obstruction.
    /// </summary>
    private Obstructable m_lastObstructable;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DetectPlayerObstruction());
    }
    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    /// <summary>
    /// Detects whether a player is obstructed and sets the transparency of that object.
    /// </summary>
    /// <returns></returns>
    IEnumerator DetectPlayerObstruction()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            ///A vector containing the direction of the raycast.
            Vector3 direction = (player.transform.position - Camera.main.transform.position).normalized;
            ///A raycast used to get information about the collision.
            RaycastHit m_rayCastHit;

            if (Physics.Raycast(Camera.main.transform.position, direction, out m_rayCastHit, Mathf.Infinity))
            {
                ///The object causing an obstruction.
                Obstructable obstruction = m_rayCastHit.collider.gameObject.GetComponent<Obstructable>();
                if (obstruction)
                {
                    obstruction.SetTransparent();
                    m_lastObstructable = obstruction;
                }
                else
                {
                    if (m_lastObstructable != null)
                    {
                        m_lastObstructable.SetNormal();
                    }
                }
            }
        }
    }
}
