using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CookieAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


    private Coroutine _coroutine;

    public void PlayClickAnimation(float duration)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(ClickAnim(duration));
    }

    private IEnumerator ClickAnim(float duration)
    {
        yield return transform.DOScale(Vector3.one * 2f, duration / 2f).WaitForCompletion();
        yield return transform.DOScale(Vector3.one, duration / 2f).WaitForCompletion();
    }
}