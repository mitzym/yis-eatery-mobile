using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator customerAnim;

    public void SitDownAnim(bool isSitting)
    {
        customerAnim.SetBool("isSitting", isSitting);

        Debug.Log("SitDownAnim " + isSitting);
    }


}
