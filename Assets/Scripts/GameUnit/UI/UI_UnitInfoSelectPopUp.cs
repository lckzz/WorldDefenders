using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitInfoSelectPopUp : UI_Base
{

    //캐릭터 셋팅에서 선택한 유닛의 정보를 볼수 있고 배치할 수 있는 버튼이 있는 곳이다.
    //여기서 연결해야 되는 부분들은 일단 내가 선택한 유닛의 정보를 가지고 와야한다.
    //가지고 온 유닛의 데이터를 받아와서 UI(이름, 공격력,체력,레벨, 캐릭터 생성, 설명) 텍스트를 연결하고
    //배치버튼을 누르면 팝업이 꺼지고 해당 캐릭터의 정보를 선택창에 보내준다.
    //스킬은 따로 스킬인포를 만들어주고 유닛 타입을 만들어서 기본 유닛이면 스페셜스킬오브젝트를 꺼주고 온오프로 관리
    //스킬 인포스크립트를 이용해서 해당 스킬이미지를 누르게 되면 툴팁이 나오게 설정
    [Header("Unit Info")]
    [SerializeField] private TextMeshProUGUI unitInfoNameTxt;
    [SerializeField] private TextMeshProUGUI unitInfoTxt;
    [SerializeField] private TextMeshProUGUI unitInfoLvTxt;

    [SerializeField] private TextMeshProUGUI unitInfoDescTxt;
    [SerializeField] private GameObject unitInfoBgObj;

    [Space(10)]
    [Header("Button")]
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button assignBtn;

    private UnitClass unitClass;
    private UnitNodeUI nodeUI;
    private UnitStat uniStat;


    private string[] warriorStr = { "WarriorLv1", "WarriorLv2", "WarriorLv3" };
    private string[] archerStr = { "ArcherLv1", "ArcherLv2", "ArcherLv3" };
    private string[] spearStr = { "SpearManLv1", "SpearManLv2", "SpearManLv3" };
    private string magicianStr = "MagicianLv1";


    public override bool Init()
    {
        base.Init();
        return true;
    }


    public override void Start()
    {
        base.Start();

        if (closeBtn != null)
            closeBtn.onClick.AddListener(() =>
            {
                if (nodeUI != null)
                    nodeUI.ClickImageOnOff(false);
                Managers.UI.ClosePopUp(this);
            });


        if (assignBtn != null)
            assignBtn.onClick.AddListener(() =>
            {
                if (nodeUI != null)
                    nodeUI.ClickImageOnOff(false);
                Managers.UI.ClosePopUp(this);
                Managers.UI.PeekPopupUI<UI_UnitSettingWindow>()?.MaskObjectOnOff(true);
                //배치를 누르면 팝업이 꺼지고 선택할 수 있게
            });
    }

    public void PopUpOpenUnitInfoSetting(UnitClass uniClass,UnitNodeUI nodeUI)
    {
        unitClass = uniClass;
        this.nodeUI = nodeUI;
        uniStat = new UnitStat();
        switch(unitClass)
        {
            case UnitClass.Warrior:
                uniStat = Managers.Data.warriorDict[GlobalData.g_UnitWarriorLv];
                break;
            case UnitClass.Archer:
                uniStat = Managers.Data.archerDict[GlobalData.g_UnitArcherLv];
                break;
            case UnitClass.Spear:
                uniStat = Managers.Data.spearDict[GlobalData.g_UnitSpearLv];
                break;
            case UnitClass.Magician:
                uniStat = Managers.Data.magicDict[GlobalData.g_UnitMagicianLv];
                break;

            default:
                Debug.LogError("UnitClass empty");
                break;

        }

        UISeting();
        InstantiateUnitObj();

    }



    void UISeting()
    {
        unitInfoNameTxt.text = uniStat.name;           //이름 추가해서 넣기
        unitInfoLvTxt.text = $"Level : {uniStat.level}";
        unitInfoTxt.text = $"공격력 : {uniStat.att}\n체력 : {uniStat.hp}\n코스트 : {uniStat.cost}";
        unitInfoDescTxt.text = uniStat.desc.ToString();


    }

    void InstantiateUnitObj()
    {
        switch (unitClass)
        {
            case UnitClass.Warrior:
                if (GlobalData.g_UnitWarriorLv < 5)
                    UnitInstantiate($"UI/UIUnit/Warrior/{warriorStr[0]}");
                else
                    UnitInstantiate($"UI/UIUnit/Warrior/{warriorStr[1]}");


                break;
            case UnitClass.Archer:
                if (GlobalData.g_UnitArcherLv < 5)
                    UnitInstantiate($"UI/UIUnit/Archer/{archerStr[0]}");

                else
                    UnitInstantiate($"UI/UIUnit/Archer/{archerStr[1]}");

                break;
            case UnitClass.Spear:
                if (GlobalData.g_UnitSpearLv < 5)
                    UnitInstantiate($"UI/UIUnit/Spear/{spearStr[0]}");

                else
                    UnitInstantiate($"UI/UIUnit/Spear/{spearStr[1]}");

                break;
            case UnitClass.Magician:
                UnitInstantiate($"UI/UIUnit/Magician/{magicianStr}");


                break;

            default:
                Debug.LogError("UnitClass empty");
                break;

        }
        
    }


    void UnitInstantiate(string path)
    {
        Managers.Resource.Instantiate(path, unitInfoBgObj.transform);

    }

}
