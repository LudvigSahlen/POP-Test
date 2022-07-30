using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Graph : MonoBehaviour
{
    public Bank myBank;
    public Marketplace myMarket;
    public Master myMaster;
    public float myValue, myUpdateFreq = 1f, myGraphModifier = 0.001f;
    public TMP_Text myTopText, myBottomText, myActualText;
    public string myGraphedValue;

    public GameObject myActualGO, myBottomGO, myTopGO, myCircle, myBackground;

    float myTop, myBottom, myTimer, myRange, mySavedValue, myTimer2;
    void Start()
    {
        myRange = myTopGO.transform.localPosition.y * 0.9f;
        mySavedValue = 1000;
    }

    void FixedUpdate()
    {
        switch (myGraphedValue)
        {
            case "Consumption":
                myValue = myMaster.myTotConsumption;
                break;
            case "Produce":
                myValue = myMaster.myTotProduction;
                break;
            case "Total":
                myValue = myMarket.myStoredFood;
                break;
            case "Money":
                myValue = myMaster.myTotMoney;
                break;
            case "FoodPrice":
                myValue = myMarket.RetrievePrice("Food");
                break;
            case "FoodTraded":
                myValue = myMarket.myDailyFoodTrades;
                break;
            case "MoneyDelta":
                myValue = myBank.myMoneyDelta;
                break;
            default:
                break;
        }

        if (myValue != 0)
        {
            myTimer2 += Time.fixedDeltaTime;
            myTimer += Time.fixedDeltaTime;

            if (myValue > 10 || myValue < -10)
                myValue = Mathf.Round(myValue);
            else
                myValue = (float)Math.Round(myValue, 2);

            myActualText.text = myValue.ToString();
            if (myValue > mySavedValue)
            {
                myActualGO.transform.localPosition = new Vector2(myActualGO.transform.localPosition.x, myActualGO.transform.localPosition.y + (((myValue - mySavedValue) * myGraphModifier))
                    - (myTopGO.transform.localPosition.y * (myActualGO.transform.localPosition.y / myTopGO.transform.localPosition.y)) * 0.15f);
            }
            else if (myValue < mySavedValue)
            {
                myActualGO.transform.localPosition = new Vector2(myActualGO.transform.localPosition.x, myActualGO.transform.localPosition.y - ((mySavedValue - myValue) * myGraphModifier)
                    + (myBottomGO.transform.localPosition.y * (myActualGO.transform.localPosition.y / myTopGO.transform.localPosition.y)) * 0.15f);
            }
            FindActualPos();

            if (myTimer2 > 0.01f)
            {
                myTimer2 = 0;
                GameObject tempCircle = Instantiate(myCircle, gameObject.transform);
                tempCircle.GetComponent<graphObj>().myTop = myTopGO;
                switch (myGraphedValue)
                {
                    case "Consumption":
                        tempCircle.GetComponent<SpriteRenderer>().color = Color.red;
                        break;
                    case "Produce":
                        tempCircle.GetComponent<SpriteRenderer>().color = Color.green;
                        break;
                    case "Total":
                        tempCircle.GetComponent<SpriteRenderer>().color = Color.blue;
                        break;
                    case "Money":
                        tempCircle.GetComponent<SpriteRenderer>().color = Color.yellow;
                        break;
                    case "FoodPrice":
                        tempCircle.GetComponent<SpriteRenderer>().color = Color.magenta;
                        break;
                    case "FoodTraded":
                        tempCircle.GetComponent<SpriteRenderer>().color = Color.blue;
                        break;
                    case "MoneyDelta":
                        tempCircle.GetComponent<SpriteRenderer>().color = Color.cyan;
                        break;
                    default:
                        break;
                }
                tempCircle.transform.localPosition = new Vector2(myActualGO.transform.localPosition.x + 4.1f, myActualGO.transform.localPosition.y + 0.88f);
            }
                if (myTimer > myUpdateFreq)
                {
                    myTop = myValue * 1.2f;
                    myBottom = myValue * 0.8f;

                    if (myValue > 10 || myValue < -10)
                    {
                        myTop = Mathf.Round(myTop);
                        myBottom = Mathf.Round(myBottom);
                    }
                    else
                    {
                        myTop = (float)Math.Round(myTop, 2);
                        myBottom = (float)Math.Round(myBottom, 2);
                    }

                    myTopText.text = myTop.ToString();
                    myBottomText.text = myBottom.ToString();

                    mySavedValue = myValue;

                    myTimer = 0;
                }
        }
    }

    void FindActualPos()
    {
        if(myActualGO.transform.localPosition.y > myRange)
        {
            myActualGO.transform.localPosition = new Vector2(myActualGO.transform.localPosition.x, myRange);
        }
        else if (myActualGO.transform.localPosition.y < -myRange)
        {
            myActualGO.transform.localPosition = new Vector2(myActualGO.transform.localPosition.x, -myRange);
        }
    }
}
