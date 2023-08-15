using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_UnitSettingWindow : UI_Base
{

    [SerializeField] private GameObject unitNodeContent;
    [SerializeField] private GameObject unitMaskObj;            //유닛클릭시 마스크오브젝트
    [SerializeField] private GameObject slots;            //유닛클릭시 마스크오브젝트
    [SerializeField] private Button backLobbyBtn;            //유닛클릭시 마스크오브젝트

    List<UnitSlotUI> unitSlotUiList = new List<UnitSlotUI>();

    private UnitNodeUI unitNodeUI;
    private UnitSlotUI unitSlotUI;
    private UnitSlotUI unitSlotUICancel;
    private UnitSlotUI mousePointerUnitSlotUI;
    private UnitSlotUI tempMousePointerUnitSlotUI;


    private UnitNodeUI curUnitNodeUI;
    private UnitSlotUI curUnitSlotUI;  //클릭했을 때 슬롯의 정보
    private bool nodeUiClick = false; //유닛을 클릭했을 때 현재 클릭한 정보를 가지고있다면 true고 true일 때 마우스 클릭을 하면 다시 false
    private bool slotUiClick = false;
   


    //터치 
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;
    //터치


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
        //enum의 순서대로 생성해주면서 UnitClass를 넣어준다.
        if(unitNodeContent != null)
        {
            for(int ii = 0; ii < (int)UnitClass.Count; ii++)
            {
                if (ii == (int)UnitClass.Warrior)
                {
                    if(GlobalData.g_UnitWarriorLv > 0)                          //해당 유닛의 레벨이 0보다 커야 유닛셋팅에 생성
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

            //먼저 유닛들을 셋팅에 생성해주고 
            //나중에 유닛들이 다 생성되면 스페셜유닛을 생성해준다.


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
            unitSlotUiList[ii].SetUnitClass(GlobalData.g_SlotUnitClass[ii]);        //각각의 슬롯에 초기화된 유닛클래슬르 넣어준다.
            unitSlotUiList[ii].RefreshUnitImg();  //유닛클래스와 해당 레벨에 맞는 이미지가 갱신된다.
            unitSlotUiList[ii].SlotIdx = ii;     // 슬롯별 순서인덱스 정해주기
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

                if (unitMaskObj.activeSelf)  //마스크 오브젝트가 켜져있다면 리턴
                    unitMaskObj.SetActive(false);

                //유닛배치슬롯을 클릭했을 때
                if (nodeUiClick)     //슬롯을 클릭했을때 그전에 노드를 클릭했다면
                {

                    for (int ii = 0; ii < unitSlotUiList.Count; ii++)
                        Debug.Log(unitSlotUiList[ii].E_UnitClass);
                    LoopEqualUnitSearch(unitSlotUI.SlotIdx);
                    nodeUiClick = false;        //노드클릭은 꺼줌
                    curUnitNodeUI.ClickImageOnOff(nodeUiClick);
                    curUnitNodeUI = null;
                }
                else
                {
                    if(!slotUiClick)
                    {

                        curUnitSlotUI = unitSlotUI;  //선택한슬롯을 현재 가지고있는 슬롯에 넣어주고
                        if(curUnitSlotUI.E_UnitClass != UnitClass.Count)
                        {
                            slotUiClick = true;     //노드를 클릭하지않았다면 슬롯클릭체크
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

                //유닛노드를 클릭했을 때
                if (unitMaskObj.activeSelf || slotUiClick)  //마스크 오브젝트가 켜져있거나 슬롯클릭이 켜져있다면 리턴
                    return;


                nodeUiClick = true;
                unitMaskObj.SetActive(true);
                curUnitNodeUI = unitNodeUI;     //현재 클릭한 유닛노드에 넣어주고
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
                if (unitMaskObj.activeSelf)  //마스크 오브젝트가 켜져있다면 끄고 유닛노드도 null처리
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

    void OnMousePointer()       //마우스로 슬롯을 가리키면 해당 선택슬롯이미지가 활성화
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
        //리스트전체를 돌아서 현재 같은 유닛클래스가 있는지 판별
        UnitClass uniClass = unitSlotUI.E_UnitClass;           //슬롯이 덮어쓰여지기전에 있던 유닛클래스를 저장함
        unitSlotUI.SetUnitClass(curUnitNodeUI.E_UnitClass);    //눌럿던 노드의 클래스를 받아서 적용                                                       
        unitSlotUiList[unitSlotUI.SlotIdx].SetUnitClass(unitSlotUI.E_UnitClass);  //리스트에도 적용



        for (int ii = 0; ii  < unitSlotUiList.Count; ii++)
        {
            if(ii != idx)
            {
                if(unitSlotUiList[idx].E_UnitClass == unitSlotUiList[ii].E_UnitClass)
                {
                    unitSlotUiList[ii].E_UnitClass = uniClass;       //중복되는 곳의 유닛은 저장된 유닛클래스를 넣어줌
                    
                }
                       

            }
            unitSlotUiList[ii].RefreshUnitImg();  //유닛클래스와 해당 레벨에 맞는 이미지가 갱신된다.

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
            unitSlotUiList[ii].RefreshUnitImg();  //유닛클래스와 해당 레벨에 맞는 이미지가 갱신된다.

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
                unitSlotUiList[ii].RefreshUnitImg();  //유닛클래스와 해당 레벨에 맞는 이미지가 갱신된다.
        }

    }

    T  OtherUIRayCast<T>(T ui, ref T tempUi) where T : Component
    {
        rrList.Clear();
        gr.Raycast(ped, rrList);

        if (rrList.Count <= 0)
            return ui;

        if (ui !=  null)     //원하는 ui가 있는 상태에서
        {

            if (!ui.name.Equals(rrList[0].gameObject.name))
            {
                //리스트에 아무것도 없거나 원하는 ui랑 지금 보이는 ui랑 이름이 같지않다면
                tempUi = ui;
                ui = null;          //널 처리
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
