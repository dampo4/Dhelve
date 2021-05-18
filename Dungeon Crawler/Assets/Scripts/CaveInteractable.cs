using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveInteractable : Interactable
{
    /// <summary>
    /// An override for the InteractedWith method - Loads the dungeon scene.
    /// </summary>
    public override void InteractedWith()
    {

        SceneManager.LoadScene("Dungeon");
    }
}
