using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class enterCave : MonoBehaviour
{
    // Update is called once per frame
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Player")
        {
            SceneManager.LoadScene("Dungeon");
        }
    }
}
