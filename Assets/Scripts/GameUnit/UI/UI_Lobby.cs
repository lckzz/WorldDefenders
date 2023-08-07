using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : UI_Base
{
    public Button upgradeBtn;
    public GameObject upgradeObj;
    // Start is called before the first frame update
    void Start()
    {
        ButtonEvent(upgradeBtn.gameObject, UpgradeOn, UIEvnet.PointerDown);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void ButtonEvent(GameObject obj, Action action = null,UIEvnet type = UIEvnet.PointerDown)
    {
        UI_EventHandler evt = null;
        obj.TryGetComponent(out evt);


        if (evt != null)
        {
            switch(type)
            {
                case UIEvnet.PointerDown:
                    evt.OnPointerDownHandler -= action;
                    evt.OnPointerDownHandler += action;

                    break;
            }
        }
    }


    void UpgradeOn()
    {
        Managers.UI.ClosePopUp(this);
        Managers.UI.ShowPopUp<UI_UpgradeWindow>();
    }

 
}
