using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_Lobby : UI_Base
{

    [Header("----------Lobby Buttons------------")]
    public Button upgradeBtn;
    public Button unitSettingBtn;
    public Button skillBtn;
    public Button stageSelectBtn;
    public Button settingBtn;


    [Space(20)]
    [Header("------------Object---------------")]
    public LobbyScene lobbyScene;
    public Image fadeImg;
    [SerializeField] private Image skillImg;
    [SerializeField] private TextMeshProUGUI skilltxt;
    [SerializeField] private TextMeshProUGUI profileLvTxt;
    [SerializeField] private TextMeshProUGUI goldTxt;
    [SerializeField] private Sprite[] skilliconSptrites;

    [SerializeField] private Button test;

    [SerializeField] private GameObject dialogCanvas;



    private DialogueCtrl dialogCtrl;

    //다이얼로그 마스크
    [Space(20)]
    [Header("------------DialogMask------------")]
    [SerializeField] private GameObject dialogMaskGo;

    private DialogMask dialogMask;
    private int order;
    //다이얼로그 마스크





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

        dialogCanvas?.TryGetComponent(out dialogCtrl);
        dialogMaskGo?.TryGetComponent(out dialogMask);


        if(Managers.Game.TutorialEnd == false)
        {
            MaskOrderSet((int)DialogOrder.Upgrade);     //처음에 마스크해줄 위치인 업그레이드를 위해서 오더값을 셋팅
            Managers.Dialog.dialogEndedInt -= ShowDialogMask;
            Managers.Dialog.dialogEndedInt += ShowDialogMask;
            dialogCtrl.StartDialog(DialogKey.tutorial.ToString(), DialogType.Dialog, DialogSize.Large);
            dialogCtrl?.NoSelectDialogInit(NoClickTutorialEnd);
        }    


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

    void NoClickTutorialEnd()
    {
        Managers.Game.TutorialEnd = true;
    }


    void TutorialMaskReset(int order)
    {
        //튜토리얼이라면 다이얼로그마스크의 위치가 원래대로 돌아게끔
        if(Managers.Game.TutorialEnd == false)      //튜토리얼이 안끝났다면
        {
            dialogMask?.ObjectSiblingReset(order);

        }
    }

    void UpgradeOn()
    {
        Managers.Sound.Play("Effect/UI_Click");
        LobbyUIOnOff(false);
        Managers.Dialog.dialogEndedInt -= ShowDialogMask;
        Managers.UI.ShowPopUp<UI_UpgradeWindow>();
        TutorialMaskReset(order);

    }

    void UnitSettingOn()
    {
        Managers.Sound.Play("Effect/UI_Click");
        LobbyUIOnOff(false);
        Managers.UI.ShowPopUp<UI_UnitSettingWindow>();
        TutorialMaskReset(order);
    }

    void PlayerSkillOn()
    {
        Managers.Sound.Play("Effect/UI_Click");
        LobbyUIOnOff(false);
        Managers.UI.ShowPopUp<UI_PlayerSkillWindow>();
        TutorialMaskReset(order);

    }

    void StageSelectOn()
    {
        Managers.Sound.Play("Effect/UI_Click");
        LobbyUIOnOff(false);
        Managers.UI.ShowPopUp<UI_StageSelectPopUp>();
        TutorialMaskReset(order);

    }


    void OpenSettingPopUp()
    {
        Managers.Sound.Play("Effect/UI_Click");
        LobbyUIOnOff(false);
        Managers.UI.ShowPopUp<UI_SettingPopUp>();
        Managers.UI.PeekPopupUI<UI_SettingPopUp>().SettingType(Define.SettingType.LobbySetting);
    }

    void ShowDialogMask(int id)
    {
        //튜토리얼 다이얼로그가 끝나면 마스크오브젝트로 현재 위치만 눌리도록 유도하기 위함
        //아이디가 다이얼로그마스크를 가지고 있지않다면 마스크오브젝트는 켜지지않음
        if(id == (int)Define.DialogId.DialogMask)
        {
            dialogMaskGo.SetActive(true);
            dialogMask.MaskGameObjectPosSet(order);
        }

    }


    void MaskOrderSet(int order)
    {
        //마스크오브젝트를 표시할때 현재 위치와 순서를 셋팅함
        this.order = order;     
    }

    public void HideDialogMask()
    {
        dialogMaskGo.SetActive(false);

    }

    public void DialogMaskSet(int id , int order)
    {
        MaskOrderSet(order);
        ShowDialogMask(id);
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
