using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeWindow : MonoBehaviour
{
    public TextMeshProUGUI playerLvTxt;
    public TextMeshProUGUI playerHpTxt;
    public TextMeshProUGUI playerAttTxt;
    public Button upgradeBtn;
    public Button[] unitUpgradeBtn;

    public GameObject playerUpgradePopUp;
    public GameObject unitUpgradePopUp;

    TowerStat tower = new TowerStat();

    // Start is called before the first frame update
    void Start()
    {


        if (GlobalData.g_PlayerLevel == 0)
            GlobalData.g_PlayerLevel = 1;

        if (upgradeBtn != null)
            upgradeBtn.onClick.AddListener(UpgradeBtnOpen);
        
        for(int ii = 0; ii < unitUpgradeBtn.Length; ii++)
        {
            unitUpgradeBtn[ii].onClick.AddListener(() =>
            {
                unitUpgradePopUp.SetActive(true);
            }
            );
        }
    }

    // Update is called once per frame
    void Update()
    {
        RefreshTextUI();


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
}
