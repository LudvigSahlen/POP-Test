using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Bank : MonoBehaviour
{
    public Master myMaster;
    public float myMoney, myMoneyDelta;
    public List<float> myTaxRates, myTaxBreaks, myCommodityStorage;
    public Marketplace myMarket;
    public TMP_Dropdown myDropDown;
    public TMP_InputField myInput1, myInput2;
    public Button myButton;
    public TMP_Text myPriceText;
    void Start()
    {
        myCommodityStorage = new List<float>();
        myCommodityStorage.Add(0); //Food

        myTaxRates.Add(0.2f);
        myTaxRates.Add(0.1f);
        myTaxRates.Add(0.1f);
        myTaxRates.Add(0.1f); //These should not exceed 1

        myTaxBreaks.Add(1f);
        myTaxBreaks.Add(3f);
        myTaxBreaks.Add(5f);
        myTaxBreaks.Add(5f);

        myButton.onClick.AddListener(TaskOnClick);
    }

    void BankOrder()
    {
        float tempAmount = (float)Convert.ToDouble(myInput1.text);
        float tempGoodAmount = (float)Convert.ToDouble(myInput2.text);


        if (tempAmount >= 0 && tempGoodAmount >= 0)
        {
            if (myDropDown.value == 0 && myMoney >= tempAmount)
            {
                Order tempOrder = new Order();
                tempOrder.myGood = "food";
                tempOrder.myType = "buy";
                tempOrder.myMonetaryAmount = tempAmount;
                tempOrder.myGoodAmount = tempGoodAmount;
                tempOrder.myPOP = null;
                tempOrder.myAskingPrice = tempAmount / tempGoodAmount;
                myMaster.myBankOrders.Add(tempOrder);
            }
            else if (myDropDown.value == 1 && myCommodityStorage[0] >= tempGoodAmount)
            {
                Order tempOrder = new Order();
                tempOrder.myGood = "food";
                tempOrder.myType = "sell";
                tempOrder.myMonetaryAmount = tempAmount;
                tempOrder.myGoodAmount = tempGoodAmount;
                tempOrder.myPOP = null;
                tempOrder.myAskingPrice = tempAmount / tempGoodAmount;
                myMaster.myBankOrders.Add(tempOrder);
            }
        }
    }

    private void TaskOnClick()
    {
        BankOrder();
    }

    private void Update()
    {
        if (myInput1.text != "" && myInput2.text != "")
        {
            if ((float)Convert.ToDouble(myInput1.text) != 0 && (float)Convert.ToDouble(myInput2.text) != 0)
            {
                float tempValue = (float)Convert.ToDouble(myInput1.text) / (float)Convert.ToDouble(myInput2.text);
                tempValue = (float)Math.Round(tempValue, 2);
                myPriceText.text = tempValue.ToString();
            }
        }
    }
}
