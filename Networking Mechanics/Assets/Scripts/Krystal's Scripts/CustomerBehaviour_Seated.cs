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

    [SerializeField] private GameObject orderIcon;
    [SerializeField] private GameObject dirtyDishPrefab;
    [SerializeField] private Transform dishSpawnPoint;

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

    //when customer has been brought to a table with enough seats, this method is called
    public void CustomerJustSeated(TableScript tableScript)
    {
        //animate the customer sitting down and browsing menu
        CustomerAnimScript.SitDownAnim();
        CustomerAnimScript.BrowseMenuAnim();
        Debug.Log("Animating customer sitting and browsing menu");

        //generate the customer's order
        GenerateOrder();

        //assing the table the customer is seated at as their table
        tableSeatedAt = tableScript;
    }


    //generates and assigns an order to the customer
    public void GenerateOrder()
    {
        customersOrder = OrderGeneration.CreateNewOrder();

        if(orderIcon != null)
        {
            orderIcon.GetComponent<CustomerOrderIconCustomization>();
        }
        
    }

    //after the customer's order has been taken, they will wait for their food
    public void DisplayOrderAndWait()
    {
        Debug.Log("Displaying customer order");
        CustomerAnimScript.WaitForFoodAnim();

        //if the customer waits too long for their food, they will SitAngrily() will be called
        TriggerPatienceMeter(true, CustomerPatienceStats.customerPatience_FoodWait, SitAngrily);
    }


    //when customer waits too long for their order, they will sit angrily
    public void SitAngrily()
    {
        Debug.Log("Sit angrily");
    }


    //customer leaving the restaurant. if angry, play angry anim
    public void LeaveRestaurant(bool isCustomerAngry)
    {
        //animate customer standing up
        CustomerAnimScript.LeaveAnim();
        Debug.Log("Standing from table");

        //if the customer is angry, play angry anim
        if (isCustomerAngry)
        {
            //animate the customer being angry
            Debug.Log("customer is angry!");
        }

        //customer fades out of existence
        Debug.Log("Customer fading out of existence");
        Destroy(this.gameObject, 5f);

    }


    //customer has bee served the right food and is eating it
    public void EatingFood()
    {
        //declare that the table has been dirtied
        tableSeatedAt.isTableDirty = true;

        //enable eating animation
        CustomerAnimScript.StartEatingAnim();
        Debug.Log("Animating customer eating food");

        //eat for customerEatingDuration amount of time
        Invoke("CustomerFinishedFood", CustomerPatienceStats.customerEatingDuration);

    }

    //function to call once customer finishes eating food
    public void CustomerFinishedFood()
    {
        finishedEating = true;

        //disable eating animation
        CustomerAnimScript.StopEatingAnim();
        Debug.Log("Customer is done eating food");

        //Instantiate dirty dish in front of customer
        Instantiate(dirtyDishPrefab, dishSpawnPoint.position, dishSpawnPoint.localRotation);
        Debug.Log("Spawning dirty dishes");

        //all customers leave if they have all finished eating
        if (tableSeatedAt.CheckIfAllFinishedEating())
        {
            tableSeatedAt.EmptyTable();
        }
    }
}