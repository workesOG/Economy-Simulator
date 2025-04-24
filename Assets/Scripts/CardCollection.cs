using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

public class CardCollection : MonoBehaviour
{
    public static CardCollection instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public static Dictionary<string, CardData> Cards = new Dictionary<string, CardData>()
    {
        {"Eat_With_Friends", new CardData{
            title = "Go out or stay in?",
            description = "Your friends have offered you a seat at a restaurant. Will you eat out with friends or make food at home for yourself?",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "It'll be fun!", action = () =>
                { StatHandler.instance.SocialStanding += 22; StatHandler.instance.MentalHealth += 12; } },
                new EffectData() {positive = false, text = "It's not the cheapest restaurant", action = () =>
                { StatHandler.instance.Money -= 25;} },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Nice and cheap", action = () =>
                { StatHandler.instance.Money -= 5; } },
                new EffectData() {positive = false, text = "Quite boring", action = () =>
                { StatHandler.instance.SocialStanding -= 14; StatHandler.instance.MentalHealth -= 7; } },
            },
        }},
        {"Buy_Used_Car", new CardData{
            title = "Buy a Used Car?",
            description = "You found a decent used car. It'll cost you continually to maintain, but it could make your life easier. Do you buy it?",
            unique = true,
            investment = true,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Freedom and independence", action = () =>
                { StatHandler.instance.MentalHealthChange += 10f; StatHandler.instance.SocialStandingChange += 6f; } },
                new EffectData() {positive = false, text = "Ongoing costs and upfront payment", action = () =>
                { StatHandler.instance.Money -= 2000; StatHandler.instance.MoneyChange -= 50f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Save money for other needs", action = () =>
                { } },
                new EffectData() {positive = false, text = "Still stuck with public transport", action = () =>
                { StatHandler.instance.MentalHealth -= 15f; } },
            },
        }},
        {"Get_Student_Job", new CardData{
            title = "Get a Student Job?",
            description = "You found a part-time job at a local store. It pays €75 a week and fits your schedule, but it'll cost you some time and energy. Do you take it?",
            unique = true,
            investment = false,
            triggerKey = "student_job_aquired",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Steady side income", action = () =>
                { StatHandler.instance.MoneyChange += 75f; } },
                new EffectData() {positive = false, text = "Less free time and energy", action = () =>
                { StatHandler.instance.MentalHealthChange -= 5f; StatHandler.instance.SchoolPerformanceChange -= 3f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = false, text = "No change", action = () =>
                { } },
            },
        }},
    };

    public (string, CardData)? GetNextCard(List<string> triggeredKeys, HashSet<string> usedUniqueKeys)
    {
        List<KeyValuePair<string, CardData>> validCards = new List<KeyValuePair<string, CardData>>();

        foreach (var pair in Cards)
        {
            var card = pair.Value;
            string key = pair.Key;

            if (card.unique && usedUniqueKeys.Contains(key))
                continue;

            if (!string.IsNullOrEmpty(card.requiredTrigger) && !triggeredKeys.Contains(card.requiredTrigger))
                continue;

            validCards.Add(pair);
        }

        if (validCards.Count == 0)
            return null;

        var selected = validCards[Random.Range(0, validCards.Count)];
        return (selected.Key, selected.Value);
    }


    public CardData GetRandomCardData()
    {
        if (Cards.Count == 0) return null;

        int randomIndex = Random.Range(0, Cards.Values.Count);
        int currentIndex = 0;

        foreach (var card in Cards.Values)
        {
            if (currentIndex == randomIndex)
                return card;
            currentIndex++;
        }

        return null;
    }
}

public class CardData
{
    public string title;
    public string description;
    public bool unique;
    public bool investment;
    public string requiredTrigger;
    public string triggerKey;

    public List<EffectData> right;
    public List<EffectData> left;
}