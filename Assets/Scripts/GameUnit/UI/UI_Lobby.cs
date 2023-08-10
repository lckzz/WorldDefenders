using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : UI_Base
{
    public Button upgradeBtn;
    public Button unitSettingBtn;
    public LobbyScene lobbyScene;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("LobbyScene").TryGetComponent(out lobbyScene);
        LobbySceneRefresh();
        ButtonEvent(upgradeBtn.gameObject, UpgradeOn, UIEvnet.PointerDown);
        ButtonEvent(unitSettingBtn.gameObject, UnitSettingOn, UIEvnet.PointerDown);

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

    void UnitSettingOn()
    {
        Managers.UI.ClosePopUp(this);
        Managers.UI.ShowPopUp<UI_UnitSettingWindow>();
    }

    void LobbySceneRefresh()
    {
        if (lobbyScene != null)
        {
            lobbyScene.RefreshUnit();

        }
    }

}
