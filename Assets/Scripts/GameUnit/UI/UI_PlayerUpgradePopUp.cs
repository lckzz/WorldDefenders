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
    [Header("----------------curLv----------------")]
    [SerializeField] private GameObject curLvObj;

    [SerializeField] private TextMeshProUGUI curLvTxt;
    [SerializeField] private TextMeshProUGUI curHpTxt;
    [SerializeField] private TextMeshProUGUI curAttTxt;

    [Space(20)]
    [Header("----------------NextLv----------------")]

    [SerializeField] private GameObject nextLvObj;

    [SerializeField] private TextMeshProUGUI nextLvTxt;
    [SerializeField] private TextMeshProUGUI nextHpTxt;
    [SerializeField] private TextMeshProUGUI nextAttTxt;

    [Space(20)]
    [Header("----------------MaxLv----------------")]

    [SerializeField] private GameObject maxLvObj;
    [SerializeField] private TextMeshProUGUI maxLvTxt;
    [SerializeField] private TextMeshProUGUI maxHpTxt;
    [SerializeField] private TextMeshProUGUI maxAttTxt;

    [Space(20)]
    [SerializeField] private GameObject nextArrowObj;
    [SerializeField] private TextMeshProUGUI goldTxt;

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
        //towerLv = Managers.Game.PlayerLevel;


        //if (towerLv > 10)
        //    return;

        //tower = Managers.Data.towerDict[towerLv];
        //curLvTxt.text = tower.level.ToString();
        //curHpTxt.text = tower.hp.ToString();
        //curAttTxt.text = tower.att.ToString();


        //if (towerLv < 10)
        //{
        //    int nextLv = towerLv + 1;
        //    tower = Managers.Data.towerDict[nextLv];


        //    nextLvTxt.text = tower.level.ToString();
        //    nextHpTxt.text = tower.hp .ToString();
        //    nextAttTxt.text = tower.att.ToString();
        //    goldTxt.text = tower.price.ToString();
        //}
        //else
        //{
        //    nextLvTxt.text = tower.level.ToString();
        //    nextHpTxt.text = tower.hp.ToString();
        //    nextAttTxt.text = tower.att.ToString();
        //}
        RefreshTextUI();


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


        if (IsTowerMaxLevel() == false)
        {
            //타워가 만렙이 아니라면
            curLvObj.SetActive(true);
            nextArrowObj.SetActive(true);
            nextLvObj.SetActive(true);
            maxLvObj.SetActive(false);

            curLvTxt.text = tower.level.ToString();
            curHpTxt.text = tower.hp.ToString();
            curAttTxt.text = tower.att.ToString();


            int nextLv = towerLv + 1;
            tower = Managers.Data.towerDict[nextLv];
            nextLvTxt.text = tower.level.ToString();
            nextHpTxt.text = tower.hp.ToString();
            nextAttTxt.text = tower.att.ToString();
            goldTxt.text = tower.price.ToString();
            Debug.Log("테스트2");

        }

        else
        {

            Debug.Log("테스트");
            //타워가 만렙이라면
            curLvObj.SetActive(false);
            nextArrowObj.SetActive(false);
            nextLvObj.SetActive(false);
            maxLvObj.SetActive(true);

            maxLvTxt.text = tower.level.ToString();
            maxHpTxt.text = tower.hp.ToString();
            maxAttTxt.text = tower.att.ToString();
            goldTxt.text = "Max";
        }



        if (towerLv == 3 && Managers.Game.FireArrowSkillLv < 1)  //3레벨 달성시 폭발화살 레벨업!해서 개방
        {
            Managers.Game.FireArrowSkillLv = 1;
            skillOpen.SetActive(true);
            skillOpenNotice.SetPlayerSkillInfo(Define.PlayerSkill.FireArrow);
        }

        if (towerLv == 7 && Managers.Game.WeaknessSkillLv < 1) //7레벨 달성시 약화 레벨업해서 개방
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


    bool IsTowerMaxLevel()
    {
        if (towerLv < towerMaxLv)
            return false;

        return true;
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
