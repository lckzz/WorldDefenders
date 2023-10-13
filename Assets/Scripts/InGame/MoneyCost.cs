using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCost : MonoBehaviour
{


    [SerializeField] Transform[] spawnPos;
    [SerializeField] float coolTime = 4.5f;

    float curCost = .0f;
    float maxCost = 500.0f;
    float costCoolTime = 4.5f;
    float maxCostCoolTime = 4.5f;
    [SerializeField] private float cost = 30.0f;

    public float CurCost { get { return curCost; } set { curCost = value; } }
    public float MaxCost { get { return maxCost; } set { maxCost = value; } }

    // Start is called before the first frame update
    void Start()
    {
        costCoolTime = 4.5f;
    }

    public void CostCoolTimer()
    {

        if (costCoolTime > .0f)
        {
            Debug.Log(curCost);
            costCoolTime -= Time.deltaTime;
            if (costCoolTime <= .0f)
            {
                costCoolTime = maxCostCoolTime;
                curCost += cost;  //���� �ڽ�Ʈ�� �ڽ�Ʈ�� �߰����ְ�
                Managers.UI.GetSceneUI<UI_GamePlay>().UpdateCost(curCost); //UI���� ���� �ڽ�Ʈ�� �Ѱ��༭ �������ش�.

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
}
