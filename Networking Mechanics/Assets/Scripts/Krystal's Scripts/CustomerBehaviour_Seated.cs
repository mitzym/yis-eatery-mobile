//Usage: attach to the parentobj of THE CUSTOMER SEATED PREFAB.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerBehaviour_Seated : CustomerBehaviour
{
    private ChickenRice customersOrder = null;
    private TableScript tableSeatedAt = null;
    private bool finishedEating;
    [SerializeField] private GameObject dirtyDishPrefab;
    [SerializeField] private Transform dirtyDishSpawnPoint;

    #region Getters and Setters
    public ChickenRice CustomersOrder
    {
        get { return customersOrder; }
        private set { customersOrder = value; }
    }

    public TableScript TableSeatedAt
    {
        get { return tableSeatedAt; }
        private set { tableSeatedAt = value; }
    }

    public bool FinishedEating
    {
        get { return finishedEating; }
        private set { finishedEating = value; }
    }
    #endregion


    //---------------------BEHAVIOUR WHEN SEATED---------------------
    //when customer has been brought to a table with enough seats, 
    //they will sit down and browse the menu for a while 
    public void SitAndBrowseMenu()
    {
        Debug.Log("Animating customer sitting and browsing menu");
    }


    //called when customer is waiting for 
    public void GenerateOrder()
    {
        customersOrder = OrderGeneration.CreateNewOrder();
    }


    //assigns a reference to the table the customer is seated at to the customer
    public void AssignTableScript(TableScript tableScript)
    {
        tableSeatedAt = tableScript;
    }


    public void DisplayOrderAndWait()
    {
        Debug.Log("Displaying customer order");

    }


    //when customer waits too long for their order, they will sit angrily
    public void SitAngrily()
    {
        Debug.Log("Sit angrily");
    }


    //When a customer is left waiting too long, they will leave the restaurant
    public void LeaveRestaurant()
    {
        Debug.Log("Standing from table");

        //animate customer standing up
        CustomerAnimScript.SitDownAnim(false);

    }

    public void EatingFood()
    {
        //declare that the table has been dirtied
        tableSeatedAt.isTableDirty = true;

        //enable eating animation
        Debug.Log("Animating customer eating food");

        //eat for a random amount of time
        Invoke("CustomerFinishedFood", CustomerPatienceStats.customerEatingDuration);

    }

    //function to call once customer finishes eating food
    public void CustomerFinishedFood()
    {
        finishedEating = true;

        //disable eating animation
        Debug.Log("Customer is done eating food");

        //Instantiate dirty dish in front of customer
        Instantiate(dirtyDishPrefab, dirtyDishSpawnPoint.localPosition, dirtyDishSpawnPoint.localRotation);

        //all customers leave if they have all finished eating
        if (tableSeatedAt.CheckIfAllFinishedEating())
        {
            tableSeatedAt.EmptyTable();
        }
    }
}