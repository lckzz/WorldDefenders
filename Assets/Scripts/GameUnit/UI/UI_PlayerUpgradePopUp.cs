using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerUpgradePopUp : MonoBehaviour
{
    public TextMeshProUGUI curLvTxt;
    public TextMeshProUGUI curHpTxt;
    public TextMeshProUGUI curAttTxt;

    public TextMeshProUGUI nextLvTxt;
    public TextMeshProUGUI nextHpTxt;
    public TextMeshProUGUI nextAttTxt;

    public Button upgradeBtn;
    public Button closeBtn;

    TowerStat tower = new TowerStat();




    void Init()
    {
        if (GlobalData.g_PlayerLevel > 10)
            return;

        tower = Managers.Data.towerDict[GlobalData.g_PlayerLevel];
        curLvTxt.text = tower.level.ToString();
        curHpTxt.text = tower.hp.ToString();
        curAttTxt.text = tower.att.ToString();
        Debug.Log(GlobalData.g_PlayerLevel);


        if (GlobalData.g_PlayerLevel < 10)
        {
            int nextLv = GlobalData.g_PlayerLevel + 1;
            tower = Managers.Data.towerDict[nextLv];

            
            nextLvTxt.text = tower.level.ToString();
            nextHpTxt.text = tower.hp .ToString();
            nextAttTxt.text = tower.att.ToString();
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
        Init();
        if (upgradeBtn != null)
            upgradeBtn.onClick.AddListener(UpgradeBtn);

        if (closeBtn != null)
            closeBtn.onClick.AddListener(() =>
            {
                if (this.gameObject.activeSelf)
                    gameObject.SetActive(false);
            });
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    void UpgradeBtn()
    {
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
        }
        else
        {
            nextLvTxt.text = tower.level.ToString();
            nextHpTxt.text = tower.hp.ToString();
            nextAttTxt.text = tower.att.ToString();
        }
    }
}
