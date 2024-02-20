using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UnitSkillInfo : MonoBehaviour
{
    public enum SkillOrder
    {
        SkillObj,
        ToolTip,
        Count
    }


    [SerializeField] private GameObject skillInfoObj;
    private Image skillInfoImg;
    // Start is called before the first frame update
    private string toolTipStr = "";
    private ImageClickEvent imgEvt;
    private UnitInfoToolTip skillTip;
    private UnitClass unitClass;
    private UnitSkillInfo unitSkillInfo;

    private void Start()
    {
        transform.GetChild((int)SkillOrder.ToolTip).gameObject.TryGetComponent(out skillTip);



        ClickImage(Managers.UI.PeekPopupUI<UI_UnitInfoSelectPopUp>().SkillInfoTooltipAllOff);
    }

    public void SkillInfoImgInit(UnitClass unitclass,Sprite sprite,string toolTip,UnitSkillInfo unitInfoSkill = null)
    {
        if(skillInfoImg == null)
            skillInfoObj.transform.GetChild(0).gameObject.TryGetComponent(out skillInfoImg);

        this.unitClass = unitclass;
        skillInfoImg.sprite = sprite;
        toolTipStr = toolTip;
        unitSkillInfo = unitInfoSkill;
    }

    public void SkillInfoOn()
    {
        skillInfoObj?.SetActive(true);
        

    }

    public bool CheckSkillInfoActive()
    {
        if (skillInfoObj.activeSelf == false)  //�����ִٸ�
            return false;
        else
            return true;
    }

    public void ToolTipSetOn()
    {
        if (CheckSkillInfoActive() && !skillTip.CheckToolTipisOn())       //�̹����� �����ְ� ������ ������������
        {
            skillTip.ToolTipOff();
            skillTip.ToolTipSet(toolTipStr);
        }
    }


    public void ToolTipSetOff()
    {
        if (skillTip.CheckToolTipisOn())     //�����ִٸ�
            skillTip.ToolTipOff();      //���� ����
    }

    public void ClickImage(Action action = null)
    {
        TryGetComponent(out imgEvt);
        if (action != null)
            imgEvt.imgClickAction += action;
        imgEvt.imgClickAction -= ToolTipSetOn;
        imgEvt.imgClickAction += ToolTipSetOn;

    }


    private void OnDestroy()
    {
        if(imgEvt != null)
            imgEvt.imgClickAction -= ToolTipSetOn;

    }


}
