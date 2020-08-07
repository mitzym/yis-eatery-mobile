using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomizeVROrderSlip : MonoBehaviour
{
    [SerializeField] private GameObject orderSlipParent;

    //Order slip icons to toggle, depending on order
    [Header("Toggled icons")]
    [SerializeField] private GameObject[] isChicRoasted = new GameObject[2];
    [SerializeField] private GameObject[] isRicePlain = new GameObject[2];
    [SerializeField] private GameObject[] includesEgg = new GameObject[2];

    
    private void OnEnable()
    {
        //ensure that the gameobject is not visible yet
        orderSlipParent.SetActive(false);
    }



    //Toggle icons on order slip depending on the requirements of the dish
    public void CustomizeOrderSlip(ChickenRice order)
    {
        isChicRoasted[Convert.ToInt32(order.RoastedChic)].SetActive(true);
        isRicePlain[Convert.ToInt32(order.RicePlain)].SetActive(true);
        includesEgg[Convert.ToInt32(order.HaveEgg)].SetActive(true);
    }
}
