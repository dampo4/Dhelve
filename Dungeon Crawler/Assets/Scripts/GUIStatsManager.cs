using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GUIStatsManager : MonoBehaviour
{
    /// <summary>
    /// A slider variable denoting the health slider UI element.
    /// </summary>
    [SerializeField] private Image m_healthImg = null;
    /// <summary>
    /// A slider variable denoting the corruption slider UI element.
    /// </summary>
    [SerializeField] private Image m_corruptionImg = null;
    /// <summary>
    /// A CharacterStats  variable denoting the entitites stats class.
    /// </summary>
    private PlayerStats m_charStats = null;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private GameObject m_StatsScreen;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private TextMeshProUGUI m_StrengthTxt;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private TextMeshProUGUI m_VitalityTxt;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private TextMeshProUGUI m_DamageTxt;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private TextMeshProUGUI m_ArmourTxt;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private TextMeshProUGUI m_AttackSpeedTxt;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private TextMeshProUGUI m_goldTxt;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private TextMeshProUGUI m_playerLevel;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private TextMeshProUGUI m_playerName;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Slider m_playerExperience;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private TextMeshProUGUI m_playerExperienceTxt;
    // Start is called before the first frame update
    void Start()
    {
        //Sanity check - Make sure stats screen is disabled.
        m_StatsScreen.SetActive(false);

        //Sanity check - Make sure player name is checked by default
        m_playerName.text = "Player Name";
    }

    
    /// <summary>
    /// Called once per frame, calls the UpdateGUI method.
    /// </summary>
    void Update()
    {
        UpdateGUI();

        CheckForInput();
    }
    /// <summary>
    /// Updates all GUI elements.
    /// </summary>
    void UpdateGUI()
    {
        if (m_charStats == null)
        {
            m_charStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        }
            ///Get max values of GUI elements
            float healthMax = m_charStats.m_maxHealth;
            float corMax = m_charStats.m_maxInsanity.GetValue();
        
        ///Divide current stats by max stats to get stat/1
        float currentHealth = m_charStats.m_currentHealth / healthMax;
        float currentCorruption = m_charStats.m_currentInsanity / corMax;

        ///Update UI elements
        m_healthImg.fillAmount = currentHealth;
        m_corruptionImg.fillAmount = currentCorruption;

        m_StrengthTxt.text = m_charStats.GetStrength().ToString();
        m_VitalityTxt.text = m_charStats.GetVitality().ToString();
        m_DamageTxt.text = m_charStats.GetDamage().ToString();
        m_AttackSpeedTxt.text = m_charStats.GetAttackSpeed().ToString();
        m_ArmourTxt.text = m_charStats.GetArmour().ToString();

        m_goldTxt.text = m_charStats.GetGold().ToString();

        //Set max experience
        m_playerExperience.maxValue = m_charStats.GetExperienceToLevel();

        //Set value to current experience
        m_playerExperience.value = m_charStats.m_experience.GetValue();

        //Set value to current level
        m_playerLevel.text = "Player Level: " + m_charStats.m_level.ToString();
        m_playerExperienceTxt.text = "XP: " + m_charStats.m_experience.GetValue().ToString() + " / " + m_charStats.GetExperienceToLevel();
    }

    /// <summary>
    /// 
    /// </summary>
    void CheckForInput()
    {
        if (Input.GetButtonDown("StatsToggle"))
        {
            //Debug test
            Debug.Log("Stats screen toggled");
            //Toggle screen 
            if (!m_StatsScreen.activeInHierarchy)
            {
                Time.timeScale = 0.0f;
                m_StatsScreen.SetActive(true);
            }
            else
            {
                Time.timeScale = 1.0f;
                m_StatsScreen.SetActive(false);
            }
        }
    }
}
