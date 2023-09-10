using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : UI_Base
{

    public Button upgradeBtn;
    public Button unitSettingBtn;
    public Button skillBtn;

    public Button stageSelectBtn;
    public LobbyScene lobbyScene;
    public Image fadeImg;
    [SerializeField] Image skillImg;
    [SerializeField] TextMeshProUGUI skilltxt;
    [SerializeField] Sprite[] skilliconSptrites;



    bool upgradeBtnFadeCheck = false;
    bool unitBtnFadeCheck = false;
    bool stageBtnFadeCheck = false;
    bool lobbyPanelCheck = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("LobbyScene").TryGetComponent(out lobbyScene);
        LobbySceneRefresh();
        ButtonEvent(upgradeBtn.gameObject, UpgradeOn, UIEvent.PointerDown);
        ButtonEvent(unitSettingBtn.gameObject, UnitSettingOn, UIEvent.PointerDown);
        ButtonEvent(skillBtn.gameObject, PlayerSkillOn, UIEvent.PointerDown);

        ButtonEvent(stageSelectBtn.gameObject, StageSelectOn, UIEvent.PointerDown);
        RefreshSKillicon(GlobalData.g_CurPlayerEquipSkill);
        startFadeOut = true;


    }

    // Update is called once per frame
    void Update()
    {
        Util.FadeOut(ref startFadeOut, fadeImg);
        //UpgradeFadeIn(fadeImg, this, upgradeBtnFadeCheck);
        //UnitFadeIn(fadeImg, this, unitBtnFadeCheck);
        //StageFadeIn(fadeImg, this, stageBtnFadeCheck);

    }


    void ButtonEvent(GameObject obj, Action action = null,UIEvent type = UIEvent.PointerDown)
    {
        UI_EventHandler evt = null;
        obj.TryGetComponent(out evt);


        if (evt != null)
        {
            switch(type)
            {
                case UIEvent.PointerDown:
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

    void PlayerSkillOn()
    {
        Managers.UI.ClosePopUp(this);
        Managers.UI.ShowPopUp<UI_PlayerSkillWindow>();
    }

    void StageSelectOn()
    {
        Managers.UI.ClosePopUp(this);
        Managers.UI.ShowPopUp<UI_StageSelectPopUp>();

    }

    void LobbySceneRefresh()
    {
        if (lobbyScene != null)
        {
            lobbyScene.RefreshUnit();

        }
    }

    public void RefreshSKillicon(Define.PlayerSkill playersk)
    {
        if (playersk == Define.PlayerSkill.Count)
        {
            skillImg.gameObject.SetActive(false);
            skilltxt.gameObject.SetActive(true);
            return;

        }

        if(!skillImg.gameObject.activeSelf)
            skillImg.gameObject.SetActive(true);

        skilltxt.gameObject.SetActive(false);
        skillImg.sprite = skilliconSptrites[(int)playersk];

    }

}
