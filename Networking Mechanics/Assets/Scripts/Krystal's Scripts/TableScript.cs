using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using UnityEngine;

public class TableScript : MonoBehaviour
{
    [HideInInspector, Range(0, 6)] public int numSeats = 0;
    private int numSeated = 0;

    //method to find seated customer prefabs
    [SerializeField] private string customerTag = "Customer";
    [SerializeField] private List<GameObject> customers = new List<GameObject>();
    public List<GameObject> Customers
    {
        get { return customers; }
        private set { customers = value; }
    }
    public GameObject dirtyDishPrefab;

    public TableFeedback tableFeedbackScript;


    void Start()
    {
        //add current table to table collider manager list
        TableColliderManager.AddTableToTableColliderManager(gameObject);

        /*
        //get all the customers childed to the table by identifying them via their tag
        if(customerTag != null)
        {
            FindObjectwithTag(customerTag, customers);
        }
        */

        //update the number of seats the table has
        numSeats = customers.Count;

        //disable all customers in thehierarchy at the start of the game
        foreach(GameObject customer in customers)
        {
            customer.SetActive(false);
        }
    }



    #region Find Objects and Add Them to Lists
    //-----------------------------------------FIGURE OUT THE NUMBER OF SEATS AT THE TABLE
    //get all children of the table
    public void FindObjectwithTag(string tag, List<GameObject> list)
    {
        list.Clear();
        Transform parent = transform;
        GetChildObject(parent, tag, list);
    }


    //check the tags all the children of the table. Add all children tagged tag to a list.
    public void GetChildObject(Transform parent, string tag, List<GameObject> list)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child.CompareTag(tag))
            {
                list.Add(child.gameObject);
            }
        }
    }

#endregion



    //-------------------------------------------------------- METHODS RELATED TO CUSTOMERS INTERACTING WITH TABLE AND SEATS
    //check number of customers
    public bool CheckSufficientSeats(int numGuests)
    {
        Debug.Log("checking if there are sufficient seats");

        if (numGuests <= numSeats)
        {
            if (numGuests < numSeats)
            {
                Debug.Log("less guests than seats");
            }
            else if (numGuests > numSeats)
            {
                Debug.Log("enough seats for guests");
            }

            //seat the guests
            SeatGuests(numGuests);

            return true;
        }
        else
        {
            Debug.Log("more guests than seats");

            //feedback to player that there are insufficient seats
            tableFeedbackScript.NotEnoughSeats();

            return false;
        }
        
    }


    //position 1 guest at every seat, then call the method on the customer to manage their sitting animation
    public void SeatGuests(int numGuests)
    {
        Debug.Log("Guests are being seated");
        //call the seated event

        for (int i = 0; i < numGuests; i++)
        {   
            //make customers visisble
            customers[i].SetActive(true);

            //animate customers
            Debug.Log("Animate the customers");
        }

        numSeated = numGuests;
    }


    //call this method when the table has no guests seated at it
    public void EmptyTable(bool isTableDirty)
    {
        if (isTableDirty)
        {
            Debug.Log("Table needs to be cleared");

            //spawn dirty dishes on table
            SpawnDirtyDishes(numSeated);

        }

    }


    //function to spawn dirty dishes
    public void SpawnDirtyDishes(int numDishes)
    {
        Debug.Log("Spawn " + numDishes + " dirty dishes");
        //--------------------------------------------------------------------------------------------------------------------------add function later
    }



}//end of tablescript class
