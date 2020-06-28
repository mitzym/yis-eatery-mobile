using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject spawnArea, waitingArea;
    public numGuestsSpawnRates[] spawnRates;

    [SerializeField] private string guestTag = "Guest";
    [SerializeField] private GameObject sphereVisualisation;
    [SerializeField] private float checkRad = 0.625f, spawnFrequency = 2f, customerMoveSpd = 0.2f;

    private float timeSinceLastSpawn;
    private bool isCoroutineRunning = false;
    private Coroutine moveLerpCor;



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
            Vector3 newWaitingPos = GetRandomPosition(spawnArea);

            if (CheckPositionIsEmpty(newWaitingPos, checkRad))
            {
                moveLerpCor = StartCoroutine(MoveAndActivate(egg, newPos));
                break;
            }

            yield return null;
        } // end of while


        isCoroutineRunning = false;
        yield return null;
    }// end of coroutine



    // get a new position within the spawn area
    Vector3 GetRandomPosition(GameObject availableArea)
    {
        float halfWidth = (availableArea.GetComponent<Collider>().bounds.size.x) / 2;
        float halfDepth = (availableArea.GetComponent<Collider>().bounds.size.z) / 2;

        Vector3 newPos = new Vector3(Random.Range(-halfWidth, halfWidth), 0,
                                    Random.Range(-halfDepth, halfDepth));

        return newPos;
    }



    //move the gameobject egg to the position newpos and set it active in the hierarchy //-------------------------------------change egg.........
    IEnumerator MoveAndActivate(Transform customer, Vector3 newPos)
    {
        Vector3 startPos = customer.position;
        float journeyProgress = 0;
        
        while(customer.position != newPos)
        {
            yield return new WaitForSeconds(customerMoveSpd);

            journeyProgress += customerMoveSpd;
            customer.position = Vector3.Lerp(startPos, newPos, journeyProgress);
        }

        timeSinceLastSpawn = 0f;

        //start patience meter
        customer.gameObject.GetComponent<CustomerPatience>().StartPatienceMeter(CustomerPatienceStats.customerPatience_Queue);

        yield return null;
    }



    //returns true if the coordinates passedi ndo not overlap with any obstacle colliders
    private bool CheckPositionIsEmpty(Vector3 pos, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(pos, radius);

        for (int j = 0; j < hitColliders.Length; j++)
        {
            if (hitColliders[j].gameObject.CompareTag(guestTag))
            {
                return false;
            }
        }

        return true;

    }
}


[System.Serializable]
public class numGuestsSpawnRates
{
    public int numGuests = 0;
    public int minProbability = 0;
    public int maxProbability = 100;

}