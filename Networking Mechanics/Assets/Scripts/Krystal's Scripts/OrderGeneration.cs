using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderGeneration : MonoBehaviour
{
    #region Singleton

    private static OrderGeneration _instance;
    public static OrderGeneration Instance { get { return _instance; } }

    private void Awake()
    {
        Debug.Log(this.gameObject.name);

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

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
<<<<<<< Updated upstream
=======

    //Identifies which food order icon should be displayed
    public GameObject IdentifyIcon(ChickenRice.PossibleChickenRiceLabel chickenRiceLabel)
    {
        switch (chickenRiceLabel)
        {
            case ChickenRice.PossibleChickenRiceLabel.RoastedChicWPlainRice:
                return roastedPlain;

            case ChickenRice.PossibleChickenRiceLabel.RoastedChicWPlainRiceEgg:
                return roastedPlain_egg;

            case ChickenRice.PossibleChickenRiceLabel.RoastedChicWRiceBall:
                return roastedBall;

            case ChickenRice.PossibleChickenRiceLabel.RoastedChicWRiceBallEgg:
                return roastedBall_egg;

            case ChickenRice.PossibleChickenRiceLabel.SteamedChicWPlainRice:
                return steamedPlain;

            case ChickenRice.PossibleChickenRiceLabel.SteamedChicWPlainRiceEgg:
                return steamedPlain_egg;

            case ChickenRice.PossibleChickenRiceLabel.SteamedChicWRiceBall:
                return steamedBall;

            case ChickenRice.PossibleChickenRiceLabel.SteamedChicWRiceBallEgg:
                return steamedBall_egg;

            default:
                return null;
        }
    }
>>>>>>> Stashed changes
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
