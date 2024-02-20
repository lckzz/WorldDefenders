using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageClickEvent : MonoBehaviour,IPointerDownHandler
{

    public Action imgClickAction;


    public void OnPointerDown(PointerEventData eventData)
    {
        imgClickAction.Invoke();
    }


}
