using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


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





    [Header("--------------UnitPrefab---------------")]
    [SerializeField] private GameObject[] warriorPrefabs;
    [SerializeField] private GameObject[] archerPrefabs;
    [SerializeField] private GameObject[] spearPrefabs;
    [SerializeField] private GameObject[] priestPrefabs;


    [Header("------------SpeacialUnit---------------")]
    [SerializeField] private GameObject magicianObj;
    [SerializeField] private GameObject cavarlyObj;



    private int unitidx = 0;                //받아올 유닛의 클래스의정보
    



    // Start is called before the first frame update
    public override void Start()
    {
        UnitInit();

        if (closeBtn != null)
            closeBtn.onClick.AddListener(() =>
            {
                
                Managers.Sound.Play("Effect/UI_Click");

                Managers.UI.ClosePopUp(this);
                Managers.UI.PeekPopupUI<UI_UpgradeWindow>().UpgradeUnitRefresh(unitidx);
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
                RefreshUnitImgAnim(GlobalData.g_UnitWarriorLv, warriorPrefabs);
                break;
            case (int)UnitClass.Archer:
                
                RefreshUnitStatUI(GlobalData.g_UnitArcherLv, Managers.Data.archerDict);
                RefreshUnitImgAnim(GlobalData.g_UnitArcherLv,archerPrefabs);
                break;
            case (int)UnitClass.Spear:
                RefreshUnitStatUI(GlobalData.g_UnitSpearLv, Managers.Data.spearDict);
                RefreshUnitImgAnim(GlobalData.g_UnitSpearLv, spearPrefabs);
                break;
            case (int)UnitClass.Priest:
                RefreshUnitStatUI(GlobalData.g_UnitPriestLv, Managers.Data.priestDict);
                RefreshUnitImgAnim(GlobalData.g_UnitPriestLv, priestPrefabs);
                break;
            case (int)UnitClass.Magician:
                RefreshUnitStatUI(GlobalData.g_UnitMagicianLv, Managers.Data.magicDict);
                RefreshUnitImgAnim(magicianObj);
                break;
            case (int)UnitClass.Cavalry:
                RefreshUnitStatUI(GlobalData.g_UnitCarlvryLv, Managers.Data.cavarlyDict);
                RefreshUnitImgAnim(cavarlyObj);
                break;
        }

    }


    void RefreshUnitStatUI(int unitLv, Dictionary<int,UnitStat> unitDict)
    {
        UnitStat unit = new UnitStat();

        unit = unitDict[unitLv];


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

    void UpgradeUnit()
    {
        Managers.Sound.Play("Effect/UI_Click");

        switch (unitidx)     // .버튼을 통해서 선택된 유닛
        {
            case (int)UnitClass.Warrior:
                if(GlobalData.g_UnitWarriorLv < 10)
                    GlobalData.g_UnitWarriorLv++;
                RefreshUnitStatUI(GlobalData.g_UnitWarriorLv, Managers.Data.warriorDict);
                RefreshUnitImgAnim(GlobalData.g_UnitWarriorLv, warriorPrefabs);
                break;
            case (int)UnitClass.Archer:
                if (GlobalData.g_UnitArcherLv < 10)
                    GlobalData.g_UnitArcherLv++;
                RefreshUnitStatUI(GlobalData.g_UnitArcherLv, Managers.Data.archerDict);
                RefreshUnitImgAnim(GlobalData.g_UnitArcherLv, archerPrefabs);
                break;
            case (int)UnitClass.Spear:
                if (GlobalData.g_UnitSpearLv < 10)
                    GlobalData.g_UnitSpearLv++;
                RefreshUnitStatUI(GlobalData.g_UnitSpearLv, Managers.Data.spearDict);
                RefreshUnitImgAnim(GlobalData.g_UnitSpearLv, spearPrefabs);
                break;
            case (int)UnitClass.Priest:
                if (GlobalData.g_UnitPriestLv < 10)
                    GlobalData.g_UnitPriestLv++;
                RefreshUnitStatUI(GlobalData.g_UnitPriestLv, Managers.Data.priestDict);
                RefreshUnitImgAnim(GlobalData.g_UnitPriestLv, priestPrefabs);
                break;

            case (int)UnitClass.Magician:
                if (GlobalData.g_UnitMagicianLv < 10)
                    GlobalData.g_UnitMagicianLv++;
                RefreshUnitStatUI(GlobalData.g_UnitMagicianLv, Managers.Data.magicDict);
                RefreshUnitImgAnim(magicianObj);
                break;
            case (int)UnitClass.Cavalry:
                if (GlobalData.g_UnitCarlvryLv < 10)
                    GlobalData.g_UnitCarlvryLv++;
                RefreshUnitStatUI(GlobalData.g_UnitCarlvryLv, Managers.Data.cavarlyDict);
                RefreshUnitImgAnim(cavarlyObj);
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
