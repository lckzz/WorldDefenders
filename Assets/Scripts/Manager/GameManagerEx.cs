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

    public void NormalMonsterSpawn(Define.MonsterSpawnType spawnType)
    {
        monsterSpawn.MonsterSpawnTimer(spawnType);
    }

    public void EliteMonsterSpawn()
    {
        monsterSpawn.EliteMonsterSpawn();
    }
    //몬스터 소환
    #endregion

    #region 이벤트 타입 변경

    //몬스터 스폰 이벤트
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

    //몬스터 스폰 이벤트

    #endregion

}
