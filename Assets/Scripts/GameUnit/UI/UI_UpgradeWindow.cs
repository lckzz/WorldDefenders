using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private GameObject[] unitUpgradePrefabs;
    [SerializeField] private GameObject[] unitUpgradeObjs;


    [Space(25)]

    [SerializeField] private Button backLobbyBtn;

    
    TowerStat tower = new TowerStat();

    private UnitClass unitClass;

    bool backFadeCheck = false;

    public bool StartFadeCheck { get { return startFadeOut; } }

    UpgradeUnitNode unitUpgradeNode;


    private Dictionary<int, Action<int>> upgradeUnitRefreshDict = new Dictionary<int, Action<int>>();

    private Action<int> upgradeActionInt;

    // Start is called before the first frame update
    public override void Start()
    {
        if (Managers.Game.PlayerLevel == 0)
            Managers.Game.PlayerLevel = 1;

        


        unitUpgradePrefabs =  new GameObject[(int)UnitClass.Count];
        unitUpgradeObjs =  new GameObject[(int)UnitClass.Count];


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
                    lobby.LobbyTouchUnitInit();
                }

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
        RefreshTextUI();        //매 프레임마다 갱신하지말고 콜백함수를 통해서 값이 변경되면 콜백함수를 통해서 갱신되게
        
    }



    void UnitInit()
    {
        for (int ii = 0; ii < (int)UnitClass.Count; ii++)
        {
            Debug.Log(unitUpgradePrefabs.Length);
            switch (ii)
            {
                case (int)UnitClass.Warrior:
                    unitUpgradePrefabs[ii] = Managers.Resource.Load<GameObject>("Prefabs/UI/UIUnit/WarriorUpgrade");
                    break;
                case (int)UnitClass.Archer:
                    unitUpgradePrefabs[ii] = Managers.Resource.Load<GameObject>("Prefabs/UI/UIUnit/ArcherUpgrade");
                    break;
                case (int)UnitClass.Spear:
                    unitUpgradePrefabs[ii] = Managers.Resource.Load<GameObject>("Prefabs/UI/UIUnit/SpearManUpgrade");
                    break;
                case (int)UnitClass.Priest:
                    unitUpgradePrefabs[ii] = Managers.Resource.Load<GameObject>("Prefabs/UI/UIUnit/PriestUpgrade");
                    break;
                case (int)UnitClass.Magician:
                    unitUpgradePrefabs[ii] = Managers.Resource.Load<GameObject>("Prefabs/UI/UIUnit/MagicianUpgrade");
                    break;

                case (int)UnitClass.Cavalry:
                    unitUpgradePrefabs[ii] = Managers.Resource.Load<GameObject>("Prefabs/UI/UIUnit/CavalryUpgrade");
                    break;

                default:
                    Debug.Log("아직 못넣음");
                    break;
            }
        }


        if (upgradeContent != null)
        {
            for (int i = 0; i < (int)UnitClass.Count; i++)
            {
                if (unitUpgradePrefabs[i] != null)
                {
                    unitUpgradeObjs[i] = Managers.Resource.Instantiate(unitUpgradePrefabs[i], upgradeContent.transform);
                }
            }
        }
    }






    public void RefreshTextUI()
    {
        tower = Managers.Data.towerDict[Managers.Game.PlayerLevel];
        playerLvTxt.text = $"<#FF9F13>Lv</color> {tower.level}";
        playerHpTxt.text = tower.hp.ToString();
        playerAttTxt.text = tower.att.ToString();
        goldTxt.text = Managers.Game.Gold.ToString();
    }







  


    void PlayerUpgradeOpen()
    {
        Managers.Sound.Play("Effect/UI_Click");

        Managers.UI.ShowPopUp<UI_PlayerUpgradePopUp>();
    }




    public void UpgradeUnitRefresh(int idx)
    {

        Managers.Game.UnitLvDictRefresh();      //딕셔너리에 넣어논 모든 유닛레벨을 갱신해준다.
        int unitLv = 0;

        if (unitUpgradeObjs[idx] != null)
        {
            unitLv = Managers.Game.UpgradeUnitLvDict[idx];      //현재 인덱스의 유닛레벨을 받아온다.
            Debug.Log(unitLv);
            unitUpgradeObjs[idx].TryGetComponent(out unitUpgradeNode);
            
            if(upgradeUnitRefreshDict.TryGetValue(idx,out upgradeActionInt) == false)
            {
                //만약에 딕셔너리에 값이 검색이 안된다면 그값을 넣어주기
                upgradeActionInt = unitUpgradeNode.RefreshUnitImg;
                upgradeActionInt(unitLv);
                upgradeUnitRefreshDict.Add(idx, upgradeActionInt);
            }

            if (upgradeUnitRefreshDict.TryGetValue(idx, out upgradeActionInt))
                upgradeActionInt(unitLv);


            //switch (idx)
            //{

            //    case (int)UnitClass.Warrior:
            //        unitUpgradeNode.RefreshUnitImg(Managers.Game.UnitWarriorLv);
            //        break;
            //    case (int)UnitClass.Archer:
            //        unitUpgradeNode.RefreshUnitImg(Managers.Game.UnitArcherLv);
            //        break;
            //    case (int)UnitClass.Spear:
            //        unitUpgradeNode.RefreshUnitImg(Managers.Game.UnitSpearLv);
            //        break;
            //    case (int)UnitClass.Priest:
            //        unitUpgradeNode.RefreshUnitImg(Managers.Game.UnitPriestLv);
            //        break;
            //    case (int)UnitClass.Magician:
            //        unitUpgradeNode.RefreshUnitImg(Managers.Game.UnitMagicianLv);
            //        break;
            //    case (int)UnitClass.Cavalry:
            //        unitUpgradeNode.RefreshUnitImg(Managers.Game.UnitCarlvlry);
            //        break;
            //}
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
                            lobby.LobbyTouchUnitInit();
                        }


                    }
                }
            }
        }

    }
}
