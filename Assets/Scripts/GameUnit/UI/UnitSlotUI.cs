using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UnitSlotUI : UI_BaseSettingUnit
{
    Vector2 defalutSizeDelta = new Vector2(120.0f, 135.0f);
    Vector3 defalutTr = new Vector3(0.0f, -20.0f, 0.0f);
    Vector2 spearSizeDelta = new Vector2(150.0f, 135.0f);
    Vector3 spearTr = new Vector3(-16.0f, -20.0f, 0.0f);

    [SerializeField] private GameObject slotTxtObj;
    [SerializeField] private GameObject selectImgObj;
    [SerializeField] private Button slotUnitClearBtn;
    [SerializeField] private Button slotUnitInfoBtn;
    [SerializeField] private TextMeshProUGUI slotUnitLvTxt;

    private RectTransform slotUnitClearRt;
    private RectTransform slotUnitInfoRt;

    private float clickOnPosY = -95.0f;
    private float clickOffPosY = -160.0f;


    public UnitClass E_UnitClass { get { return e_UnitClass; } set { e_UnitClass = value; } }
    private int slotidx = 0;


    public int SlotIdx { get { return slotidx; } set { slotidx = value; } }

    // Start is called before the first frame update
    void Start()
    {
        Init();

        if (slotUnitClearBtn != null)
            slotUnitClearBtn.onClick.AddListener(() =>
            {
                if (Managers.UI.PeekPopupUI<UI_UnitSettingWindow>().SlotListCount() < 4)  //���Ծ��� ����ִ°����� 4�̻��̸�
                {
                    //������ ��ġ����
                    e_UnitClass = UnitClass.Count;
                    RefreshUnitImg();
                    Managers.UI.PeekPopupUI<UI_UnitSettingWindow>().UnitUIInit();
                }
                else
                {
                    SlotClickUnitInfoBtnOnOff(false);       //����Ŭ���� ������  UI�� �ϴ� ���α�
                    Managers.UI.PeekPopupUI<UI_UnitSettingWindow>().UnitUIInit();

                }


            });


        if (slotUnitInfoBtn != null)
            slotUnitInfoBtn.onClick.AddListener(() =>
            {
                Managers.UI.ShowPopUp<UI_UnitInfoSelectPopUp>().PopUpOpenUnitInfoSetting(E_UnitClass, Define.UnitInfoSelectType.Slot);
            });


    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    protected override void Init()
    {

        slotUnitClearBtn?.TryGetComponent(out slotUnitClearRt);
        slotUnitInfoBtn?.TryGetComponent(out slotUnitInfoRt);

        SlotUnitLvTxtObjOnOff(false);           //ó���� ������ ������ ���� ����(���Ծȿ� ������ ��ġ�Ǿ�߸� ��)
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

        SlotUnitLvTxtObjOnOff(true);
        SlotClickUnitInfoBtnOnOff(false);       //����Ŭ���� ������  UI�� �ϴ� ���α�

        switch (e_UnitClass)
        {

            case UnitClass.Warrior:
                //Debug.Log($"������ ����! {e_UnitClass}");
                rt.sizeDelta = defalutSizeDelta;
                rt.transform.localPosition = defalutTr;
                UnitUISpriteInit(e_UnitClass, Managers.Game.UnitWarriorLv, "KnifeUnitLv1Img", "KnifeUnitLv2Img");
                SlotUnitLvInit(Managers.Game.UnitWarriorLv);
                break;

            case UnitClass.Archer:
                //Debug.Log($"��ó ����! {e_UnitClass}");
                rt.sizeDelta = defalutSizeDelta;
                rt.transform.localPosition = defalutTr;
                UnitUISpriteInit(e_UnitClass, Managers.Game.UnitArcherLv, "BowUnitLv1Img", "BowUnitLv2Img");
                SlotUnitLvInit(Managers.Game.UnitArcherLv);
                break;

            case UnitClass.Spear:
                //Debug.Log($"â�� ����! {e_UnitClass}");

                rt.sizeDelta = spearSizeDelta;
                rt.transform.localPosition = spearTr;
                UnitUISpriteInit(e_UnitClass, Managers.Game.UnitSpearLv, "SpearUnitLv1Img", "SpearUnitLv2Img");
                SlotUnitLvInit(Managers.Game.UnitSpearLv);
                break;

            case UnitClass.Priest:
                //Debug.Log($"��ó ����! {e_UnitClass}");
                rt.sizeDelta = defalutSizeDelta;
                rt.transform.localPosition = defalutTr;
                UnitUISpriteInit(e_UnitClass, Managers.Game.UnitPriestLv, "PriestLv1Img", "PriestLv2Img");
                SlotUnitLvInit(Managers.Game.UnitPriestLv);
                break;

            case UnitClass.Magician:
                //Debug.Log($"â�� ����! {e_UnitClass}");

                rt.sizeDelta = defalutSizeDelta;
                rt.transform.localPosition = defalutTr;
                UnitUISpriteInit(e_UnitClass, "Magician_Idle");
                SlotUnitLvInit(Managers.Game.UnitMagicianLv);
                break;
            case UnitClass.Cavalry:
                //Debug.Log($"â�� ����! {e_UnitClass}");

                rt.sizeDelta = defalutSizeDelta;
                rt.transform.localPosition = defalutTr;
                UnitUISpriteInit(e_UnitClass, "Cavalry_Idle");
                SlotUnitLvInit(Managers.Game.UnitCarlvlry);
                break;

            default:
                if (unitImg != null)
                    unitImg.gameObject.SetActive(false);

                SlotClickUnitInfoBtnOnOff(false);       //����Ŭ���� ������  UI�� �ϴ� ���α�
                SlotUnitLvTxtObjOnOff(false);           //ó���� ������ ������ ���� ����(���Ծȿ� ������ ��ġ�Ǿ�߸� ��)
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

    //������ ���� ���ӿ�����Ʈ�� �¿���
    public void SlotUnitLvTxtObjOnOff(bool isOn)
    {
        slotUnitLvTxt?.gameObject.SetActive(isOn);
        
    }

    //�� ������ Ŭ���ϸ� ��ġ������ư�� ����������ư�� ��Ÿ����.
    public void SlotClickUnitInfoBtnOnOff(bool btnIsOn)
    {


        //���� ������ 
        if(btnIsOn)
        {
            slotUnitClearBtn?.gameObject.SetActive(btnIsOn);
            slotUnitInfoBtn?.gameObject.SetActive(btnIsOn);
            slotUnitClearRt?.DOLocalMoveY(clickOnPosY, 0.2f).SetEase(Ease.Linear);
            slotUnitInfoRt?.DOLocalMoveY(clickOnPosY, 0.2f).SetEase(Ease.Linear);

        }
        else
        {

            slotUnitClearRt?.DOLocalMoveY(clickOffPosY, 0.2f).SetEase(Ease.Linear).OnComplete(()=>
            {
                slotUnitClearBtn?.gameObject.SetActive(false);
            });
            slotUnitInfoRt?.DOLocalMoveY(clickOffPosY, 0.2f).SetEase(Ease.Linear).OnComplete(()=>
            {
                slotUnitInfoRt?.gameObject.SetActive(false);

            });

            

        }
    }

    



    void SlotUnitLvInit(int lv)
    {
        slotUnitLvTxt.text = $"<#FF9F13>Lv</color> {lv}";
    }
}
