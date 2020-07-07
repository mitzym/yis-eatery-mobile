using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderGeneration : MonoBehaviour
{
    #region Debug Shortcuts
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateNewOrder();
        }
    }
    */
    #endregion


    //static method to randomly generate an order
    public static ChickenRice CreateNewOrder()
    {
        ChickenRice newOrder = new ChickenRice(Random.value > 0.5f, Random.value > 0.5f, Random.value > 0.5f);
        //Debug.Log("Order generated. Does customer want chicken roasted? " + newOrder.RoastedChic + ". Rice plain? " + newOrder.RicePlain + ". Include egg? " + newOrder.HaveEgg + ". Include cucumber? " + newOrder.Cucumber);

        return newOrder;
    }
}

//Object with order properties
public class ChickenRice
{
    private bool cucumber = true;

    #region Getters and Setters
    public bool RoastedChic {get; set;}
    public bool RicePlain { get; set; }
    public bool HaveEgg { get; set; }
    public bool Cucumber
    {
        get { return cucumber; }
        private set { cucumber = value; }
    }
    #endregion


    //chicken rice order constructor
    public ChickenRice(bool roastedChic, bool ricePlain, bool haveEgg)
    {
        this.RoastedChic = roastedChic;
        this.RicePlain = ricePlain;
        this.HaveEgg = haveEgg;
        this.Cucumber = true;
    }
}
