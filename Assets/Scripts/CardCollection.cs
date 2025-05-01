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
            imageID = "restaurant",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "It'll be fun!", action = () =>
                { StatHandler.instance.SocialStanding += 22; StatHandler.instance.MentalHealth += 15; } },
                new EffectData() {positive = false, text = "It's not the cheapest restaurant", action = () =>
                { StatHandler.instance.Money -= 25;} },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Nice and cheap", action = () =>
                { StatHandler.instance.Money -= 5; } },
                new EffectData() {positive = false, text = "Quite boring", action = () =>
                { StatHandler.instance.SocialStanding -= 14; StatHandler.instance.MentalHealth -= 5; } },
            },
        }},
        {"Buy_Used_Car", new CardData{
            title = "Buy a Used Car?",
            description = "You found a decent used car. It'll cost you continually to maintain, but it could make your life easier. Do you buy it?",
            imageID = "used_car",
            unique = true,
            investment = true,
            triggerKey = "bought_car",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Freedom and independence", action = () =>
                { StatHandler.instance.MentalHealthChange += 12f; StatHandler.instance.SocialStandingChange += 6f; } },
                new EffectData() {positive = false, text = "Ongoing costs and upfront payment", action = () =>
                { StatHandler.instance.Money -= 2000; StatHandler.instance.MoneyChange -= 50f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Save money for other needs", action = () =>
                { } },
                new EffectData() {positive = false, text = "Still stuck with public transport", action = () =>
                { StatHandler.instance.MentalHealth -= 12f; } },
            },
        }},
        {"Get_Student_Job", new CardData{
            title = "Get a Student Job?",
            description = "You found a part-time job at a local store. It pays well and fits your schedule. Do you take it?",
            imageID = "student_job",
            unique = true,
            investment = false,
            triggerKey = "student_job_acquired",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Steady side income", action = () =>
                { StatHandler.instance.MoneyChange += 75f; } },
                new EffectData() {positive = false, text = "Less free time and energy", action = () =>
                { StatHandler.instance.MentalHealthChange -= 4f; StatHandler.instance.SchoolPerformanceChange -= 3f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = false, text = "No change", action = () =>
                { } },
            },
        }},
        {"Pick_Up_Extra_Shifts", new CardData{
            title = "Pick Up Extra Shifts?",
            description = "Your manager asks if you can work extra this week. It pays well. Do you help out?",
            imageID = "extra_shifts",
            unique = false,
            investment = false,
            requiredTrigger = "student_job_acquired",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Nice cash boost", action = () =>
                { StatHandler.instance.Money += 120f; } },
                new EffectData() {positive = false, text = "You're more tired than usual", action = () =>
                { StatHandler.instance.MentalHealth -= 10f; StatHandler.instance.SchoolPerformance -= 8f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Time to focus on school and friends", action = () =>
                { StatHandler.instance.SchoolPerformance += 6f; StatHandler.instance.SocialStanding += 4f; } },
            },
        }},
        {"Take_On_More_Hours", new CardData{
            title = "Take On More Hours?",
            description = "You've been offered more hours at work. It's not a lot, but it adds up. Do you accept?",
            imageID = "more_hours",
            unique = true,
            investment = false,
            requiredTrigger = "student_job_acquired",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "More weekly income", action = () =>
                { StatHandler.instance.MoneyChange += 40f; } },
                new EffectData() {positive = false, text = "More stress and less study time", action = () =>
                { StatHandler.instance.MentalHealthChange -= 2f; StatHandler.instance.SchoolPerformanceChange -= 2f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = false, text = "No change", action = () =>
                { } },
            },
        }},
        {"Skip_Breakfast", new CardData{
            title = "Skip Breakfast?",
            description = "You're in a rush. You can grab breakfast or skip it. What do you do?",
            imageID = "skip_breakfast",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Save a few minutes", action = () =>
                { StatHandler.instance.MentalHealth += 5f; } },
                new EffectData() {positive = false, text = "Low energy all morning", action = () =>
                { StatHandler.instance.SchoolPerformance -= 12f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = false, text = "You're a bit late to class", action = () =>
                { StatHandler.instance.SchoolPerformance -= 8f; } },
            },
        }},
        {"Clean_Your_Room", new CardData{
            title = "Clean Your Room?",
            description = "Your room's a mess. You could clean now or leave it for later. What do you do?",
            imageID = "clean_room",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Feels fresh and organized", action = () =>
                { StatHandler.instance.MentalHealth += 18f; } },
                new EffectData() {positive = false, text = "Takes time from studying", action = () =>
                { StatHandler.instance.SchoolPerformance -= 8f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Time for studying", action = () =>
                { StatHandler.instance.SchoolPerformance += 15f; } },
                new EffectData() {positive = false, text = "The mess stresses you out", action = () =>
                { StatHandler.instance.MentalHealth -= 6f; } },
            },
        }},
        {"Join_Study_Group", new CardData{
            title = "Join a Study Group?",
            description = "You've been invited to a weekly study group. It could help. Do you join?",
            imageID = "study_group",
            unique = false,
            investment = false,
            triggerKey = "joined_study_club",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Study support and structure", action = () =>
                { StatHandler.instance.SchoolPerformanceChange += 9f; } },
                new EffectData() {positive = false, text = "Less time for yourself", action = () =>
                { StatHandler.instance.MentalHealthChange -= 5f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "More time for yourself and friends", action = () =>
                { StatHandler.instance.MentalHealth += 5f; StatHandler.instance.SocialStanding += 4f; } },
                new EffectData() {positive = false, text = "You fall behind on some topics", action = () =>
                { StatHandler.instance.SchoolPerformance -= 10f; } },
            },
        }},
        {"Help_A_Friend_Move", new CardData{
            title = "Help a Friend Move?",
            description = "A friend asks for help moving. It's a full afternoon commitment. Do you say yes?",
            imageID = "help_friend_move",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Builds your friendship", action = () =>
                { StatHandler.instance.SocialStanding += 22f; } },
                new EffectData() {positive = false, text = "Physically exhausting", action = () =>
                { StatHandler.instance.MentalHealth -= 8f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = false, text = "They're a bit disappointed", action = () =>
                { StatHandler.instance.SocialStanding -= 8f; } },
            },
        }},
        {"Meal_Prep_Or_Takeout", new CardData{
            title = "Meal Prep or Takeout?",
            description = "You have to find an alternative to your current meal situation. Meal prep or outsource?",
            imageID = "meal_prep",
            unique = true,
            investment = false,
            triggerKey = "meal_prep_mastered",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Cheaper and healthier", action = () =>
                { StatHandler.instance.MoneyChange += 5f; StatHandler.instance.MentalHealthChange += 3f; } },
                new EffectData() {positive = false, text = "Takes time and effort", action = () =>
                { StatHandler.instance.SchoolPerformanceChange -= 2f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Fast, easy, tasty", action = () =>
                { StatHandler.instance.MentalHealthChange += 5f; } },
                new EffectData() {positive = false, text = "Gets pricey over time", action = () =>
                { StatHandler.instance.MoneyChange -= 8f; } },
            },
        }},
        {"Open_Student_Account", new CardData{
            title = "Open a Student Account?",
            description = "The bank offers a student account with benefits. Do you sign up?",
            imageID = "student_account",
            unique = true,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Better interest and no fees", action = () =>
                { StatHandler.instance.MoneyChange += 5f; } },
                new EffectData() {positive = false, text = "Confusing paperwork", action = () =>
                { StatHandler.instance.MentalHealth -= 8f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Avoid the hassle", action = () =>
                { } },
                new EffectData() {positive = false, text = "You miss some perks", action = () =>
                { StatHandler.instance.MoneyChange -= 3f; } },
            },
        }},
        {"Buy_Generic_Products", new CardData{
            title = "Buy Generic Products?",
            description = "Groceries are getting expensive. Do you go for generic brands?",
            imageID = "generic_products",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Savings over time", action = () =>
                { StatHandler.instance.MoneyChange += 7f; } },
                new EffectData() {positive = false, text = "Some items are lower quality", action = () =>
                { StatHandler.instance.MentalHealthChange -= 2f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Brand loyalty feels rewarding", action = () =>
                { StatHandler.instance.MentalHealthChange += 3f; } },
                new EffectData() {positive = false, text = "You overspend a bit", action = () =>
                { StatHandler.instance.MoneyChange -= 6f; } },
            },
        }},
        {"Buy_Budget_Laptop", new CardData{
            title = "Buy a Budget Laptop?",
            description = "Your old laptop is slow. You have found a new, decent one. Do you buy it?",
            imageID = "buy_new_laptop",
            unique = true,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Faster, less frustration", action = () =>
                { StatHandler.instance.SchoolPerformanceChange += 5f; StatHandler.instance.MentalHealthChange += 2f; } },
                new EffectData() {positive = false, text = "It's expensive", action = () =>
                { StatHandler.instance.Money -= 800f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You hold onto your savings", action = () =>
                { } },
                new EffectData() {positive = false, text = "Studying is a painful", action = () =>
                { StatHandler.instance.SchoolPerformance -= 8f; StatHandler.instance.MentalHealthChange -= 3f; } },
            },
        }},
        {"Pay_For_Subscription", new CardData{
            title = "Pay for a Subscription?",
            description = "Your free trial ends. The service isn't too expensive. Do you keep it?",
            imageID = "keep_subscription",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "It makes life easier", action = () =>
                { StatHandler.instance.MentalHealth += 6f; } },
                new EffectData() {positive = false, text = "Ongoing cost", action = () =>
                { StatHandler.instance.MoneyChange -= 10f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "One less bill", action = () =>
                { StatHandler.instance.MoneyChange += 4f; } },
                new EffectData() {positive = false, text = "You miss the convenience", action = () =>
                { StatHandler.instance.MentalHealth -= 3f; } },
            },
        }},
        {"Buy_Groceries_Bulk", new CardData{
            title = "Buy Groceries in Bulk?",
            description = "Buying in bulk saves money but costs more upfront. Worth it?",
            imageID = "buy_bulk",
            unique = true,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Save in the long run", action = () =>
                { StatHandler.instance.MoneyChange += 8f;  } },
                new EffectData() {positive = false, text = "It strains your current budget", action = () =>
                { StatHandler.instance.MentalHealth -= 4f; StatHandler.instance.Money -= 200f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Less spending now", action = () =>
                { } },
                new EffectData() {positive = false, text = "Higher costs over time", action = () =>
                { StatHandler.instance.MoneyChange -= 4f; } },
            },
        }},
        {"Cheap_Haircut", new CardData{
            title = "Get a Cheap Haircut?",
            description = "The salon is expensive, but your friend can cut it cheaper. Do you trust them?",
            imageID = "cheap_haircut",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You save money", action = () =>
                { StatHandler.instance.Money -= 10f; } },
                new EffectData() {positive = false, text = "It's not quite what you wanted", action = () =>
                { StatHandler.instance.SocialStanding -= 2f; StatHandler.instance.MentalHealth -= 4f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "It looks professional", action = () =>
                { StatHandler.instance.SocialStanding += 6f; } },
                new EffectData() {positive = false, text = "You pay full price", action = () =>
                { StatHandler.instance.Money -= 40f; } },
            },
        }},
        {"Laundry_Choice", new CardData{
            title = "Go to a laundromat?",
            description = "You can use the laundromat or do laundry at your friend's place. Which do you choose?",
            imageID = "laundry_at_friends",
            unique = true,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You save a bit weekly", action = () =>
                { StatHandler.instance.MoneyChange += 5f; } },
                new EffectData() {positive = true, text = "You get to hang out", action = () =>
                { StatHandler.instance.SocialStandingChange += 2f; } },
                new EffectData() {positive = false, text = "Takes more time and effort", action = () =>
                { StatHandler.instance.MentalHealthChange -= 4f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Quick and easy", action = () =>
                { StatHandler.instance.MentalHealthChange += 5f;  } },
                new EffectData() {positive = false, text = "Laundry costs add up", action = () =>
                { StatHandler.instance.MoneyChange -= 10f; } },
            },
        }},
        {"Buy_Friend_Gift", new CardData{
            title = "Buy a Gift for a Friend?",
            description = "Your friend's birthday is coming up. Do you spend money on a gift?",
            imageID = "gift_for_friend",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "They really appreciate it", action = () =>
                { StatHandler.instance.SocialStanding += 30f;  } },
                new EffectData() {positive = true, text = "You feel good", action = () =>
                { StatHandler.instance.MentalHealth += 15f;  } },
                new EffectData() {positive = false, text = "It sets back your savings", action = () =>
                { StatHandler.instance.Money -= 40f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You hold onto your budget", action = () =>
                { } },
                new EffectData() {positive = false, text = "Your friend feels overlooked", action = () =>
                { StatHandler.instance.SocialStanding -= 6f; } },
            },
        }},
        {"Cancel_Club_Membership", new CardData{
            title = "Cancel Club Membership?",
            description = "You're rarely using your student club membership. Cancel it to save money?",
            imageID = "club_membership",
            unique = true,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "More room in your budget", action = () =>
                { StatHandler.instance.MoneyChange += 12f; } },
                new EffectData() {positive = false, text = "Lose out on social events", action = () =>
                { StatHandler.instance.SocialStandingChange -= 5f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Stay connected", action = () =>
                { StatHandler.instance.SocialStandingChange += 4f; } },
                new EffectData() {positive = false, text = "You keep paying monthly", action = () =>
                { } },
            },
        }},
        {"Sell_Old_Textbooks", new CardData{
            title = "Sell Some Old Textbooks?",
            description = "You've finished your classes. Sell your old textbooks or keep them?",
            imageID = "sell_books",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You make a bit of money", action = () =>
                { StatHandler.instance.Money += 120f; } },
                new EffectData() {positive = false, text = "You might need them again", action = () =>
                { StatHandler.instance.SchoolPerformanceChange -= 3f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Reference material on hand", action = () =>
                { StatHandler.instance.SchoolPerformanceChange += 2f; } },
                new EffectData() {positive = false, text = "Clutter and no cash boost", action = () =>
                { StatHandler.instance.MentalHealth -= 20f; } },
            },
        }},
        {"Refund_Lucky_Refund", new CardData{
            title = "Unexpected Refund!",
            description = "Turns out you were overcharged last month. The bank refunds the money!",
            imageID = "bank_refund",
            unique = true,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Free money! Thanks, bank", action = () =>
                { StatHandler.instance.Money += 225f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = false, text = "You do nothing", action = () =>
                { } },
            },
        }},
        {"Laptop_Breaks", new CardData{
            title = "Laptop Breaks Down",
            description = "Your laptop dies right before a deadline. Repairs are costly. Do you want to repair it?",
            imageID = "computer_crash",
            unique = true,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You make the deadline", action = () =>
                { StatHandler.instance.SchoolPerformance += 15f; } },
                new EffectData() {positive = false, text = "It's expensive", action = () =>
                { StatHandler.instance.Money -= 300f; } },
                new EffectData() {positive = false, text = "The stress lingers", action = () =>
                { StatHandler.instance.MentalHealth -= 12f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You save the money", action = () =>
                {  } },
                new EffectData() {positive = false, text = "The assignment is not delivered", action = () =>
                { StatHandler.instance.SchoolPerformance -= 30f; } },
            },
        }},
        {"Stock_Up_On_Sale", new CardData{
            title = "Stock Up During Sale?",
            description = "Your local store is running a big sale. Stock up while you can?",
            imageID = "stock_up_during_sale",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You grab great deals", action = () =>
                { StatHandler.instance.MoneyChange += 8f; } },
                new EffectData() {positive = false, text = "Large upfront cost", action = () =>
                { StatHandler.instance.Money -= 100f; } },
                new EffectData() {positive = false, text = "You sacrifice flexibility", action = () =>
                { StatHandler.instance.MentalHealth -= 2f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You keep your cash liquid", action = () =>
                { StatHandler.instance.MentalHealth += 3f; } },
                new EffectData() {positive = false, text = "You miss out on deals", action = () =>
                { StatHandler.instance.MoneyChange -= 2f; } },
            },
        }},
        {"Free_Pizza_Night", new CardData{
            title = "Free Pizza Night?",
            description = "A student group is hosting free pizza. You're invited. Do you go?",
            imageID = "pizza_party",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Free food and socializing", action = () =>
                { StatHandler.instance.Money += 10f; StatHandler.instance.SocialStanding += 15f; } },
                new EffectData() {positive = false, text = "Late night, hard to focus later", action = () =>
                { StatHandler.instance.SchoolPerformance -= 7f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Stay home and rest", action = () =>
                { StatHandler.instance.MentalHealth += 13f; } },
                new EffectData() {positive = false, text = "You miss out", action = () =>
                { StatHandler.instance.SocialStanding -= 7f; } },
            },
        }},
        {"Try_Budgeting_App", new CardData{
            title = "Try a Budgeting App?",
            description = "You feel like you've been overspending. A friend recommends a budgeting app. Try it?",
            imageID = "budgeting_app",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Gain financial control", action = () =>
                { StatHandler.instance.MoneyChange += 15f; } },
                new EffectData() {positive = false, text = "Takes time to track everything", action = () =>
                { StatHandler.instance.MentalHealthChange -= 3f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Less stress for now", action = () =>
                { StatHandler.instance.MentalHealth += 14f; } },
                new EffectData() {positive = false, text = "Overspending continues", action = () =>
                { StatHandler.instance.MoneyChange -= 4f; } },
            },
        }},
        {"Buy_Fancy_Groceries", new CardData{
            title = "Buy Fancy Groceries?",
            description = "You spot some expensive snacks and drinks. Do you treat yourself?",
            imageID = "fancy_groceries",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Nice treat for the week", action = () =>
                { StatHandler.instance.MentalHealth += 18f;  } },
                new EffectData() {positive = false, text = "Now your budget's tight", action = () =>
                { StatHandler.instance.Money -= 45f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Restraint pays off economically", action = () =>
                { StatHandler.instance.Money += 25f; } },
                new EffectData() {positive = false, text = "You miss out on comfort", action = () =>
                { StatHandler.instance.MentalHealth -= 6f; } },
            },
        }},
        {"Ignore_Small_Fine", new CardData{
            title = "Ignore a Small Fine?",
            description = "You were fined a small sum for a library book. Pay now or hope they forget?",
            imageID = "ignore_fine",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Debt cleared, no stress", action = () =>
                { StatHandler.instance.MentalHealth += 18f; } },
                new EffectData() {positive = false, text = "It costs a little", action = () =>
                { StatHandler.instance.Money -= 70f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Save the money", action = () =>
                { StatHandler.instance.Money += 70f; } },
                new EffectData() {positive = false, text = "They might notice", action = () =>
                { StatHandler.instance.MoneyChange -= 8f; } },
            },
        }},
        {"Buy_Test_Prep", new CardData{
            title = "Buy Test Prep Course?",
            description = "A prep course for finals could help, but it's a little pricey. Worth the cost?",
            imageID = "prep_course",
            unique = true,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Boosts your exam confidence", action = () =>
                { StatHandler.instance.SchoolPerformanceChange += 6f; } },
                new EffectData() {positive = false, text = "You gotta spend money", action = () =>
                { StatHandler.instance.Money -= 180f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You save your money", action = () =>
                { } },
                new EffectData() {positive = false, text = "You struggle with studying", action = () =>
                { StatHandler.instance.SchoolPerformanceChange -= 2f; StatHandler.instance.SchoolPerformance -= 10f; } },
            },
        }},
        {"Skip_Weekly_Groceries", new CardData{
            title = "Skip This Week's Groceries?",
            description = "You're low on money. You could skip groceries this week to save. Risk it?",
            imageID = "skip_groceries",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You stretch your budget", action = () =>
                { StatHandler.instance.Money += 80f; } },
                new EffectData() {positive = false, text = "You feel sluggish and unfocused", action = () =>
                { StatHandler.instance.MentalHealth -= 18f; StatHandler.instance.SchoolPerformance -= 12f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You stay well-fed", action = () =>
                { StatHandler.instance.MentalHealth += 18f; } },
                new EffectData() {positive = false, text = "Tighter budget ahead", action = () =>
                { StatHandler.instance.Money -= 80f; } },
            },
        }},
        {"Buy_Cheap_Desk_Chair", new CardData{
            title = "Buy a Cheap Desk Chair?",
            description = "Your chair is awful. You found a cheap alternative, but won't last. Do you buy it?",
            imageID = "buy_cheap_chair",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Immediate comfort", action = () =>
                { StatHandler.instance.MentalHealth += 6f; StatHandler.instance.Money -= 40f; } },
                new EffectData() {positive = false, text = "Breaks again soon", action = () =>
                { StatHandler.instance.MoneyChange -= 6f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Save for something better", action = () =>
                { } },
                new EffectData() {positive = false, text = "Back pain keeps distracting you", action = () =>
                { StatHandler.instance.SchoolPerformance -= 6f; } },
            },
        }},
        {"Delay_Utility_Bill", new CardData{
            title = "Pay your bills immediately?",
            description = "You received your electricity bill. Pay it now or hold off until next week?",
            imageID = "pay_bills",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Clean slate, no stress", action = () =>
                { StatHandler.instance.MentalHealth += 18f; } },
                new EffectData() {positive = false, text = "Paying bills ain't free", action = () =>
                { StatHandler.instance.Money -= 90f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "More room in this week's budget", action = () =>
                { StatHandler.instance.Money += 30f; } },
                new EffectData() {positive = false, text = "They'll notice eventually", action = () =>
                { StatHandler.instance.MoneyChange -= 10f; } },
            },
        }},
        {"Donate_To_Fundraiser", new CardData{
            title = "Donate to a Fundraiser?",
            description = "A campus fundraiser is raising money for a good cause. Do you chip in?",
            imageID = "donate_to_fundraiser",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You feel good helping out", action = () =>
                { StatHandler.instance.MentalHealthChange += 7f; } },
                new EffectData() {positive = false, text = "Need to cut back elsewhere", action = () =>
                { StatHandler.instance.Money -= 25f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You save your money", action = () =>
                { } },
                new EffectData() {positive = false, text = "You feel a bit guilty", action = () =>
                { StatHandler.instance.MentalHealth -= 2f; } },
            },
        }},
        {"Pay_For_Cleaning", new CardData{
            title = "Pay for Cleaning Service?",
            description = "Your place is a mess. A cleaning service is cheap for students. Pay up?",
            imageID = "cleaning_services",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Clean space, clear mind", action = () =>
                { StatHandler.instance.MentalHealth += 20f; } },
                new EffectData() {positive = false, text = "Gotta pay up", action = () =>
                { StatHandler.instance.Money -= 50f; } },
                new EffectData() {positive = false, text = "Feels a little indulgent", action = () =>
                { StatHandler.instance.SocialStanding -= 3f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You save the money", action = () =>
                { } },
                new EffectData() {positive = false, text = "Gotta clean yourself", action = () =>
                { StatHandler.instance.MentalHealth -= 12f; } },
            },
        }},
        {"Help_Uncle_Move", new CardData{
            title = "Help Your Uncle Move?",
            description = "Your uncle asks if you can help move furniture this weekend. Do you say yes?",
            imageID = "help_uncle_move",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "He gives you some cash", action = () =>
                { StatHandler.instance.Money += 150f; } },
                new EffectData() {positive = false, text = "You're sore and tired", action = () =>
                { StatHandler.instance.MentalHealth -= 6f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = false, text = "You stay home an rest", action = () =>
                { StatHandler.instance.MentalHealth += 5f; } },
                new EffectData() {positive = false, text = "You feel a bit bad", action = () =>
                { StatHandler.instance.SocialStanding -= 5f; } },
            },
        }},
        {"Join_DND_Club", new CardData{
            title = "Join the DND Club?",
            description = "A classmate invites you to a weekly DND group. Do you join the club?",
            imageID = "join_dnd_club",
            unique = true,
            investment = false,
            triggerKey = "joined_dnd_club",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "It becomes a weekly highlight", action = () =>
                { StatHandler.instance.SocialStandingChange += 7f; StatHandler.instance.MentalHealthChange += 4f; } },
                new EffectData() {positive = false, text = "Less time on your hands", action = () =>
                { StatHandler.instance.SchoolPerformanceChange -= 5f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = false, text = "You miss out on fun and friends", action = () =>
                { StatHandler.instance.SocialStanding -= 8f; } },
            },
        }},
        {"Family_Dinner", new CardData{
            title = "Family dinner invite",
            description = "Your parents invite you over for a home-cooked meal. Do you visit?",
            imageID = "family_dinner",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Comfort food and love", action = () =>
                { StatHandler.instance.MentalHealth += 7f; StatHandler.instance.SocialStanding += 8f; } },
                new EffectData() {positive = true, text = "Save money on dinner", action = () =>
                { StatHandler.instance.Money += 15f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You stay in and rest instead", action = () =>
                { StatHandler.instance.MentalHealth += 18f; } },
            },
        }},
        {"Join_Theatre_Crew", new CardData{
            title = "Join Theatre Crew?",
            description = "The school theatre group needs help backstage. You could join. Do you sign up?",
            imageID = "join_theatre_group",
            unique = true,
            investment = false,
            triggerKey = "join_theatre_group",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You make new friends", action = () =>
                { StatHandler.instance.SocialStandingChange += 6f; } },
                new EffectData() {positive = true, text = "There is much to learn", action = () =>
                { StatHandler.instance.SchoolPerformanceChange += 3f; } },
                new EffectData() {positive = false, text = "Time-consuming commitment", action = () =>
                { StatHandler.instance.MentalHealthChange -= 3f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = false, text = "You miss a rare opportunity", action = () =>
                { StatHandler.instance.SocialStanding -= 6f; } },
            },
        }},
        {"Clean_Airbnb", new CardData{
            title = "Clean an Airbnb?",
            description = "A neighbor offers a weekend gig cleaning their rental flat. It pays well. Do you take it?",
            imageID = "clean_airbnb",
            unique = false,
            investment = false,
            triggerKey = "cleaned_airbnb_once",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You get paid fast", action = () =>
                { StatHandler.instance.Money += 120f; } },
                new EffectData() {positive = false, text = "It's exhausting work", action = () =>
                { StatHandler.instance.MentalHealth -= 12f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You relax this weekend", action = () =>
                { StatHandler.instance.MentalHealth += 7f; } },
                new EffectData() {positive = false, text = "Missed extra income", action = () =>
                { } },
            },
        }},
        {"Friend_Sneaker_Flip", new CardData{
            title = "Sneaker Flip Scheme?",
            description = "Your friend has a plan to flip sneakers for a quick profit. It sounds hyped. Do you invest?",
            imageID = "sneaker_flip_scheme",
            unique = false,
            investment = true,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "If it works, you double up", action = () =>
                { StatHandler.instance.Money -= 200f; StatHandler.instance.MoneyChange += 5f; } },
                new EffectData() {positive = false, text = "It's riskier than it seems", action = () =>
                { StatHandler.instance.MoneyChange -= 10f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You avoid a risky play", action = () =>
                { StatHandler.instance.Money += 10f; } },
                new EffectData() {positive = false, text = "Your friend seems disappointed", action = () =>
                { StatHandler.instance.SocialStanding -= 2f; } },
            },
        }},
        {"All_Nighter_Plan", new CardData{
            title = "Pull an All-Nighter?",
            description = "You've got momentum. Do you stay up late to finish next week's assignments early?",
            imageID = "all_nighter_plan",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You stay ahead of the curve", action = () =>
                { StatHandler.instance.SchoolPerformance += 15f; } },
                new EffectData() {positive = false, text = "You burn out midweek", action = () =>
                { StatHandler.instance.MentalHealth -= 8f; StatHandler.instance.MentalHealthChange -= 4f; StatHandler.instance.SchoolPerformanceChange -= 5f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You rest and pace yourself", action = () =>
                { StatHandler.instance.MentalHealth += 18f; } },
                new EffectData() {positive = false, text = "You're a little behind schedule", action = () =>
                { StatHandler.instance.SchoolPerformance -= 8f; } },
            },
        }},
        {"Group_Teasing", new CardData{
            title = "Join in the Teasing?",
            description = "Some friends poke fun at a classmate in group chat. You can join in or stay quiet.",
            imageID = "group_teasing",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "They laugh and bond with you", action = () =>
                { StatHandler.instance.SocialStanding += 15f; } },
                new EffectData() {positive = false, text = "Others take note quietly", action = () =>
                { StatHandler.instance.SocialStandingChange -= 7f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You show maturity", action = () =>
                { StatHandler.instance.MentalHealth += 12f; StatHandler.instance.SocialStandingChange += 3f; } },
                new EffectData() {positive = false, text = "You feel slightly left out", action = () =>
                { StatHandler.instance.SocialStanding -= 5f; } },
            },
        }},
        {"Smart_Youtube_Video", new CardData{
            title = "Watch a 'Smart' Video?",
            description = "You're meant to study, but that educational video looks interesting. Watch it instead?",
            imageID = "smart_youtube_video",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "It's informative and relaxing", action = () =>
                { StatHandler.instance.MentalHealth += 12f; } },
                new EffectData() {positive = false, text = "But not on topic", action = () =>
                { StatHandler.instance.SchoolPerformance -= 14f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You stay focused on studying", action = () =>
                { StatHandler.instance.SchoolPerformance += 16f; } },
                new EffectData() {positive = false, text = "You miss a small mental break", action = () =>
                { StatHandler.instance.MentalHealth -= 5f; } },
            },
        }},
        {"Limited_Time_Meal_Deal", new CardData{
            title = "Grab the Meal Deal?",
            description = "Your favorite fast-food chain has a limited-time combo deal. It's still more than cooking at home. Do you get it?",
            imageID = "limited_time_meal_deal",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Tasty and convenient", action = () =>
                { StatHandler.instance.MentalHealth += 5f; StatHandler.instance.Money -= 50f; } },
                new EffectData() {positive = false, text = "Not great for your wallet long-term", action = () =>
                { StatHandler.instance.MoneyChange -= 5f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You save by eating in", action = () =>
                { StatHandler.instance.Money += 10f; } },
                new EffectData() {positive = false, text = "Feels like you're missing out", action = () =>
                { StatHandler.instance.MentalHealth -= 1f; } },
            },
        }},
        {"Casino_Night", new CardData{
            title = "Test your luck?",
            description = "Your friends are going to the casino for fun. You've got a bit of extra cash. Do you join them?",
            imageID = "casino_night",
            unique = false,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Thrill and laughs with friends", action = () =>
                { StatHandler.instance.MentalHealth += 12f; StatHandler.instance.SocialStanding += 7f; } },
                new EffectData() {positive = false, text = "The losses sting later", action = () =>
                { StatHandler.instance.Money -= 60f; StatHandler.instance.MoneyChange -= 8f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You play it safe and skip it", action = () =>
                { StatHandler.instance.Money += 10f; } },
                new EffectData() {positive = false, text = "You miss out on social time", action = () =>
                { StatHandler.instance.SocialStanding -= 10f; } },
            },
        }},
        {"Airbnb_Cleaning_Gig", new CardData{
            title = "Take Regular Airbnb Gig?",
            description = "The owner was impressed. They offer you a regular cleaning gig. Do you accept?",
            imageID = "airbnb_regular_cleaning",
            unique = true,
            investment = false,
            requiredTrigger = "cleaned_airbnb_once",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Steady weekend income", action = () =>
                { StatHandler.instance.MoneyChange += 50f; } },
                new EffectData() {positive = false, text = "Less time for rest and friends", action = () =>
                { StatHandler.instance.MentalHealthChange -= 3f; StatHandler.instance.SocialStandingChange -= 2f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You keep your weekends free", action = () =>
                { StatHandler.instance.MentalHealth += 18f; } },
                new EffectData() {positive = false, text = "You miss steady income", action = () =>
                { } },
            },
        }},
        {"Study_Club_Leadership", new CardData{
            title = "Join Study Club Leadership?",
            description = "They ask if you'll take on a leadership role. It's more work, but it looks great. Accept?",
            imageID = "study_club_leader",
            unique = true,
            investment = true,
            requiredTrigger = "joined_study_club",
            triggerKey = "study_club_leader",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Great on your resume", action = () =>
                { StatHandler.instance.SchoolPerformanceChange += 8f; StatHandler.instance.SocialStandingChange += 4f; } },
                new EffectData() {positive = false, text = "Extra coordination and stress", action = () =>
                { StatHandler.instance.MentalHealthChange -= 4f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You avoid extra pressure", action = () =>
                { StatHandler.instance.MentalHealth += 18f; } },
                new EffectData() {positive = false, text = "You miss a leadership opportunity", action = () =>
                { StatHandler.instance.SchoolPerformanceChange -= 8f; } },
            },
        }},
        {"Drive_Friends_Round", new CardData{
            title = "Drive Friends Around?",
            description = "Your friends keep asking for rides. It could build bonds, but costs time. Do you do it?",
            imageID = "drive_friends",
            unique = false,
            investment = false,
            requiredTrigger = "bought_car",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You're a hero to your friends", action = () =>
                { StatHandler.instance.SocialStanding += 25f; } },
                new EffectData() {positive = false, text = "Fuel costs and fatigue", action = () =>
                { StatHandler.instance.Money -= 40f; StatHandler.instance.MentalHealth -= 5f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You prioritize your own schedule", action = () =>
                { StatHandler.instance.MentalHealth += 12f; } },
                new EffectData() {positive = false, text = "You seem less helpful", action = () =>
                { StatHandler.instance.SocialStanding -= 8f; } },
            },
        }},
        {"Start_DND_Campaign", new CardData{
            title = "Start a DND Campaign?",
            description = "The group wants you to run the next campaign. Do you take the lead?",
            imageID = "start_dnd_campaign",
            unique = true,
            investment = true,
            requiredTrigger = "joined_dnd_club",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Big social win", action = () =>
                { StatHandler.instance.SocialStandingChange += 6f; } },
                new EffectData() {positive = false, text = "More prep and commitment", action = () =>
                { StatHandler.instance.MentalHealthChange -= 2f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You avoid the pressure", action = () =>
                { } },
                new EffectData() {positive = false, text = "Someone else steps up", action = () =>
                { StatHandler.instance.SocialStanding -= 10f; } },
            },
        }},
        {"Negotiate_Raise", new CardData{
            title = "Negotiate a Raise?",
            description = "You've been doing well at your job. Ask for a raise?",
            imageID = "negotiate_raise",
            unique = true,
            investment = false,
            requiredTrigger = "student_job_acquired",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "They agree!", action = () =>
                { StatHandler.instance.MoneyChange += 25f; } },
                new EffectData() {positive = false, text = "They expect more now", action = () =>
                { StatHandler.instance.MentalHealthChange -= 2f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You stay in your comfort zone", action = () =>
                { StatHandler.instance.MentalHealth += 12f; } },
                new EffectData() {positive = false, text = "You miss a chance for more", action = () =>
                { StatHandler.instance.SocialStanding -= 6f; } },
            },
        }},
        {"Start_Tutoring_Program", new CardData{
            title = "Start a Tutoring Program?",
            description = "You're asked to organize peer tutoring at school. It'll take effort. Do you do it?",
            imageID = "tutoring_program",
            unique = true,
            investment = true,
            requiredTrigger = "study_club_leader",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Great experience and community respect", action = () =>
                { StatHandler.instance.SchoolPerformanceChange += 10f; StatHandler.instance.SocialStandingChange += 6f; } },
                new EffectData() {positive = false, text = "It's very time-consuming", action = () =>
                { StatHandler.instance.MentalHealthChange -= 5f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You keep your free time", action = () =>
                { StatHandler.instance.MentalHealth += 6f; } },
                new EffectData() {positive = false, text = "Your club is a bit let down", action = () =>
                { StatHandler.instance.SocialStanding -= 4f; } },
            },
        }},
        {"Perform_School_Play", new CardData{
            title = "Perform in the School Play?",
            description = "You get offered a role in the upcoming play. It's a big time ask. Do you accept?",
            imageID = "school_play",
            unique = true,
            investment = false,
            requiredTrigger = "join_theatre_group",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Spotlight and applause", action = () =>
                { StatHandler.instance.SocialStanding += 40f; } },
                new EffectData() {positive = false, text = "It takes a toll on your studies", action = () =>
                { StatHandler.instance.SchoolPerformance -= 15f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You keep your routine intact", action = () =>
                { StatHandler.instance.SchoolPerformance += 8f; } },
                new EffectData() {positive = false, text = "You miss your moment", action = () =>
                { StatHandler.instance.SocialStanding -= 5f; } },
            },
        }},
        {"Weekly_Cooking_Offer", new CardData{
            title = "Cook for Friends Weekly?",
            description = "Your friends loved your cooking. They ask if you'll do it every week. Say yes?",
            imageID = "weekly_meal_prep",
            unique = true,
            investment = false,
            requiredTrigger = "meal_prep_mastered",
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Your friends enjoy the food", action = () =>
                { StatHandler.instance.SocialStandingChange += 5f; } },
                new EffectData() {positive = true, text = "You get some economic help", action = () =>
                { StatHandler.instance.MoneyChange += 15f;  } },
                new EffectData() {positive = false, text = "It takes a lot of time", action = () =>
                { StatHandler.instance.MentalHealthChange -= 3f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You keep it casual", action = () =>
                { } },
                new EffectData() {positive = false, text = "Friends are a bit disappointed", action = () =>
                { StatHandler.instance.SocialStanding -= 8f; } },
            },
        }},
        {"Join_Fitness_Club", new CardData{
            title = "Join a Fitness Club?",
            description = "You found a cheap student gym membership. It could boost your energy. Join?",
            imageID = "fitness_membership",
            unique = true,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Regular workouts clear your head", action = () =>
                { StatHandler.instance.MentalHealthChange += 7f; } },
                new EffectData() {positive = false, text = "Ongoing payment", action = () =>
                { StatHandler.instance.MoneyChange -= 15f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Save the membership cost", action = () => { } },
                new EffectData() {positive = false, text = "Miss a chance to boost health", action = () =>
                { StatHandler.instance.MentalHealth -= 8f; } },
            },
        }},
        {"Nature_Walk_Habit", new CardData{
            title = "Start Nature Walks?",
            description = "You find a peaceful park nearby. Commit to weekly nature walks?",
            imageID = "nature_walks",
            unique = true,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Nature heals the mind", action = () =>
                { StatHandler.instance.MentalHealthChange += 6f; } },
                new EffectData() {positive = false, text = "Slight missed study and friend time", action = () =>
                { StatHandler.instance.SchoolPerformanceChange -= 1f; StatHandler.instance.SocialStandingChange -= 1f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "No new habits formed", action = () => { } },
                new EffectData() {positive = false, text = "You miss peaceful downtime", action = () =>
                { StatHandler.instance.MentalHealth -= 6f; } },
            },
        }},
        {"Join_Meditation_Group", new CardData{
            title = "Join Meditation Group?",
            description = "A meditation group meets weekly at school. It has a membership fee. Would you join?",
            imageID = "meditation_group",
            unique = true,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You become more balanced", action = () =>
                { StatHandler.instance.MentalHealthChange += 8f; } },
                new EffectData() {positive = false, text = "Small weekly membership fee", action = () =>
                { StatHandler.instance.MoneyChange -= 10f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Save your money", action = () => { } },
                new EffectData() {positive = false, text = "High stress remains", action = () =>
                { StatHandler.instance.MentalHealth -= 8f; } },
            },
        }},
        {"Buy_Study_Desk", new CardData{
            title = "Buy a Better Study Desk?",
            description = "Your current desk is terrible. A sturdy new one could help studying. Buy it?",
            imageID = "buy_study_desk",
            unique = true,
            investment = false,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Better posture, better focus", action = () =>
                { StatHandler.instance.SchoolPerformanceChange += 6f; } },
                new EffectData() {positive = false, text = "High quality is expensive", action = () =>
                { StatHandler.instance.Money -= 180f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You save your money", action = () => { } },
                new EffectData() {positive = false, text = "Long hours stay uncomfortable", action = () =>
                { StatHandler.instance.SchoolPerformance -= 10f; } },
            },
        }},
        { "Friend_Offered_Investment", new CardData{
            title = "Recommended investment",
            description = "A pretty experienced trader friend of yours has given you a recommendation for an investment. Wanna give it a go?",
            imageID = "friend_offered_investment",
            unique = true,
            investment = true,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "It'll probably pay off well", action = () =>
                {
                    StatHandler.instance.Investments.Add(new Investment()
                    {
                        Name = "Friend's Investment Oppertunity",
                        Amount = 600,
                        Recurring = 0,
                        Returns = 1.05f,
                        Rate = 3,
                    });
                } },
                new EffectData() {positive = false, text = "It's a pricey investment", action = () =>
                { StatHandler.instance.Money -= 600f;  } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Avoid spending your money", action = () =>
                { } },
                new EffectData() {positive = false, text = "It's a one-time oppertunity", action = () =>
                { StatHandler.instance.MentalHealth -= 10; } },
            },
        }},
        {"Invest_Local_Cafe", new CardData{
            title = "Invest in Local Café?",
            description = "A classmate's cousin is opening a café and offers you a small stake. Interested?",
            imageID = "invest_local_cafe",
            unique = true,
            investment = true,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Could grow with the neighborhood", action = () =>
                {
                    StatHandler.instance.Investments.Add(new Investment()
                    {
                        Name = "Café Investment",
                        Amount = 800,
                        Recurring = 0,
                        Returns = 1.08f,
                        Rate = 4,
                    });
                } },
                new EffectData() {positive = false, text = "Is a sizable investment", action = () =>
                { StatHandler.instance.Money -= 800f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Avoid risky ventures", action = () => { } },
                new EffectData() {positive = false, text = "It might've become the next hotspot", action = () =>
                { StatHandler.instance.MentalHealth -= 8f; } },
            },
        }},
        {"Invest_3D_Printer", new CardData{
            title = "Buy a 3D Printer?",
            description = "You can buy a 3D printer cheap and sell custom prints online. Worth the setup?",
            imageID = "3d_printer_investment",
            unique = true,
            investment = true,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Could be a nice side income", action = () =>
                {
                    StatHandler.instance.Investments.Add(new Investment()
                    {
                        Name = "3D Printing Side Hustle",
                        Amount = 1000,
                        Recurring = 0,
                        Returns = 1.06f,
                        Rate = 3,
                    });
                } },
                new EffectData() {positive = false, text = "3D printers are expensive", action = () =>
                { StatHandler.instance.Money -= 1000f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You don't take the risk", action = () => { } },
                new EffectData() {positive = false, text = "You might regret not trying", action = () =>
                { StatHandler.instance.MentalHealth -= 12f; } },
            },
        }},
        {"Micro_Invest_App", new CardData{
            title = "Use Micro-Investment App?",
            description = "A new app invests a small amount of your weekly income. Returns grow over time. Try it?",
            imageID = "micro_invest_app",
            unique = true,
            investment = true,
            right = new List<EffectData>
            {
                new EffectData() {positive = true, text = "Effortless growth", action = () =>
                {
                    StatHandler.instance.Investments.Add(new Investment()
                    {
                        Name = "Micro-Invest App",
                        Amount = 100,
                        Recurring = 50,
                        Returns = 1.03f,
                        Rate = 2,
                    });
                } },
                new EffectData() {positive = false, text = "It chips away on weekly income", action = () =>
                { StatHandler.instance.Money -= 100f; StatHandler.instance.MoneyChange -= 50f; } },
            },
            left = new List<EffectData>
            {
                new EffectData() {positive = true, text = "You keep things simple", action = () => { } },
                new EffectData() {positive = false, text = "You might miss easy gains", action = () =>
                { StatHandler.instance.MentalHealth -= 5f; } },
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
    public string imageID;
    public bool unique;
    public bool investment;
    public string requiredTrigger;
    public string triggerKey;

    public List<EffectData> right;
    public List<EffectData> left;
}