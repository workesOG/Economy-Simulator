using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;

    public Transform cardWindow;
    public TMP_Text weekNumber;
    public List<string> triggeredKeys = new List<string>();
    public HashSet<string> usedUniqueKeys = new HashSet<string>();

    private int m_turn;
    public int Turn
    {
        get { return m_turn; }
        set
        {
            m_turn = value;
            weekNumber.text = $"Week {value}";
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        BeginGame();
    }

    void BeginGame()
    {
        StatHandler statHandler = StatHandler.instance;
        statHandler.Money = Random.Range(750, 1251);
        statHandler.Investments = new List<Investment>();
        statHandler.MentalHealth = Random.Range(600, 700);
        statHandler.SocialStanding = Random.Range(600, 700);
        statHandler.SchoolPerformance = Random.Range(600, 700);
        statHandler.MoneyChange = 20;
        statHandler.MentalHealthChange = -10;
        statHandler.SocialStandingChange = -10;
        statHandler.SchoolPerformanceChange = -10;
        Turn = 1;

        (string, CardData) card = ((string, CardData))CardCollection.instance.GetNextCard(triggeredKeys, usedUniqueKeys);
        InstantiateCard(card.Item2, card.Item1);
    }

    void Update()
    {

    }

    public void AdvanceTurn(List<EffectData> effects)
    {
        StatHandler statHandler = StatHandler.instance;
        foreach (EffectData effect in effects)
        {
            effect.action.Invoke();
        }
        Turn++;
        (string, CardData) card = ((string, CardData))CardCollection.instance.GetNextCard(triggeredKeys, usedUniqueKeys);
        InstantiateCard(card.Item2, card.Item1);
        foreach (Investment investment in statHandler.Investments)
        {
            investment.HandleInvestment();
        }
        statHandler.Money += statHandler.MoneyChange;
        statHandler.MentalHealth += statHandler.MentalHealthChange;
        statHandler.SocialStanding += statHandler.SocialStandingChange;
        statHandler.SchoolPerformance += statHandler.SchoolPerformanceChange;
    }

    public void ChooseCard(string cardKey, CardData card, bool choseRight)
    {
        if (choseRight && !string.IsNullOrEmpty(card.triggerKey))
        {
            if (!triggeredKeys.Contains(card.triggerKey))
                triggeredKeys.Add(card.triggerKey);
        }

        if (card.unique && !usedUniqueKeys.Contains(cardKey))
        {
            usedUniqueKeys.Add(cardKey);
        }

        List<EffectData> effects = choseRight ? card.right : card.left;
        foreach (var effect in effects)
        {
            effect.action.Invoke();
        }
    }

    private void InstantiateCard(CardData cardData, string cardKey)
    {
        Prefabs.Card(cardWindow, cardData.title, cardData.description, CardImage.Cafe, cardData, cardKey);
    }
}
