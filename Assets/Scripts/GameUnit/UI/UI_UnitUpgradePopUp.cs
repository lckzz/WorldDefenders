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


    private int unitidx = 0;                //�޾ƿ� ������ Ŭ����������
    
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
                levelUp.DoKill();           //�����ִ� ��ƼŬ�� ��Ʈ���� ���� �����Ѵ�.
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



        switch (unitidx)     // .��ư�� ���ؼ� ���õ� ����
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

        if(JudgmentMaxLevel() == true)      //������ �ƽ������̶��
        {
            maxLvTxt.text = $"Level {unit.level}";
            maxHpTxt.text = unit.hp.ToString();
            maxAttTxt.text = unit.att.ToString();
            maxCostTxt.text = unit.cost.ToString();
            goldTxt.text = "Max";

        }

        else  //�ƽ������� �ƴ϶��
        {
            //���� ����
            curLvTxt.text = $"Level {unit.level}";
            curHpTxt.text = unit.hp.ToString();
            curAttTxt.text = unit.att.ToString();
            curCostTxt.text = unit.cost.ToString();

            //�������� ����
            nextLvTxt.text = $"Level {nextUnit.level}";
            nextHpTxt.text = nextUnit.hp.ToString();
            nextAttTxt.text = nextUnit.att.ToString();
            nextCostTxt.text = nextUnit.cost.ToString();
            goldTxt.text = nextUnit.price.ToString();
            

        }
    }

    void RefreshUnitImgAnim()
    {
        if(curUnitParentObj.transform.childCount > 0)       //���� �ȿ� �̹� ���� ������Ʈ���� �����Ѵٸ� �� ������
        {
            for (int ii = 0; ii < curUnitParentObj.transform.childCount; ii++)
            {
                Destroy(curUnitParentObj.transform.GetChild(ii).gameObject);
                Destroy(nextUnitParentObj.transform.GetChild(ii).gameObject);

            }
        }

        if(unit.level < unitMaxLv)     //���� ������ ������ �������� ������ ����� ���������� �´� �̹��� ����  
        {
            Managers.Resource.Instantiate(unit.unitUIPrefabs, curUnitParentObj.transform);
            Managers.Resource.Instantiate(nextUnit.unitUIPrefabs, nextUnitParentObj.transform);
        }
        else    //������ ���� ���ָ� ����
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

    bool JudgmentMaxLevel()      //������ �ƽ����� �Ǵ�
    {
        if(unitLv >= unitMaxLv)  //���� �ִ뷹���̶�� �ƽ�������ǥ�õǰ�
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

        switch (unitidx)     // .��ư�� ���ؼ� ���õ� ����
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
                Managers.Game.SetSpecialUnitSkillInit((UnitClass)unitidx, Managers.Game.UnitMagicianLv);             //�ش� ������ ������ ���� ��ų���� ����
                break;
            case (int)UnitClass.Cavalry:
                if (Managers.Game.UnitCavalryLv < 10)
                    Managers.Game.UnitCavalryLv++;
                RefreshUnitStatUI(Managers.Game.UnitCavalryLv, Managers.Data.cavarlyDict);
                Managers.Game.SetSpecialUnitSkillInit((UnitClass)unitidx, Managers.Game.UnitCavalryLv);             // �ش� ������ ������ ���� ��ų���� ����

                break;

        }

        RefreshUnitImgAnim();

    }

    public void LevelUpParticleOn()
    {
        if (levelUpParticle.activeSelf)
            levelUpParticle.SetActive(false);           //�����ִٸ� ������ ���� �ٽ� ����

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
        //�ش�Ǵ� Ŭ������ ������ �޾ƿͼ� �׿� �´� �̹����� ��ȯ
        //�ش� Ŭ������ json������ �޾ƿͼ� ����
    }



}
