using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    Marketplace myMarket;
    Slider mySlider;

    private float myCurrentValue;
    public string myMeasuredValue;
    void Start()
    {
        mySlider = gameObject.GetComponent<Slider>();
        myMarket = GameObject.FindGameObjectWithTag("Market").GetComponent<Marketplace>();
    }

    void FixedUpdate()
    {
        if(myMeasuredValue == "StoredFood")
        {
            mySlider.minValue = 0;
            mySlider.maxValue = myMarket.myMaxStoredFood;

            if(mySlider.value < myMarket.myStoredFood)
            {
                mySlider.value += (Time.fixedDeltaTime * 100);
            }
            else if (mySlider.value > myMarket.myStoredFood)
            {
                mySlider.value -= (Time.fixedDeltaTime * 100);
            }
        }
    }
}
