using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UI_UnitSettingWindow : UI_Base
{

    [SerializeField] private GameObject unitNodeContent;
    [SerializeField] private GameObject unitMaskObj;            //����Ŭ���� ����ũ������Ʈ
    [SerializeField] private GameObject slots;            //����Ŭ���� ����ũ������Ʈ
    [SerializeField] private Button backLobbyBtn;            //����Ŭ���� ����ũ������Ʈ

    [SerializeField] private Button leftScrollBtn;           
    [SerializeField] private Button rightScrollBtn;      
    [SerializeField] private Scrollbar scrollbar;
    private bool rightScrollCheck = false;
    private bool LeftScrollCheck = false;
    private float scrollbarSpeed = 1.25f;



    List<UnitSlotUI> unitSlotUiList = new List<UnitSlotUI>();

    private UnitNodeUI unitNodeUI;
    private UnitSlotUI unitSlotUI;
    private UnitSlotUI unitSlotUICancel;
    private UnitSlotUI mousePointerUnitSlotUI;
    private UnitSlotUI tempMousePointerUnitSlotUI;


    private UnitNodeUI curUnitNodeUI;
    private UnitSlotUI curUnitSlotUI;  //Ŭ������ �� ������ ����
    private bool nodeUiClick = false; //������ Ŭ������ �� ���� Ŭ���� ������ �������ִٸ� true�� true�� �� ���콺 Ŭ���� �ϸ� �ٽ� false
    private bool slotUiClick = false;
    private float nodeClickTimer = .0f;


    //��ġ 
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;
    //��ġ

    [SerializeField] private Image fadeImg;


    bool backFadeCheck = false;


    // Start is called before the first frame update
    public override void Start()
    {
        UiInit();
        UI_UnitSetInit();


        if (backLobbyBtn != null)
            backLobbyBtn.onClick.AddListener(() =>
            {
                Managers.Sound.Play("Effect/UI_Click");

                Managers.UI.ClosePopUp(this);
                Managers.UI.ShowPopUp<UI_Lobby>();
                GlobalData.SetUnitClass(unitSlotUiList);
            });


        ButtonEvent(rightScrollBtn.gameObject, RightScrollMoveOn, UIEvent.PointerDown);
        ButtonEvent(rightScrollBtn.gameObject, RightScrollMoveOff, UIEvent.PointerUp);
        ButtonEvent(leftScrollBtn.gameObject, LeftScrollMoveOn, UIEvent.PointerDown);
        ButtonEvent(leftScrollBtn.gameObject, LeftScrollMoveOff, UIEvent.PointerUp);
        //ButtonEvent(leftScrollBtn.gameObject, RightBtnOn, UIEvent.PointerDown);


        startFadeOut = true;

    }

    // Update is called once per frame
    void Update()
    {
        Util.FadeOut(ref startFadeOut, fadeImg);
        BackFadeIn(fadeImg, this, backFadeCheck);

        ped.position = Input.mousePosition;
        OnMousePointerDown();
        OnMousePointerUp();
        OnMousePointerDownCancel();
        RightScrollMove();
        LeftScrollMove();
#if UNITY_EDITOR
        OnMousePointer();
#endif
    }


    void  UI_UnitSetInit()
    {
        //enum�� ������� �������ָ鼭 UnitClass�� �־��ش�.
        if(unitNodeContent != null)
        {
            for(int ii = 0; ii < (int)UnitClass.Count; ii++)
            {
                if (ii == (int)UnitClass.Warrior)
                {
                    if(GlobalData.g_UnitWarriorLv > 0)                          //�ش� ������ ������ 0���� Ŀ�� ���ּ��ÿ� ����
                        UnitNodeUiInstantiate(UnitClass.Warrior);
                }

                else if (ii == (int)UnitClass.Archer)
                {
                    if (GlobalData.g_UnitArcherLv > 0)
                        UnitNodeUiInstantiate(UnitClass.Archer);
                }

                else if (ii == (int)UnitClass.Spear)
                { 
                    if (GlobalData.g_UnitSpearLv > 0)
                        UnitNodeUiInstantiate(UnitClass.Spear);
                }
                else if (ii == (int)UnitClass.Magician)
                {
                    if (GlobalData.g_UnitMagicianLv > 0)
                        UnitNodeUiInstantiate(UnitClass.Magician);
                }
            }

            //���� ���ֵ��� ���ÿ� �������ְ� 
            //���߿� ���ֵ��� �� �����Ǹ� ����������� �������ش�.


        }
    }

    void UiInit()
    {

        TryGetComponent(out gr);
        if(gr == null)
        {
            GameObject go = this.transform.GetChild(0).gameObject;
            if (go != null)
                go.TryGetComponent<GraphicRaycaster>(out gr);
        }

        ped = new PointerEventData(EventSystem.current);
        rrList = new List<RaycastResult>(10);

        for(int ii = 0; ii < slots.transform.childCount; ii++)
        {
            unitSlotUiList.Add(slots.transform.GetChild(ii).GetComponent<UnitSlotUI>());
        }

        for(int ii = 0; ii< unitSlotUiList.Count;ii++)
        {
            unitSlotUiList[ii].SetUnitClass(GlobalData.g_SlotUnitClass[ii]);        //������ ���Կ� �ʱ�ȭ�� ����Ŭ������ �־��ش�.
            unitSlotUiList[ii].RefreshUnitImg();  //����Ŭ������ �ش� ������ �´� �̹����� ���ŵȴ�.
            unitSlotUiList[ii].SlotIdx = ii;     // ���Ժ� �����ε��� �����ֱ�
        }
        
    }

    void ButtonEvent(GameObject obj, Action action = null, UIEvent type = UIEvent.PointerDown)
    {
        UI_EventHandler evt;
        obj.TryGetComponent(out evt);

        switch (type)
        {
            case UIEvent.PointerDown:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action;
                break;

            case UIEvent.PointerUp:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action;
                break;
        }

    }
    void UnitNodeUiInstantiate(UnitClass uniClass)
    {
        GameObject obj = Managers.Resource.Instantiate("UI/Unit", unitNodeContent.transform);
        obj.TryGetComponent<UnitNodeUI>(out unitNodeUI);
        unitNodeUI.SetUnitClass(uniClass);
    }


    #region ��ư������ ��ũ�ѹ� ������

    void RightScrollMoveOn()
    {
        rightScrollCheck = true;
    }
    void RightScrollMoveOff()
    {
        rightScrollCheck = false;
    }

    void LeftScrollMoveOn()
    {
        LeftScrollCheck = true;
    }
    void LeftScrollMoveOff()
    {
        LeftScrollCheck = false;
    }


    void RightScrollMove()
    {
        if(rightScrollCheck)
        {
            if(scrollbar.value < 1.0f)
            {
                scrollbar.value += Time.deltaTime * scrollbarSpeed;
                if (scrollbar.value >= 1.0f)
                    scrollbar.value = 1.0f;
            }
        }
    }

    void LeftScrollMove()
    {
        if (LeftScrollCheck)
        {
            if (scrollbar.value >= 0.0f)
            {
                scrollbar.value -= Time.deltaTime * scrollbarSpeed;
                if (scrollbar.value <= 0.0f)
                    scrollbar.value = 0.0f;
            }
        }
    }
    #endregion

    void OnMousePointerDown()
    {
        if(Input.GetMouseButton(0))
            nodeClickTimer += Time.deltaTime;
    }

    void OnMousePointerUp()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if (Managers.UI.PeekPopupUI<UI_UnitInfoSelectPopUp>() != null)
                return;




            unitSlotUI = UiRaycastGetFirstComponent<UnitSlotUI>(gr);
            unitNodeUI = UiRaycastGetFirstComponent<UnitNodeUI>(gr);

            if(unitSlotUI != null)
            {
                Managers.Sound.Play("Effect/UI_Click");

                if (unitMaskObj.activeSelf)  //����ũ ������Ʈ�� �����ִٸ� ����
                    unitMaskObj.SetActive(false);

                //���ֹ�ġ������ Ŭ������ ��
                if (nodeUiClick)     //������ Ŭ�������� ������ ��带 Ŭ���ߴٸ�
                {
                    LoopEqualUnitSearch(unitSlotUI.SlotIdx);
                    nodeUiClick = false;        //���Ŭ���� ����
                    curUnitNodeUI.ClickImageOnOff(nodeUiClick);
                    curUnitNodeUI = null;
                }
                else
                {
                    if(!slotUiClick)  //������ Ŭ�������ʾ����� (���Գ��� �ڸ��ٲٱ�)
                    {

                        curUnitSlotUI = unitSlotUI;  //�����ѽ����� ���� �������ִ� ���Կ� �־��ְ�
                        if(curUnitSlotUI.E_UnitClass != UnitClass.Count)
                        {
                            slotUiClick = true;     //��带 Ŭ�������ʾҴٸ� ����Ŭ��üũ
                            curUnitSlotUI.ClickImageOnOff(slotUiClick);

                        }
                    }
                    else  //������ Ŭ�������� (���Գ��� �ڸ��ٲٱ�)
                    {
                        SwitchingSlotToSlot(curUnitSlotUI.SlotIdx,unitSlotUI.SlotIdx);
                        slotUiClick = false;
                        curUnitSlotUI.ClickImageOnOff(slotUiClick);
                        curUnitSlotUI = null;


                    }

                }


            }

            else if(unitNodeUI != null)
            {
                if (nodeClickTimer > 0.15f)    //Ŭ���� �ð��� 0.15���̻� ��������
                {
                    nodeClickTimer = 0.0f;
                    return;
                }

                Managers.Sound.Play("Effect/UI_Click");

                //���ֳ�带 Ŭ������ ��
                if (unitMaskObj.activeSelf || slotUiClick)  //����ũ ������Ʈ�� �����ְų� ����Ŭ���� �����ִٸ� ����
                    return;


                nodeUiClick = true;
                //unitMaskObj.SetActive(true);
                curUnitNodeUI = unitNodeUI;     //���� Ŭ���� ���ֳ�忡 �־��ְ�
                curUnitNodeUI.ClickImageOnOff(nodeUiClick);
                Managers.UI.ShowPopUp<UI_UnitInfoSelectPopUp>().PopUpOpenUnitInfoSetting(curUnitNodeUI.E_UnitClass,curUnitNodeUI);


            }

            else
            {
                if (nodeUiClick)
                {
                    unitMaskObj.SetActive(false);
                    nodeUiClick = false;
                    curUnitNodeUI.ClickImageOnOff(nodeUiClick);
                    unitNodeUI = null;
                    curUnitNodeUI = null;
                }

                if (slotUiClick)
                {
                    slotUiClick = false;
                    curUnitSlotUI.ClickImageOnOff(slotUiClick);
                    unitSlotUI = null;
                    curUnitSlotUI = null;

                }
            }

            nodeClickTimer = 0.0f;

        }
    }
    void OnMousePointerDownCancel()
    {
        if (Input.GetMouseButtonDown(1))
        {

            if (unitNodeUI == null && unitSlotUI == null)
                return;

            unitSlotUICancel = UiRaycastGetFirstComponent<UnitSlotUI>(gr);


            if (unitNodeUI != null)
            {
                if (unitMaskObj.activeSelf)  //����ũ ������Ʈ�� �����ִٸ� ���� ���ֳ�嵵 nulló��
                {
                    unitMaskObj.SetActive(false);
                    nodeUiClick = false;
                    curUnitNodeUI.ClickImageOnOff(nodeUiClick);
                    unitNodeUI = null;
                    curUnitNodeUI = null;
                }
            }

            //else if(unitSlotUI != null)
            //{
            //    slotUiClick = false;
            //    curUnitSlotUI.ClickImageOnOff(slotUiClick);
            //    unitSlotUI = null;
            //    curUnitSlotUI = null;

            //}

            else if(unitSlotUICancel != null)
            {
                SlotUnitCancel();
            }

        }
    }

    void OnMousePointer()       //���콺�� ������ ����Ű�� �ش� ���ý����̹����� Ȱ��ȭ
    {
        if (slotUiClick)
            return;

        if (Managers.UI.PeekPopupUI<UI_UnitInfoSelectPopUp>() != null)
            return;

        if (mousePointerUnitSlotUI == null)
            mousePointerUnitSlotUI = UiRaycastGetFirstComponent<UnitSlotUI>(gr);


        if(mousePointerUnitSlotUI != null)
        {
            if(!nodeUiClick)
            {
                if (mousePointerUnitSlotUI.E_UnitClass != UnitClass.Count)
                    mousePointerUnitSlotUI.ClickImageOnOff(true);
            }
            else
                mousePointerUnitSlotUI.ClickImageOnOff(true);
        }

        //Debug.Log(mousePointerUnitSlotUI);
        mousePointerUnitSlotUI = OtherUIRayCast<UnitSlotUI>(mousePointerUnitSlotUI, ref tempMousePointerUnitSlotUI);
        if (mousePointerUnitSlotUI == null)
        {
            if(tempMousePointerUnitSlotUI != null)
                tempMousePointerUnitSlotUI.ClickImageOnOff(false);
        }
    }

    private void LoopEqualUnitSearch(int idx)
    {
        //����Ʈ��ü�� ���Ƽ� ���� ���� ����Ŭ������ �ִ��� �Ǻ�
        UnitClass uniClass = unitSlotUI.E_UnitClass;           //������ ������������� �ִ� ����Ŭ������ ������
        unitSlotUI.SetUnitClass(curUnitNodeUI.E_UnitClass);    //������ ����� Ŭ������ �޾Ƽ� ����                                                       
        unitSlotUiList[unitSlotUI.SlotIdx].SetUnitClass(unitSlotUI.E_UnitClass);  //����Ʈ���� ����



        for (int ii = 0; ii  < unitSlotUiList.Count; ii++)
        {
            if(ii != idx)
            {
                if(unitSlotUiList[idx].E_UnitClass == unitSlotUiList[ii].E_UnitClass)
                {
                    unitSlotUiList[ii].E_UnitClass = uniClass;       //�ߺ��Ǵ� ���� ������ ����� ����Ŭ������ �־���
                    
                }
                       

            }
            unitSlotUiList[ii].RefreshUnitImg();  //����Ŭ������ �ش� ������ �´� �̹����� ���ŵȴ�.

        }

    }


    private void SwitchingSlotToSlot(int curIdx , int selIdx)
    {
        if (unitSlotUiList[curIdx].E_UnitClass == UnitClass.Count)
            return;


        UnitClass tempUnitClass = UnitClass.Count;
        tempUnitClass = unitSlotUiList[curIdx].E_UnitClass;
        unitSlotUiList[curIdx].E_UnitClass = unitSlotUiList[selIdx].E_UnitClass;
        unitSlotUiList[selIdx].E_UnitClass = tempUnitClass;

        for(int ii = 0; ii < unitSlotUiList.Count; ii++)
            unitSlotUiList[ii].RefreshUnitImg();  //����Ŭ������ �ش� ������ �´� �̹����� ���ŵȴ�.

    }


    private void SlotUnitCancel()
    {
        int cnt = 0;

        for(int ii = 0; ii < unitSlotUiList.Count; ii++)
        {
            if (unitSlotUiList[ii].E_UnitClass == UnitClass.Count)
                cnt++;

        }
        if(cnt < 4)
        {
            unitSlotUiList[unitSlotUICancel.SlotIdx].E_UnitClass = UnitClass.Count;
            for (int ii = 0; ii < unitSlotUiList.Count; ii++)
                unitSlotUiList[ii].RefreshUnitImg();  //����Ŭ������ �ش� ������ �´� �̹����� ���ŵȴ�.
        }

    }

    T  OtherUIRayCast<T>(T ui, ref T tempUi) where T : Component
    {
        rrList.Clear();
        gr.Raycast(ped, rrList);

        if (rrList.Count <= 0)
            return ui;

        if (ui !=  null)     //���ϴ� ui�� �ִ� ���¿���
        {

            if (!ui.name.Equals(rrList[0].gameObject.name))
            {
                //����Ʈ�� �ƹ��͵� ���ų� ���ϴ� ui�� ���� ���̴� ui�� �̸��� �����ʴٸ�
                tempUi = ui;
                ui = null;          //�� ó��
                return ui;
               
            }
        }


        return ui;
        
    }



    T UiRaycastGetFirstComponent<T>(GraphicRaycaster gr) where T : Component
    {
        rrList.Clear();

        gr.Raycast(ped, rrList);

        if (rrList.Count == 0)
            return null;
        


        return rrList[0].gameObject.GetComponent<T>();
    }


    public void MaskObjectOnOff(bool check)
    {
        if(unitMaskObj != null)
        {
            unitMaskObj.SetActive(check);
        }
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
                        Managers.UI.ShowPopUp<UI_Lobby>();


                    }
                }
            }
        }

    }
}
