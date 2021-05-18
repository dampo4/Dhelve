using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopDescriptionController : MonoBehaviour
{
    /// <summary>
    /// The item that the description is derived from.
    /// </summary>
    [SerializeField] private Item m_item;
    /// <summary>
    /// The GUI text that shows the items hover text.
    /// </summary>
    [SerializeField] private TextMeshProUGUI m_descriptionText;
    /// <summary>
    /// A GUI text showing the items cost.
    /// </summary>
   // [SerializeField] private TextMeshProUGUI m_itemCost;
    
    /// <summary>
    /// Occurs before the first update cycle.
    /// </summary>
    private void Start()
    {
        AddDescription(m_item);
    }
    /// <summary>
    /// Assigns the description of the item to the GUI text.
    /// </summary>
    /// <param name="item">Takes in an item as a parameter.</param>
    private void AddDescription(Item item)
    {
        m_descriptionText.text = item.m_hoverOverText +"\n" + "Potion Cooldown: " + item.m_itemCooldown + " seconds" + "\n" + "Cost: " + item.m_itemCost.ToString() + " Gold Pieces"; ;
    }
}
