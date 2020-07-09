﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CustomerSpawn : MonoBehaviour
{
    [SerializeField] private bool customerSpawnDebuggingMode = false;

    [SerializeField] private GameObject spawnArea, waitingArea;
    public numGuestsSpawnRates[] spawnRates;

    [SerializeField] private string customerTag = "Customer";
    [SerializeField] private GameObject customerQueueingPrefab, customerParent, sphereVisualisation;
    [SerializeField] private float checkRad = 0.625f, spawnFrequency = 15f, customerMoveSpd = 0.05f, maxGroupsOfCustomersInWaitingArea = 6;

    private float timeSinceLastSpawn = 0f, currentNumWaitingCustomers = 0f;
    private bool isCoroutineRunning = false;
    private Coroutine moveLerpCoroutine;


    #region Debug CallSpawn Event
    void OnEnable()
    {
        EventManager.OnCustomerSpawn += AnnounceSpawn;
    }


    void OnDisable()
    {
        EventManager.OnCustomerSpawn -= AnnounceSpawn;
    }


    void AnnounceSpawn()
    {
        Debug.Log("Customer has spawned, announcing spawn");
    }

    #endregion



    private void Update()
    {
        //if the number of customers in the waiting area is below the max num of customers, 
        if(currentNumWaitingCustomers < maxGroupsOfCustomersInWaitingArea)
        {
            if (!customerSpawnDebuggingMode)
            {
                //update the amount of time since the last customer group was spawned
                timeSinceLastSpawn += Time.deltaTime;

                //check that the last customer spawned at least spawnFrequency seconds ago
                if (timeSinceLastSpawn > spawnFrequency)
                {
                    //call the spawn coroutine
                    StartCoroutine(SpawnAndCheck());

                    //reset the time since the last customer group was spawned
                    timeSinceLastSpawn = 0;
                }
            } else
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    //call the spawn coroutine
                    StartCoroutine(SpawnAndCheck());
                }
            }
            
        }


        #region Debug shortcuts
        /* 
        //Debug command. On pressing V, the spawn customer method will be called
        if (Input.GetKeyDown(KeyCode.V))
        {
            SpawnCustomer(new Vector3(0,0,0));
        }
        */
        #endregion
    }


    //generates two random positions, one within the wait area and one within the spawn area
    //If the position within the wait area isn't too close to other customers, a customer is spawned in the spawn area,
    // then the customer slowly moves from the spawn area to the area in the wait area.
    IEnumerator SpawnAndCheck()
    {
        //check if another coroutine is running
        if (isCoroutineRunning)
        {
            //end coroutine if another coroutine is running
            yield break;
        }
        else
        {
            //ensure that no other coroutines can start after this
            isCoroutineRunning = true;
        }

        while (true)
        {
            Vector3 newSpawnPos = GetRandomPosition(spawnArea);
            Vector3 newWaitingPos = GetRandomPosition(waitingArea);

            if (CheckPositionIsEmpty(newWaitingPos, checkRad))
            {
                Debug.Log("new spawn position: " + newSpawnPos + ", newWaitingPos: " + newWaitingPos);
                GameObject newGrpOfCustomers = SpawnCustomer(newSpawnPos);
                moveLerpCoroutine = StartCoroutine(MoveAndActivate(newGrpOfCustomers, newWaitingPos));
                break;
            }

            yield return null;
        } // end of while


        isCoroutineRunning = false;
        yield return null;
    }// end of coroutine



    // get a new position within the area provided
    Vector3 GetRandomPosition(GameObject availableArea)
    {
        float halfWidth = (availableArea.GetComponent<Collider>().bounds.size.x) / 2;
        float halfDepth = (availableArea.GetComponent<Collider>().bounds.size.z) / 2;

        Vector3 positionOffset = availableArea.transform.position;

        Vector3 newPos = new Vector3(Random.Range(-halfWidth, halfWidth) + positionOffset.x, 0,
                                    Random.Range(-halfDepth, halfDepth) + positionOffset.z);

        return newPos;
    }


    //spawn a customer prefab, assign a group size to it, then make it a child of an object in the scene
    private GameObject SpawnCustomer(Vector3 spawnPos)
    {
        //create a new group of customers, and assign a group size to the customer
        GameObject newGroupOfCustomers = Instantiate(customerQueueingPrefab, spawnPos, Quaternion.identity).gameObject;
        newGroupOfCustomers.GetComponent<CustomerBehaviour_Queueing>().GenerateSizeOfGroup(spawnRates);

        //make the new customer group spawned a child of the customerParent gameobj 
        newGroupOfCustomers.transform.parent = customerParent.transform;

        //update the number of customers in the waiting area
        currentNumWaitingCustomers = customerParent.transform.childCount;
        
        // announce that a customer has spawned using the spawn event
        Debug.Log("CallSpawnEvent()");
        EventManager.CallSpawnEvent();

        return newGroupOfCustomers;
    }



    //move the gameobject egg to the position newpos and set it active in the hierarchy //-------------------------------------change egg.........
    IEnumerator MoveAndActivate(GameObject customer, Vector3 newPos)
    {
        Vector3 startPos = customer.transform.position;
        float journeyProgress = 0;
        
        while(customer.transform.position != newPos)
        {
            yield return new WaitForSeconds(0.1f);

            journeyProgress += customerMoveSpd;
            customer.transform.position = Vector3.Lerp(startPos, newPos, journeyProgress);
        }
        
        //customer starts waiting upon reaching waiting position
        customer.gameObject.GetComponent<CustomerBehaviour_Queueing>().CustomerStartsWaiting();

        yield return null;
    }



    //returns true if the coordinates passed in does not overlap with any customer colliders
    private bool CheckPositionIsEmpty(Vector3 pos, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(pos, radius);

        //for debugging. if the sphereVisualisation object has been assigned, move it to the position that is being check currently
        if(sphereVisualisation != null)
        {
            sphereVisualisation.transform.position = pos;
            sphereVisualisation.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
        }

        //if the current position is too close to other customers in the wait area, return false
        for (int j = 0; j < hitColliders.Length; j++)
        {
            if (hitColliders[j].gameObject.CompareTag(customerTag))
            {
                return false;
            }
        }

        return true;

    }
} //end of customer spawn class




//object that holds a possible size for the group of customers + the probability that size of group will be spawned
[System.Serializable]
public class numGuestsSpawnRates
{
    public int numGuests = 0;
    public int minProbability = 0;
    public int maxProbability = 100;

}