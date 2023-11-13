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
    public Button settingBtn;

    public Button stageSelectBtn;
    public LobbyScene lobbyScene;
    public Image fadeImg;
    [SerializeField] Image skillImg;
    [SerializeField] TextMeshProUGUI skilltxt;
    [SerializeField] TextMeshProUGUI profileLvTxt;
    [SerializeField] TextMeshProUGUI goldTxt;
    [SerializeField] Sprite[] skilliconSptrites;

    [SerializeField] Button test;



    // Start is called before the first frame update
    public override void Start()
    {
        GameObject.Find("LobbyScene").TryGetComponent(out lobbyScene);
        //LobbySceneRefresh();
        #region 버튼
        if (upgradeBtn != null)
            upgradeBtn.onClick.AddListener(UpgradeOn);

        if (unitSettingBtn != null)
            unitSettingBtn.onClick.AddListener(UnitSettingOn);

        if (skillBtn != null)
            skillBtn.onClick.AddListener(PlayerSkillOn);

        if (stageSelectBtn != null)
            stageSelectBtn.onClick.AddListener(StageSelectOn);

        if (settingBtn != null)
            settingBtn.onClick.AddListener(OpenSettingPopUp);
        #endregion

        Managers.Game.Gold = 100000;

        profileLvTxt.text = $"<#FF9F13>Lv</color> {Managers.Game.PlayerLevel}";
        goldTxt.text = Managers.Game.Gold.ToString();
        RefreshSKillicon(Managers.Game.CurPlayerEquipSkill);
        startFadeOut = true;


        if (Managers.Game.LobbyToGameScene)
        {
            //게임씬에서 로비로 돌아왔다면
            StageSelectOn();        //스테이지 팝업씬을 켜주고
            Managers.Game.LobbyToGameScene = false;      //false로 바꿔주기

        }


    }

    // Update is called once per frame
    void Update()
    {
        Util.FadeOut(ref startFadeOut, fadeImg);
        //UpgradeFadeIn(fadeImg, this, upgradeBtnFadeCheck);
        //UnitFadeIn(fadeImg, this, unitBtnFadeCheck);
        //StageFadeIn(fadeImg, this, stageBtnFadeCheck);

    }

    private void OnEnable()
    {
        Managers.UI.ShowSceneUI<UI_Lobby>();
        profileLvTxt.text = $"<#FF9F13>Lv</color> {Managers.Game.PlayerLevel}";
        goldTxt.text = Managers.Game.Gold.ToString();
        RefreshSKillicon(Managers.Game.CurPlayerEquipSkill);

    }


    void UpgradeOn()
    {
        LobbyUIOnOff(false);
        Managers.UI.ShowPopUp<UI_UpgradeWindow>();

    }

    void UnitSettingOn()
    {
        LobbyUIOnOff(false);
        Managers.UI.ShowPopUp<UI_UnitSettingWindow>();

    }

    void PlayerSkillOn()
    {
        LobbyUIOnOff(false);
        Managers.UI.ShowPopUp<UI_PlayerSkillWindow>();
    }

    void StageSelectOn()
    {
        LobbyUIOnOff(false);
        Managers.UI.ShowPopUp<UI_StageSelectPopUp>();

    }

    void LobbySceneRefresh()
    {
        if (lobbyScene != null)
        {
            lobbyScene.RefreshUnit();
            lobbyScene.LobbyTouchUnitInit();

        }
    }


    void OpenSettingPopUp()
    {
        LobbyUIOnOff(false);
        Managers.UI.ShowPopUp<UI_SettingPopUp>();
        Managers.UI.PeekPopupUI<UI_SettingPopUp>().SettingType(Define.SettingType.LobbySetting);
    }


    public void LobbyUIOnOff(bool isOn)
    {
        this.gameObject.SetActive(isOn);
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
