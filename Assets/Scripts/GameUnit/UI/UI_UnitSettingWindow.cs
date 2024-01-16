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

    //Ʃ�丮�� ���̾�α�

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
            tutorialDialogObj?.SetActive(!Managers.Game.TutorialEnd);        //Ʃ�丮���� �������ʾҴٸ� ���̾�α� �ѱ�
            dialogCtrl?.MaskDialogInit(tutorialMaskCtrl, backLobbyBtn.gameObject, GameObjectSiblingLastSet);
            dialogCtrl?.StartDialog(DialogKey.tutorialParty.ToString(), DialogType.Dialog, DialogSize.Small, DialogId.NextDialog);
        }


        if (backLobbyBtn != null)
            backLobbyBtn.onClick.AddListener(() =>
            {   
                //�ڷΰ��� 
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
            //��尡 �ִٸ� ����Ʈ �ڽ��� ���Ƽ� ����
            for(int ii = 0; ii < unitNodeContent.transform.childCount; ii++)
            {
                Destroy(unitNodeContent.transform.GetChild(ii).gameObject);
            }
        }

        //enum�� ������� �������ָ鼭 UnitClass�� �־��ش�.
        if(unitNodeContent != null)
        {
            for(int ii = 0; ii < (int)UnitClass.Count; ii++)
            {
                if (ii == (int)UnitClass.Warrior)
                {
                    if(Managers.Game.UnitWarriorLv > 0)                          //�ش� ������ ������ 0���� Ŀ�� ���ּ��ÿ� ����
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

        Debug.Log(unitSlotUiList.Count);

        for (int ii = 0; ii< unitSlotUiList.Count;ii++)
        {
            unitSlotUiList[ii].SetUnitClass(Managers.Game.SlotUnitClass[ii]);        //������ ���Կ� �ʱ�ȭ�� ����Ŭ������ �־��ش�.
            unitSlotUiList[ii].RefreshUnitImg();  //����Ŭ������ �ش� ������ �´� �̹����� ���ŵȴ�.
            unitSlotUiList[ii].SlotIdx = ii;     // ���Ժ� �����ε��� �����ֱ�
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
            //������ ���Ƽ� ������ ����Ÿ�԰� ������ ����� Ÿ���� ������ �ִٸ� �������� ���·� ���ؼ� ������
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

            if(unitSlotUI != null)  //������ Ŭ���ߴٸ�
            {
                Managers.Sound.Play("Effect/UI_Click");
                Debug.Log("�׽�ƮŬ��");

                if (unitMaskObj.activeSelf)  //����ũ ������Ʈ�� �����ִٸ� ����
                    unitMaskObj.SetActive(false);



                //���ֹ�ġ������ Ŭ������ ��
                if (nodeUiClick)     //������ Ŭ�������� ������ ��带 Ŭ���ߴٸ�
                {
                    Debug.Log("����");
                    LoopEqualUnitSearch(unitSlotUI.SlotIdx,unitSlotUI);
                    nodeUiClick = false;        //���Ŭ���� ����
                    curUnitNodeUI.ClickImageOnOff(nodeUiClick);
                    curUnitNodeUI = null;

                    UI_UnitSetInit();

                }
                else
                {
                    if(!slotUiClick)  //������ Ŭ�������ʾ����� (���Գ��� �ڸ��ٲٱ�)
                    {

                        curUnitSlotUI = unitSlotUI;  //�����ѽ����� ���� �������ִ� ���Կ� �־��ְ�
                        if (curUnitSlotUI.E_UnitClass != UnitClass.Count)
                        {
                            curUnitSlotUI.SlotClickUnitInfoBtnOnOff(true);
                            slotUiClick = true;     //��带 Ŭ�������ʾҴٸ� ����Ŭ��üũ
                            curUnitSlotUI.ClickImageOnOff(slotUiClick);

                        }
                    }
                    else  //������ Ŭ�������� (���Գ��� �ڸ��ٲٱ�)
                    {
                        curUnitSlotUI.SlotClickUnitInfoBtnOnOff(false);     //������ ������ư�� �ٽ� ����
                        SwitchingSlotToSlot(curUnitSlotUI.SlotIdx,unitSlotUI.SlotIdx);
                        slotUiClick = false;
                        curUnitSlotUI.ClickImageOnOff(slotUiClick);
                        curUnitSlotUI = null;


                    }

                }

            }

            else if(unitNodeUI != null)  //������ Ŭ���ߴٸ�
            {
                if (nodeClickTimer > 0.15f)    //Ŭ���� �ð��� 0.15���̻� ��������
                {
                    nodeClickTimer = 0.0f;
                    return;
                }

                Managers.Sound.Play("Effect/UI_Click");

                //���ֳ�带 Ŭ������ ��
                if (unitMaskObj.activeSelf)  //����ũ ������Ʈ�� �����ְų� ����Ŭ���� �����ִٸ� ����
                    return;

                if(slotUiClick)     //������ Ŭ�������� ������ ������ Ŭ���Ǿ��ٸ�
                {
                    Debug.Log("���� ���Ŭ���ϰ� ����Ŭ��");
                    //�����ڸ��� Ŭ���� ������ �־������
                    nodeUiClick = true;
                    curUnitNodeUI = unitNodeUI;     //���� Ŭ���� ���ֳ�忡 �־��ְ�
                    LoopEqualUnitSearch(curUnitSlotUI.SlotIdx, curUnitSlotUI);
                    curUnitSlotUI.SlotClickUnitInfoBtnOnOff(false);
                    UnitUIInit();
                    UI_UnitSetInit();


                }

                else
                {
                    Debug.Log("�׽�ƮŬ�����ֳ��");

                    nodeUiClick = true;
                    curUnitNodeUI = unitNodeUI;     //���� Ŭ���� ���ֳ�忡 �־��ְ�
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
                if (unitMaskObj.activeSelf)  //����ũ ������Ʈ�� �����ִٸ� ���� ���ֳ�嵵 nulló��
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

    private void LoopEqualUnitSearch(int idx ,UnitSlotUI unitSlot)
    {
        //����Ʈ��ü�� ���Ƽ� ���� ���� ����Ŭ������ �ִ��� �Ǻ�
        UnitClass uniClass = unitSlot.E_UnitClass;           //������ ������������� �ִ� ����Ŭ������ ������
        unitSlot.SetUnitClass(curUnitNodeUI.E_UnitClass);    //������ ����� Ŭ������ �޾Ƽ� ����                                                       
        unitSlotUiList[unitSlot.SlotIdx].SetUnitClass(unitSlot.E_UnitClass);  //����Ʈ���� ����



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

    private void StartDialog()
    {
        if (Managers.Game.TutorialEnd == true)
            return;

        //Managers.Dialog.dialogEndedStringInt -= UpgradeDialogEnd;
        //Managers.Dialog.dialogEndedStringInt += UpgradeDialogEnd;
        //tutorialDialogObj.TryGetComponent(out dialogCtrl);
        //tutorialDialogObj?.SetActive(!Managers.Game.TutorialEnd);        //Ʃ�丮���� �������ʾҴٸ� ���̾�α� �ѱ�
        //dialogCtrl?.StartDialog(DialogKey.tutorialUpgrade.ToString(), DialogType.Dialog, DialogSize.Small, DialogId.NextDialog);

    }


    public void UnitUIInit()
    {
        //���� ���Կ� ���� ���� �ʱ�ȭ
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
