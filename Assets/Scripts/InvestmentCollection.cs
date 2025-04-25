using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestmentCollection : MonoBehaviour
{
    public static InvestmentCollection instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public static Dictionary<string, Investment> Cards = new Dictionary<string, Investment>()
    {
        {"Stocks", new Investment()
        {
            Name = "Stocks",
            Amount = 800,
            Returns = 1.08f,
            Rate = 4,
        }}
    };
}

public class Investment
{
    public string Name { get; set; }
    public float Amount { get; set; }
    public float Recurring { get; set; }
    public float Returns { get; set; }
    public int Rate { get; set; }

    private int weeksPassed = 0;

    public void HandleInvestment()
    {
        weeksPassed++;

        if (weeksPassed == Rate - 1)
        {
            StatHandler statHandler = StatHandler.instance;
            statHandler.Money += (Amount * Returns) - Amount;
            Amount += (Amount * Returns) - Amount;
            Amount += Recurring;
            weeksPassed = 0;
        }
    }
}