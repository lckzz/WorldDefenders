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
    [SerializeField] private Button clearBtn;   //슬롯에서 정보버튼클릭하면 생기는 배치해제버튼    

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
                //배치를 누르면 팝업이 꺼지고 선택할 수 있게
            });

        if (clearBtn != null)
            clearBtn.onClick.AddListener(() =>
            {
                Managers.Sound.Play("Effect/UI_Click");
                Managers.UI.ClosePopUp(this);
                Managers.UI.PeekPopupUI<UI_UnitSettingWindow>()?.SlotUnitCancel();

                //배치를 누르면 팝업이 꺼지고 선택할 수 있게
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

        //인포창 유닛에 맞게 초기화 해줌
        //normalSkillObj?.transform.GetChild(0).TryGetComponent(out normalImg);

        if (UnitClass.Magician <= unitClass)  //스페셜 유닛일때만 정보창에 스페셜스킬이 나오도록
        {
            specialSkillInfo.SkillInfoOn();     //스킬인포이미지 켜주기
            skillData = unitSkillDict[unitClass][Managers.Game.SpecialUnitSkillLvDict[unitClass]];

        }


        if(Managers.Game.UnitStatDict.TryGetValue(unitClass, out Dictionary<int,UnitStat> stat))
        {
            uniStat = stat[Managers.Game.GetUnitLevel(unitClass)];
            normalSkillInfo?.SkillInfoImgInit(unitClass,Managers.Resource.Load<Sprite>($"Sprite/{skillStr[(int)unitWeaponDict[unitClass]]}"),$"일반공격\n{uniStat.attackDesc}");
            if (specialSkillInfo != null)     //스킬이 켜져있다면
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
        unitInfoNameTxt.text = uniStat.name;           //이름 추가해서 넣기
        unitInfoLvTxt.text = $"Level : {uniStat.level}";
        unitInfoTxt.text = $"Hp : {uniStat.hp}\n공격력 : {uniStat.att}\n코스트 : {uniStat.cost}";
        unitInfoDescTxt.text = uniStat.desc.ToString();


    }

 

    void InstantiateUnitObj()
    {
        int lv = Managers.Game.GetUnitLevel(unitClass);     //해당 유닛 클래스의 레벨을 받아옴

        Managers.Resource.Instantiate(uniStat.unitUIPrefabs, unitInfoBgObj.transform);


        //if (unitInfoDict.ContainsKey(unitClass))
        //{
        //    var(path, strArray) = unitInfoDict[unitClass];

        //    if (strArray.Length > 1) //1보다 크면 (일반 유닛)
        //    {
        //        if (lv < 5)
        //            UnitInstantiate(path + strArray[0]);
        //        else
        //            UnitInstantiate(path + strArray[1]);
        //    }
        //    else  //1보다 작으면 (스페셜 유닛)
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
