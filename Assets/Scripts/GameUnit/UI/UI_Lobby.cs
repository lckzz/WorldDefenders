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
    [SerializeField] Sprite[] skilliconSptrites;

    [SerializeField] Button test;



    // Start is called before the first frame update
    public override void Start()
    {
        GameObject.Find("LobbyScene").TryGetComponent(out lobbyScene);
        //LobbySceneRefresh();
        #region ¹öÆ°
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

    private void OnEnable()
    {
        Managers.UI.ShowSceneUI<UI_Lobby>();

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
