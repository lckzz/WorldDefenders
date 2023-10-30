using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerUpgradePopUp : UI_Base
{
    public TextMeshProUGUI curLvTxt;
    public TextMeshProUGUI curHpTxt;
    public TextMeshProUGUI curAttTxt;

    public TextMeshProUGUI nextLvTxt;
    public TextMeshProUGUI nextHpTxt;
    public TextMeshProUGUI nextAttTxt;
    public TextMeshProUGUI goldTxt;

    public Button upgradeBtn;
    public Button closeBtn;

    TowerStat tower = new TowerStat();

    [SerializeField] private GameObject noticePanel;


    void PlayerInit()
    {
        if (GlobalData.g_PlayerLevel > 10)
            return;

        tower = Managers.Data.towerDict[GlobalData.g_PlayerLevel];
        curLvTxt.text = tower.level.ToString();
        curHpTxt.text = tower.hp.ToString();
        curAttTxt.text = tower.att.ToString();


        if (GlobalData.g_PlayerLevel < 10)
        {
            int nextLv = GlobalData.g_PlayerLevel + 1;
            tower = Managers.Data.towerDict[nextLv];

            
            nextLvTxt.text = tower.level.ToString();
            nextHpTxt.text = tower.hp .ToString();
            nextAttTxt.text = tower.att.ToString();
            goldTxt.text = tower.price.ToString();
        }
        else
        {
            nextLvTxt.text = tower.level.ToString();
            nextHpTxt.text = tower.hp.ToString();
            nextAttTxt.text = tower.att.ToString();
        }

      
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerInit();
        if (upgradeBtn != null)
            upgradeBtn.onClick.AddListener(UpgradeNoticePanelOn);

        if (closeBtn != null)
            closeBtn.onClick.AddListener(() =>
            {
                Managers.UI.ClosePopUp(this);
            });
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void Upgrade()
    {
        Managers.Sound.Play("Effect/UI_Click");

        if (GlobalData.g_PlayerLevel < 10)
            GlobalData.g_PlayerLevel++;

        RefreshTextUI();
    }


    void RefreshTextUI()
    {

        tower = Managers.Data.towerDict[GlobalData.g_PlayerLevel];
        curLvTxt.text = tower.level.ToString();
        curHpTxt.text = tower.hp.ToString();
        curAttTxt.text = tower.att.ToString();

        if (GlobalData.g_PlayerLevel < 10)
        {
            int nextLv = GlobalData.g_PlayerLevel + 1;
            tower = Managers.Data.towerDict[nextLv];
            nextLvTxt.text = tower.level.ToString();
            nextHpTxt.text = tower.hp .ToString();
            nextAttTxt.text = tower.att .ToString();
            goldTxt.text = tower.price.ToString();
        }
        else
        {
            nextLvTxt.text = tower.level.ToString();
            nextHpTxt.text = tower.hp.ToString();
            nextAttTxt.text = tower.att.ToString();
        }
    }


    void UpgradeNoticePanelOn()
    {
        noticePanel.SetActive(true);
    }

}
