using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// Created using Game Dev Guide's 'Creating a Custom Tab System in Unity https://www.youtube.com/watch?v=211t6r12XPQ
/// </summary>
[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    /// <summary>
    /// A reference to the tab group the button is a part of.
    /// </summary>
    [SerializeField] private TabGroup m_tabGroup;
    /// <summary>
    /// The background image of the button.
    /// </summary>
    public Image m_background;

    // Start is called before the first frame update - Subscribes the button to the tabgroup.
    void Start()
    {
        m_background = GetComponent<Image>();
        m_tabGroup.Subscribe(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// When a tab is clicked, calls the tabGroup OnTabSelected method.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        m_tabGroup.OnTabSelected(this);
    }
    /// <summary>
    /// When a tab is hovered over, calls the tabGroup OnTabEnter method.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        m_tabGroup.OnTabEnter(this);
    }
    /// <summary>
    /// When a tab is exited, resets all tabs.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        m_tabGroup.OnTabExit(this);
    }
}
