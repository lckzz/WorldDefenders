using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using static Define;

public class UI_UnitSettingWindow : UI_Base
{

    [SerializeField] private GameObject unitNodeContent;
    [SerializeField] private GameObject unitMaskObj;            //유닛클릭시 마스크오브젝트
    [SerializeField] private GameObject slots;            //유닛클릭시 마스크오브젝트
    [SerializeField] private Button backLobbyBtn;            //유닛클릭시 마스크오브젝트

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
    private UnitSlotUI curUnitSlotUI;  //클릭했을 때 슬롯의 정보
    private bool nodeUiClick = false; //유닛을 클릭했을 때 현재 클릭한 정보를 가지고있다면 true고 true일 때 마우스 클릭을 하면 다시 false
    private bool slotUiClick = false;
    private float nodeClickTimer = .0f;


    //터치 
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;
    //터치

    [SerializeField] private Image fadeImg;


    bool backFadeCheck = false;

    //튜토리얼 다이얼로그

    private MaskDialogCtrl dialogCtrl;

    private GameObject maskGameObject;
    private TutorialMaskCtrl tutorialMaskCtrl;

    private GameObject tutorialDialogObj;

    //



    // Start is called before the first frame update
    public override void Start()
    {
        UiInit();
        UI_UnitSetInit();


        unitMaskObj?.SetActive(false);
        tutorialDialogObj = gameObject.transform.Find("DialogueCanvas").gameObject;
        GameObject parentGo = backLobbyBtn?.gameObject.transform.parent.gameObject;
        maskGameObject = parentGo.transform.Find("MaskGameObject").gameObject;
        maskGameObject.TryGetComponent(out tutorialMaskCtrl);

        if (Managers.Game.TutorialEnd == false)
        {
            //Managers.Dialog.dialogEndedStringInt -= UpgradeDialogEnd;
            //Managers.Dialog.dialogEndedStringInt += UpgradeDialogEnd;
            tutorialDialogObj.TryGetComponent(out dialogCtrl);
            tutorialDialogObj?.SetActive(!Managers.Game.TutorialEnd);        //튜토리얼이 끝나지않았다면 다이얼로그 켜기
            dialogCtrl?.MaskDialogInit(tutorialMaskCtrl, backLobbyBtn.gameObject, GameObjectSiblingLastSet);
            dialogCtrl?.StartDialog(DialogKey.tutorialParty.ToString(), DialogType.Dialog, DialogSize.Small, DialogId.NextDialog);
        }


        if (backLobbyBtn != null)
            backLobbyBtn.onClick.AddListener(() =>
            {   
                //뒤로가면 
                Managers.Sound.Play("Effect/UI_Click");
                Managers.Game.FileSave();

                Managers.UI.ClosePopUp(this);
                GlobalData.SetUnitClass(unitSlotUiList);
                if (Managers.Scene.CurrentScene is LobbyScene lobby)
                {
                    lobby.LobbyUIOnOff(true);
                    lobby.LobbyTouchUnitInit();
                }

                if (Managers.Game.TutorialEnd == false)
                    Managers.UI.GetSceneUI<UI_Lobby>().DialogMaskSet((int)Define.DialogId.DialogMask, (int)Define.DialogOrder.Skill);



            });

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

#if UNITY_EDITOR
        OnMousePointer();
#endif
    }


