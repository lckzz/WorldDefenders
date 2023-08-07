using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Animations;


public class UI_UnitUpgradePopUp : UI_Base
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
    [SerializeField] private Animator curAnim;


    [Header("--------------NextUnit---------------")]
    [SerializeField] private TextMeshProUGUI nextLvTxt;
    [SerializeField] private TextMeshProUGUI nextHpTxt;
    [SerializeField] private TextMeshProUGUI nextAttTxt;
    [SerializeField] private TextMeshProUGUI nextCostTxt;
    [SerializeField] private Animator nextAnim;


    [Header("--------------UnitImg---------------")]
    private const int unitUpgradeLevel = 3;       //업글단계는 3단계까지 존재
    [SerializeField] private Sprite[] warriorSprites = new Sprite[unitUpgradeLevel];
    [SerializeField] private Sprite[] archerSprites = new Sprite[unitUpgradeLevel];
    [SerializeField] private Sprite[] spearSprites = new Sprite[unitUpgradeLevel];

    [Header("--------------AnimaoterController---------------")]
    [SerializeField] private AnimatorController[] warriorAnims;
    [SerializeField] private AnimatorController[] archerAnims;
    [SerializeField] private AnimatorController[] spearAnims;

    private int unitidx = 0;                //받아올 유닛의 클래스의정보
    



    // Start is called before the first frame update
    void Start()
    {
        UnitInit();

        if (closeBtn != null)
            closeBtn.onClick.AddListener(() =>
            {
                Managers.UI.ClosePopUp(this);
                Managers.UI.PeekPopupUI<UI_UpgradeWindow>().RefreshUnitImgAnim(GlobalData.g_UnitWarriorLv, GlobalData.g_UnitArcherLv, GlobalData.g_UnitSpearLv);
            });

        if (upgradeBtn != null)
            upgradeBtn.onClick.AddListener(UpgradeUnit);




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
                RefreshUnitStatUI(GlobalData.g_UnitWarriorLv,Managers.Data.warriorDict);
                RefreshUnitImgAnim(GlobalData.g_UnitWarriorLv, warriorAnims);
                break;
            case (int)UnitClass.Archer:
                
                RefreshUnitStatUI(GlobalData.g_UnitArcherLv, Managers.Data.archerDict);
                RefreshUnitImgAnim(GlobalData.g_UnitArcherLv,archerAnims);
                break;
            case (int)UnitClass.Spear:
                RefreshUnitStatUI(GlobalData.g_UnitSpearLv, Managers.Data.spearDict);
                RefreshUnitImgAnim(GlobalData.g_UnitSpearLv, spearAnims);

                break;

        }

    }


    void RefreshUnitStatUI(int unitLv, Dictionary<int,UnitStat> unitDict)
    {
        UnitStat unit = new UnitStat();

        unit = Managers.Data.archerDict[unitLv];


        curLvTxt.text = $"Level {unit.level}";
        curHpTxt.text = unit.hp.ToString();
        curAttTxt.text = unit.att.ToString();
        curCostTxt.text = unit.cost.ToString();


        if(unitLv < 10)
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

    void RefreshUnitImgAnim(int unitLv,AnimatorController[] unitAni)
    {
        if (unitLv < 5)
            curAnim.runtimeAnimatorController = unitAni[0];
        else
            curAnim.runtimeAnimatorController = unitAni[1];

        if(unitLv + 1 < 5)
            nextAnim.runtimeAnimatorController = unitAni[0];
        else
            nextAnim.runtimeAnimatorController = unitAni[1];


    }


    void UpgradeUnit()
    {
        switch (unitidx)     // .버튼을 통해서 선택된 유닛
        {
            case (int)UnitClass.Warrior:
                if(GlobalData.g_UnitWarriorLv < 10)
                    GlobalData.g_UnitWarriorLv++;
                RefreshUnitStatUI(GlobalData.g_UnitWarriorLv, Managers.Data.warriorDict);
                RefreshUnitImgAnim(GlobalData.g_UnitWarriorLv, warriorAnims);
                break;
            case (int)UnitClass.Archer:
                if (GlobalData.g_UnitArcherLv < 10)
                    GlobalData.g_UnitArcherLv++;
                RefreshUnitStatUI(GlobalData.g_UnitArcherLv, Managers.Data.archerDict);
                RefreshUnitImgAnim(GlobalData.g_UnitArcherLv, archerAnims);
                break;
            case (int)UnitClass.Spear:
                if (GlobalData.g_UnitSpearLv < 10)
                    GlobalData.g_UnitSpearLv++;
                RefreshUnitStatUI(GlobalData.g_UnitSpearLv, Managers.Data.spearDict);
                RefreshUnitImgAnim(GlobalData.g_UnitSpearLv, spearAnims);

                break;

        }
    }


    public void GetUnitIndex(int unitidx)  { this.unitidx = unitidx; }
    



    public void OpenUpgradePopUpInit()
    {
        //해당되는 클래스의 정보를 받아와서 그에 맞는 이미지로 변환
        //해당 클래스의 json형식을 받아와서 적용
    }



}
