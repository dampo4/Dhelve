using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopInteractable : Interactable
{
    [SerializeField] private GameObject m_ShopUI; 

    /// <summary>
    /// Overrides the original InteractedWith method.
    /// </summary>
    public override void InteractedWith()
    {
        ///Debugs Interacted with + Gameobject name.
        base.InteractedWith();
        m_ShopUI.SetActive(true);
    }
    /// <summary>
    /// Disables the shop GUI if the player leaves the vicinity of the shop.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LeaveShop();
        }
    }
    /// <summary>
    /// Disables the shop GUI.
    /// </summary>
    public void LeaveShop()
        {
            m_ShopUI.SetActive(false);
        }
    
}
