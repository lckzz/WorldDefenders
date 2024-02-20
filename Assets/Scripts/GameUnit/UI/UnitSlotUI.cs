using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEditor;

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
    private IOpenPanel openPanel;

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
                Managers.Sound.Play("Effect/UI_Click");

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

                Managers.UI.PeekPopupUI<UI_UnitSettingWindow>().UI_UnitSetInit();


            });


        if (slotUnitInfoBtn != null)
            slotUnitInfoBtn.onClick.AddListener(() =>
            {
                Managers.Sound.Play("Effect/UI_Click");
                Managers.UI.ShowPopUp<UI_UnitInfoSelectPopUp>().PopUpOpenUnitInfoSetting(E_UnitClass, Define.UnitInfoSelectType.Slot);
                openPanel = Managers.UI.PeekPopupUI<UI_UnitInfoSelectPopUp>();
                openPanel.OpenRectTransformScaleSet();
            });


    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    protected override void Init()
    {
        base.Init();
        slotUnitClearBtn?.TryGetComponent(out slotUnitClearRt);
        slotUnitInfoBtn?.TryGetComponent(out slotUnitInfoRt);

        SlotUnitLvTxtObjOnOff(false);           //ó���� ������ ������ ���� ����(���Ծȿ� ������ ��ġ�Ǿ�߸� ��)
        //ó���� �����Ǹ� �ش簻�����ֱ�

        if (selectImgObj != null)
            selectImgObj.SetActive(false);




        RefreshUnitImg();
    }


    protected override void UnitUISpriteInit()
    {
        base.UnitUISpriteInit();

        if (unitPosObj != null)  //���ֳ���� ��ġ�� ������Ʈ�� �ִٸ�
            Managers.Resource.Instantiate(unitStat.unitSpriteUIPrefabs, unitPosObj.transform);        //���ֳ���� ��ġ�� ������Ʈ�� ������ ������ �����Ѵ�.
    }

    public void SetUnitClass(UnitClass uniClass)
    {
        this.e_UnitClass = uniClass;
    }

    

    public void RefreshUnitImg()
    {
        if(e_UnitClass == UnitClass.Count)
        {
            SlotUnitLvTxtObjOnOff(false);
            SlotClickUnitInfoBtnOnOff(false);       //����Ŭ���� ������  UI�� �ϴ� ���α�

            if (unitPosObj.transform.childCount > 0)     //���� �ʱ�ȭ�ϴµ� ���������ȿ� ����������Ʈ�� �ִٸ�
                Destroy(unitPosObj.transform.GetChild(0).gameObject);           //�� ������Ʈ�� �������ش�.

            return;
        }

        SlotUnitLvTxtObjOnOff(true);
        SlotClickUnitInfoBtnOnOff(false);       //����Ŭ���� ������  UI�� �ϴ� ���α�

        UnitUISpriteInit();
        SlotUnitLvInit(Managers.Game.GetUnitLevel(e_UnitClass));

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
