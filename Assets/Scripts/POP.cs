using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POP
{
    public Bank myBank;
    public Master myMaster;
    public Marketplace myMarket;
    public int myType; //0 = Farmer, 1 = Craftsman, 2 = Engineer, 3 = Wizard
    public float myConsumption, myProduce, myMoney = 3, myHunger, mySurplus, myStoredFood, myEarnedMoney;
    public bool myDead = false, myPayTax = false;

    float myBaseConsumption = 1, myBaseProduce = 1.01f, myMaxStoredFood = 10, myFoodDeath = 30, mySavedMoney;
    public void DayUpdate()
    {
        if (!myDead)
        {
            myConsumption = myBaseConsumption * TypeModifier("Consumption", myType) * (Random.Range(myMaster.myRNGModifiers[2], myMaster.myRNGModifiers[3]) / 10);
            myHunger += myConsumption;

            if (myStoredFood > myMaxStoredFood)
                myStoredFood = myMaxStoredFood;

            myPayTax = false;
            if (myMoney > mySavedMoney)
            {
                myEarnedMoney = myMoney - mySavedMoney;
                myPayTax = true;
            }
            mySavedMoney = myMoney;

            if (myPayTax)
            {
                float tempTax = myEarnedMoney * myBank.myTaxRates[myType];
                myBank.myMoney += tempTax;
                myMoney -= tempTax;
                myBank.myMoneyDelta += tempTax;
            }

            switch (myType)
            {
                case 0:
                    FarmerUpdate();
                    break;
                case 1:
                    CraftsmenUpdate();
                    break;
                case 2:
                    EngineerUpdate();
                    break;
                case 3:
                    WizardUpdate();
                    break;
                default:
                    break;
            }
            if (myHunger >= myFoodDeath)
            {
                myDead = true;
                Debug.Log("dead: "+ myType); 
            }
        }
    }

    float TypeModifier(string aModifier, int aType)
    {
        if(aModifier == "Consumption")
        {
            switch (myType)
            {
                case 0:
                    return 1;
                case 1:
                    return 1.5f;
                case 2:
                    return 3;
                case 3:
                    return 3;
                default:
                    break;
            }
        }
        return 1;
    }

    void FarmerUpdate()
    {
        myProduce = myBaseProduce * (Random.Range(myMaster.myRNGModifiers[0], myMaster.myRNGModifiers[1]) / 10);
        myStoredFood += myProduce;

        Consume();
    }

    void CraftsmenUpdate()
    {
        myMoney += 5;


        Consume();
    }
    void EngineerUpdate()
    {
        myMoney += 8;
        Consume();
    }
    void WizardUpdate()
    {
        myMoney += 8;

        Consume();
    }
    void Consume()
    {
        if (myStoredFood >= myHunger)
        {
            myStoredFood = myStoredFood - myHunger;
            myHunger = 0;
        }
        else
        {
            myHunger = myHunger - myStoredFood;
            myStoredFood = 0;
        }

        if (myStoredFood > 0)
        {
            myMarket.MakeOrder("food", "sell", WillingToPay("sell", "food"), myStoredFood, this);
        }
        else
        {
            myMarket.MakeOrder("food", "buy", WillingToPay("buy", "food"), myHunger, this);
        }
    }
    float WillingToPay(string aType, string aGood)
    {
        if (aGood == "food")
        {
            if (aType == "buy")
            {
                //float tempPay = (myMarket.myFoodBasePrice * (myHunger / 2)) * myHunger;
                float tempPay = (myMarket.myFoodBasePrice * myHunger) * (myHunger / 10) + (myMoney / 20);
                if (tempPay < myMoney && myHunger > myFoodDeath * 0.85f)
                {
                    return tempPay;
                }
                else return myMoney;
            }
            else if (aType == "sell")
            {
                float tempPay = myMarket.myFoodBasePrice * ((myMaxStoredFood / 3) / myStoredFood);
                if(tempPay <= myMarket.myFoodBasePrice * 20)
                {
                    return myMarket.myFoodBasePrice * 20;
                }
                else
                {
                    return tempPay;
                }
            }
        }

        return 0;
    }
}