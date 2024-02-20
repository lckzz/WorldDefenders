using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_UpgradeWindow : UI_Base
{
    [Header("-------PlayerUI-------------")]
    [SerializeField] private TextMeshProUGUI playerLvTxt;
    [SerializeField] private TextMeshProUGUI playerHpTxt;
    [SerializeField] private TextMeshProUGUI playerAttTxt;
    [SerializeField] private TextMeshProUGUI goldTxt;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private GameObject upgradeContent;

    [SerializeField] private Image fadeImg;

    [SerializeField] private GameObject unitUpgradePrefab;
    [SerializeField] private GameObject[] unitUpgradeObjs;


    [Space(25)]

    [SerializeField] private Button backLobbyBtn;
    private GameObject tutorialDialogObj;

    
    TowerStat tower = new TowerStat();

    private UnitClass unitClass;

    private bool backFadeCheck = false;

    private IOpenPanel openPanel;

    public bool StartFadeCheck { get { return startFadeOut; } }

    UpgradeUnitNode unitUpgradeNode;

    private MaskDialogCtrl dialogCtrl;

    private GameObject maskGameObject;
    private TutorialMaskCtrl tutorialMaskCtrl;
    private float dialogYPos = -32.5f;


    // Start is called before the first frame update
    public override void Start()
    {
        if (Managers.Game.PlayerLevel == 0)
            Managers.Game.PlayerLevel = 1;

        
       
        unitUpgradeObjs =  new GameObject[(int)UnitClass.Count];
        tutorialDialogObj = gameObject.transform.Find("DialogueCanvas").gameObject; 
        GameObject parentGo = backLobbyBtn?.gameObject.transform.parent.gameObject;
        maskGameObject = parentGo.transform.Find("MaskGameObject").gameObject;
        maskGameObject.TryGetComponent(out tutorialMaskCtrl);

        if (Managers.Game.TutorialEnd == false)
        {
            //Managers.Dialog.dialogEndedStringInt -= UpgradeDialogEnd;
            //Managers.Dialog.dialogEndedStringInt += UpgradeDialogEnd;
            tutorialDialogObj.TryGetComponent(out dialogCtrl);
            Debug.Log(dialogCtrl);

            tutorialDialogObj?.SetActive(!Managers.Game.TutorialEnd);        //Ʃ�丮���� �������ʾҴٸ� ���̾�α� �ѱ�
            dialogCtrl?.MaskDialogInit(tutorialMaskCtrl, backLobbyBtn.gameObject, GameObjectSiblingLastSet);
            dialogCtrl?.StartDialog(DialogKey.tutorialUpgrade.ToString(), DialogType.Dialog, DialogSize.Small,DialogId.NextDialog);
        }



        if (upgradeBtn != null)
            upgradeBtn.onClick.AddListener(PlayerUpgradeOpen);

        if (backLobbyBtn != null)
            backLobbyBtn.onClick.AddListener(() =>
            {
                Managers.Sound.Play("Effect/UI_Click");
                Managers.UI.ClosePopUp(this);
                if (Managers.Scene.CurrentScene is LobbyScene lobby)
                {
                    lobby.LobbyUIOnOff(true);
                    lobby.LobbyUnitInit();
                }
                if (Managers.Game.TutorialEnd == false)
                    Managers.UI.GetSceneUI<UI_Lobby>().DialogMaskSet((int)Define.DialogId.DialogMask,(int)Define.DialogOrder.Party);

            });

        startFadeOut = true;
        goldTxt.text = Managers.Game.Gold.ToString();

        UnitInit();



        //WindowInit();
        
    }

    // Update is called once per frame
    void Update()
    {
        Util.FadeOut(ref startFadeOut, fadeImg);
        //BackFadeIn(fadeImg, this, backFadeCheck);
        RefreshTextUI();        //�� �����Ӹ��� ������������ �ݹ��Լ��� ���ؼ� ���� ����Ǹ� �ݹ��Լ��� ���ؼ� ���ŵǰ�
        
    }



    void UnitInit()
    {
        //for (int ii = 0; ii < (int)UnitClass.Count; ii++)
        //{
        //    Debug.Log(unitUpgradePrefabs.Length);
        //    switch (ii)
        //    {
        //        case (int)UnitClass.Warrior:
        //            unitUpgradePrefabs[ii] = Managers.Resource.Load<GameObject>("Prefabs/UI/UIUnit/WarriorUpgrade");
        //            break;
        //        case (int)UnitClass.Archer:
        //            unitUpgradePrefabs[ii] = Managers.Resource.Load<GameObject>("Prefabs/UI/UIUnit/ArcherUpgrade");
        //            break;
        //        case (int)UnitClass.Spear:
        //            unitUpgradePrefabs[ii] = Managers.Resource.Load<GameObject>("Prefabs/UI/UIUnit/SpearManUpgrade");
        //            break;
        //        case (int)UnitClass.Priest:
        //            unitUpgradePrefabs[ii] = Managers.Resource.Load<GameObject>("Prefabs/UI/UIUnit/PriestUpgrade");
        //            break;
        //        case (int)UnitClass.Magician:
        //            unitUpgradePrefabs[ii] = Managers.Resource.Load<GameObject>("Prefabs/UI/UIUnit/MagicianUpgrade");
        //            break;

        //        case (int)UnitClass.Cavalry:
        //            unitUpgradePrefabs[ii] = Managers.Resource.Load<GameObject>("Prefabs/UI/UIUnit/CavalryUpgrade");
        //            break;

        //        default:
        //            Debug.Log("���� ������");
        //            break;
        //    }
        //}
        unitUpgradePrefab = Managers.Resource.Load<GameObject>("Prefabs/UI/UIUnit/UnitUpgrade");

        if (upgradeContent != null)
        {
            for (int i = 0; i < (int)UnitClass.Count; i++)
            {

                unitUpgradeObjs[i] = Managers.Resource.Instantiate(unitUpgradePrefab, upgradeContent.transform);
                unitUpgradeObjs[i].TryGetComponent(out unitUpgradeNode);
                unitUpgradeNode.UpgradeUnitInit(i);
                
            }
        }
    }


    void PlayerUpgradeOpen()
    {
        Managers.Sound.Play("Effect/UI_Click");
        Managers.UI.ShowPopUp<UI_PlayerUpgradePopUp>();
        openPanel = Managers.UI.PeekPopupUI<UI_PlayerUpgradePopUp>();
        openPanel.OpenRectTransformScaleSet();
    }


    //void UpgradeDialogEnd(string key, int id)
    //{
    //    if(id == (int)DialogId.NextDialog)
    //    {
    //        DialogKey dialogKey = (DialogKey)Enum.Parse(typeof(DialogKey), key);
    //        if(dialogKey >= DialogKey.tutorialUpgrade && dialogKey < DialogKey.tutorialUpgradeUnit)
    //        {  
    //            //���׷��̵����� (���׷��̵� ���̾�α״� ���ҵǾ�����)
    //            dialogKey += 1;
    //            dialogCtrl.StartDialog(dialogKey.ToString(), DialogType.Dialog, DialogSize.Small, DialogId.NextDialog);
    //        }
    //        else if(dialogKey == DialogKey.tutorialUpgradeUnit)
    //        {
    //            //Ʃ�丮����׷��̵尡 ������ �����ϸ� �������̶� 
    //            tutorialMaskCtrl.ShowMaskObject((int)Define.DialogMask.BackMask);
    //            UpgradeGoSiblingSet(backLobbyBtn.gameObject);
    //            return;

    //        }


    //        switch (dialogKey)
    //        {
    //            case DialogKey.tutorialUpgradeTower:
    //                tutorialMaskCtrl.ShowMaskObject((int)Define.DialogMask.FirstMask);
    //                dialogCtrl.DialogPosReset();

    //                break;

    //            case DialogKey.tutorialUpgradeUnit:
    //                tutorialMaskCtrl.ShowMaskObject((int)Define.DialogMask.SecondMask);
    //                dialogCtrl.ChangeDialogPos(dialogYPos);

    //                break;

    //        }
          
    //    }
    //}



    public void RefreshTextUI()
    {
        tower = Managers.Data.towerDict[Managers.Game.PlayerLevel];
        playerLvTxt.text = $"<#FF9F13>Lv</color> {tower.level}";
        playerHpTxt.text = tower.hp.ToString();
        playerAttTxt.text = tower.att.ToString();
        goldTxt.text = Managers.Game.Gold.ToString();
    }


    



    public void UpgradeUnitRefresh(int idx)
    {

        Managers.Game.UnitLvDictRefresh();      //��ųʸ��� �־�� ��� ���ַ����� �������ش�.
        int unitLv = 0;

        if (unitUpgradeObjs[idx] != null)
        {
            unitLv = Managers.Game.UpgradeUnitLvDict[idx];      //���� �ε����� ���ַ����� �޾ƿ´�.

           
            unitUpgradeObjs[idx].TryGetComponent(out unitUpgradeNode);

            unitUpgradeNode.UpgradeUnitInit(idx);
            ////unitUpgradeNode.UnitIdx = idx;
            //if(upgradeUnitRefreshDict.TryGetValue(idx,out upgradeActionInt) == false)
            //{
            //    //���࿡ ��ųʸ��� ���� �˻��� �ȵȴٸ� �װ��� �־��ֱ�
            //    upgradeActionInt = unitUpgradeNode.RefreshUnitImg;
            //    upgradeActionInt(unitLv);
            //    upgradeUnitRefreshDict.Add(idx, upgradeActionInt);
            //}

            //if (upgradeUnitRefreshDict.TryGetValue(idx, out upgradeActionInt))
            //    upgradeUnitRefreshDict[idx](unitLv);


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
                        if (Managers.Scene.CurrentScene is LobbyScene lobby)
                        {
                            lobby.LobbyUIOnOff(true);
                            lobby.LobbyUnitInit();
                        }


                    }
                }
            }
        }

    }
}
