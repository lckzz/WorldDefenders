using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitNodeUI : MonoBehaviour
{
    //���ֹ�ġ�� �پ��ִ� ���ֳ����� ��ũ��Ʈ
    [SerializeField] private Image unitImg;
    [SerializeField] private UnitClass e_UnitClass = UnitClass.Count;

    public UnitClass E_UnitClass { get { return e_UnitClass; }}


    void Init()
    {
        //ó���� �����Ǹ� �ش簻�����ֱ�
        switch (e_UnitClass)
        {
            case UnitClass.Warrior:
                Debug.Log($"������ ����! {e_UnitClass}");
                UnitNodeUISpriteInit(GlobalData.g_UnitWarriorLv, "KnifeUnitLv1Img", "KnifeUnitLv2Img");
                break;

            case UnitClass.Archer:
                Debug.Log($"��ó ����! {e_UnitClass}");
                UnitNodeUISpriteInit(GlobalData.g_UnitWarriorLv, "BowUnitLv1Img", "BowUnitLv2Img");

                break;

            case UnitClass.Spear:
                Debug.Log($"â�� ����! {e_UnitClass}");
                UnitNodeUISpriteInit(GlobalData.g_UnitWarriorLv, "SpearUnitLv1Img", "SpearUnitLv2Img");

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

    private void Update()
    {
        
    }


    private void UnitNodeUISpriteInit(int unitLv, string pathLv1, string pathLv2)
    {
        //���ֳ��UI�� �̹��� ��������Ʈ�� �ٲ��ش�.
        if (unitLv < 5)
            unitImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/{pathLv1}");
        else
            unitImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/{pathLv2}");
    }


    public UnitClass GetUnitClass(UnitClass uniclass)
    {
        e_UnitClass = uniclass;
        return e_UnitClass;
    }


}
