using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seasons : MonoBehaviour
{
    Master myMaster;
    string mySeason;
    void Start()
    {
        myMaster = GameObject.FindGameObjectWithTag("Master").GetComponent<Master>();
    }

    public void SeasonUpdate()
    {
        if(myMaster.myDay >= 0 && myMaster.myDay < 100)
        {
            mySeason = "summer";
        }
        else if (myMaster.myDay >= 100 && myMaster.myDay < 150)
        {
            mySeason = "fall";
        }
        else if (myMaster.myDay >= 150 && myMaster.myDay < 250)
        {
            mySeason = "winter";
        }
        else if (myMaster.myDay >= 250 && myMaster.myDay < 300)
        {
            mySeason = "spring";
        }
    }

    public void GetSeasonalModifier()
    {
        switch (GetSeason())
        {
            case "summer":
                myMaster.myRNGModifiers[0] = 9f;
                myMaster.myRNGModifiers[1] = 19f;
                myMaster.myRNGModifiers[2] = 9f;
                myMaster.myRNGModifiers[3] = 10f;
                break;
            case "fall":
                myMaster.myRNGModifiers[0] = 9f;
                myMaster.myRNGModifiers[1] = 14f;
                myMaster.myRNGModifiers[2] = 9f;
                myMaster.myRNGModifiers[3] = 11f;
                break;
            case "winter":
                myMaster.myRNGModifiers[0] = 9f;
                myMaster.myRNGModifiers[1] = 10f;
                myMaster.myRNGModifiers[2] = 9f;
                myMaster.myRNGModifiers[3] = 12f;
                break;
            case "spring":
                myMaster.myRNGModifiers[0] = 9f;
                myMaster.myRNGModifiers[1] = 14f;
                myMaster.myRNGModifiers[2] = 9f;
                myMaster.myRNGModifiers[3] = 11f;
                break;
            default:
                break;
        }
    }

    public string GetSeason()
    {
        return mySeason;
    }
}
