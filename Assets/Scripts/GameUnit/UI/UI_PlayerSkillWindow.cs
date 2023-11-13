using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PlayerSkillWindow : UI_Base
{
    [SerializeField] private GameObject skillNodePrefab;
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


    //터치 
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;
    //터치

    [SerializeField] private Image fadeImg;
    bool backFadeCheck = false;


    // Start is called before the first frame update
    public override void Start()
    {
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



        skillNodeParentObj = GameObject.Find("SkillWindow");
        GameObject bgObj = GameObject.Find("SkillWindowBg");
        if (bgObj != null)
            skillInfoObj = bgObj.transform.Find("SkillInfo").gameObject;

        skillInfoObj.TryGetComponent(out skillInfo);

        for (int ii = 0; ii < (int)Define.PlayerSkill.Count; ii++)
        {
            GameObject obj = Object.Instantiate(skillNodePrefab, skillNodeParentObj.transform);
            SkillNode node;
            obj.TryGetComponent<SkillNode>(out node);
            node.PlayerSkillType = (Define.PlayerSkill)ii;
            node.SkillNodeSetting((Define.PlayerSkill)ii);
            if (ii == (int)Define.PlayerSkill.Heal)
                clickNode = node;

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
                    lobby.LobbyTouchUnitInit();
                }
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


                curClickNode.ClickNodeSelectImgOnOff(true);
                if(clickNode != null)
                    clickNode.ClickNodeSelectImgOnOff(false);


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


    void EquipSkill()
    {
        if(clickNode != null)
        {
            if (clickNode.SkillData.level <= 0)
                return;

            Managers.Game.CurPlayerEquipSkill = clickNode.PlayerSkillType;
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
                            lobby.LobbyTouchUnitInit();
                        }


                    }
                }
            }
        }

    }

}
