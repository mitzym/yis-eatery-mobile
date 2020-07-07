using Mirror.Examples.Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomerInteractionManager : MonoBehaviour
{
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



    private bool CheckThatArgsAreCorrect(List<GameObject> playerInventory, GameObject tableGameobj)
    {
        Debug.Log("Check args method called");

        if (tableGameobj.GetComponent<TableScript>() && playerInventory.Contains(customerBeingHeld))
        {
            Debug.Log("player is holding customer and is looking at table");
            return true;
        }
        else if(!tableGameobj.GetComponent<TableScript>())
        {
            Debug.Log("player is not looking at table");
            return false;
        }
        else
        {
            Debug.Log("player is not holding customer??");
            return false;
        }
    }

} //end of class
