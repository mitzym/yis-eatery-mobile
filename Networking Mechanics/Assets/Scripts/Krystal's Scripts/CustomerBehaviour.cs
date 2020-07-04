//Usage: attach to the parent object of ALL CUSTOMER PREFAB.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerBehaviour : MonoBehaviour
{
    [Header("Customer Feedback Scripts Variables")]
    [SerializeField] private CustomerAnimationManager CustomerAnimScript;
    [SerializeField] private CustomerPatience CustomerPatienceScript;
    [SerializeField] private Collider CustomerCollider;
    
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
        if(table != null)
        {
            Debug.Log("Standing from table");

            //animate customer standing up
            CustomerAnimScript.SitDownAnim(false);

            //empty the table
            table.EmptyTable();
        }
        
    }

    public void FinishMeal(TableScript table)
    {
        table.EmptyTable(true);
    }



    //Patience Meter is started / stopped, depending on the bool passed into it
    public void TriggerPatienceMeter(bool startPatience, Action callback = null)
    {
        if(CustomerPatienceScript != null)
        {
            if (startPatience)
            {
                CustomerPatienceScript.StartPatienceMeter(CustomerPatienceStats.customerPatience_Queue, callback);
            }
            else
            {
                CustomerPatienceScript.StopPatienceMeter();
            }
        }
        else
        { 
            Debug.Log("Please assign the customer patience script to the customer prefab");
        }
        
    }


    //enable / disable the customer's collider, and/or set it to trigger
    public void TriggerCustomerCollider(bool isEnabled, bool isTrigger)
    {
        CustomerCollider.isTrigger = isTrigger;
        CustomerCollider.enabled = isEnabled;
    }



} //end of customer behaviour class
