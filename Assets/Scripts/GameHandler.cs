using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;

    string grade_E = "<b><color=#B00000ff>E</color></b>";
    string grade_D = "<b><color=#FF4500ff>D</color></b>";
    string grade_C = "<b><color=#FFD700ff>C</color></b>";
    string grade_B = "<b><color=#1E90FFff>B</color></b>";
    string grade_A = "<b><color=#139F27ff>A</color></b>";
    string grade_S = "<b><color=#8A2BE2ff>S</color></b>";

    public int turns;
    public Transform cardWindow;
    public TMP_Text weekNumber;
    public List<TMP_Text> statIndicators = new List<TMP_Text>();
    public GameObject lossScreen;
    public List<GameObject> lossScreenTexts = new List<GameObject>();
    public GameObject helpScreen;
    public GameObject winScreen;
    public List<TMP_Text> winScreenTexts = new List<TMP_Text>();
    public TMP_Text winScreenInvestments;
    public TMP_Text winScreenSummaryText;
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
        statHandler.Money = UnityEngine.Random.Range(2250, 2751);
        statHandler.Investments = new List<Investment>();
        statHandler.MentalHealth = UnityEngine.Random.Range(600, 700);
        statHandler.SocialStanding = UnityEngine.Random.Range(600, 700);
        statHandler.SchoolPerformance = UnityEngine.Random.Range(600, 700);
        statHandler.MoneyChange = 20;
        statHandler.MentalHealthChange = -5;
        statHandler.SocialStandingChange = -5;
        statHandler.SchoolPerformanceChange = -5;
        UpdateStatIndicators();
        Turn = 1;

        (string, CardData) card = ((string, CardData))CardCollection.instance.GetNextCard(triggeredKeys, usedUniqueKeys);
        InstantiateCard(card.Item2, card.Item1);
    }

    void LoseGame(int lossType)
    {
        lossScreen.SetActive(true);
        lossScreenTexts[lossType].SetActive(true);
    }

    void Update()
    {

    }

    public void AdvanceTurn(List<EffectData> effects)
    {
        Turn++;
        StatHandler statHandler = StatHandler.instance;
        foreach (EffectData effect in effects)
        {
            effect.action.Invoke();
        }
        bool isGameFinished = false;
        if (Turn == turns + 1)
        {
            isGameFinished = true;
        }
        else
        {
            (string, CardData) card = ((string, CardData))CardCollection.instance.GetNextCard(triggeredKeys, usedUniqueKeys);
            InstantiateCard(card.Item2, card.Item1);
        }
        foreach (Investment investment in statHandler.Investments)
        {
            investment.HandleInvestment();
        }

        if (statHandler.Money < 1)
        {
            LoseGame(0);
            return;
        }
        if (statHandler.MentalHealth < 1)
        {
            LoseGame(1);
            return;
        }
        if (statHandler.SocialStanding < 1)
        {
            LoseGame(2);
            return;
        }
        if (statHandler.SchoolPerformance < 1)
        {
            LoseGame(3);
            return;
        }

        Debug.Log($"MoneyChange: {statHandler.MoneyChange}, MentalHealthChange: {statHandler.MentalHealthChange}, SocialStandingChange: {statHandler.SocialStandingChange}, SchoolPerformanceChange: {statHandler.SchoolPerformanceChange}");

        int mentalAdjust = statHandler.MentalHealthChange > 0 ? (int)Math.Floor((float)statHandler.MentalHealthChange / 8f) : (int)Math.Ceiling((float)statHandler.MentalHealthChange / 8f);
        int socialAdjust = statHandler.SocialStandingChange > 0 ? (int)Math.Floor((float)statHandler.SocialStandingChange / 8f) : (int)Math.Ceiling((float)statHandler.SocialStandingChange / 8f);
        int schoolAdjust = statHandler.SchoolPerformanceChange > 0 ? (int)Math.Floor((float)statHandler.SchoolPerformanceChange / 8f) : (int)Math.Ceiling((float)statHandler.SchoolPerformanceChange / 8f);

        statHandler.MentalHealthChange -= mentalAdjust;
        statHandler.SocialStandingChange -= socialAdjust;
        statHandler.SchoolPerformanceChange -= schoolAdjust;

        UpdateStatIndicators();

        statHandler.Money += statHandler.MoneyChange;
        statHandler.MentalHealth += statHandler.MentalHealthChange;
        statHandler.SocialStanding += statHandler.SocialStandingChange;
        statHandler.SchoolPerformance += statHandler.SchoolPerformanceChange;

        if (isGameFinished)
        {
            PrepareWinScreen();
            ShowWinScreen();
        }
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
    }

    private void InstantiateCard(CardData cardData, string cardKey)
    {
        Debug.Log($"Next Up: {cardKey}");
        Prefabs.Card(cardWindow, cardData.title, cardData.description, cardData, cardKey);
    }

    private void PrepareWinScreen()
    {
        StatHandler statHandler = StatHandler.instance;
        winScreenTexts[0].text = $"Economic Outcome: {GetGrade((float)(statHandler.Money * (1 + 0.5 * statHandler.Investments.Count)), 0, 20000)}";
        winScreenTexts[1].text = $"Mental Health Outcome: {GetGrade(statHandler.MentalHealth, 0, 1000)}";
        winScreenTexts[2].text = $"Social Standing Outcome: {GetGrade(statHandler.SocialStanding, 0, 1000)}";
        winScreenTexts[3].text = $"School Performance Outcome: {GetGrade(statHandler.SchoolPerformance, 0, 1000)}";

        float combinedScore = statHandler.MentalHealth + statHandler.SocialStanding + statHandler.SchoolPerformance;
        combinedScore += Math.Min((float)(statHandler.Money * (1 + 0.5 * statHandler.Investments.Count)) / 10, 2000);
        winScreenTexts[4].text = $"Overall Performance: {GetGrade(combinedScore, 0, 5000)}";

        winScreenInvestments.text = new string('•', statHandler.Investments.Count);

        winScreenSummaryText.text = GenerateSummary(statHandler.Money, statHandler.Investments.Count, statHandler.MentalHealth, statHandler.SocialStanding, statHandler.SchoolPerformance);
    }

    private void ShowWinScreen()
    {
        winScreen.SetActive(true);
    }

    public void HideHelpScreen()
    {
        helpScreen.SetActive(false);
    }

    public void UpdateStatIndicators()
    {
        StatHandler statHandler = StatHandler.instance;
        float totalMoneyChange = statHandler.MoneyChange;
        foreach (Investment investment in StatHandler.instance.Investments)
        {
            totalMoneyChange += ((investment.Amount * investment.Returns) - investment.Amount) / investment.Rate;
            totalMoneyChange -= investment.Recurring / investment.Rate;
        }
        statIndicators[0].text = $"{GetArrowString(totalMoneyChange / 20)}";
        statIndicators[1].text = $"{GetArrowString(statHandler.MentalHealthChange)}";
        statIndicators[2].text = $"{GetArrowString(statHandler.SocialStandingChange)}";
        statIndicators[3].text = $"{GetArrowString(statHandler.SchoolPerformanceChange)}";
    }

    private string GetArrowString(float change)
    {
        if (change == 0) return "";

        string arrow = change > 0 ? "<color=#139F27ff>▲</color>" : "<color=#f6354fff>▼</color>";
        float absChange = Mathf.Abs(change);

        if (absChange <= 6) return arrow;
        if (absChange <= 12) return arrow + arrow;
        return arrow + arrow + arrow;
    }

    public string GetGrade(float value, float worst, float best)
    {
        if (best <= worst)
        {
            Debug.LogError("Best must be greater than worst.");
            return grade_E;
        }

        float normalized = math.clamp((value - worst) / (best - worst), 0f, 1f);

        if (normalized < 0.15f)
            return grade_E;
        else if (normalized < 0.35f)
            return grade_D;
        else if (normalized < 0.55f)
            return grade_C;
        else if (normalized < 0.75f)
            return grade_B;
        else if (normalized < 0.90f)
            return grade_A;
        else
            return grade_S;
    }

    private string GenerateSummary(double money, int investmentCount, float mentalHealth, float socialStanding, float schoolPerformance)
    {
        double economy = money * (1 + 0.5 * investmentCount);

        bool economyGood = economy > 15000;
        bool economyBad = economy < 3500;

        bool mentalGood = mentalHealth > 700;
        bool mentalBad = mentalHealth < 300;

        bool socialGood = socialStanding > 700;
        bool socialBad = socialStanding < 300;

        bool schoolGood = schoolPerformance > 700;
        bool schoolBad = schoolPerformance < 300;

        if (economyBad && mentalBad && socialBad && schoolBad)
            return "You faced hardships on all fronts. Financial instability has made the road ahead steeper — wise choices will be crucial from here.";

        if (economyGood && mentalBad)
            return "You built an early fortune, but personal wellbeing suffered. Remember: wealth means little without health.";

        if (economyBad && mentalGood)
            return "You protected your wellbeing, but financial struggles may limit your future options. Financial stability should not be overlooked.";

        if (economyGood && mentalGood && socialGood && schoolGood)
            return "You built an exceptional foundation — strong in wealth, mind, and community. A bright future lies ahead.";

        if (economyBad && mentalGood && socialGood)
            return "Your relationships and mental strength are solid, but lacking financial stability may close some doors. Plan wisely moving forward.";

        if (economyGood && mentalGood)
            return "You struck a strong balance between financial security and personal wellbeing. Few achieve this so early.";

        if (economyBad && socialGood)
            return "You cultivated strong connections, but neglected your financial base. Future opportunities could slip without economic stability.";

        if (schoolGood && mentalBad)
            return "You achieved academically, but struggled internally. Balancing mental health will be key to sustaining success.";

        if (socialGood && economyGood)
            return "You leave your early years with both wealth and strong bonds — a powerful start few achieve.";

        if (schoolBad && economyGood)
            return "While academic results were lacking, you established real-world success early. Knowledge can still be built over time.";

        if (economyBad)
            return "You struggled to secure financial stability. Without a solid economic foundation, the years ahead may be more challenging — but it's never too late to learn and improve.";

        if (mentalGood)
            return "You step into adulthood with resilience and inner strength — a priceless foundation for what's to come.";

        if (socialGood)
            return "You forged strong relationships early — an invaluable support for the years ahead.";

        if (schoolGood)
            return "You built a strong academic base that will support your future ambitions.";

        return "At a young age, every journey is still in its early chapters — your choices from here will define your story.";
    }
}
