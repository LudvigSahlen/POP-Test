using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POPHandler : MonoBehaviour
{
    public Bank myBank;
    public Master myMaster;
    List<POP> myPOPList = new List<POP>();
    List<int> myIDList = new List<int>();
    float myTimer;
    int myMaxChance = 1000, myMaxPopTypes = 4;
    void Start()
    {
        for (int i = 0; i < myMaxPopTypes; i++)
        {
            myIDList.Add(0);
        }
    }

    void FixedUpdate()
    {
        myTimer += Time.fixedDeltaTime;
        if(myTimer > 0.1 && myPOPList.Count < 1000 && !myMaster.myRunning)
        {
            myTimer = 0;

            for (int i = 0; i < 100; i++)
            {
                POP tempPOP = new POP();
                int tempRNG = Random.Range(0, myMaxChance);

                if(tempRNG > (myMaxChance * 0.05))
                {
                    tempPOP.myType = 0;
                    myIDList[tempPOP.myType]++;
                }
                else if(tempRNG > myMaxChance * 0.01 && tempRNG < myMaxChance * 0.05)
                {
                    tempPOP.myType = 1;
                    myIDList[tempPOP.myType]++;
                }
                else if (tempRNG < myMaxChance * 0.01)
                {
                    int tempRNG2 = Random.Range(0, 2);
                    if (tempRNG2 == 0) { tempPOP.myType = 2; myIDList[tempPOP.myType]++; }
                    if (tempRNG2 == 1) { tempPOP.myType = 3; myIDList[tempPOP.myType]++; }
                }
                tempPOP.myMarket = GameObject.FindGameObjectWithTag("Market").GetComponent<Marketplace>();
                tempPOP.myMaster = GameObject.FindGameObjectWithTag("Master").GetComponent<Master>();
                tempPOP.myBank = GameObject.FindGameObjectWithTag("Player").GetComponent<Bank>();
                tempPOP.myEarnedMoney = tempPOP.myMoney;

                if (tempPOP.myType != 0)
                {
                    tempPOP.myStoredFood = 10;
                }

                myPOPList.Add(tempPOP);
            }
        }

        if (myPOPList.Count >= 1000 && !myMaster.myRunning)
        {
            myMaster.myPOPList = myPOPList;
            myMaster.myIDList = myIDList;

            string tempPopString = "Farmers: "+ myIDList[0]+ ". Craftsman: " + myIDList[1] + ". Engineers: " + myIDList[2] + ". Wizards: " + myIDList[3];
            Debug.Log(tempPopString);
            myMaster.myRunning = true;
        }

    }
}
