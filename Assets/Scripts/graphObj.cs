using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class graphObj : MonoBehaviour
{
    public GameObject myTop;
    void Start()
    {
        //myMaster.myTotalGraphObj++;
    }
    void FixedUpdate()
    {
        gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - (Time.fixedDeltaTime), gameObject.transform.localPosition.y);
        if(gameObject.transform.localPosition.x <= myTop.transform.localPosition.x -0.8f)
        {
            Destroy(gameObject);
        }
    }
}
