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

        SlotUnitLvTxtObjOnOff(false);           //처음에 들어오면 레벨은 전부 꺼줌(슬롯안에 유닛이 배치되어야만 온)
        //처음에 생성되면 해당갱신해주기
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
        SlotClickUnitInfoBtnOnOff(false);       //슬롯클릭시 나오는  UI를 일단 꺼두기

        switch (e_UnitClass)
        {

            case UnitClass.Warrior:
                //Debug.Log($"워리어 갱신! {e_UnitClass}");
                rt.sizeDelta = defalutSizeDelta;
                rt.transform.localPosition = defalutTr;
                UnitUISpriteInit(e_UnitClass, Managers.Game.UnitWarriorLv, "KnifeUnitLv1Img", "KnifeUnitLv2Img");
                SlotUnitLvInit(Managers.Game.UnitWarriorLv);
                break;

            case UnitClass.Archer:
                //Debug.Log($"아처 갱신! {e_UnitClass}");
                rt.sizeDelta = defalutSizeDelta;
                rt.transform.localPosition = defalutTr;
                UnitUISpriteInit(e_UnitClass, Managers.Game.UnitArcherLv, "BowUnitLv1Img", "BowUnitLv2Img");
                SlotUnitLvInit(Managers.Game.UnitArcherLv);
                break;

            case UnitClass.Spear:
                //Debug.Log($"창병 갱신! {e_UnitClass}");

                rt.sizeDelta = spearSizeDelta;
                rt.transform.localPosition = spearTr;
                UnitUISpriteInit(e_UnitClass, Managers.Game.UnitSpearLv, "SpearUnitLv1Img", "SpearUnitLv2Img");
                SlotUnitLvInit(Managers.Game.UnitSpearLv);
                break;

            case UnitClass.Priest:
                //Debug.Log($"아처 갱신! {e_UnitClass}");
                rt.sizeDelta = defalutSizeDelta;
                rt.transform.localPosition = defalutTr;
                UnitUISpriteInit(e_UnitClass, Managers.Game.UnitPriestLv, "PriestLv1Img", "PriestLv2Img");
                SlotUnitLvInit(Managers.Game.UnitPriestLv);
                break;

            case UnitClass.Magician:
                //Debug.Log($"창병 갱신! {e_UnitClass}");

                rt.sizeDelta = defalutSizeDelta;
                rt.transform.localPosition = defalutTr;
                UnitUISpriteInit(e_UnitClass, "Magician_Idle");
                SlotUnitLvInit(Managers.Game.UnitMagicianLv);
                break;
            case UnitClass.Cavalry:
                //Debug.Log($"창병 갱신! {e_UnitClass}");

                rt.sizeDelta = defalutSizeDelta;
                rt.transform.localPosition = defalutTr;
                UnitUISpriteInit(e_UnitClass, "Cavalry_Idle");
                SlotUnitLvInit(Managers.Game.UnitCarlvlry);
                break;

            default:
                if (unitImg != null)
                    unitImg.gameObject.SetActive(false);

                SlotClickUnitInfoBtnOnOff(false);       //슬롯클릭시 나오는  UI를 일단 꺼두기
                SlotUnitLvTxtObjOnOff(false);           //처음에 들어오면 레벨은 전부 꺼줌(슬롯안에 유닛이 배치되어야만 온)
                //Debug.Log("유닛노드에 유닛클래스가 설정이 안됫어요;");
                break;

        }
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