    void  UI_UnitSetInit()
    {
        if(unitNodeContent.transform.childCount > 0)
        {
            //노드가 있다면 콘텐트 자식을 돌아서 삭제
            for(int ii = 0; ii < unitNodeContent.transform.childCount; ii++)
            {
                Destroy(unitNodeContent.transform.GetChild(ii).gameObject);
            }
        }

        //enum의 순서대로 생성해주면서 UnitClass를 넣어준다.
        if(unitNodeContent != null)
        {
            for(int ii = 0; ii < (int)UnitClass.Count; ii++)
            {
                if (ii == (int)UnitClass.Warrior)
                {
                    if(Managers.Game.UnitWarriorLv > 0)                          //해당 유닛의 레벨이 0보다 커야 유닛셋팅에 생성
                        UnitNodeUiInstantiate(UnitClass.Warrior);
                }

                else if (ii == (int)UnitClass.Archer)
                {
                    if (Managers.Game.UnitArcherLv > 0)
                        UnitNodeUiInstantiate(UnitClass.Archer);
                }

                else if (ii == (int)UnitClass.Spear)
                { 
                    if (Managers.Game.UnitSpearLv > 0)
                        UnitNodeUiInstantiate(UnitClass.Spear);
                }
                else if (ii == (int)UnitClass.Priest)
                {
                    if (Managers.Game.UnitPriestLv > 0)
                        UnitNodeUiInstantiate(UnitClass.Priest);
                }
                else if (ii == (int)UnitClass.Magician)
                {
                    if (Managers.Game.UnitMagicianLv > 0)
                        UnitNodeUiInstantiate(UnitClass.Magician);
                }
                else if (ii == (int)UnitClass.Cavalry)
                {
                    if (Managers.Game.UnitCarlvlry > 0)
                        UnitNodeUiInstantiate(UnitClass.Cavalry);
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

        Debug.Log(unitSlotUiList.Count);

        for (int ii = 0; ii< unitSlotUiList.Count;ii++)
        {
            unitSlotUiList[ii].SetUnitClass(Managers.Game.SlotUnitClass[ii]);        //각각의 슬롯에 초기화된 유닛클래슬르 넣어준다.
            unitSlotUiList[ii].RefreshUnitImg();  //유닛클래스와 해당 레벨에 맞는 이미지가 갱신된다.
            unitSlotUiList[ii].SlotIdx = ii;     // 슬롯별 순서인덱스 정해주기
        }
        
    }

    //void ButtonEvent(GameObject obj, Action action = null, UIEvent type = UIEvent.PointerDown)
    //{
    //    UI_EventHandler evt;
    //    obj.TryGetComponent(out evt);

    //    switch (type)
    //    {
    //        case UIEvent.PointerDown:
    //            evt.OnPointerDownHandler -= action;
    //            evt.OnPointerDownHandler += action;
    //            break;

    //        case UIEvent.PointerUp:
    //            evt.OnPointerUpHandler -= action;
    //            evt.OnPointerUpHandler += action;
    //            break;
    //    }

    //}
    void UnitNodeUiInstantiate(UnitClass uniClass)
    {
        GameObject obj = Managers.Resource.Instantiate("UI/Unit", unitNodeContent.transform);
        obj.TryGetComponent<UnitNodeUI>(out unitNodeUI);
        unitNodeUI.SetUnitClass(uniClass);

        for(int ii = 0; ii < unitSlotUiList.Count; ii++)
        {
            //슬롯을 돌아서 슬롯의 유닛타입과 생성한 노드의 타입이 같은게 있다면 장착중인 상태로 인해서 노드삭제
            if (unitSlotUiList[ii].E_UnitClass == unitNodeUI.E_UnitClass)
                unitNodeUI.CheckNodeEquip(Define.UnitNodeState.Equip);
        }
    }



    void OnMousePointerDown()
    {
        if(Input.GetMouseButton(0))
            nodeClickTimer += Time.deltaTime;
    }

    void OnMousePointerUp()
    {

        if (Input.GetMouseButtonUp(0))
        {
            if (Managers.UI.PeekPopupUI<UI_UnitInfoSelectPopUp>() != null)
                return;



            unitSlotUI = UiRaycastGetFirstComponent<UnitSlotUI>(gr);
            unitNodeUI = UiRaycastGetFirstComponent<UnitNodeUI>(gr);

            if(unitSlotUI != null)  //슬롯을 클릭했다면
            {
                Managers.Sound.Play("Effect/UI_Click");
                Debug.Log("테스트클릭");

                if (unitMaskObj.activeSelf)  //마스크 오브젝트가 켜져있다면 리턴
                    unitMaskObj.SetActive(false);



                //유닛배치슬롯을 클릭했을 때
                if (nodeUiClick)     //슬롯을 클릭했을때 그전에 노드를 클릭했다면
                {
                    Debug.Log("요기용");
                    LoopEqualUnitSearch(unitSlotUI.SlotIdx,unitSlotUI);
                    nodeUiClick = false;        //노드클릭은 꺼줌
                    curUnitNodeUI.ClickImageOnOff(nodeUiClick);
                    curUnitNodeUI = null;

                    UI_UnitSetInit();

                }
                else
                {
                    if(!slotUiClick)  //슬롯을 클릭하지않았을때 (슬롯끼리 자리바꾸기)
                    {

                        curUnitSlotUI = unitSlotUI;  //선택한슬롯을 현재 가지고있는 슬롯에 넣어주고
                        if (curUnitSlotUI.E_UnitClass != UnitClass.Count)
                        {
                            curUnitSlotUI.SlotClickUnitInfoBtnOnOff(true);
                            slotUiClick = true;     //노드를 클릭하지않았다면 슬롯클릭체크
                            curUnitSlotUI.ClickImageOnOff(slotUiClick);

                        }
                    }
                    else  //슬롯을 클릭했을때 (슬롯끼리 자리바꾸기)
                    {
                        curUnitSlotUI.SlotClickUnitInfoBtnOnOff(false);     //기존의 인포버튼을 다시 꺼줌
                        SwitchingSlotToSlot(curUnitSlotUI.SlotIdx,unitSlotUI.SlotIdx);
                        slotUiClick = false;
                        curUnitSlotUI.ClickImageOnOff(slotUiClick);
                        curUnitSlotUI = null;


                    }

                }

            }

            else if(unitNodeUI != null)  //유닛을 클릭했다면
            {
                if (nodeClickTimer > 0.15f)    //클릭한 시간이 0.15초이상 지났으면
                {
                    nodeClickTimer = 0.0f;
                    return;
                }

                Managers.Sound.Play("Effect/UI_Click");

                //유닛노드를 클릭했을 때
                if (unitMaskObj.activeSelf)  //마스크 오브젝트가 켜져있거나 슬롯클릭이 켜져있다면 리턴
                    return;

                if(slotUiClick)     //유닛을 클릭했을때 그전에 슬롯이 클릭되었다면
                {
                    Debug.Log("여기 노드클릭하고 슬롯클릭");
                    //슬롯자리에 클릭한 유닛을 넣어줘야함
                    nodeUiClick = true;
                    curUnitNodeUI = unitNodeUI;     //현재 클릭한 유닛노드에 넣어주고
                    LoopEqualUnitSearch(curUnitSlotUI.SlotIdx, curUnitSlotUI);
                    curUnitSlotUI.SlotClickUnitInfoBtnOnOff(false);
                    UnitUIInit();
                    UI_UnitSetInit();


                }

                else
                {
                    Debug.Log("테스트클릭유닛노드");

                    nodeUiClick = true;
                    curUnitNodeUI = unitNodeUI;     //현재 클릭한 유닛노드에 넣어주고
                    curUnitNodeUI.ClickImageOnOff(nodeUiClick);
                    Managers.UI.ShowPopUp<UI_UnitInfoSelectPopUp>().PopUpOpenUnitInfoSetting(curUnitNodeUI.E_UnitClass,Define.UnitInfoSelectType.Node);

                }


            }

            else if(curUnitSlotUI == null && curUnitNodeUI == null)
            {
                if (nodeUiClick)
                {
                    //unitMaskObj.SetActive(false);
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

            else if (unitSlotUI != null)
            {
                slotUiClick = false;
                curUnitSlotUI?.ClickImageOnOff(slotUiClick);
                curUnitSlotUI?.SlotClickUnitInfoBtnOnOff(false);
                unitSlotUI = null;
                curUnitSlotUI = null;
            }

            if(unitSlotUICancel != null)
            {
                curUnitSlotUI = unitSlotUICancel;
                SlotUnitCancel();
                UI_UnitSetInit();

            }

        }
    }

    void OnMousePointer()       //마우스로 슬롯을 가리키면 해당 선택슬롯이미지가 활성화
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

    private void LoopEqualUnitSearch(int idx ,UnitSlotUI unitSlot)
    {
        //리스트전체를 돌아서 현재 같은 유닛클래스가 있는지 판별
        UnitClass uniClass = unitSlot.E_UnitClass;           //슬롯이 덮어쓰여지기전에 있던 유닛클래스를 저장함
        unitSlot.SetUnitClass(curUnitNodeUI.E_UnitClass);    //눌럿던 노드의 클래스를 받아서 적용                                                       
        unitSlotUiList[unitSlot.SlotIdx].SetUnitClass(unitSlot.E_UnitClass);  //리스트에도 적용



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

    private void StartDialog()
    {
        if (Managers.Game.TutorialEnd == true)
            return;

        //Managers.Dialog.dialogEndedStringInt -= UpgradeDialogEnd;
        //Managers.Dialog.dialogEndedStringInt += UpgradeDialogEnd;
        //tutorialDialogObj.TryGetComponent(out dialogCtrl);
        //tutorialDialogObj?.SetActive(!Managers.Game.TutorialEnd);        //튜토리얼이 끝나지않았다면 다이얼로그 켜기
        //dialogCtrl?.StartDialog(DialogKey.tutorialUpgrade.ToString(), DialogType.Dialog, DialogSize.Small, DialogId.NextDialog);

    }


    public void UnitUIInit()
    {
        //유닛 슬롯에 대한 값들 초기화
        slotUiClick = false;
        nodeUiClick = false;
        curUnitNodeUI?.ClickImageOnOff(nodeUiClick);
        curUnitSlotUI?.SlotClickUnitInfoBtnOnOff(false);
        curUnitSlotUI = null;
        curUnitNodeUI = null;

    }

    public void SlotUnitCancel()
    {
        int cnt = 0;

        for(int ii = 0; ii < unitSlotUiList.Count; ii++)
        {
            if (unitSlotUiList[ii].E_UnitClass == UnitClass.Count)
                cnt++;

        }
        if(cnt < 4)
        {
            unitSlotUiList[curUnitSlotUI.SlotIdx].E_UnitClass = UnitClass.Count;
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


    public void MaskObjectOnOff(bool check)
    {

        if(unitMaskObj != null)
            unitMaskObj.SetActive(check);

        
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


    public int SlotListCount()
    {
        int count = 0;

        for(int ii = 0; ii < unitSlotUiList.Count; ii++)
        {
            if (unitSlotUiList[ii].E_UnitClass == UnitClass.Count)
                count++;
        }

        return count;
    }
}
