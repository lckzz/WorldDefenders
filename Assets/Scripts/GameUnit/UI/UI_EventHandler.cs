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
    public Action<int> OnPointerDownIntHandler = null;
    string path = null;
    int idx = -1;


    public void OnPointerDown(PointerEventData eventData)
    {
        Managers.Sound.Play("Effect/UI_Click");
        OnPointerDownHandler?.Invoke();
        OnPointerDownUnitHandler?.Invoke(path,idx);
        OnPointerDownIntHandler?.Invoke(idx);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpHandler?.Invoke();
    }


}
