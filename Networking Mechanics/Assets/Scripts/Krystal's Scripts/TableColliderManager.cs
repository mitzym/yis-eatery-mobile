using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableColliderManager : MonoBehaviour
{
    private static List<GameObject> allTableColliders = new List<GameObject>();
    private static string tableLayer = "Table", environmentLayer = "Environment";


    //add current table to the list of tables in TableColliderManager script
    public static void AddTableToTableColliderManager(GameObject table)
    {
        allTableColliders.Add(table);
    }


    //switch the layers of all the tables 
    public static void ToggleTableDetection(bool allowDetection)
    {
        if (allowDetection)
        {
            foreach (GameObject table in allTableColliders)
            {
                table.gameObject.layer = LayerMask.NameToLayer(tableLayer);
            }
        } 
        else
        {
            foreach (GameObject table in allTableColliders)
            {
                table.gameObject.layer = LayerMask.NameToLayer(environmentLayer);
            }
        }
        
    }


    #region Debug shortcut
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleTableDetection(true);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleTableDetection(false);
        }
    }
    */
    #endregion
}
