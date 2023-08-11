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

    protected void UnitUISpriteInit(int unitLv, string pathLv1, string pathLv2)
    {
        if (unitImg != null)
            unitImg.gameObject.SetActive(true);
        //���ֳ��UI�� �̹��� ��������Ʈ�� �ٲ��ش�.
        if (unitLv < 5)
        {
            unitImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/{pathLv1}");
            Debug.Log(GlobalData.g_UnitArcherLv);

        }
        else
        {
            unitImg.sprite = Managers.Resource.Load<Sprite>($"Sprite/{pathLv2}");

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
