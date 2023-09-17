using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitInfoSelectPopUp : UI_Base
{

    //ĳ���� ���ÿ��� ������ ������ ������ ���� �ְ� ��ġ�� �� �ִ� ��ư�� �ִ� ���̴�.
    //���⼭ �����ؾ� �Ǵ� �κе��� �ϴ� ���� ������ ������ ������ ������ �;��Ѵ�.
    //������ �� ������ �����͸� �޾ƿͼ� UI(�̸�, ���ݷ�,ü��,����, ĳ���� ����, ����) �ؽ�Ʈ�� �����ϰ�
    //��ġ��ư�� ������ �˾��� ������ �ش� ĳ������ ������ ����â�� �����ش�.
    //��ų�� ���� ��ų������ ������ְ� ���� Ÿ���� ���� �⺻ �����̸� ����Ƚ�ų������Ʈ�� ���ְ� �¿����� ����
    //��ų ������ũ��Ʈ�� �̿��ؼ� �ش� ��ų�̹����� ������ �Ǹ� ������ ������ ����
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
                //��ġ�� ������ �˾��� ������ ������ �� �ְ�
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
        unitInfoNameTxt.text = uniStat.name;           //�̸� �߰��ؼ� �ֱ�
        unitInfoLvTxt.text = $"Level : {uniStat.level}";
        unitInfoTxt.text = $"���ݷ� : {uniStat.att}\nü�� : {uniStat.hp}\n�ڽ�Ʈ : {uniStat.cost}";
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
