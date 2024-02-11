using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour,IPointerDownHandler
{

    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Init()
    {

    }

    public abstract void Clear();



    public void OnPointerDown(PointerEventData eventData)
    {
        Managers.Sound.Play("Effect/UI_Click");

    }
}
