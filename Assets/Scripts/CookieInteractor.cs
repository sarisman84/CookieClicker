using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;

public class CookieInteractor : MonoBehaviour
{
    public UnityEvent<string> onCookieClicked;

    public void ClickCookie()
    {
        Debug.Log("Cookie Clicked");
        onCookieClicked?.Invoke(CookieClicker.SingletonAccess.CurrentAddValue.ToString());
        CookieClicker.SingletonAccess.IncrementValue();
    }
}