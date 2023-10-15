using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx 
{
    //�ΰ��ӳ��� ������ ����

    private MoneyCost moneyCost = new MoneyCost();
    private UnitSpawn unitSpawn = new UnitSpawn();
    private MonsterSpawn monsterSpawn = new MonsterSpawn();
    private MonsterEvent monsterEvent = new MonsterEvent();

    //�ڽ�Ʈ ����
    #region �ڽ�Ʈ ����
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
    #endregion


    #region ���� ��ȯ

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

    #endregion

    #region ���� ��ȯ
    //���� ��ȯ
    public void MonsterSpawnInit(Transform parentTr)
    {
        monsterSpawn.MonsterSpawnInit(parentTr);
    }

    public void NormalMonsterSpawn(Define.MonsterSpawnType spawnType)
    {
        monsterSpawn.MonsterSpawnTimer(spawnType);
    }

    public void EliteMonsterSpawn()
    {
        monsterSpawn.EliteMonsterSpawn();
    }
    //���� ��ȯ
    #endregion

    #region �̺�Ʈ Ÿ�� ����

    //���� ���� �̺�Ʈ
    public Define.MonsterSpawnType MonsterWaveEvent(Define.MonsterSpawnType spawnType, int gateHp)
    {
        return monsterEvent.MonsterWaveEvent(spawnType, gateHp);
    }

    public Define.MonsterSpawnType MonsterWave(Define.MonsterSpawnType spawnType)
    {
        return monsterEvent.MonsterWave(spawnType);
    }


    public bool MonsterWaveCheck()
    {
        return monsterEvent.MonsterWaveCheck();
    }

    public bool EliteMonsterCheck()
    {
        return monsterEvent.EliteMonsterCheck();
    }

    public void EliteMonsterEvent()
    {
        monsterEvent.EliteMonsterEvent();
    }

    public Define.MonsterSpawnType EliteMonsterEventSpawn(Define.MonsterSpawnType spawnType)
    {
        return monsterEvent.EliteMonsterEventSpawn(spawnType);

    }

    //���� ���� �̺�Ʈ

    #endregion

}
