using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitInfoToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI toolTipTxt;
    [SerializeField] private GameObject toolTipObj;



    public void ToolTipSet(string desc)
    {
        //�ش� ��ų Ŭ���� ������ ����
        toolTipObj.SetActive(true);
        toolTipTxt.text = "";
        toolTipTxt.text = desc;


    }

    public void ToolTipOff()
    {
        toolTipObj.SetActive(false);
    }

    public bool CheckToolTipisOn()
    {
        if (toolTipObj.activeSelf == false)
            return false;
        else
            return true;
    }


}
