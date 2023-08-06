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
        RefreshTextUI();        //�� �����Ӹ��� ������������ �ݹ��Լ��� ���ؼ� ���� ����Ǹ� �ݹ��Լ��� ���ؼ� ���ŵǰ�

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
                    Debug.Log("������ �����ǳ� ��!");
                    break;
                }
            case (int)UnitClass.Archer:
                {
                    Debug.Log("��ó �����ǳ� ��!");
                    break;
                }
            case (int)UnitClass.Spear:
                {
                    Debug.Log("â�� �����ǳ� ��!!");
                    break;
                }
        }



    }
}
