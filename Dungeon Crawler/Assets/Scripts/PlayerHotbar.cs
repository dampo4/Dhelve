using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerHotbar : MonoBehaviour
{

    /// <summary>
    /// A reference to the hotbar items. 
    /// </summary>
    public GameObject[] m_hotBarItems = new GameObject[4];
    /// <summary>
    /// A reference to the hotbar icons. 
    /// </summary>
    public Image[] m_hotBarIcons = new Image[4];
    public Sprite[] m_sprites = new Sprite[3];
    public Sprite empty;
    private void Start()
    {
        LoadPlayerHotbar();
    }
    // Update is called once per frame
    void Update()
    {
        
        CheckForInput();
    }
    /// <summary>
    /// Checks for input, I.E whether the project defined inputs for hotbar slots have been pressed down. If so, activates whatever is in that slot.
    /// </summary>
    void CheckForInput()
    {
        if (Input.GetButtonDown("Hotbar Slot 1"))
        {
            if (m_hotBarItems[0] != null)
            {
                ActivateEffect(m_hotBarItems[0]);
                m_hotBarItems[0] = null;
                m_hotBarIcons[0].sprite = empty;
                SaveSystem.SavePlayer(this);
            }
        }
        else if (Input.GetButtonDown("Hotbar Slot 2"))
        {
            if (m_hotBarItems[1] != null)
            {
                ActivateEffect(m_hotBarItems[1]);
                m_hotBarItems[1] = null;
                m_hotBarIcons[1].sprite = empty;
                SaveSystem.SavePlayer(this);
            }
        }
        else if (Input.GetButtonDown("Hotbar Slot 3"))
        {
            if (m_hotBarItems[2] != null)
            {
                ActivateEffect(m_hotBarItems[2]);
                m_hotBarItems[2] = null;
                m_hotBarIcons[2].sprite = empty;
                SaveSystem.SavePlayer(this);
            }
        }
        else if (Input.GetButtonDown("Hotbar Slot 4"))
        {
            if (m_hotBarItems[3] != null)
            {
                ActivateEffect(m_hotBarItems[3]);
                m_hotBarItems[3] = null;
                m_hotBarIcons[3].sprite = empty;
                SaveSystem.SavePlayer(this);
            }
        }
    }
    /// <summary>
    /// Activates the effect of the item in the slot.
    /// </summary>
    /// <param name="item"></param>
    private void ActivateEffect(GameObject item)
    {
        GameObject tempItem = Resources.Load(item.name, typeof(GameObject)) as GameObject;
        tempItem = Instantiate(tempItem);
        tempItem.GetComponent<Renderer>().enabled = false;
        tempItem.GetComponent<Collider>().enabled = false;
        tempItem.GetComponent<Item>().ActivateItem();        
    }

    private void LoadPlayerHotbar()
    {
        PlayerData data = SaveSystem.LoadPlayerHotbar();
        for (int i = 0; i < data.m_hotbarItems.Length; i++)
        {
            if (data.m_hotbarItems[i] != null)
            {
                m_hotBarItems[i] = Resources.Load(data.m_hotbarItems[i], typeof(GameObject)) as GameObject;
                if (m_hotBarItems[i].name == "Armour Potion")
                {
                    m_hotBarIcons[i].sprite = m_sprites[0];
                }
                else if (m_hotBarItems[i].name == "Health Potion")
                {
                    m_hotBarIcons[i].sprite = m_sprites[1];
                }
                else if (m_hotBarItems[i].name == "Strength Potion")
                {
                    m_hotBarIcons[i].sprite = m_sprites[2];
                }
                //m_hotBarIcons[i].sprite = m_hotBarItems[i].GetComponent<Item>().m_hotBarIcon;
                
            }
        }
    }
}
