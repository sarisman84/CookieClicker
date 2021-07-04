using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CookieManager : MonoBehaviour
{
    private static CookieManager _ins;

    public static CookieManager SingletonAccess
    {
        get
        {
            _ins = _ins ? _ins :
                FindObjectOfType<CookieManager>() is { } foundManager ? foundManager :
                new GameObject("Cookie Manager").AddComponent<CookieManager>();

            return _ins;
        }
    }


    //Events
    public UnityEvent<string> onScoreAlteration;
    public UnityEvent<string> onIncrementAmmAlteration;

    //Handles score
    public int CurrentScore { get; set; }
    public int CurrentClickIncrementAmm { get; set; } = 1;

    private void Start()
    {
        UpdateIncrementAmmDisplay();
        UpdateScoreDisplay();
    }


    public void ManuallyIncrementScore()
    {
        CurrentScore += CurrentClickIncrementAmm;
        UpdateScoreDisplay();
    }

    public void IncrementScoreBy(int amm)
    {
        CurrentScore += amm;
        UpdateScoreDisplay();
    }

    public void UpdateScoreDisplay()
    {
        onScoreAlteration?.Invoke(CurrentScore.ToString());
    }

    public void UpdateIncrementAmmDisplay()
    {
        onIncrementAmmAlteration?.Invoke(CurrentClickIncrementAmm.ToString());
    }
}