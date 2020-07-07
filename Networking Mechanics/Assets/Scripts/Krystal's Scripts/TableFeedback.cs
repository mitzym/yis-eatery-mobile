using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableFeedback : MonoBehaviour
{
    [SerializeField] private Animator feedbackAnim;


    //feedback that shows the table has insufficient seats
    public void NotEnoughSeats()
    {
        Debug.Log("Table feedback: Not enough seats");
    }


    //feedback that shows that the customers are ready to order
    public void ReadyToOrder()
    {
        Debug.Log("Table feedback: customers ready to order");
    }
}
