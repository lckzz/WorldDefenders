using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeWindow : UI_Base
{
    public TextMeshProUGUI playerLvTxt;
    public TextMeshProUGUI playerHpTxt;
    public TextMeshProUGUI playerAttTxt;
    public Button upgradeBtn;
    public Button[] unitUpgradeBtn;

    public GameObject playerUpgradePopUp;
    public GameObject unitUpgradePopUp;

    TowerStat tower = new TowerStat();

    private UnitClass unitClass;

    // Start is called before the first frame update
    void Start()
    {


        if (GlobalData.g_PlayerLevel == 0)
            GlobalData.g_PlayerLevel = 1;

        if (upgradeBtn != null)
            upgradeBtn.onClick.AddListener(UpgradeBtnOpen);

        UnitButtonEvent(unitUpgradeBtn[(int)UnitClass.Warrior].gameObject,(int)UnitClass.Warrior, OpenUpgradeUnitPopUp, UIEvnet.PointerDown);
        UnitButtonEvent(unitUpgradeBtn[(int)UnitClass.Archer].gameObject, (int)UnitClass.Archer, OpenUpgradeUnitPopUp, UIEvnet.PointerDown);
        UnitButtonEvent(unitUpgradeBtn[(int)UnitClass.Spear].gameObject, (int)UnitClass.Spear, OpenUpgradeUnitPopUp, UIEvnet.PointerDown);
    }

    // Update is called once per frame
    void Update()
    {
        RefreshTextUI();        //매 프레임마다 갱신하지말고 콜백함수를 통해서 값이 변경되면 콜백함수를 통해서 갱신되게

    }




    void UnitButtonEvent(GameObject obj, int idx ,Action<int> action = null, UIEvnet type = UIEvnet.PointerDown)
    {
        UI_EventHandler evt;
        obj.TryGetComponent(out evt);

        if(type == UIEvnet.PointerDown)
        {
            evt.OnPointerDownUnitUpgradeHandler -= (unUsedIdx) => action(idx);
            evt.OnPointerDownUnitUpgradeHandler += (unUsedIdx) => action(idx);

        }
    }


    void RefreshTextUI()
    {
        tower = Managers.Data.towerDict[GlobalData.g_PlayerLevel];
        playerLvTxt.text = $"Lv {tower.level}";
        playerHpTxt.text = tower.hp.ToString();
        playerAttTxt.text = tower.att.ToString();
    }

    void UpgradeBtnOpen()
    {
        if(playerUpgradePopUp != null)
        {
            if (playerUpgradePopUp.activeSelf == false)
                playerUpgradePopUp.SetActive(true);
        }
    }


    void OpenUpgradeUnitPopUp(int unitIdx)
    {
        switch (unitIdx)
        {
            case (int)UnitClass.Warrior:
                {
                    Debug.Log("워리어 업그판넬 온!");
                    break;
                }
            case (int)UnitClass.Archer:
                {
                    Debug.Log("아처 업글판넬 온!");
                    break;
                }
            case (int)UnitClass.Spear:
                {
                    Debug.Log("창병 업글판넬 온!!");
                    break;
                }
        }



    }
}
