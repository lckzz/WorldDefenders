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
    [SerializeField] private GameObject normalSkillObj;
    [SerializeField] private GameObject specialSkillObj;
    private Image normalImg;
    private Image specialImg;



    [Space(10)]
    [Header("Button")]
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button assignBtn;
    [SerializeField] private Button clearBtn;   //���Կ��� ������ưŬ���ϸ� ����� ��ġ������ư    

    private UnitClass unitClass;
    private Define.UnitInfoSelectType unitInfoType = Define.UnitInfoSelectType.Node;
    private UnitNodeUI nodeUI;
    private UnitStat uniStat;


    private string[] warriorStr = { "WarriorLv1", "WarriorLv2", "WarriorLv3" };
    private string[] archerStr = { "ArcherLv1", "ArcherLv2", "ArcherLv3" };
    private string[] spearStr = { "SpearManLv1", "SpearManLv2", "SpearManLv3" };
    private string[] priestStr = { "PriestLv1", "PriestLv2", "PriestLv3" };

    private string magicianStr = "MagicianLv1";
    private string cavalryStr = "CavalryLv1";

    private string[] skillStr = { "Icon_Sword", "Icon_Bow", "Icon_Spear", "Icon_Magic", "Icon_MagicianSkill", "Icon_CavalrySkill" };


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
                Managers.UI.PeekPopupUI<UI_UnitSettingWindow>()?.UnitUIInit();

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

        if (clearBtn != null)
            clearBtn.onClick.AddListener(() =>
            {
                Managers.UI.ClosePopUp(this);
                Managers.UI.PeekPopupUI<UI_UnitSettingWindow>()?.SlotUnitCancel();

                //��ġ�� ������ �˾��� ������ ������ �� �ְ�
            });




    }

    public void PopUpOpenUnitInfoSetting(UnitClass uniClass,Define.UnitInfoSelectType unitInfoType)
    {
        this.unitInfoType = unitInfoType;
        unitClass = uniClass;
        uniStat = new UnitStat();


        if (unitInfoType == Define.UnitInfoSelectType.Node)
        {
            assignBtn.gameObject.SetActive(true);
            clearBtn.gameObject.SetActive(false);
        }
        else
        {
            assignBtn.gameObject.SetActive(false);
            clearBtn.gameObject.SetActive(true);
        }



        normalSkillObj?.transform.GetChild(0).TryGetComponent(out normalImg);
        if (UnitClass.Magician <= unitClass)  //����� �����϶��� ����â�� ����Ƚ�ų�� ��������
        {
            specialSkillObj.SetActive(true);
            specialSkillObj?.transform.GetChild(0).TryGetComponent(out specialImg);
        }

        switch (unitClass)
        {
            case UnitClass.Warrior:
                uniStat = Managers.Data.warriorDict[GlobalData.g_UnitWarriorLv];
                normalImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/{skillStr[(int)Define.UnitWeaponType.Sword]}");
                break;
            case UnitClass.Archer:
                uniStat = Managers.Data.archerDict[GlobalData.g_UnitArcherLv];
                normalImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/{skillStr[(int)Define.UnitWeaponType.Bow]}");
                break;
            case UnitClass.Spear:
                uniStat = Managers.Data.spearDict[GlobalData.g_UnitSpearLv];
                normalImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/{skillStr[(int)Define.UnitWeaponType.Spear]}");
                break;
            case UnitClass.Priest:
                uniStat = Managers.Data.priestDict[GlobalData.g_UnitPriestLv];
                normalImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/{skillStr[(int)Define.UnitWeaponType.Magic]}");
                break;
            case UnitClass.Magician:
                uniStat = Managers.Data.magicDict[GlobalData.g_UnitMagicianLv];
                normalImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/{skillStr[(int)Define.UnitWeaponType.Magic]}");
                specialImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/{skillStr[(int)Define.UnitWeaponType.MagicianSkill]}");
                break;
            case UnitClass.Cavalry:
                uniStat = Managers.Data.cavarlyDict[GlobalData.g_UnitCarlvryLv];
                normalImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/{skillStr[(int)Define.UnitWeaponType.Spear]}");
                specialImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/{skillStr[(int)Define.UnitWeaponType.CavalrySkill]}");
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
        unitInfoTxt.text = $"Hp : {uniStat.hp}\n���ݷ� : {uniStat.att}\n�ڽ�Ʈ : {uniStat.cost}";
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
            case UnitClass.Priest:
                if (GlobalData.g_UnitPriestLv < 5)
                    UnitInstantiate($"UI/UIUnit/Priest/{priestStr[0]}");

                else
                    UnitInstantiate($"UI/UIUnit/Priest/{priestStr[1]}");

                break;
            case UnitClass.Magician:
                UnitInstantiate($"UI/UIUnit/Magician/{magicianStr}");
                break;

            case UnitClass.Cavalry:
                UnitInstantiate($"UI/UIUnit/Cavalry/{cavalryStr}");
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
