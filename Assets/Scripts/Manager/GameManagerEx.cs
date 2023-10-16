using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx 
{
    //인게임내의 데이터 관리

    private MoneyCost moneyCost = new MoneyCost();
    private UnitSpawn unitSpawn = new UnitSpawn();
    private MonsterSpawn monsterSpawn = new MonsterSpawn();
    private MonsterEvent monsterEvent = new MonsterEvent();

    //코스트 관리
    #region 코스트 관리
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

    //코스트 관리
    #endregion


    #region 유닛 소환

    //유닛 소환
    public void UnitSummonEnqueue(GameObject unitObj,float unitCost,UnitNode node)
    {
        unitSpawn.UnitSummonEnqueue(unitObj, unitCost, node);
    }

    public void unitSummonDequeue(GameObject go, Transform[] tr)
    {
        unitSpawn.UnitSummonDequeue(go, tr);
    }

    //유닛 소환

    #endregion

    #region 몬스터 소환
    //몬스터 소환
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
    //몬스터 소환
    #endregion

    #region 이벤트 타입 변경

    //몬스터 스폰 이벤트

    public void EventInit()
    {
        monsterEvent.EventInit();               //씬을 다시 불러오면 변수 초기화 해줘야함
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




    //몬스터 스폰 이벤트

    #endregion

}
