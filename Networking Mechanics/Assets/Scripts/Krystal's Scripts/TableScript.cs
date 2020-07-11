using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using UnityEngine;

public class TableScript : MonoBehaviour
{
    [HideInInspector, Range(0, 6)] public int numSeats = 0; //number of seats the table has
    private int numSeated = 0; //number of customers seated at table

    [SerializeField] private TableFeedback tableFeedbackScript;
    [SerializeField] private CustomerPatience patienceScript;

    //customer-related fields
    [SerializeField] private string customerTag = "Customer";
    [SerializeField] private List<Transform> seatPositions = new List<Transform>();
    [SerializeField] private Vector2 minAndMaxOrderGenTime = new Vector2(3f, 5f);

    //prefabs required
    public GameObject dirtyDishPrefab;
    public GameObject customerSeatedPrefab;


    //list of customers that are seated at table
    private List<GameObject> customersSeated = new List<GameObject>();
    public List<GameObject> CustomersSeated
    {
        get { return customersSeated; }
        private set { customersSeated = value; }
    }


    //list of orders of customers that are seated at table
    private List<ChickenRice> tableOrders = new List<ChickenRice>();
    public List<ChickenRice> TableOrders
    {
        get { return tableOrders; }
        private set { tableOrders = value; }
    }


    [HideInInspector] public bool isTableDirty = false;


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
        numSeats = seatPositions.Count;

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


    //instantiate 1 customer at every seat, add them to a list, then call the method on the customer to manage their sitting animation + order
    public void SeatGuests(int numGuests)
    {
        Debug.Log("Guests are being seated");
        
        //call the seated event
        for (int i = 0; i < numGuests; i++)
        {
            //instantiate customer and get its script
            GameObject newSittingCustomer = Instantiate(customerSeatedPrefab, seatPositions[i].position, seatPositions[i].rotation).gameObject;
            CustomerBehaviour_Seated newCustomerScript = newSittingCustomer.GetComponent<CustomerBehaviour_Seated>();

            //animate customer sitting, assign this table to the customer, and get it to generate an order
            newCustomerScript.CustomerJustSeated(this);

            //add customer and their to list of customers seated at table
            if (newCustomerScript.CustomersOrder != null)
            {
                customersSeated.Add(newSittingCustomer);
                tableOrders.Add(newCustomerScript.CustomersOrder);
            }
            else
            {
                Debug.Log("tried to add customer to list, but customer's order was null");
            }
        }

        numSeated = numGuests;
        Debug.Log("numGuests: " + numSeated + ", customersSeated: " + customersSeated.Count);

        //after a random amount of time, call a server to take their order
        Invoke("ReadyToOrder", Random.Range(minAndMaxOrderGenTime.x, minAndMaxOrderGenTime.y));

    }



    //enable the ui and start the patience meter.
    public void ReadyToOrder()
    {
        tableFeedbackScript.ToggleOrderIcon(true);
        patienceScript.StartPatienceMeter(CustomerPatienceStats.customerPatience_TakeOrder, OrderNotTaken);
    }


    //call this method when customer waits too long for their order
    public void OrderNotTaken()
    {
        //disable the order icon
        tableFeedbackScript.ToggleOrderIcon(false);
        isTableDirty = false; 

        //clear the table of customers and have them leave angrily
        EmptyTable(true);
    }



    //call this method when the table has no guests seated at it
    public void EmptyTable(bool isCustomerAngry = false)
    {
        if (isTableDirty)
        {
            Debug.Log("Table needs to be cleared");

            //spawn dirty dishes on table
            SpawnDirtyDishes(numSeated);

        }

        //animate customers leaving
        foreach (GameObject customer in customersSeated)
        {
            CustomerBehaviour_Seated customerScript = customer.GetComponent<CustomerBehaviour_Seated>();
            customerScript.LeaveRestaurant(isCustomerAngry);
        }
    }


    //function to spawn dirty dishes
    public void SpawnDirtyDishes(int numDishes)
    {
        Debug.Log("Spawn " + numDishes + " dirty dishes");
        //--------------------------------------------------------------------------------------------------------------------------add function later
    }



    //check whether all customers at the table are done eating
    public bool CheckIfAllFinishedEating()
    {
        foreach (GameObject customer in customersSeated)
        {
            CustomerBehaviour_Seated customerScript = customer.GetComponent<CustomerBehaviour_Seated>();

            if (!customerScript.FinishedEating)
            {
                return false;
            }
        }

        return true;
    }



}//end of tablescript class
