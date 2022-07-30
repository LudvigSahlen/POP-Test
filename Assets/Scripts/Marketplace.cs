using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Marketplace : MonoBehaviour
{
    public Bank myBank;
    public Master myMaster;
    public float myStoredFood, myMaxStoredFood;
    public float myDailyFoodTrades, myDesiredSells, myDesiredBuys;
    public List<float> myCommodityPriceList = new List<float>();

    public float myFoodBasePrice = 1f, myFoodActualPrice, myExcutedFoodTrades, myFoodAveragePrice;

    public List<Order> myBuyOrders, mySellOrders;
    public bool myRunMarket;

    void Start()
    {
        myFoodAveragePrice = myFoodBasePrice;
        myRunMarket = true;
        myBuyOrders = new List<Order>();
        mySellOrders = new List<Order>();

        myMaxStoredFood = 1000;
        myCommodityPriceList.Add(1);
    }

    public float RetrievePrice(string aString)
    {
        if(aString == "Food")
        {
            return myFoodAveragePrice;
        }
        return 1;
    }

    public float Sell(string aString, float aAmount)
    {
        if (aString == "Food")
        {
            myStoredFood += aAmount;
        }

        myDailyFoodTrades -= aAmount;
        return aAmount * RetrievePrice(aString); //Returns how much money is made from the sold items.
    }
    public float BuyLimited(string aString, float aAmount)
    {
        if (aString == "Food")
        {
            if (myStoredFood > aAmount)
            {
                myStoredFood -= aAmount / RetrievePrice(aString);
            }
            else
            {
                aAmount = myStoredFood / RetrievePrice(aString);
            }
        }
        myDailyFoodTrades += aAmount;
        return aAmount/RetrievePrice(aString); //Returns how many of that item is bought 
    }
    public void PriceAdjust(string aString, float aTime)
    {
        if (aString == "Food")
        {
           /* if(myDailyFoodTrades > 0)
            {
                myFoodActualPrice += ((myFoodStartPrice * aTime) * (myDailyFoodTrades / 100) * (1 + myDesiredBuys));
            }
            else if(myDailyFoodTrades < 0)
            {
                myFoodActualPrice -= (myFoodStartPrice * aTime) * -myDailyFoodTrades / 100;
            }
            MyFoodTargetPrice = myFoodStartPrice + -(myDailyFoodTrades);*/
        }
        //myFoodActualPrice = (float)Math.Round(myFoodActualPrice, 2);
    }
    public void DesiredTrade(string aType, float anAmount, bool aSellorBuy)
    {
        if(aType == "Food")
        {
            if(!aSellorBuy)
            myDesiredBuys += anAmount;
        }
    }

    public void MakeOrder(string aGood, string aType, float aMonetaryAmount, float aGoodAmount, POP aPop)
    {
        Order tempOrder = new Order();
        tempOrder.myGood = aGood;
        tempOrder.myType = aType;
        tempOrder.myMonetaryAmount = aMonetaryAmount;
        tempOrder.myGoodAmount = aGoodAmount;
        tempOrder.myPOP = aPop;
        tempOrder.myAskingPrice = aMonetaryAmount / aGoodAmount;

        if (aType == "buy")
            myBuyOrders.Add(tempOrder);
        else if(aType == "sell")
            mySellOrders.Add(tempOrder);
    }
    public void MakeFinishedOrder(Order anOrder)
    {
        if (anOrder.myType == "buy")
            myBuyOrders.Add(anOrder);
        else if (anOrder.myType == "sell")
            mySellOrders.Add(anOrder);
    }

    public void MarketMaker()
    {
        myBuyOrders.Sort(delegate (Order x, Order y) { return y.myAskingPrice.CompareTo(x.myAskingPrice); });
        mySellOrders.Sort(delegate(Order x, Order y) { return x.myAskingPrice.CompareTo(y.myAskingPrice); });
        myRunMarket = true;

        while (myRunMarket)
        {
            myRunMarket = false;
            RunMarket();
        }
    }

    private void RunMarket()
    {
        for (int i = 0; i < mySellOrders.Count; i++)
        {
            for (int j = 0; j < myBuyOrders.Count; j++)
            {
                if (myBuyOrders[j].myPOP == null)
                    Debug.Log("null ask: " + myBuyOrders[j].myAskingPrice);

                if (myBuyOrders[j].myAskingPrice >= mySellOrders[i].myAskingPrice)
                {
                    float tempBuyAmount;

                    if (myBuyOrders[j].myGoodAmount >= mySellOrders[i].myGoodAmount)
                    {
                        tempBuyAmount = mySellOrders[i].myGoodAmount;
                    }
                    else
                    {
                        tempBuyAmount = myBuyOrders[j].myGoodAmount;
                    }

                    if(myBuyOrders[j].myPOP == null)
                    {
                        Debug.Log("null");
                        myBank.myCommodityStorage[0] += tempBuyAmount;
                        myBank.myMoney -= tempBuyAmount * mySellOrders[i].myAskingPrice;
                        myBuyOrders[j].myGoodAmount -= tempBuyAmount;
                    }
                    else
                    {
                        Debug.Log("not null");
                        myBuyOrders[j].myPOP.myStoredFood += tempBuyAmount;
                        myBuyOrders[j].myPOP.myMoney -= tempBuyAmount * mySellOrders[i].myAskingPrice;
                        myBuyOrders[j].myGoodAmount -= tempBuyAmount;
                    }

                    if(mySellOrders[i].myPOP == null)
                    {
                        myBank.myCommodityStorage[0] -= tempBuyAmount;
                        myBank.myMoney += tempBuyAmount * mySellOrders[i].myAskingPrice;
                        mySellOrders[i].myGoodAmount -= tempBuyAmount;
                    } 
                    else
                    {
                        mySellOrders[i].myPOP.myStoredFood -= tempBuyAmount;
                        mySellOrders[i].myPOP.myMoney += tempBuyAmount * mySellOrders[i].myAskingPrice;
                        mySellOrders[i].myGoodAmount -= tempBuyAmount;
                    }

                    myFoodActualPrice += mySellOrders[i].myAskingPrice;
                    myExcutedFoodTrades += 1;
                    myFoodAveragePrice = myFoodActualPrice / myExcutedFoodTrades;

                    if (myBuyOrders[j].myGoodAmount <= 0)
                    {
                        myBuyOrders.RemoveAt(j);
                    }
                    if (mySellOrders[i].myGoodAmount <= 0)
                    {
                        mySellOrders.RemoveAt(i);
                    }
                    //Debug.Log(mySellOrders[i].myAskingPrice);

                   myRunMarket = true;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
