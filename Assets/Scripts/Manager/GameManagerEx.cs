using System;
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

    public void NormalMonsterSpawn(Define.MonsterSpawnType spawnType,Action action = null)
    {
        monsterSpawn.MonsterSpawnTimer(spawnType, action);
    }

    public void EliteMonsterSpawn()
    {
        monsterSpawn.EliteMonsterSpawn();
    }
    //���� ��ȯ
    #endregion

    #region �̺�Ʈ Ÿ�� ����

    //���� ���� �̺�Ʈ

    public void EventInit()
    {
        monsterEvent.EventInit();               //���� �ٽ� �ҷ����� ���� �ʱ�ȭ �������
    }

    public Define.MonsterSpawnType GetMonSpawnType()
    {
        return monsterEvent.MonSpawnType;
    }
    
    public void SetMonSpawnType(Define.MonsterSpawnType type)
    {
        monsterEvent.MonSpawnType = type;
    }

    public void MonsterWaveEvent(Action action =null)
    {
        monsterEvent.MonsterWaveEvent(action);
    }

    public void MonsterWave()
    {
        monsterEvent.MonsterWave();
    }
    public void EliteMonsterEvent(Action action)
    {
        monsterEvent.EliteMonsterEvent(action);
    }

    public bool MonsterNormalCheck()
    {
        return monsterEvent.MonsterNormalCheck();
    }

    public bool MonsterWaveCheck()
    {
        return monsterEvent.MonsterWaveCheck();
    }

    public bool EliteMonsterCheck()
    {
        return monsterEvent.EliteMonsterCheck();
    }
    public bool FinalMonsterCheck()
    {
        return monsterEvent.FinalMonsterCheck();
    }

    public void FinalMonsterWave(Action action, int eliteMonsterCount)
    {
        monsterEvent.FinalMonsterWave(action,eliteMonsterCount);
    }




    //���� ���� �̺�Ʈ

    #endregion

}
