using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UnitNodeUI : UI_BaseSettingUnit
{
    //���ֹ�ġ�� �پ��ִ� ���ֳ����� ��ũ��Ʈ
    //[SerializeField] private Image unitImg;
    //[SerializeField] private UnitClass e_UnitClass = UnitClass.Count;
    private TextMeshProUGUI unitLvTxt;
    Vector2 spearSizeDelta = new Vector2(110.0f, 100.0f);
    Vector3 spearTr = new Vector3(-12.0f, 0.0f, 0.0f);
    private Define.UnitNodeState unitNodeState = Define.UnitNodeState.None;



    public UnitClass E_UnitClass { get { return e_UnitClass; } set { e_UnitClass = value; } }


    protected override void Init()
    {
       
        unitImg.TryGetComponent<RectTransform>(out rt);
        for(int ii = 0; ii < transform.childCount; ii++)
        {
            if (transform.GetChild(ii).name.Contains("Lv"))
                transform.GetChild(ii).TryGetComponent(out unitLvTxt);
        }



        //ó���� �����Ǹ� �ش簻�����ֱ�
        switch (e_UnitClass)
        {
            case UnitClass.Warrior:
                Debug.Log($"������ ����! {e_UnitClass}");
                UnitUISpriteInit(e_UnitClass, GlobalData.g_UnitWarriorLv, "KnifeUnitLv1Img", "KnifeUnitLv2Img");
                unitLvTxt.text = $"<#FF9F13>Lv</color> {GlobalData.g_UnitWarriorLv}";
                break;

            case UnitClass.Archer:
                Debug.Log($"��ó ����! {e_UnitClass}");
                UnitUISpriteInit(e_UnitClass, GlobalData.g_UnitArcherLv, "BowUnitLv1Img", "BowUnitLv2Img");
                unitLvTxt.text = $"<#FF9F13>Lv</color> {GlobalData.g_UnitArcherLv}";
                break;

            case UnitClass.Spear:
                Debug.Log($"â�� ����! {e_UnitClass}");
                rt.sizeDelta = spearSizeDelta;
                rt.transform.localPosition = spearTr;
                UnitUISpriteInit(e_UnitClass, GlobalData.g_UnitSpearLv, "SpearUnitLv1Img", "SpearUnitLv2Img");
                unitLvTxt.text = $"<#FF9F13>Lv</color> {GlobalData.g_UnitSpearLv}";
                break;
            case UnitClass.Priest:
                Debug.Log($"���� ����! {e_UnitClass}");
                UnitUISpriteInit(e_UnitClass, GlobalData.g_UnitPriestLv, "PriestLv1Img", "PriestLv2Img");
                unitLvTxt.text = $"<#FF9F13>Lv</color> {GlobalData.g_UnitPriestLv}";
                break;

            case UnitClass.Magician:
                Debug.Log($"������ ����! {e_UnitClass}");
                UnitUISpriteInit(e_UnitClass, "Magician_Idle");
                unitLvTxt.text = $"<#FF9F13>Lv</color> {GlobalData.g_UnitMagicianLv}";
                break;
            case UnitClass.Cavalry:
                Debug.Log($"������ ����! {e_UnitClass}");
                UnitUISpriteInit(e_UnitClass, "Cavalry_Idle");
                unitLvTxt.text = $"<#FF9F13>Lv</color> {GlobalData.g_UnitCarlvryLv}";
                break;

            default:
                Debug.Log("���ֳ�忡 ����Ŭ������ ������ �ȵ̾��;");
                break;

        }


    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    //private void Update()
    //{
        
    //}


    //private void UnitNodeUISpriteInit(int unitLv, string pathLv1, string pathLv2)
    //{
    //    //���ֳ��UI�� �̹��� ��������Ʈ�� �ٲ��ش�.
    //    if (unitLv < 5)
    //        unitImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/{pathLv1}");
    //    else
    //        unitImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/{pathLv2}");
    //}


    public UnitClass SetUnitClass(UnitClass uniclass)
    {
        e_UnitClass = uniclass;
        return e_UnitClass;
    }


    public void CheckNodeEquip(Define.UnitNodeState type)
    {
        unitNodeState = type;  

        if (unitNodeState == Define.UnitNodeState.Equip)
            Destroy(this.gameObject);
    }


}
