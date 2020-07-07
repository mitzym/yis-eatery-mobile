//Usage: attach to the parentobj of THE CUSTOMER SEATED PREFAB.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerBehaviour_Seated : CustomerBehaviour
{
    //---------------------BEHAVIOUR WHEN SEATED---------------------
    //when customer has been brought to a table with enough seats, 
    //they will sit down and browse the menu for a while 
    public void SitAndBrowseMenu()
    {
        Debug.Log("Sitting and browsing menu");
    }


    //called when customer is waiting for 
    public void GenerateOrderAndWait()
    {
        Debug.Log("order generated, now waiting for order");
    }


    //when customer waits too long for their order, they will sit angrily
    public void SitAngrily()
    {
        Debug.Log("Sit angrily");
    }


    //When a customer is left waiting too long, they will leave the restaurant
    public void LeaveRestaurantAngrily(TableScript table = null)
    {
        //stand up if needed
        if (table != null)
        {
            Debug.Log("Standing from table");

            //animate customer standing up
            CustomerAnimScript.SitDownAnim(false);

            //empty the table
            table.EmptyTable(false);
        }

    }

    public void FinishMeal(TableScript table)
    {
        table.EmptyTable(true);
    }
}