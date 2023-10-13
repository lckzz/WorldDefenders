using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx 
{
    //�ΰ��ӳ��� ������ ����

    private MoneyCost moneyCost = new MoneyCost();
    private UnitSpawn unitSpawn = new UnitSpawn();
    private MonsterSpawn monsterSpawn = new MonsterSpawn();

    //�ڽ�Ʈ ����

    public float Cost { get { return moneyCost.CurCost; } }

    public void CostIncreaseTime()
    {
        moneyCost.CostCoolTimer();
    }

    public bool CostCheck(float unitCost)
    {
        return moneyCost.CostCheck(unitCost);
    }
    
    public float CostUse(float unitCost)
    {
        return moneyCost.CostUse(unitCost);
    }

    //�ڽ�Ʈ ����


    //���� ��ȯ
    public void UnitSummonEnqueue(GameObject unitObj,float unitCost,UnitNode node)
    {
        unitSpawn.UnitSummonEnqueue(unitObj, unitCost, node);
    }

    public void unitSummonDequeue(GameObject go, Transform[] tr)
    {
        unitSpawn.UnitSummonDequeue(go, tr);
    }

    //���� ��ȯ

    //���� ��ȯ
    public void MonsterSpawnInit(Transform parentTr)
    {
        monsterSpawn.MonsterSpawnInit(parentTr);
    }

    public void NormalMonsterSpawn()
    {
        monsterSpawn.NormalMonsterSpawn();
    }

    public void EliteMonsterSpawn()
    {

    }
    //���� ��ȯ


}
