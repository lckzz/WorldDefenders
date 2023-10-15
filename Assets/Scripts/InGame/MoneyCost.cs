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

    public float CurCost { get { return curCost; } set { curCost = value; } }
    public float MaxCost { get { return maxCost; } set { maxCost = value; } }

    public void CostCoolTimer()
    {

        if (costCoolTime > .0f)
        {
            costCoolTime -= Time.deltaTime;
            if (costCoolTime <= .0f)
            {
                costCoolTime = maxCostCoolTime;
                curCost += cost;  //현재 코스트에 코스트를 추가해주고
                //Managers.UI.GetSceneUI<UI_GamePlay>().UpdateCost(curCost); //UI에서 현재 코스트를 넘겨줘서 갱신해준다.

            }
        }
    }


    public bool CostCheck(float unitCost)
    {
        if (curCost - unitCost < .0f) //0보다 작다면
            return false;

        return true;

    }

    public float CostUse(float unitCost)
    {
        curCost -= unitCost;

        return curCost;
    }
}
