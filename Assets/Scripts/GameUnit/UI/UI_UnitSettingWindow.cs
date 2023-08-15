using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_UnitSettingWindow : UI_Base
{

    [SerializeField] private GameObject unitNodeContent;
    [SerializeField] private GameObject unitMaskObj;            //����Ŭ���� ����ũ������Ʈ
    [SerializeField] private GameObject slots;            //����Ŭ���� ����ũ������Ʈ
    [SerializeField] private Button backLobbyBtn;            //����Ŭ���� ����ũ������Ʈ

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
   


    //��ġ 
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;
    //��ġ


    // Start is called before the first frame update
    void Start()
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

    }

    // Update is called once per frame
    void Update()
    {
        ped.position = Input.mousePosition;
        OnMousePointerDown();
        OnMousePointerDownCancel();
        OnMousePointer();
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


    void UnitNodeUiInstantiate(UnitClass uniClass)
    {
        GameObject obj = Managers.Resource.Instantiate("UI/Unit", unitNodeContent.transform);
        obj.TryGetComponent<UnitNodeUI>(out unitNodeUI);
        unitNodeUI.SetUnitClass(uniClass);
    }


    void OnMousePointerDown()
    {
        if(Input.GetMouseButtonDown(0))
        {

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

                    for (int ii = 0; ii < unitSlotUiList.Count; ii++)
                        Debug.Log(unitSlotUiList[ii].E_UnitClass);
                    LoopEqualUnitSearch(unitSlotUI.SlotIdx);
                    nodeUiClick = false;        //���Ŭ���� ����
                    curUnitNodeUI.ClickImageOnOff(nodeUiClick);
                    curUnitNodeUI = null;
                }
                else
                {
                    if(!slotUiClick)
                    {

                        curUnitSlotUI = unitSlotUI;  //�����ѽ����� ���� �������ִ� ���Կ� �־��ְ�
                        if(curUnitSlotUI.E_UnitClass != UnitClass.Count)
                        {
                            slotUiClick = true;     //��带 Ŭ�������ʾҴٸ� ����Ŭ��üũ
                            curUnitSlotUI.ClickImageOnOff(slotUiClick);

                        }
                    }
                    else
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
                Managers.Sound.Play("Effect/UI_Click");

                //���ֳ�带 Ŭ������ ��
                if (unitMaskObj.activeSelf || slotUiClick)  //����ũ ������Ʈ�� �����ְų� ����Ŭ���� �����ִٸ� ����
                    return;


                nodeUiClick = true;
                unitMaskObj.SetActive(true);
                curUnitNodeUI = unitNodeUI;     //���� Ŭ���� ���ֳ�忡 �־��ְ�
                curUnitNodeUI.ClickImageOnOff(nodeUiClick);
                


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


        if(mousePointerUnitSlotUI == null)
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
}
