using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public Action OnPointerDownHandler = null;
    public Action OnPointerUpHandler = null;
    public Action<string,int> OnPointerDownUnitHandler = null;
    string path = null;
    int idx = -1;


    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownHandler?.Invoke();
        OnPointerDownUnitHandler?.Invoke(path,idx);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpHandler?.Invoke();
    }


}
