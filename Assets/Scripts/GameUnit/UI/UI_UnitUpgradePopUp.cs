using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_UnitUpgradePopUp : UI_Base,IOpenPanel
{

    [SerializeField] private Button closeBtn;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private Image curUnitImg;
    [SerializeField] private Image nextUnitImg;
    [SerializeField] private TextMeshProUGUI goldTxt;


    [Header("--------------CurUnit---------------")]
    [SerializeField] private TextMeshProUGUI curLvTxt;
    [SerializeField] private TextMeshProUGUI curHpTxt;
    [SerializeField] private TextMeshProUGUI curAttTxt;
    [SerializeField] private TextMeshProUGUI curCostTxt;



    [Header("--------------NextUnit---------------")]
    [SerializeField] private TextMeshProUGUI nextLvTxt;
    [SerializeField] private TextMeshProUGUI nextHpTxt;
    [SerializeField] private TextMeshProUGUI nextAttTxt;
    [SerializeField] private TextMeshProUGUI nextCostTxt;



    [Header("--------------MaxLevel---------------")]
    [SerializeField] private TextMeshProUGUI maxLvTxt;
    [SerializeField] private TextMeshProUGUI maxHpTxt;
    [SerializeField] private TextMeshProUGUI maxAttTxt;
    [SerializeField] private TextMeshProUGUI maxCostTxt;


    [Header("--------------LevelObj---------------")]
    [SerializeField] private GameObject normalLevelObj;
    [SerializeField] private GameObject maxLevelObj;


    [Space(50)]
    [SerializeField] private GameObject upgradeObj;
    [SerializeField] private GameObject noticePanel;
    [SerializeField] private GameObject levelUpParticle;

    private LevelUpParticle levelUp;
    private UpgradeNotice upgradeNotice;

    private RectTransform rt;
    private int unitLv = 0;
    private int nextUnitLv = 0;
    private int unitMaxLv = 10;


    [SerializeField] private GameObject curUnitParentObj;
    [SerializeField] private GameObject nextUnitParentObj;
    [SerializeField] private GameObject maxUnitParentObj;


    private int unitidx = 0;                //받아올 유닛의 클래스의정보
    
    private UnitStat unit = new UnitStat();
    private UnitStat nextUnit = new UnitStat();


    // Start is called before the first frame update
    public override void Start()
    {
        levelUpParticle.TryGetComponent(out levelUp);
        upgradeObj.TryGetComponent(out rt);
        noticePanel.transform.Find("Notice").TryGetComponent(out upgradeNotice);


        UnitInit();

        if (closeBtn != null)
            closeBtn.onClick.AddListener(() =>
            {
                Managers.Sound.Play("Effect/UI_Click");
                levelUp.DoKill();           //남아있는 파티클의 두트윈을 전부 삭제한다.
                Managers.UI.ClosePopUp(this);
                Managers.UI.PeekPopupUI<UI_UpgradeWindow>().UpgradeUnitRefresh(unitidx);
            });

        if (upgradeBtn != null)
            upgradeBtn.onClick.AddListener(UpgradeNoticePanelOn);




    }

    // Update is called once per frame
    //void Update()
    //{

    //}


    void UnitInit()
    {



        switch (unitidx)     // .버튼을 통해서 선택된 유닛
        {
            case (int)UnitClass.Warrior:
                RefreshUnitStatUI(Managers.Game.UnitWarriorLv, Managers.Data.warriorDict);
                break;
            case (int)UnitClass.Archer:
                RefreshUnitStatUI(Managers.Game.UnitArcherLv, Managers.Data.archerDict);
                break;
            case (int)UnitClass.Spear:
                RefreshUnitStatUI(Managers.Game.UnitSpearLv, Managers.Data.spearDict);
                break;
            case (int)UnitClass.Priest:
                RefreshUnitStatUI(Managers.Game.UnitPriestLv, Managers.Data.priestDict);
                break;
            case (int)UnitClass.Magician:
                RefreshUnitStatUI(Managers.Game.UnitMagicianLv, Managers.Data.magicDict);
                break;
            case (int)UnitClass.Cavalry:
                RefreshUnitStatUI(Managers.Game.UnitCavalryLv, Managers.Data.cavarlyDict);
                break;
        }

        RefreshUnitImgAnim();


    }


    void RefreshUnitStatUI(int unitLv, Dictionary<int,UnitStat> unitDict)
    {

        unit = unitDict[unitLv];

        nextUnitLv = unitLv + 1;
        if(nextUnitLv >= unitMaxLv)
        {
            nextUnitLv = unitMaxLv;
        }

        nextUnit = unitDict[nextUnitLv];

        this.unitLv = unitLv;

        if(JudgmentMaxLevel() == true)      //유닛이 맥스레벨이라면
        {
            maxLvTxt.text = $"Level {unit.level}";
            maxHpTxt.text = unit.hp.ToString();
            maxAttTxt.text = unit.att.ToString();
            maxCostTxt.text = unit.cost.ToString();
            goldTxt.text = "Max";

        }

        else  //맥스레벨이 아니라면
        {
            //현재 유닛
            curLvTxt.text = $"Level {unit.level}";
            curHpTxt.text = unit.hp.ToString();
            curAttTxt.text = unit.att.ToString();
            curCostTxt.text = unit.cost.ToString();

            //다음레벨 유닛
            nextLvTxt.text = $"Level {nextUnit.level}";
            nextHpTxt.text = nextUnit.hp.ToString();
            nextAttTxt.text = nextUnit.att.ToString();
            nextCostTxt.text = nextUnit.cost.ToString();
            goldTxt.text = nextUnit.price.ToString();
            

        }
    }

    void RefreshUnitImgAnim()
    {
        if(curUnitParentObj.transform.childCount > 0)       //만약 안에 이미 하위 오브젝트들이 존재한다면 다 없애줌
        {
            for (int ii = 0; ii < curUnitParentObj.transform.childCount; ii++)
            {
                Destroy(curUnitParentObj.transform.GetChild(ii).gameObject);
                Destroy(nextUnitParentObj.transform.GetChild(ii).gameObject);

            }
        }

        if(unit.level < unitMaxLv)     //현재 유닛의 레벨이 만렙보다 작으면 현재와 다음레벨에 맞는 이미지 생성  
        {
            Managers.Resource.Instantiate(unit.unitUIPrefabs, curUnitParentObj.transform);
            Managers.Resource.Instantiate(nextUnit.unitUIPrefabs, nextUnitParentObj.transform);
        }
        else    //만렙시 현재 유닛만 생성
            Managers.Resource.Instantiate(unit.unitUIPrefabs, maxUnitParentObj.transform);




    }



    void UpgradeNoticePanelOn()
    {
        if (unitLv >= unitMaxLv)
            return;

        Managers.Sound.Play("Effect/UI_Click");
        noticePanel.SetActive(true);
        upgradeNotice.SetUpgradeGold(unit.price);
    }

    bool JudgmentMaxLevel()      //레벨이 맥스인지 판단
    {
        if(unitLv >= unitMaxLv)  //만약 최대레벨이라면 맥스레벨만표시되게
        {
            maxLevelObj.SetActive(true);
            normalLevelObj.SetActive(false);
            return true;
        }
        else
        {
            maxLevelObj.SetActive(false);
            normalLevelObj.SetActive(true);
            return false;
        }
    }

    public void UpgradeUnit()
    {

        switch (unitidx)     // .버튼을 통해서 선택된 유닛
        {
            case (int)UnitClass.Warrior:
                if(Managers.Game.UnitWarriorLv < 10)
                    Managers.Game.UnitWarriorLv++;
                RefreshUnitStatUI(Managers.Game.UnitWarriorLv, Managers.Data.warriorDict);
                break;
            case (int)UnitClass.Archer:
                if (Managers.Game.UnitArcherLv < 10)
                    Managers.Game.UnitArcherLv++;
                RefreshUnitStatUI(Managers.Game.UnitArcherLv, Managers.Data.archerDict);
                break;
            case (int)UnitClass.Spear:
                if (Managers.Game.UnitSpearLv < 10)
                    Managers.Game.UnitSpearLv++;
                RefreshUnitStatUI(Managers.Game.UnitSpearLv, Managers.Data.spearDict);
                break;
            case (int)UnitClass.Priest:
                if (Managers.Game.UnitPriestLv < 10)
                    Managers.Game.UnitPriestLv++;
                RefreshUnitStatUI(Managers.Game.UnitPriestLv, Managers.Data.priestDict);
                break;

            case (int)UnitClass.Magician:
                if (Managers.Game.UnitMagicianLv < 10)
                    Managers.Game.UnitMagicianLv++;
                RefreshUnitStatUI(Managers.Game.UnitMagicianLv, Managers.Data.magicDict);
                Managers.Game.SetSpecialUnitSkillInit((UnitClass)unitidx, Managers.Game.UnitMagicianLv);             //해당 유닛의 레벨에 따라서 스킬레벨 변경
                break;
            case (int)UnitClass.Cavalry:
                if (Managers.Game.UnitCavalryLv < 10)
                    Managers.Game.UnitCavalryLv++;
                RefreshUnitStatUI(Managers.Game.UnitCavalryLv, Managers.Data.cavarlyDict);
                Managers.Game.SetSpecialUnitSkillInit((UnitClass)unitidx, Managers.Game.UnitCavalryLv);             // 해당 유닛의 레벨에 따라서 스킬레벨 변경

                break;

        }

        RefreshUnitImgAnim();

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


    public void GetUnitIndex(int unitidx)  { this.unitidx = unitidx; }
    



    public void OpenUpgradePopUpInit()
    {
        //해당되는 클래스의 정보를 받아와서 그에 맞는 이미지로 변환
        //해당 클래스의 json형식을 받아와서 적용
    }



}
