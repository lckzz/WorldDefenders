using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitSlotUI : UI_BaseSettingUnit
{
    Vector2 defalutSizeDelta = new Vector2(120.0f, 135.0f);
    Vector3 defalutTr = new Vector3(0.0f, 58.0f, 0.0f);
    Vector2 spearSizeDelta = new Vector2(150.0f, 135.0f);
    Vector3 spearTr = new Vector3(-16.0f, 58.0f, 0.0f);

    [SerializeField] private GameObject slotTxtObj;
    [SerializeField] private GameObject selectImgObj;

    public UnitClass E_UnitClass { get { return e_UnitClass; } set { e_UnitClass = value; } }
    private int slotidx = 0;


    public int SlotIdx { get { return slotidx; } set { slotidx = value; } }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    protected override void Init()
    {
        //ó���� �����Ǹ� �ش簻�����ֱ�
        unitImg.TryGetComponent(out rt);
        if (selectImgObj != null)
            selectImgObj.SetActive(false);


        RefreshUnitImg();
    }




    public void SetUnitClass(UnitClass uniClass)
    {
        this.e_UnitClass = uniClass;
    }

    

    public void RefreshUnitImg()
    {
        if(rt == null)
            unitImg.TryGetComponent(out rt);

        switch (e_UnitClass)
        {

            case UnitClass.Warrior:
                //Debug.Log($"������ ����! {e_UnitClass}");
                rt.sizeDelta = defalutSizeDelta;
                rt.transform.localPosition = defalutTr;
                UnitUISpriteInit(e_UnitClass, GlobalData.g_UnitWarriorLv, "KnifeUnitLv1Img", "KnifeUnitLv2Img");
                break;

            case UnitClass.Archer:
                //Debug.Log($"��ó ����! {e_UnitClass}");
                rt.sizeDelta = defalutSizeDelta;
                rt.transform.localPosition = defalutTr;
                UnitUISpriteInit(e_UnitClass, GlobalData.g_UnitArcherLv, "BowUnitLv1Img", "BowUnitLv2Img");

                break;

            case UnitClass.Spear:
                //Debug.Log($"â�� ����! {e_UnitClass}");

                rt.sizeDelta = spearSizeDelta;
                rt.transform.localPosition = spearTr;
                UnitUISpriteInit(e_UnitClass, GlobalData.g_UnitSpearLv, "SpearUnitLv1Img", "SpearUnitLv2Img");

                break;

            default:
                if (unitImg != null)
                    unitImg.gameObject.SetActive(false);
                //Debug.Log("���ֳ�忡 ����Ŭ������ ������ �ȵ̾��;");
                break;

        }
    }


    public void StartSelectUnitSlot()           //���� ������ ������ �ؽ�Ʈ�� ���ְ� ���� ���� ȭ��ǥ�� �����
    {
        if (selectImgObj != null)
            selectImgObj.SetActive(true);
        if (slotTxtObj != null)
            slotTxtObj.SetActive(false);
    }

    public void MousePointerOnClickImgOnOff(bool check)        //���콺 �����Ͱ� ������ ����Ű�� Ŭ���̹����� ���� �Ȱ���Ű�� ����
    {
        clickImgObj.SetActive(check);
    }
}
