using Mirror.Examples.Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomerInteractionManager : MonoBehaviour
{
    [SerializeField] private string customerTag = "Customer";
    [SerializeField] private string queueingCustomerLayer = "Queue", customerToServeLayer = "Table";

    //------------------------------------------------------INTERACTION WITH CUSTOMER WAITING IN QUEUE----------------------------------------------------------------------
    #region Queueing Customers

    private GameObject customerBeingHeld = null;

    //When player is looking at a queueing customer and presses the interaction button,
    public void PickCustomerUp(GameObject customerGameobj, List<GameObject> playerInventory, Transform pointAboveHead)
    {
        //refer to the parent holding the customer collider
        customerGameobj = customerGameobj.transform.parent.gameObject;
        customerBeingHeld = customerGameobj;
        Debug.Log("Customer being held");

        //animate the customer curling up + stop the patience meter
        customerGameobj.GetComponent<CustomerBehaviour_Queueing>().CustomerPickedUp(pointAboveHead);

        //Parent to attachment point and transform
        customerGameobj.transform.parent = pointAboveHead.transform;
        customerGameobj.transform.position = pointAboveHead.position;

        //add the customer to the inventory
        playerInventory.Add(customerGameobj);

        //allow tables to be detected
        TableColliderManager.ToggleTableDetection(true);

        PlayerInteractionManager.playerState = PlayerInteractionManager.PlayerState.HoldingCustomer;
    }



    //When the player is looking at a table and is carrying a customer,
    public void SeatCustomer(List<GameObject> playerInventory, GameObject tableGameobj)
    {
        Debug.Log("Seat customer method called");

        if (!tableGameobj.GetComponent<TableScript>())
        {
            Debug.Log("player is not looking at table");
            return;
        }

        if (!playerInventory.Contains(customerBeingHeld))
        {
            Debug.Log("player is not holding customer??");
            return;
        }

        TableScript tableScript = tableGameobj.GetComponent<TableScript>();

        //If the table has enough seats for the group of customers,
        if (tableScript.CheckSufficientSeats(customerBeingHeld.GetComponent<CustomerBehaviour_Queueing>().GroupSizeNum))
        {
            Debug.Log("Enough seats for customers");

            //disallow tables from being detected
            TableColliderManager.ToggleTableDetection(false);

            //remove the customer from the inventory
            playerInventory.Remove(customerBeingHeld);

            //stop holding the customer + activate the customers at the table
            Destroy(customerBeingHeld.gameObject);

            customerBeingHeld = null;

            PlayerInteractionManager.playerState = PlayerInteractionManager.PlayerState.Default;
        }
    }

    #endregion


    //------------------------------------------------------TAKING / SERVING CUSTOMERS' ORDERS----------------------------------------------------------------------
    #region Picking orders up
    public void PickOrderUp(GameObject dishObj, List<GameObject> playerInventory, Transform pointAboveHead)
    {
        //Parent to attachment point and transform
        dishObj.transform.parent = pointAboveHead.transform;
        dishObj.transform.position = pointAboveHead.position;

        //add the customer to the inventory
        playerInventory.Add(dishObj);

        //change player state
        PlayerInteractionManager.playerState = PlayerInteractionManager.PlayerState.HoldingOrder;
    }
    #endregion


    #region Taking customers' orders

    public void CheckHandsEmpty(List<GameObject> playerInventory, GameObject tableGameobj)
    {
        //check if the player is looking at a table
        if (tableGameobj.GetComponent<TableScript>() == null)
        {
            Debug.Log("player is not looking at table");
            return;
        }

        //get the table script
        TableScript tableScript = tableGameobj.GetComponent<TableScript>();

        //if the player's hands are full, don't take their order
        if (playerInventory.Count > 0)
        {
            Debug.Log("player's hands are full");
            tableScript.TableFeedbackScript.HandsFullFeedback();
            return;
        }

        //else, take the order of the customers at the table
        tableScript.TakeOrder();
        PlayerInteractionManager.playerState = PlayerInteractionManager.PlayerState.Default;
    }

    #endregion


    #region Serving customers' orders
    public void CheckCanPutDownOrder(List<GameObject> _playerInventory, GameObject _customer, Transform _dropOffPoint)
    {
        GameObject heldDish = FindDishInInventory(_playerInventory);

        if(_customer != null)
        {
            //if the player is looking at a customer
            if (_customer.GetComponent<CustomerBehaviour_Seated>() != null || _customer.transform.parent.gameObject.GetComponent<CustomerBehaviour_Seated>() != null)
            {
                if (ServingCustomer(heldDish, _customer))
                {
                    UpdateStateAndInventory(_playerInventory, heldDish);
                }
            }
            else
            {
                UpdateStateAndInventory(_playerInventory, heldDish);

                //drop the dish and unparent it
                heldDish.transform.position = _dropOffPoint.position;
                heldDish.transform.parent = null;
            }
        }
        else
        {
            UpdateStateAndInventory(_playerInventory, heldDish);

            //drop the dish and unparent it
            heldDish.transform.position = _dropOffPoint.position;
            heldDish.transform.parent = null;
        }


    }


    //Update the player state and remove the delivered dish from inventory
    private void UpdateStateAndInventory(List<GameObject> _inventory, GameObject removeObj)
    {
        _inventory.Remove(removeObj);
        PlayerInteractionManager.playerState = PlayerInteractionManager.PlayerState.Default;
    }


    //check if there is a dish in the player's inventory
    private GameObject FindDishInInventory(List<GameObject> _inventory)
    {
        //find the order in the player's inventory
        foreach (GameObject obj in _inventory)
        {
            //get the parent obj of the identified collider, bc it contains the script
            //GameObject objParent = obj.transform.parent.gameObject;

            if (obj.GetComponent<OrderScript>() != null)
            {
                return obj;
            }
        }

        return null;
    }


    //check if the order is correct if the player is facing a customer
    public bool ServingCustomer(GameObject dishObj, GameObject customer)
    {
        //if the customer gameobj does not contain the required script, get it from the parent
        if (customer.GetComponent<CustomerBehaviour_Seated>() == null)
        {
            //get the parent object of the identified collider (should contain the customer behaviour script)
            customer = customer.transform.parent.gameObject;

        }

        //if the gameobj the player is looking at is indeed a customer,
        if (customer.GetComponent<CustomerBehaviour_Seated>() != null)
        {
            //if the order being served is what the customer wanted,
            return customer.GetComponent<CustomerBehaviour_Seated>().CheckOrder(dishObj);
        }

        return false;

    }//end serve method

    #endregion 

} //end of class
