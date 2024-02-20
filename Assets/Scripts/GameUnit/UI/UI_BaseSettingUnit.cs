using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UI_BaseSettingUnit : MonoBehaviour
{
    [SerializeField] protected GameObject unitPosObj;
    [SerializeField] protected GameObject clickImgObj;
    [SerializeField] protected UnitClass e_UnitClass = UnitClass.Count;
    protected RectTransform rt;
    protected UnitStat unitStat;


    protected virtual void Init()
    {
        if (unitStat == null)
            unitStat = new UnitStat();
    }

    protected virtual void UnitUISpriteInit()
    {
        if (e_UnitClass == UnitClass.Count)
            return;


        if (unitPosObj.transform.childCount > 0)     //���� �ʱ�ȭ�ϴµ� ���������ȿ� ����������Ʈ�� �ִٸ�
            Destroy(unitPosObj.transform.GetChild(0).gameObject);           //�� ������Ʈ�� �������ش�.

        if (Managers.Game.UnitStatDict.TryGetValue(e_UnitClass, out Dictionary<int, UnitStat> unitStatDict))  //�ش� ����Ŭ������ �´� ���ֵ����͸� �޾ƿ´�.
            unitStat = unitStatDict[Managers.Game.GetUnitLevel(e_UnitClass)];




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
