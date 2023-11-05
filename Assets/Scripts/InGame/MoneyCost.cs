using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCost
{


    float coolTime = 4.5f;

    float curCost = .0f;
    float maxCost = 500.0f;
    float costCoolTime = 4.5f;
    float maxCostCoolTime = 4.5f;
    private float cost = 30.0f;

    //�ð�üũ
    float timesec = 0.0f;
    //�ð�üũ

    public int GameMoney { get; set; }
    public float CurCost { get { return curCost; } set { curCost = value; } }
    public float MaxCost { get { return maxCost; } set { maxCost = value; } }

    public void MoneyCostInit()
    {
        GameMoney = 0;
        curCost = .0f;
        maxCost = 500.0f;
        costCoolTime = 4.5f;
        maxCostCoolTime = 4.5f;
        cost = 30.0f;
        timesec = .0f;
    }



    public void CostCoolTimer()
    {

        if (costCoolTime > .0f)
        {
            costCoolTime -= Time.deltaTime;
            if (costCoolTime <= .0f)
            {
                costCoolTime = maxCostCoolTime;
                curCost += cost;  //���� �ڽ�Ʈ�� �ڽ�Ʈ�� �߰����ְ�
                //Managers.UI.GetSceneUI<UI_GamePlay>().UpdateCost(curCost); //UI���� ���� �ڽ�Ʈ�� �Ѱ��༭ �������ش�.

            }
        }
    }


    public bool CostCheck(float unitCost)
    {
        if (curCost - unitCost < .0f) //0���� �۴ٸ�
            return false;

        return true;

    }

    public float CostUse(float unitCost)
    {
        curCost -= unitCost;

        return curCost;
    }


    public void InGameTimer()
    {
        //�ΰ��� Ÿ�̸�
        timesec += Time.deltaTime;
    }

    public float GetInGameTime()
    {
        return timesec;
    }
}
