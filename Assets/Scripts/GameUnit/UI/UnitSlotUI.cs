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

                if (Managers.UI.PeekPopupUI<UI_UnitSettingWindow>().SlotListCount() < 4)  //슬롯안의 비어있는개수가 4이상이면
                {
                    //슬롯의 배치해제
                    e_UnitClass = UnitClass.Count;
                    RefreshUnitImg();
                    Managers.UI.PeekPopupUI<UI_UnitSettingWindow>().UnitUIInit();
                }
                else
                {
                    SlotClickUnitInfoBtnOnOff(false);       //슬롯클릭시 나오는  UI를 일단 꺼두기
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

        SlotUnitLvTxtObjOnOff(false);           //처음에 들어오면 레벨은 전부 꺼줌(슬롯안에 유닛이 배치되어야만 온)
        //처음에 생성되면 해당갱신해주기

        if (selectImgObj != null)
            selectImgObj.SetActive(false);




        RefreshUnitImg();
    }


    protected override void UnitUISpriteInit()
    {
        base.UnitUISpriteInit();

        if (unitPosObj != null)  //유닛노드의 위치할 오브젝트가 있다면
            Managers.Resource.Instantiate(unitStat.unitSpriteUIPrefabs, unitPosObj.transform);        //유닛노드의 위치한 오브젝트의 하위에 유닛을 생성한다.
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
            SlotClickUnitInfoBtnOnOff(false);       //슬롯클릭시 나오는  UI를 일단 꺼두기

            if (unitPosObj.transform.childCount > 0)     //만약 초기화하는데 유닛포스안에 하위오브젝트가 있다면
                Destroy(unitPosObj.transform.GetChild(0).gameObject);           //그 오브젝트는 삭제해준다.

            return;
        }

        SlotUnitLvTxtObjOnOff(true);
        SlotClickUnitInfoBtnOnOff(false);       //슬롯클릭시 나오는  UI를 일단 꺼두기

        UnitUISpriteInit();
        SlotUnitLvInit(Managers.Game.GetUnitLevel(e_UnitClass));

    }


    public void StartSelectUnitSlot()           //슬롯 선택이 켜지면 텍스트는 없애고 슬롯 선택 화살표는 생기게
    {
        if (selectImgObj != null)
            selectImgObj.SetActive(true);
        if (slotTxtObj != null)
            slotTxtObj.SetActive(false);
    }

    public void MousePointerOnClickImgOnOff(bool check)        //마우스 포인터가 슬롯을 가리키면 클릭이미지가 켜짐 안가리키면 꺼짐
    {
        clickImgObj.SetActive(check);
    }

    //슬롯의 레벨 게임오브젝트의 온오프
    public void SlotUnitLvTxtObjOnOff(bool isOn)
    {
        slotUnitLvTxt?.gameObject.SetActive(isOn);
        
    }

    //이 슬롯을 클릭하면 배치해제버튼과 유닛정보버튼이 나타난다.
    public void SlotClickUnitInfoBtnOnOff(bool btnIsOn)
    {


        //만약 켜지면 
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
