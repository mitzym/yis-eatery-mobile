using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TableFeedback : MonoBehaviour
{
    [SerializeField] private Animator canvasAnim, wordAnim;

    [Header("ready to order icon")]
    [SerializeField] private GameObject readyToOrderIcon;

    [Header("text to display")]
    [SerializeField] private TextMeshProUGUI word_tmpObj; 
    [SerializeField] private string insufficientSeats = "Not enough seats";


    private void Start()
    {
        //deactivating all icons / feedback
        readyToOrderIcon.SetActive(false);
        word_tmpObj.gameObject.SetActive(false);
    }


    //feedback that shows the table has insufficient seats
    public void NotEnoughSeats()
    {
        Debug.Log("Table feedback: Not enough seats");

        StartCoroutine(FadeInFadeOutText(insufficientSeats, true, true));
    }


    //feedback that shows that the customers are ready to order
    public void ToggleOrderIcon(bool enable)
    {
        if (enable)
        {
            Debug.Log("Table feedback: customers ready to order");
        }
        else
        {
            Debug.Log("Table feedback: done ordering");
        }

        //toggle the order icon and make it bob
        readyToOrderIcon.SetActive(enable);
        canvasAnim.SetTrigger("bob");

    }


    IEnumerator FadeInFadeOutText(string _text, bool _fadeIn, bool _fadeOut)
    {
        word_tmpObj.text = _text;
        word_tmpObj.gameObject.SetActive(true);

        //make the canvas rise
        canvasAnim.SetTrigger("rise");

        if (_fadeIn)
        {
            wordAnim.SetBool("fadeIn", true);

            while (word_tmpObj.fontMaterial.color.a < 1)
            {
                Debug.Log("textmeshpro alpha: " + word_tmpObj.fontMaterial.color.a);
                yield return new WaitForSeconds(0.2f);
            }
        }

        if (_fadeOut)
        {
            wordAnim.SetBool("fadeIn", false);

            while (word_tmpObj.fontMaterial.color.a > 0)
            {
                Debug.Log("textmeshpro alpha: " + word_tmpObj.fontMaterial.color.a);
                yield return new WaitForSeconds(0.2f);
            }
        }

        word_tmpObj.gameObject.SetActive(false);

        yield return null;
    }




}
