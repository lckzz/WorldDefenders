using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitNodeUI : MonoBehaviour
{
    //유닛배치에 붙어있는 유닛노드관한 스크립트
    [SerializeField] private Image unitImg;
    [SerializeField] private UnitClass e_UnitClass = UnitClass.Count;

    public UnitClass E_UnitClass { get { return e_UnitClass; }}


    void Init()
    {
        //처음에 생성되면 해당갱신해주기
        switch (e_UnitClass)
        {
            case UnitClass.Warrior:
                Debug.Log($"워리어 갱신! {e_UnitClass}");
                UnitNodeUISpriteInit(GlobalData.g_UnitWarriorLv, "KnifeUnitLv1Img", "KnifeUnitLv2Img");
                break;

            case UnitClass.Archer:
                Debug.Log($"아처 갱신! {e_UnitClass}");
                UnitNodeUISpriteInit(GlobalData.g_UnitWarriorLv, "BowUnitLv1Img", "BowUnitLv2Img");

                break;

            case UnitClass.Spear:
                Debug.Log($"창병 갱신! {e_UnitClass}");
                UnitNodeUISpriteInit(GlobalData.g_UnitWarriorLv, "SpearUnitLv1Img", "SpearUnitLv2Img");

                break;

            default:
                Debug.Log("유닛노드에 유닛클래스가 설정이 안됫어요;");
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
        //유닛노드UI의 이미지 스프라이트를 바꿔준다.
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
