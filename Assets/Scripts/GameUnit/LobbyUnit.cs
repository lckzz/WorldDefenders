using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUnit : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprend;
    [SerializeField] private UnitClass e_UnitClass = UnitClass.Count;


    public UnitClass E_UniClass { get { return e_UnitClass; } set { e_UnitClass = value; } }
    // Start is called before the first frame update


    public void RefreshUnitSet()
    {
        switch (e_UnitClass)
        {
            case UnitClass.Warrior:

                UnitSpriteRender(e_UnitClass, GlobalData.g_UnitWarriorLv, "KnifeUnitLv1Img", "KnifeUnitLv2Img");
                break;

            case UnitClass.Archer:
                Debug.Log($"아처 갱신! {e_UnitClass}");
                UnitSpriteRender(e_UnitClass, GlobalData.g_UnitArcherLv, "BowUnitLv1Img", "BowUnitLv2Img");

                break;

            case UnitClass.Spear:
                Debug.Log($"창병 갱신! {e_UnitClass}");
                UnitSpriteRender(e_UnitClass, GlobalData.g_UnitSpearLv, "SpearUnitLv1Img", "SpearUnitLv2Img");

                break;


            case UnitClass.Magician:
                Debug.Log($"마법사 갱신! {e_UnitClass}");
                UnitSpriteRender(e_UnitClass, "Magician_Idle");

                break;
            default:
                Debug.Log("유닛노드에 유닛클래스가 설정이 안됫어요;");
                gameObject.SetActive(false);
                break;

        }
    }


    void UnitSpriteRender(UnitClass uniClass, int unitLv, string pathLv1, string pathLv2)
    {
        gameObject.SetActive(true);

        switch (uniClass)
        {
            case UnitClass.Warrior:
                if (unitLv < 5)
                {
                    sprend.sprite = Managers.Resource.Load<Sprite>($"Sprite/UnitSprite/Warrior/Lv1/{pathLv1}");
                }
                else
                {
                    sprend.sprite = Managers.Resource.Load<Sprite>($"Sprite/UnitSprite/Warrior/Lv2/{pathLv2}");
                }
                break;
            case UnitClass.Archer:
                if (unitLv < 5)
                {
                    sprend.sprite = Managers.Resource.Load<Sprite>($"Sprite/UnitSprite/Archer/Lv1/{pathLv1}");


                }
                else
                {
                    sprend.sprite = Managers.Resource.Load<Sprite>($"Sprite/UnitSprite/Archer/Lv2/{pathLv2}");


                }
                break;
            case UnitClass.Spear:
                if (unitLv < 5)
                {
                    sprend.sprite = Managers.Resource.Load<Sprite>($"Sprite/UnitSprite/Spear/Lv1/{pathLv1}");


                }
                else
                {
                    sprend.sprite = Managers.Resource.Load<Sprite>($"Sprite/UnitSprite/Spear/Lv2/{pathLv2}");


                }
                break;
        }
    }
    void UnitSpriteRender(UnitClass uniClass, string path)
    {
        gameObject.SetActive(true);

        switch (uniClass)
        {
            case UnitClass.Magician:
                sprend.sprite = Managers.Resource.Load<Sprite>($"Sprite/UnitSprite/Magician/{path}");
                break;


        }
    }
}
