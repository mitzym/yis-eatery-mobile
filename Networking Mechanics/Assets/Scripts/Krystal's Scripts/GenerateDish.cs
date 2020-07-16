using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDish : MonoBehaviour
{
    #region Singleton

    private static GenerateDish _instance;
    public static GenerateDish Instance { get { return _instance; } }

    private void Awake()
    {
        Debug.Log(this.gameObject.name);

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    [SerializeField] private GameObject roastedPlain, roastedPlain_egg, roastedBall, roastedBall_egg;
    [SerializeField] private GameObject steamedPlain, steamedPlain_egg, steamedBall, steamedBall_egg;

    //Identifies which food order icon should be displayed
    private GameObject IdentifyOrder(bool _roasted, bool _plainRice, bool _haveEgg)
    {
        if (_roasted)
        {
            if (_plainRice)
            {
                if (_haveEgg)
                {
                    return roastedPlain_egg;
                }
                else
                {
                    return roastedPlain;
                }
            }
            else
            {
                if (_haveEgg)
                {
                    return roastedBall_egg;
                }
                else
                {
                    return roastedBall;
                }
            }
        }
        else
        {
            if (_plainRice)
            {
                if (_haveEgg)
                {
                    return steamedPlain_egg;
                }
                else
                {
                    return steamedPlain;
                }
            }
            else
            {
                if (_haveEgg)
                {
                    return steamedBall_egg;
                }
                else
                {
                    return steamedBall;
                }
            }
        }
    }
}
