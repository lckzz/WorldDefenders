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
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private GameObject upgradeContent;

    [SerializeField] private Image fadeImg;

    [SerializeField] private GameObject[] unitUpgradePrefabs = new GameObject[(int)UnitClass.Count];
    [SerializeField] private GameObject[] unitUpgradeObjs = new GameObject[(int)UnitClass.Count];


    [Space(25)]

    [SerializeField] private Button backLobbyBtn;

    
    TowerStat tower = new TowerStat();

    private UnitClass unitClass;

    bool backFadeCheck = false;

    public bool StartFadeCheck { get { return startFadeOut; } }

    UpgradeUnitNode unitUpgradeNode;

    // Start is called before the first frame update
    void Start()
    {
        if (GlobalData.g_PlayerLevel == 0)
            GlobalData.g_PlayerLevel = 1;



        if (upgradeBtn != null)
            upgradeBtn.onClick.AddListener(PlayerUpgradeOpen);

        if (backLobbyBtn != null)
            backLobbyBtn.onClick.AddListener(() =>
            {
                Managers.Sound.Play("Effect/UI_Click");

                Managers.UI.ClosePopUp(this);
                Managers.UI.ShowPopUp<UI_Lobby>();

            });

        startFadeOut = true;

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
                case (int)UnitClass.Magician:
                    unitUpgradePrefabs[ii] = Managers.Resource.Load<GameObject>("Prefabs/UI/UIUnit/MagicianUpgrade");

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






    void RefreshTextUI()
    {
        tower = Managers.Data.towerDict[GlobalData.g_PlayerLevel];
        playerLvTxt.text = $"Lv {tower.level}";
        playerHpTxt.text = tower.hp.ToString();
        playerAttTxt.text = tower.att.ToString();
    }







  


    void PlayerUpgradeOpen()
    {
        Managers.Sound.Play("Effect/UI_Click");

        Managers.UI.ShowPopUp<UI_PlayerUpgradePopUp>();
    }


    public void UpgradeUnitRefresh(int idx)
    {
        

        if (unitUpgradeObjs[idx] != null)
        {

            unitUpgradeObjs[idx].TryGetComponent(out unitUpgradeNode);

            switch (idx)
            {
                case (int)UnitClass.Warrior:
                    unitUpgradeNode.RefreshUnitImg(GlobalData.g_UnitWarriorLv);
                    break;
                case (int)UnitClass.Archer:
                    unitUpgradeNode.RefreshUnitImg(GlobalData.g_UnitArcherLv);
                    break;
                case (int)UnitClass.Spear:
                    unitUpgradeNode.RefreshUnitImg(GlobalData.g_UnitSpearLv);
                    break;
                case (int)UnitClass.Magician:
                    unitUpgradeNode.RefreshUnitImg(GlobalData.g_UnitMagicianLv);
                    break;
            }
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
