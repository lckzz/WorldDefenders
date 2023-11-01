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

    [SerializeField] private GameObject levelUpParticle;

    private int towerLv = 0;
    private int towerMaxLv = 10;

    private LevelUpParticle levelUp;
    void PlayerInit()
    {
        towerLv = GlobalData.g_PlayerLevel;


        if (towerLv > 10)
            return;

        tower = Managers.Data.towerDict[towerLv];
        curLvTxt.text = tower.level.ToString();
        curHpTxt.text = tower.hp.ToString();
        curAttTxt.text = tower.att.ToString();


        if (towerLv < 10)
        {
            int nextLv = towerLv + 1;
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

        levelUpParticle.TryGetComponent(out levelUp);

        if (upgradeBtn != null)
            upgradeBtn.onClick.AddListener(UpgradeNoticePanelOn);

        if (closeBtn != null)
            closeBtn.onClick.AddListener(() =>
            {
                levelUp.DoKill();
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

        if (towerLv < 10)
            GlobalData.g_PlayerLevel++;

        RefreshTextUI();
    }


    void RefreshTextUI()
    {
        towerLv = GlobalData.g_PlayerLevel;
        tower = Managers.Data.towerDict[towerLv];
        curLvTxt.text = tower.level.ToString();
        curHpTxt.text = tower.hp.ToString();
        curAttTxt.text = tower.att.ToString();

        if (towerLv < 10)
        {
            int nextLv = towerLv + 1;
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
        if (towerLv >= towerMaxLv)
            return;

        noticePanel.SetActive(true);
    }

    public void LevelUpParticleOn()
    {
        if (levelUpParticle.activeSelf)
            levelUpParticle.SetActive(false);           //켜져있다면 강제로 끄고 다시 갱신

        levelUpParticle.SetActive(true);
    }

}
