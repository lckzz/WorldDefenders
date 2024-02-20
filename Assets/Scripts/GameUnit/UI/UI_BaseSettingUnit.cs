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


        if (unitPosObj.transform.childCount > 0)     //만약 초기화하는데 유닛포스안에 하위오브젝트가 있다면
            Destroy(unitPosObj.transform.GetChild(0).gameObject);           //그 오브젝트는 삭제해준다.

        if (Managers.Game.UnitStatDict.TryGetValue(e_UnitClass, out Dictionary<int, UnitStat> unitStatDict))  //해당 유닛클래스에 맞는 유닛데이터를 받아온다.
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
