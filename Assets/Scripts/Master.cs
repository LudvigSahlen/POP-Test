using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    public Bank myBank;
    public Seasons mySeasons;
    public Marketplace myMarket;
    public bool myRunning = false;
    public List<POP> myPOPList;
    public List<int> myIDList;
    public int myDay, myTotalGraphObj;
    public List<float> myRNGModifiers;
    public List<Order> myBankOrders;
    
    public float myTimer, myTimer2, myTotConsumption, myTotProduction, myTotMoney, myTotHunger;
    private float myResetTimer;

    void Start()
    {
        myBankOrders = new List<Order>();
        myRNGModifiers.Add(9f);
        myRNGModifiers.Add(11f);
        myRNGModifiers.Add(9f);
        myRNGModifiers.Add(11f);
    }

    void FixedUpdate()
    {
        if (myRunning)
        {
            myResetTimer++;
            myTimer += Time.fixedDeltaTime;
            myTimer2 += Time.fixedDeltaTime;
            if (myTimer > 1)
            {
                myTimer = 0;
                if(myDay > 300)
                {
                    myDay = 0;
                }
                else
                {
                   myDay++;
                }
                //Debug.Log("Day: " + myDay);

                mySeasons.SeasonUpdate();
                mySeasons.GetSeasonalModifier();

                myMarket.myBuyOrders.Clear();
                myMarket.mySellOrders.Clear(); 

                if (myResetTimer == 20)
                {
                    myResetTimer = 0;
                    myMarket.myFoodActualPrice = 0;
                    myMarket.myDailyFoodTrades = 0;
                }

                myMarket.myDailyFoodTrades = 0;
                myTotMoney = 0;
                myTotHunger = 0;
                myTotConsumption = 0;
                myTotProduction = 0;
                myBank.myMoneyDelta = 0;

                myMarket.myDesiredBuys = 0;
                myMarket.myDesiredSells = 0;

                if (myMarket.myStoredFood > myMarket.myMaxStoredFood)
                {
                    myMarket.myStoredFood = myMarket.myMaxStoredFood;
                }
                else if(myMarket.myStoredFood < 0)
                {
                    myMarket.myStoredFood = 0;
                }

                for (int i = 0; i < myPOPList.Count; i++)
                {
                    myPOPList[i].DayUpdate();

                    myTotConsumption += myPOPList[i].myConsumption;
                    myTotProduction += myPOPList[i].myProduce;
                    myTotHunger += myPOPList[i].myHunger;
                    myTotMoney += myPOPList[i].myMoney;

                    if(myPOPList[i].myDead)
                    {
                        myPOPList.RemoveAt(i);
                    }
                }
                Debug.Log(myBankOrders.Count);
                BankOrders("add");
                myMarket.MarketMaker();
                BankOrders("pull");
            }
            if(myTimer2 > 0.25f)
            {
                myTimer2 = 0;
                myMarket.PriceAdjust("Food", Time.fixedDeltaTime);
            }

        }
    }
    private void BankOrders(string aType)
    {
        if (aType == "add")
        {
            for (int i = 0; i < myBankOrders.Count; i++)
            {
                myMarket.MakeFinishedOrder(myBankOrders[i]);
            }
        }
        else if(aType == "pull")
        {
            myBankOrders.Clear();

            for (int i = 0; i < myMarket.myBuyOrders.Count; i++)
            {
                if (myMarket.myBuyOrders[i].myPOP == null)
                {
                    myBankOrders.Add(myMarket.myBuyOrders[i]);
                }
            }

            for (int i = 0; i < myMarket.mySellOrders.Count; i++)
            {
                if (myMarket.mySellOrders[i].myPOP == null)
                {
                    myBankOrders.Add(myMarket.mySellOrders[i]);
                }
            }
        }
    }
}
