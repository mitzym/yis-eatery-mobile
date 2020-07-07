using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator customerAnim;

    //customer sitting related animations. true = sitting down, false = standing up from seat
    public void SitDownAnim(bool isSitting)
    {
        customerAnim.SetBool("isSitting", isSitting); //fade customers out when they stand up from the table?

        Debug.Log("SitDownAnim " + isSitting);
    }


    //customer angry while sitting
    public void AngrySitAnim()
    {
        Debug.Log("AngrySitAnim called, nothing is here though");
    }


    //customer curling up into a ball while being carried
    public void CurlUpAnim()
    {
        Debug.Log("CurlUpAnim called, nothing is here though");
    }

}
