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
    [SerializeField] private GameObject curLevelObj;



    [Header("--------------NextUnit---------------")]
    [SerializeField] private TextMeshProUGUI nextLvTxt;
    [SerializeField] private TextMeshProUGUI nextHpTxt;
    [SerializeField] private TextMeshProUGUI nextAttTxt;
    [SerializeField] private TextMeshProUGUI nextCostTxt;
    [SerializeField] private GameObject[] nextWarriorObjs;
    [SerializeField] private GameObject[] nextArcherObjs;
    [SerializeField] private GameObject[] nextSpearObjs;
    [SerializeField] private GameObject[] nextPriestObjs;
    [SerializeField] private GameObject nextMagicianObj;
    [SerializeField] private GameObject nextCavarlyObj;
    [SerializeField] private GameObject nextLevelObj;


    [Header("--------------MaxLevel---------------")]
    [SerializeField] private TextMeshProUGUI maxLvTxt;
    [SerializeField] private TextMeshProUGUI maxHpTxt;
    [SerializeField] private TextMeshProUGUI maxAttTxt;
    [SerializeField] private TextMeshProUGUI maxCostTxt;
    [SerializeField] private GameObject maxLevelObj;


    [Header("--------------UnitPrefab---------------")]
    [SerializeField] private GameObject[] warriorPrefabs;
    [SerializeField] private GameObject[] archerPrefabs;
    [SerializeField] private GameObject[] spearPrefabs;
    [SerializeField] private GameObject[] priestPrefabs;


    [Header("------------SpeacialUnit---------------")]
    [SerializeField] private GameObject magicianObj;
    [SerializeField] private GameObject cavarlyObj;

    [Space(50)]
    [SerializeField] private GameObject upgradeObj;
    [SerializeField] private GameObject noticePanel;
    [SerializeField] private GameObject levelUpParticle;

    private LevelUpParticle levelUp;
    private UpgradeNotice upgradeNotice;

    private RectTransform rt;
    private int unitLv = 0;
    private int unitMaxLv = 10;


    private int unitidx = 0;                //받아올 유닛의 클래스의정보
    
    private UnitStat unit = new UnitStat();



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
                RefreshUnitImgAnim(Managers.Game.UnitWarriorLv, warriorPrefabs);
                break;
            case (int)UnitClass.Archer:
                RefreshUnitStatUI(Managers.Game.UnitArcherLv, Managers.Data.archerDict);
                RefreshUnitImgAnim(Managers.Game.UnitArcherLv,archerPrefabs);
                break;
            case (int)UnitClass.Spear:
                RefreshUnitStatUI(Managers.Game.UnitSpearLv, Managers.Data.spearDict);
                RefreshUnitImgAnim(Managers.Game.UnitSpearLv, spearPrefabs);
                break;
            case (int)UnitClass.Priest:
                RefreshUnitStatUI(Managers.Game.UnitPriestLv, Managers.Data.priestDict);
                RefreshUnitImgAnim(Managers.Game.UnitPriestLv, priestPrefabs);
                break;
            case (int)UnitClass.Magician:
                RefreshUnitStatUI(Managers.Game.UnitMagicianLv, Managers.Data.magicDict);
                RefreshUnitImgAnim(magicianObj);
                break;
            case (int)UnitClass.Cavalry:
                RefreshUnitStatUI(Managers.Game.UnitCarlvlry, Managers.Data.cavarlyDict);
                RefreshUnitImgAnim(cavarlyObj);
                break;
        }



    }


    void RefreshUnitStatUI(int unitLv, Dictionary<int,UnitStat> unitDict)
    {

        unit = unitDict[unitLv];

        this.unitLv = unitLv;

        if(JudgmentMaxLevel() == true)      //유닛이 맥스레벨이라면
        {
            maxLvTxt.text = $"Level {unit.level}";
            maxHpTxt.text = unit.hp.ToString();
            maxAttTxt.text = unit.att.ToString();
            maxCostTxt.text = unit.cost.ToString();
        }

        else  //맥스레벨이 아니라면
        {
            curLvTxt.text = $"Level {unit.level}";
            curHpTxt.text = unit.hp.ToString();
            curAttTxt.text = unit.att.ToString();
            curCostTxt.text = unit.cost.ToString();



            if (unitLv < unitMaxLv)
            {
                int nextLv = unitLv + 1;
                unit = unitDict[nextLv];


                nextLvTxt.text = $"Level {unit.level}";
                nextHpTxt.text = unit.hp.ToString();
                nextAttTxt.text = unit.att.ToString();
                nextCostTxt.text = unit.cost.ToString();
                goldTxt.text = unit.price.ToString();
            }
            else
            {
                nextLvTxt.text = $"Level {unit.level}";
                nextHpTxt.text = unit.hp.ToString();
                nextAttTxt.text = unit.att.ToString();
                nextCostTxt.text = unit.cost.ToString();
                goldTxt.text = "Max";
            }
        }


    }

    void RefreshUnitImgAnim(int unitLv,GameObject[] unitObjs)
    {
        if(unitObjs == warriorPrefabs)
        {
            if (unitLv < 5)
            {
                warriorPrefabs[(int)Define.UnitUILv.One].SetActive(true);
                warriorPrefabs[(int)Define.UnitUILv.Two].SetActive(false);
                warriorPrefabs[(int)Define.UnitUILv.Three].SetActive(false);

            }

            else
            {
                warriorPrefabs[(int)Define.UnitUILv.One].SetActive(false);
                warriorPrefabs[(int)Define.UnitUILv.Two].SetActive(true);
                warriorPrefabs[(int)Define.UnitUILv.Three].SetActive(false);

            }

            if (unitLv + 1 < 5)
            {
                nextWarriorObjs[(int)Define.UnitUILv.One].SetActive(true);
                nextWarriorObjs[(int)Define.UnitUILv.Two].SetActive(false);
                nextWarriorObjs[(int)Define.UnitUILv.Three].SetActive(false);

            }

            else
            {
                nextWarriorObjs[(int)Define.UnitUILv.One].SetActive(false);
                nextWarriorObjs[(int)Define.UnitUILv.Two].SetActive(true);
                nextWarriorObjs[(int)Define.UnitUILv.Three].SetActive(false);

            }
        }

        if (unitObjs == archerPrefabs)
        {
            if (unitLv < 5)
            {
                archerPrefabs[(int)Define.UnitUILv.One].SetActive(true);
                archerPrefabs[(int)Define.UnitUILv.Two].SetActive(false);
                archerPrefabs[(int)Define.UnitUILv.Three].SetActive(false);

            }

            else
            {
                archerPrefabs[(int)Define.UnitUILv.One].SetActive(false);
                archerPrefabs[(int)Define.UnitUILv.Two].SetActive(true);
                archerPrefabs[(int)Define.UnitUILv.Three].SetActive(false);

            }

            if (unitLv + 1 < 5)
            {
                nextArcherObjs[(int)Define.UnitUILv.One].SetActive(true);
                nextArcherObjs[(int)Define.UnitUILv.Two].SetActive(false);
                nextArcherObjs[(int)Define.UnitUILv.Three].SetActive(false);

            }

            else
            {
                nextArcherObjs[(int)Define.UnitUILv.One].SetActive(false);
                nextArcherObjs[(int)Define.UnitUILv.Two].SetActive(true);
                nextArcherObjs[(int)Define.UnitUILv.Three].SetActive(false);

            }
        }

        if (unitObjs == spearPrefabs)
        {
            if (unitLv < 5)
            {
                spearPrefabs[(int)Define.UnitUILv.One].SetActive(true);
                spearPrefabs[(int)Define.UnitUILv.Two].SetActive(false);
                spearPrefabs[(int)Define.UnitUILv.Three].SetActive(false);

            }

            else
            {
                spearPrefabs[(int)Define.UnitUILv.One].SetActive(false);
                spearPrefabs[(int)Define.UnitUILv.Two].SetActive(true);
                spearPrefabs[(int)Define.UnitUILv.Three].SetActive(false);

            }

            if (unitLv + 1 < 5)
            {
                nextSpearObjs[(int)Define.UnitUILv.One].SetActive(true);
                nextSpearObjs[(int)Define.UnitUILv.Two].SetActive(false);
                nextSpearObjs[(int)Define.UnitUILv.Three].SetActive(false);

            }

            else
            {
                nextSpearObjs[(int)Define.UnitUILv.One].SetActive(false);
                nextSpearObjs[(int)Define.UnitUILv.Two].SetActive(true);
                nextSpearObjs[(int)Define.UnitUILv.Three].SetActive(false);

            }
        }

        if (unitObjs == priestPrefabs)
        {
            if (unitLv < 5)
            {
                priestPrefabs[(int)Define.UnitUILv.One].SetActive(true);
                priestPrefabs[(int)Define.UnitUILv.Two].SetActive(false);
                priestPrefabs[(int)Define.UnitUILv.Three].SetActive(false);

            }

            else
            {
                priestPrefabs[(int)Define.UnitUILv.One].SetActive(false);
                priestPrefabs[(int)Define.UnitUILv.Two].SetActive(true);
                priestPrefabs[(int)Define.UnitUILv.Three].SetActive(false);

            }

            if (unitLv + 1 < 5)
            {
                nextPriestObjs[(int)Define.UnitUILv.One].SetActive(true);
                nextPriestObjs[(int)Define.UnitUILv.Two].SetActive(false);
                nextPriestObjs[(int)Define.UnitUILv.Three].SetActive(false);

            }

            else
            {
                nextPriestObjs[(int)Define.UnitUILv.One].SetActive(false);
                nextPriestObjs[(int)Define.UnitUILv.Two].SetActive(true);
                nextPriestObjs[(int)Define.UnitUILv.Three].SetActive(false);

            }
        }






    }


    void RefreshUnitImgAnim(GameObject unitObj)
    {
        if(unitObj == magicianObj)
        {
            magicianObj.SetActive(true);
            nextMagicianObj.SetActive(true);
        }
        if (unitObj == cavarlyObj)
        {
            cavarlyObj.SetActive(true);
            nextCavarlyObj.SetActive(true);
        }
    }

    void UpgradeNoticePanelOn()
    {
        if (unitLv >= unitMaxLv)
            return;

        noticePanel.SetActive(true);
        upgradeNotice.SetUpgradeGold(unit.price);
    }

    bool JudgmentMaxLevel()      //레벨이 맥스인지 판단
    {
        if(unitLv >= unitMaxLv)  //만약 최대레벨이라면 맥스레벨만표시되게
        {
            maxLevelObj.SetActive(true);
            curLevelObj.SetActive(false);
            nextLevelObj.SetActive(false);
            return true;
        }
        else
        {
            maxLevelObj.SetActive(false);
            curLevelObj.SetActive(true);
            nextLevelObj.SetActive(true);
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
                RefreshUnitImgAnim(Managers.Game.UnitWarriorLv, warriorPrefabs);
                break;
            case (int)UnitClass.Archer:
                if (Managers.Game.UnitArcherLv < 10)
                    Managers.Game.UnitArcherLv++;
                RefreshUnitStatUI(Managers.Game.UnitArcherLv, Managers.Data.archerDict);
                RefreshUnitImgAnim(Managers.Game.UnitArcherLv, archerPrefabs);
                break;
            case (int)UnitClass.Spear:
                if (Managers.Game.UnitSpearLv < 10)
                    Managers.Game.UnitSpearLv++;
                RefreshUnitStatUI(Managers.Game.UnitSpearLv, Managers.Data.spearDict);
                RefreshUnitImgAnim(Managers.Game.UnitSpearLv, spearPrefabs);
                break;
            case (int)UnitClass.Priest:
                if (Managers.Game.UnitPriestLv < 10)
                    Managers.Game.UnitPriestLv++;
                RefreshUnitStatUI(Managers.Game.UnitPriestLv, Managers.Data.priestDict);
                RefreshUnitImgAnim(Managers.Game.UnitPriestLv, priestPrefabs);
                break;

            case (int)UnitClass.Magician:
                if (Managers.Game.UnitMagicianLv < 10)
                    Managers.Game.UnitMagicianLv++;
                RefreshUnitStatUI(Managers.Game.UnitMagicianLv, Managers.Data.magicDict);
                RefreshUnitImgAnim(magicianObj);
                break;
            case (int)UnitClass.Cavalry:
                if (Managers.Game.UnitCarlvlry < 10)
                    Managers.Game.UnitCarlvlry++;
                RefreshUnitStatUI(Managers.Game.UnitCarlvlry, Managers.Data.cavarlyDict);
                RefreshUnitImgAnim(cavarlyObj);
                break;

        }
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
