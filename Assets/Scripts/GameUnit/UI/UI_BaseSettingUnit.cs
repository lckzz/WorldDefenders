using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UI_BaseSettingUnit : MonoBehaviour
{
    [SerializeField] protected Image unitImg;
    [SerializeField] protected GameObject clickImgObj;
    [SerializeField] protected UnitClass e_UnitClass = UnitClass.Count;
    protected RectTransform rt;


    protected abstract void Init();

    protected void UnitUISpriteInit(UnitClass uniClass, int unitLv, string pathLv1, string pathLv2)
    {
        if (unitImg != null)
            unitImg.gameObject.SetActive(true);
        

        //유닛노드UI의 이미지 스프라이트를 바꿔준다.

         switch (uniClass)
        {
            case UnitClass.Warrior:
                if (unitLv < 5)
                {
                    unitImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/UnitSprite/Warrior/Lv1/{pathLv1}");
                }
                else
                {
                    unitImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/UnitSprite/Warrior/Lv2/{pathLv2}");
                }
                break;
            case UnitClass.Archer:
                if (unitLv < 5)
                {
                    unitImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/UnitSprite/Archer/Lv1/{pathLv1}");


                }
                else
                {
                    unitImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/UnitSprite/Archer/Lv2/{pathLv2}");


                }
                break;
            case UnitClass.Spear:
                if (unitLv < 5)
                {
                    unitImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/UnitSprite/Spear/Lv1/{pathLv1}");


                }
                else
                {
                    unitImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/UnitSprite/Spear/Lv2/{pathLv2}");


                }
                break;
        }

    }

    protected void UnitUISpriteInit(UnitClass uniClass, string path)
    {
        if (unitImg != null)
            unitImg.gameObject.SetActive(true);


        //유닛노드UI의 이미지 스프라이트를 바꿔준다.

        switch (uniClass)
        {
            case UnitClass.Magician:
                unitImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/UnitSprite/Magician/{path}");
                break;
                
        }

    }


    public void ClickImageOnOff(bool check)
    {
        if (clickImgObj != null)
        {
            if (!clickImgObj.activeSelf && check)
                clickImgObj.SetActive(check);
            else
                clickImgObj.SetActive(check);

        }
    }
}
