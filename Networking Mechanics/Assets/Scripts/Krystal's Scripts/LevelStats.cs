using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//All stats related to the level number and score
public class LevelStats
{
    public static int level = 0;
    public static float passingScore = 60f;
    public static float highestScore = 100;

}


//All stats related to how much the customer has
public class CustomerPatienceStats
{
    public static float customerPatience_General = 10f;
    public static float customerPatience_Queue = customerPatience_General;
    public static float customerPatience_TakeOrder = customerPatience_General;
    public static float customerPatience_FoodWait = customerPatience_General;
    
}

public class GameBalanceFormulae
{

}
