using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHp : MonoBehaviour
{
    [SerializeField] private Transform parentTr;
    [SerializeField] private GameObject hudTextPrefab;

    private int itemIdx = -1;


    Color orange = new Color32(255, 110, 110, 255);
    public void Init(Transform parentTr, GameObject prefab)
    {
        this.parentTr = parentTr;
        hudTextPrefab = prefab;
    }

    public void SpawnHUDText(string text, int type)
    {
        Color color = Color.white;
        switch(type)
        {
            case (int)Define.UnitDamageType.Enemy:
                color = orange;
                break;
            case (int)Define.UnitDamageType.Team:
                color = Color.green;
                break;
            case (int)Define.UnitDamageType.Critical:
                color = Color.red;
                break;
            case (int)Define.UnitDamageType.Item:
                color = Color.blue;
                break;
        }



        GameObject clone = Managers.Resource.Instantiate(hudTextPrefab);

        clone.transform.SetParent(parentTr);

        if(type == (int)Define.UnitDamageType.Critical)
            clone.GetComponent<UIHUDText>().Play(text, color, transform.position,true);
        else if(type == (int)Define.UnitDamageType.Item)
        {
            clone.GetComponent<UIHUDText>().ItemIdx(itemIdx);
            clone.GetComponent<UIHUDText>().Play(text, color, transform.position, false, true);

        }
        else
            clone.GetComponent<UIHUDText>().Play(text, color, transform.position);


    }

    public void ItemHudInit(int idx)
    {
        itemIdx = idx;
    }

}
