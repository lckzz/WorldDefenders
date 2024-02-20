using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_PlayerSkillWindow : UI_Base
{
    [SerializeField] private GameObject skillNodePrefab;
    [SerializeField] private GameObject skillEquipNoticeObj;
    [SerializeField] private Button backLobbyBtn;            //유닛클릭시 마스크오브젝트
    [SerializeField] private Button skillEquipBtn;
    [SerializeField] private Image skillEquipImg;
    [SerializeField] private TextMeshProUGUI skillTxt;
    byte skillDisEnableColorAlpha = 160;
    byte skillEnabelColorAlpha = 255;

    Color skillEquipEnableColor;
    Color skillEquipDisEnableColor;
    Color skillEquipTxtEnableColor;
    Color skillEquipTxtDisEnableColor;



    private GameObject skillNodeParentObj;
    private GameObject skillInfoObj;

    private SkillInfo skillInfo;

    private SkillNode curClickNode;
    private SkillNode clickNode;
    private SkillEquipNotice skillEquipNotice;

    private List<SkillNode> skillNodeList = new List<SkillNode>();


    //터치 
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;
    //터치

    //튜토 다이얼로그
    private MaskDialogCtrl dialogCtrl;
    private GameObject tutorialDialogObj;
    private GameObject maskGameObject;
    private TutorialMaskCtrl tutorialMaskCtrl;
    //튜토 다이얼로그



    [SerializeField] private Image fadeImg;
    bool backFadeCheck = false;


    // Start is called before the first frame update
    public override void Start()
    {
        skillNodeList.Clear();

        TryGetComponent(out gr);
        if (gr == null)
        {
            GameObject go = this.transform.GetChild(0).gameObject;
            if (go != null)
                go.TryGetComponent<GraphicRaycaster>(out gr);
        }

        ped = new PointerEventData(EventSystem.current);
        rrList = new List<RaycastResult>(10);

        skillEquipEnableColor = new Color32(255, 255, 255, skillEnabelColorAlpha);
        skillEquipDisEnableColor = new Color32(255, 255, 255, skillDisEnableColorAlpha);
        skillEquipTxtEnableColor = new Color32(50, 50, 50, skillEnabelColorAlpha);
        skillEquipTxtDisEnableColor = new Color32(50, 50, 50, skillDisEnableColorAlpha);

        tutorialDialogObj = gameObject.transform.Find("DialogueCanvas").gameObject;
        GameObject parentGo = backLobbyBtn?.gameObject.transform.parent.gameObject;
        maskGameObject = parentGo.transform.Find("MaskGameObject").gameObject;
        maskGameObject.TryGetComponent(out tutorialMaskCtrl);

        if (Managers.Game.TutorialEnd == false)
        {
            //Managers.Dialog.dialogEndedStringInt -= UpgradeDialogEnd;
            //Managers.Dialog.dialogEndedStringInt += UpgradeDialogEnd;
            tutorialDialogObj.TryGetComponent(out dialogCtrl);
            tutorialDialogObj?.SetActive(!Managers.Game.TutorialEnd);        //튜토리얼이 끝나지않았다면 다이얼로그 켜기
            dialogCtrl?.MaskDialogInit(tutorialMaskCtrl, backLobbyBtn.gameObject, GameObjectSiblingLastSet);
            dialogCtrl?.StartDialog(DialogKey.tutorialSkill.ToString(), DialogType.Dialog, DialogSize.Small, DialogId.NextDialog);
        }


        skillNodeParentObj = GameObject.Find("SkillWindow");
        GameObject bgObj = GameObject.Find("SkillWindowBg");
        if (bgObj != null)
            skillInfoObj = bgObj.transform.Find("SkillInfo").gameObject;

        skillInfoObj.TryGetComponent(out skillInfo);
        skillEquipNoticeObj.TryGetComponent(out skillEquipNotice);

        for (int ii = 0; ii < (int)PlayerSkill.Count; ii++)
        {
            GameObject obj = Object.Instantiate(skillNodePrefab, skillNodeParentObj.transform);
            SkillNode node;
            obj.TryGetComponent<SkillNode>(out node);
            node.PlayerSkillType = (PlayerSkill)ii;
            node.SkillNodeSetting((PlayerSkill)ii);
            if (ii == (int)PlayerSkill.Heal)
                clickNode = node;

            skillNodeList.Add(node);

        }

        skillInfoObj.SetActive(true);
        skillInfo.SkillInfoInit(clickNode.SkillData.name, (clickNode.SkillData.skillValue).ToString(), (clickNode.SkillData.skillCoolTime).ToString(), clickNode.SkillData.desc, clickNode.SkillData.skillImg, clickNode.PlayerSkillType);
        clickNode.ClickNodeSelectImgOnOff(true);
        if (backLobbyBtn != null)
            backLobbyBtn.onClick.AddListener(() =>
            {
                Managers.Sound.Play("Effect/UI_Click");
                Managers.UI.ClosePopUp(this);

                if (Managers.Scene.CurrentScene is LobbyScene lobby)
                {
                    lobby.LobbyUIOnOff(true);
                    lobby.LobbyUnitInit();
                }

                if (Managers.Game.TutorialEnd == false)
                    Managers.UI.GetSceneUI<UI_Lobby>().DialogMaskSet((int)Define.DialogId.DialogMask, (int)Define.DialogOrder.Stage);
                //GlobalData.SetUnitClass(unitSlotUiList);  //스킬셋팅
            });

        if (skillEquipBtn != null)
            skillEquipBtn.onClick.AddListener(EquipSkill);


        startFadeOut = true;



    }

    // Update is called once per frame
    void Update()
    {


        Util.FadeOut(ref startFadeOut, fadeImg);
        BackFadeIn(fadeImg, this, backFadeCheck);

        ped.position = Input.mousePosition;
        OnMousePointerDown();

    }

    void OnMousePointerDown()
    {
        if (Input.GetMouseButtonDown(0))
        {

            curClickNode = UiRaycastGetFirstComponent<SkillNode>(gr);

            if (curClickNode != null)
            {
                Managers.Sound.Play("Effect/UI_Click");

                if (clickNode != null)
                    clickNode.ClickNodeSelectImgOnOff(false);

                curClickNode.ClickNodeSelectImgOnOff(true);



                clickNode = curClickNode;
                skillInfoObj.SetActive(true);
                skillInfo.SkillInfoInit(clickNode.SkillData.name, (clickNode.SkillData.skillValue).ToString(), (clickNode.SkillData.skillCoolTime).ToString(), clickNode.SkillData.desc, clickNode.SkillData.skillImg,clickNode.PlayerSkillType);
            }


            EquipButtonStateSet();

        }
    }

    void EquipButtonStateSet()
    {
        if (clickNode != null)
        {
            if (clickNode.SkillData.level <= 0)
            {
                skillEquipImg.color = skillEquipDisEnableColor;
                skillTxt.color = skillEquipTxtDisEnableColor;
            }
            else
            {
                skillEquipImg.color = skillEquipEnableColor;
                skillTxt.color = skillEquipTxtEnableColor;

            }

        }
        else
        {
            skillEquipImg.color = skillEquipDisEnableColor;
            skillTxt.color = skillEquipTxtDisEnableColor;
        }

    }


    void EquipSkill()  //장착버튼을 눌렀을시
    {
        if(clickNode != null)
        {
            if (clickNode.SkillData.level <= 0)
            {
                Managers.Sound.Play("Effect/Error");
                return;

            }

            if (clickNode.PlayerSkillSt == PlayerSkillState.Equip)
                return;

            Managers.Sound.Play("Effect/Equip");
            Managers.Game.CurPlayerEquipSkill = clickNode.PlayerSkillType;   //클릭한 노드의 스킬정보를 현재 장착한스킬에 넣어준다.
            skillEquipNoticeObj.SetActive(true);

            for (int ii = 0; ii < skillNodeList.Count; ii++)
                skillNodeList[ii].SkillNodeRefresh(skillNodeList[ii].PlayerSkillType);
            //clickNode.SkillNodeRefresh(Managers.Game.CurPlayerEquipSkill);

        }
    }


    T UiRaycastGetFirstComponent<T>(GraphicRaycaster gr) where T : Component
    {
        rrList.Clear();

        gr.Raycast(ped, rrList);
        if (rrList.Count == 0)
            return null;



        return rrList[0].gameObject.GetComponent<T>();
    }



    public void BackFadeIn(Image fadeImg, UI_Base closePopup, bool fadeCheck)
    {
        if (fadeCheck)
        {
            if (fadeImg != null)
            {
                if (!fadeImg.gameObject.activeSelf)
                {
                    fadeImg.gameObject.SetActive(true);

                }

                if (fadeImg.gameObject.activeSelf && fadeImg.color.a <= 1)
                {
                    Color col = fadeImg.color;
                    if (col.a < 255)
                        col.a += (Time.deltaTime * 2.0f);

                    fadeImg.color = col;


                    if (fadeImg.color.a >= 0.99f)
                    {
                        Managers.UI.ClosePopUp(closePopup);
                        if (Managers.Scene.CurrentScene is LobbyScene lobby)
                        {
                            lobby.LobbyUIOnOff(true);
                            lobby.LobbyUnitInit();
                        }


                    }
                }
            }
        }

    }

}
