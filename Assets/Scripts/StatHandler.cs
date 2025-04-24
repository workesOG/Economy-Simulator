using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
    public static StatHandler instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public Bar moneyBar;
    public Bar mentalHealthBar;
    public Bar socialStandingBar;
    public Bar schoolPerformanceBar;

    private float m_money;
    public float Money
    {
        get { return m_money; }
        set
        {
            m_money = Math.Min(value, 100000);
            moneyBar.UpdateBar(Mathf.RoundToInt(value), 20000);
        }
    }

    private float m_moneyChange;
    public float MoneyChange
    {
        get { return m_moneyChange; }
        set
        {
            m_moneyChange = value;
        }
    }

    private List<Investment> m_investments;
    public List<Investment> Investments
    {
        get { return m_investments; }
        set
        {
            m_investments = value;
        }
    }

    private float m_mentalHealth;
    public float MentalHealth
    {
        get { return m_mentalHealth; }
        set
        {
            m_mentalHealth = Math.Min(value, 1000);
            mentalHealthBar.UpdateBar(Mathf.RoundToInt(value), 1000);
        }
    }

    private float m_mentalHealthChange;
    public float MentalHealthChange
    {
        get { return m_mentalHealthChange; }
        set
        {
            m_mentalHealthChange = value;
        }
    }

    private float m_socialStanding;
    public float SocialStanding
    {
        get { return m_socialStanding; }
        set
        {
            m_socialStanding = Math.Min(value, 1000);
            socialStandingBar.UpdateBar(Mathf.RoundToInt(value), 1000);
        }
    }

    private float m_socialStandingChange;
    public float SocialStandingChange
    {
        get { return m_socialStandingChange; }
        set
        {
            m_socialStandingChange = value;
        }
    }

    private float m_schoolPerformance;
    public float SchoolPerformance
    {
        get { return m_schoolPerformance; }
        set
        {
            m_schoolPerformance = Math.Min(value, 1000);
            schoolPerformanceBar.UpdateBar(Mathf.RoundToInt(value), 1000);
        }
    }

    private float m_schoolPerformanceChange;
    public float SchoolPerformanceChange
    {
        get { return m_schoolPerformanceChange; }
        set
        {
            m_schoolPerformanceChange = value;
        }
    }
}
