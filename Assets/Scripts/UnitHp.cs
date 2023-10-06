using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHp : MonoBehaviour
{
    [SerializeField] private Transform parentTr;
    [SerializeField] private GameObject hudTextPrefab;


    public void Init(Transform parentTr, GameObject prefab)
    {
        this.parentTr = parentTr;
        hudTextPrefab = prefab;
    }

    public void SpawnHUDText(string text, int type)
    {
        Color color = type == 0 ? Color.red : Color.green;



        GameObject clone = Managers.Resource.Instantiate(hudTextPrefab);

        clone.transform.SetParent(parentTr);


        clone.GetComponent<UIHUDText>().Play(text, color, transform.position);
    }


}
