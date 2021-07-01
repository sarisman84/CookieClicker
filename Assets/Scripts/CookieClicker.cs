using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CookieClicker : MonoBehaviour
{
    public UnityEvent onCookieClicked;
    public void ClickCookie()
    {
        Debug.Log("Cookie Clicked");
        onCookieClicked?.Invoke();
    }
}
