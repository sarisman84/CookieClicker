using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
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
    public UnityEvent<float> onUpdateBasedOnModifierDist;

    public List<BasePoint> claimedModifiers = new();

    //Handles score

    private int _curScore;
    private float _curScoreGenAmm = -1;

    public int CurrentScore
    {
        get => _curScore;
        set => _curScore = Mathf.Clamp(value, 0, int.MaxValue);
    }

    public int CurrentClickIncrementAmm { get; set; } = 1;

    public float PointGenerationRate
    {
        get { return _curScoreGenAmm; }
        set
        {
            if (_curScoreGenAmm == -1)
                _curScoreGenAmm = 1;
            _curScoreGenAmm = value;
        }
    }

    public int PointGenerationAmm { get; set; } = 1;


    private float _curCd = 0;

    private void Start()
    {
        UpdateIncrementAmmDisplay();
        UpdateScoreDisplay();
    }

    private void Update()
    {
        onUpdateBasedOnModifierDist?.Invoke(
            Vector3.Distance(transform.position,
                claimedModifiers.Count == 0 ? transform.position : claimedModifiers[0].transform.position));


        if (PointGenerationRate >= 0)
        {
            _curCd += Time.deltaTime;

            if (_curCd >= PointGenerationRate)
            {
                CurrentScore += PointGenerationAmm;
                _curCd = 0;
                UpdateScoreDisplay();
            }
        }
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


    public void AddModifierToRegistry(BasePoint mod)
    {
        if (claimedModifiers.Contains(mod)) return;
        claimedModifiers.Add(mod);
        claimedModifiers.Sort(CompareMods);
    }

    private int CompareMods(BasePoint x, BasePoint y)
    {
        return (Vector3.Distance(x.transform.position, transform.position) <
                Vector3.Distance(y.transform.position, transform.position))
            ? 1
            : -1;
    }

    public bool HasModification(BasePoint modToCompare)
    {
        if (modToCompare)
            return claimedModifiers.Contains(modToCompare);
        return false;
    }
}