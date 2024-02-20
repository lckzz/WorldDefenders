using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UnitInfoSelectPopUp : UI_Base,IOpenPanel,IPointerDownHandler
{
    public enum UI_UnitInfoSelect
    {
        CloseBtn,
        AssignBtn,
        ClearBtn,
        UnitBg,
        Info,
        NameTek,
        Desc,
        NormalSkillObj,
        SpecialSkillObj
    }


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
    [SerializeField] private GameObject unitInfoObj;
    [SerializeField] private GameObject unitInfoBgObj;
    private GameObject normalSkillObj;
    private GameObject specialSkillObj;
    private RectTransform rt;

    private UnitSkillInfo normalSkillInfo;
    private UnitSkillInfo specialSkillInfo;




    [Space(10)]
    [Header("Button")]
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button assignBtn;
    [SerializeField] private Button clearBtn;   //���Կ��� ������ưŬ���ϸ� ����� ��ġ������ư    

    private UnitClass unitClass;
    private Define.UnitInfoSelectType unitInfoType = Define.UnitInfoSelectType.Node;
    private UnitNodeUI nodeUI;
    private UnitStat uniStat;
    private SkillData skillData;


    private readonly string[] warriorStr = { "WarriorLv1", "WarriorLv2", "WarriorLv3" };
    private readonly string[] archerStr = { "ArcherLv1", "ArcherLv2", "ArcherLv3" };
    private readonly string[] spearStr = { "SpearManLv1", "SpearManLv2", "SpearManLv3" };
    private readonly string[] priestStr = { "PriestLv1", "PriestLv2", "PriestLv3" };

    private readonly string[] magicianStr = { "MagicianLv1" };
    private readonly string[] cavalryStr = { "CavalryLv1" };

    private readonly string[] skillStr = { "Icon_Sword", "Icon_Bow", "Icon_Spear", "Icon_Magic", "Icon_MagicianSkill", "Icon_CavalrySkill" };

    //private Dictionary<UnitClass, Dictionary<int, UnitStat>> unitStatDict;
    private Dictionary<UnitClass, Define.UnitWeaponType> unitWeaponDict;
    private Dictionary<UnitClass, Define.UnitWeaponType> specialUnitSkillImgDict;
    //private Dictionary<UnitClass, (string path, string[] strArray)> unitInfoDict;
    private Dictionary<UnitClass, Dictionary<int, SkillData>> unitSkillDict;






    public override bool Init()
    {
        base.Init();

        normalSkillObj = unitInfoObj.transform.GetChild((int)UI_UnitInfoSelect.NormalSkillObj).gameObject;
        normalSkillObj.TryGetComponent(out normalSkillInfo);


        specialSkillObj = unitInfoObj.transform.GetChild((int)UI_UnitInfoSelect.SpecialSkillObj).gameObject;
        if (unitClass >= UnitClass.Magician)
            specialSkillObj.TryGetComponent(out specialSkillInfo);
        else
            specialSkillObj.SetActive(false);


        InfoDictionaryInit();


        if (closeBtn != null)
            closeBtn.onClick.AddListener(() =>
            {
                if (nodeUI != null)
                    nodeUI.ClickImageOnOff(false);

                Managers.Sound.Play("Effect/UI_Click");
                Managers.UI.ClosePopUp(this);
                Managers.UI.PeekPopupUI<UI_UnitSettingWindow>()?.UnitUIInit();

            });


        if (assignBtn != null)
            assignBtn.onClick.AddListener(() =>
            {
                if (nodeUI != null)
                    nodeUI.ClickImageOnOff(false);

                Managers.Sound.Play("Effect/UI_Click");
                Managers.UI.ClosePopUp(this);
                Managers.UI.PeekPopupUI<UI_UnitSettingWindow>()?.MaskObjectOnOff(true);
                //��ġ�� ������ �˾��� ������ ������ �� �ְ�
            });

        if (clearBtn != null)
            clearBtn.onClick.AddListener(() =>
            {
                Managers.Sound.Play("Effect/UI_Click");
                Managers.UI.ClosePopUp(this);
                Managers.UI.PeekPopupUI<UI_UnitSettingWindow>()?.SlotUnitCancel();

                //��ġ�� ������ �˾��� ������ ������ �� �ְ�
            });

        InfoInit();

        return true;
    }


    public override void Start()
    {
        Init();
        
    }

    public void PopUpOpenUnitInfoSetting(UnitClass uniClass,Define.UnitInfoSelectType unitInfoType)
    {
        this.unitInfoType = unitInfoType;
        unitClass = uniClass;
        uniStat = new UnitStat();
        skillData = new SkillData();
        


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



    }

    void InfoDictionaryInit()
    {
        //unitStatDict = new Dictionary<UnitClass, Dictionary<int, UnitStat>>
        //{
        //    {UnitClass.Warrior, Managers.Data.warriorDict },
        //    {UnitClass.Archer, Managers.Data.archerDict },
        //    {UnitClass.Spear, Managers.Data.spearDict },
        //    {UnitClass.Priest, Managers.Data.priestDict },
        //    {UnitClass.Magician, Managers.Data.magicDict },
        //    {UnitClass.Cavalry, Managers.Data.cavarlyDict },

        //};

        unitWeaponDict = new Dictionary<UnitClass, Define.UnitWeaponType>
        {
            {UnitClass.Warrior, Define.UnitWeaponType.Sword },
            {UnitClass.Archer, Define.UnitWeaponType.Bow },
            {UnitClass.Spear, Define.UnitWeaponType.Spear },
            {UnitClass.Priest, Define.UnitWeaponType.Magic },
            {UnitClass.Magician, Define.UnitWeaponType.Magic },
            {UnitClass.Cavalry, Define.UnitWeaponType.Spear },

        };

        specialUnitSkillImgDict = new Dictionary<UnitClass, Define.UnitWeaponType>
        {
            {UnitClass.Magician, Define.UnitWeaponType.MagicianSkill },
            {UnitClass.Cavalry, Define.UnitWeaponType.CavalrySkill },

        };

        //unitInfoDict = new Dictionary<UnitClass, (string path, string[] strArray)>
        //{
        //    {UnitClass.Warrior, ("UI/UIUnit/Warrior/",warriorStr) },
        //    {UnitClass.Archer, ("UI/UIUnit/Archer/",archerStr) },
        //    {UnitClass.Spear, ("UI/UIUnit/Spear/",spearStr) },
        //    {UnitClass.Priest, ("UI/UIUnit/Priest/",priestStr) },
        //    {UnitClass.Magician, ("UI/UIUnit/Magician/",magicianStr) },
        //    {UnitClass.Cavalry, ("UI/UIUnit/Cavalry/",cavalryStr) }

        //};

        unitSkillDict = new Dictionary<UnitClass, Dictionary<int, SkillData>>
        {
            {UnitClass.Magician, Managers.Data.magicSkillDict },
            {UnitClass.Cavalry, Managers.Data.cavalrySkillDict },
        };

    }




    void InfoInit()
    {

        //����â ���ֿ� �°� �ʱ�ȭ ����
        //normalSkillObj?.transform.GetChild(0).TryGetComponent(out normalImg);

        if (UnitClass.Magician <= unitClass)  //����� �����϶��� ����â�� ����Ƚ�ų�� ��������
        {
            specialSkillInfo.SkillInfoOn();     //��ų�����̹��� ���ֱ�
            skillData = unitSkillDict[unitClass][Managers.Game.SpecialUnitSkillLvDict[unitClass]];

        }


        if(Managers.Game.UnitStatDict.TryGetValue(unitClass, out Dictionary<int,UnitStat> stat))
        {
            uniStat = stat[Managers.Game.GetUnitLevel(unitClass)];
            normalSkillInfo?.SkillInfoImgInit(unitClass,Managers.Resource.Load<Sprite>($"Sprite/{skillStr[(int)unitWeaponDict[unitClass]]}"),$"�Ϲݰ���\n{uniStat.attackDesc}");
            if (specialSkillInfo != null)     //��ų�� �����ִٸ�
            {
                string specialDesc = $"{skillData.name}(Lv{skillData.level})\n{skillData.desc}";
                specialSkillInfo?.SkillInfoImgInit(unitClass,Managers.Resource.Load<Sprite>($"Sprite/{skillStr[(int)specialUnitSkillImgDict[unitClass]]}"), specialDesc);
            }
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
        int lv = Managers.Game.GetUnitLevel(unitClass);     //�ش� ���� Ŭ������ ������ �޾ƿ�

        Managers.Resource.Instantiate(uniStat.unitUIPrefabs, unitInfoBgObj.transform);


        //if (unitInfoDict.ContainsKey(unitClass))
        //{
        //    var(path, strArray) = unitInfoDict[unitClass];

        //    if (strArray.Length > 1) //1���� ũ�� (�Ϲ� ����)
        //    {
        //        if (lv < 5)
        //            UnitInstantiate(path + strArray[0]);
        //        else
        //            UnitInstantiate(path + strArray[1]);
        //    }
        //    else  //1���� ������ (����� ����)
        //        UnitInstantiate(path + strArray[0]);
        //}

    }


    void UnitInstantiate(string path)
    {
        Managers.Resource.Instantiate(path, unitInfoBgObj.transform);

    }

    public void SkillInfoTooltipAllOff()
    {
        if(normalSkillInfo != null)
            normalSkillInfo.ToolTipSetOff();
        if(specialSkillInfo != null)
            specialSkillInfo.ToolTipSetOff();
    }


    public void OpenRectTransformScaleSet()
    {
        if (rt == null)
            unitInfoObj.TryGetComponent(out rt);

        rt.localScale = new Vector3(0, 0, 0);
        rt.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.1f).SetEase(Ease.OutQuad);
    }



    public void OnPointerDown(PointerEventData eventData)
    {

        SkillInfoTooltipAllOff();

    }



}
