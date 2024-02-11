using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public class UI_PlayerUpgradePopUp : UI_Base,IOpenPanel
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
    [SerializeField] private GameObject upgradeObj;

    [SerializeField] private GameObject levelUpParticle;
    [SerializeField] private GameObject skillOpen;
    private int towerLv = 0;
    private int towerMaxLv = 10;

    private RectTransform rt;


    private LevelUpParticle levelUp;
    private UpgradeNotice upgradeNotice;
    private SkillOpenNotice skillOpenNotice;

    void PlayerInit()
    {
        towerLv = Managers.Game.PlayerLevel;


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
    public override void Start()
    {
        PlayerInit();

        levelUpParticle.TryGetComponent(out levelUp);
        noticePanel.transform.Find("Notice").TryGetComponent(out upgradeNotice);
        skillOpen.TryGetComponent(out skillOpenNotice);
        //OpenRectTransformScaleSet();


        if (upgradeBtn != null)
            upgradeBtn.onClick.AddListener(UpgradeNoticePanelOn);

        if (closeBtn != null)
            closeBtn.onClick.AddListener(() =>
            {
                levelUp.DoKill();
                Managers.Sound.Play("Effect/UI_Click");
                Managers.UI.ClosePopUp(this);
            });
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void Upgrade()
    {

        if (towerLv < 10)
            Managers.Game.PlayerLevel++;

        RefreshTextUI();
    }


    void RefreshTextUI()
    {
        towerLv = Managers.Game.PlayerLevel;
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

        if (towerLv == 3)  //3레벨 달성시 폭발화살 레벨업!해서 개방
        {
            Managers.Game.FireArrowSkillLv = 1;
            skillOpen.SetActive(true);
            skillOpenNotice.SetPlayerSkillInfo(Define.PlayerSkill.FireArrow);
        }

        if (towerLv == 7) //7레벨 달성시 약화 레벨업해서 개방
        {
            Managers.Game.WeaknessSkillLv = 1;
            skillOpen.SetActive(true);
            skillOpenNotice.SetPlayerSkillInfo(Define.PlayerSkill.Weakness);

        }

    }


    void UpgradeNoticePanelOn()
    {
        if (towerLv >= towerMaxLv)
            return;

        Managers.Sound.Play("Effect/UI_Click");
        noticePanel.SetActive(true);
        upgradeNotice.SetUpgradeGold(tower.price);
    }

    public void LevelUpParticleOn()
    {
        if (levelUpParticle.activeSelf)
            levelUpParticle.SetActive(false);           //켜져있다면 강제로 끄고 다시 갱신

        levelUpParticle.SetActive(true);
    }

    public void OpenRectTransformScaleSet()
    {
        if(rt == null)
            upgradeObj.TryGetComponent(out rt);

        rt.localScale = new Vector3(0, 0, 0);
        rt.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.1f).SetEase(Ease.OutQuad);
    }
}
