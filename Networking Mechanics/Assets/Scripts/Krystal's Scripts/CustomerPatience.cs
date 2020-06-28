/*
 *  Usage: attach to the customer capsule
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerPatience : MonoBehaviour
{
    //variables
    [SerializeField] private float updateFrequency = 0.2f;
    [SerializeField] private Image patienceMeterImg;

    private Coroutine patienceMeterCoroutine;
    private bool isCoroutineRunning = false; //bool used to ensure that coroutine does not get called while coroutine is running


    #region Debug Shortcuts
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartPatienceMeter(10);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            StopPatienceMeter();
        }
    } 
    */
    #endregion


    //getting the other scripts


    //public method to call to start coroutine
    public void StartPatienceMeter(float totalPatience)
    {
        if (isCoroutineRunning)
        {
            //bool used to ensure that coroutine does not get called while coroutine is running
            return;
        } 

        isCoroutineRunning = true;

        patienceMeterCoroutine = StartCoroutine("UpdatePatienceMeter", totalPatience);
    }


    //public method to call to stop and reset coroutine
    public void StopPatienceMeter()
    {
        if (!isCoroutineRunning)
        {
            //bool used to ensure that coroutine is only stopped once
            return;
        }

        isCoroutineRunning = false;

        //disable the image
        patienceMeterImg.enabled = false;

        StopCoroutine(patienceMeterCoroutine);
    }


    //method that updates customers' patience meter
    private IEnumerator UpdatePatienceMeter(float totalPatience)
    {
        float currentPatience = totalPatience;

        patienceMeterImg.enabled = true;

        while (currentPatience > 0)
        {
            //calculate amount of patience left
            currentPatience -= updateFrequency;
            patienceMeterImg.fillAmount = currentPatience / totalPatience;

            Debug.Log(patienceMeterImg.fillAmount);

            yield return new WaitForSeconds(updateFrequency);
        }

        Debug.Log("Calling the impatient method");

        isCoroutineRunning = false;

        yield return null;
    }

    

}
