using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Item : MonoBehaviour
{
    /// <summary>
    /// A reference to a m_playerHotbar script.
    /// </summary>
    public PlayerHotbar m_playerHotbar;
    /// <summary>
    /// An integer value denoting how many of the item can be stored in a single slot in the hotbar.
    /// </summary>
    [SerializeField] private int m_maxNumberInSlot;
    /// <summary>
    /// A reference to an image variable, which is the image shown on the hotbar UI.
    /// </summary>
    public Sprite m_hotBarIcon;
    /// <summary>
    /// A string that stores hover over text, so the player can understand what different items are and do. Also used for the shop text.
    /// </summary>
    public string m_hoverOverText;
    /// <summary>
    /// A reference to the item cooldown. Items cannot be used again within this time.
    /// </summary>
    public float m_itemCooldown;
    /// <summary>
    /// A float denoting the cost of the item in the shop.
    /// </summary>
    public float m_itemCost;
    /// <summary>
    /// A boolean denoting whether the item is available to be used again.
    /// </summary>
    public bool m_isAvailable = true;

    private void Start()
    {
       
    }
    /// <summary>
    /// Occurs when an item interacts with this. Checks that the other GameObject is a player and if so, activeates the OnPickUp script.
    /// </summary>
    /// <param name="other">A reference to the other GameObjects collider.</param>
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Trigger entered.");
            OnPickUp();
        }
    }
    /// <summary>
    /// A method which handles the interaction between player and item - Adds it to their inventory, plays animation/sound or uses it. Meant to be overriden.
    /// </summary>
    public virtual void OnPickUp()
    {
        ///if the item is a clone, remove the (clone) from its name
        this.gameObject.name = this.gameObject.name.Replace("(Clone)", "").Trim();
        ///Get the player hotbar
        m_playerHotbar = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerHotbar>();
        ///Get the ItemDatabase
 
        ///Add to hotbar
        for (int i = 0; i < m_playerHotbar.m_hotBarItems.Length; i++)
        {
            if (m_playerHotbar.m_hotBarItems[i] == null)
            {
                m_playerHotbar.m_hotBarItems[i] = this.gameObject;
                m_playerHotbar.m_hotBarIcons[i].sprite = m_hotBarIcon;
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                var go = this.gameObject.GetComponents<Collider>();
                foreach (Collider col in go)
                {
                    col.enabled = false;
                }
                
                break;
            }
        }
      

       
    }

    /// <summary>
    /// A method that activates the effect of the item, that's always overriden by other items.
    /// </summary>
    public virtual void ActivateEffect()
    {
        Debug.Log("Item activated.");
    }

    /// <summary>
    /// Activates the item effect.
    /// </summary>
    public virtual void ActivateItem()
    {
        if (m_isAvailable)
        {
            m_isAvailable = false;
            StartCoroutine("ItemCooldown");
            ///Activate potion effect here
            ActivateEffect();

        }
    }

    /// <summary>
    /// Sets m_isAvailable to true after the items cooldown.
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator ItemCooldown()
    {
        yield return new WaitForSeconds(m_itemCooldown);
        m_isAvailable = true;
    }
}
