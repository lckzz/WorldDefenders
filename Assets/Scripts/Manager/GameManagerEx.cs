using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



[Serializable]
public class GameData
{
    public int gold;
    public int playerLevel;
    public int unitWarrorLv;
    public int unitArcherLv;
    public int unitSpearLv;
    public int unitPriestLv;
    public int unitMagicianLv;
    public int unitCarlvryLv;

    public bool lobbyToGameScene;
    public Define.PlayerSkill curPlayerEquipSkill;

    public Define.SubStage curStage;
    public bool firstInit;
}

[Serializable]
public class GameArrayData
{
    public List<UnitClass> slotUnitClass = new List<UnitClass>();
    public List<Define.MonsterType> monsterTypeList = new List<Define.MonsterType>();
}

[Serializable]
public class ArrayData
{
    public UnitClass[] unitClasses;
    public Define.MonsterType[] monsterTypes;
}


public class GameManagerEx 
{
    //인게임내의 데이터 관리

    GameArrayData gameSaveArrayData = new GameArrayData();
    ArrayData arrayData = new ArrayData();


    private MoneyCost moneyCost = new MoneyCost();
    private UnitSpawn unitSpawn = new UnitSpawn();
    private MonsterSpawn monsterSpawn = new MonsterSpawn();
    private MonsterEvent monsterEvent = new MonsterEvent();
    private StageState stageState = new StageState();

    public List<UnitClass> SlotUnitClass { 
        get { return gameSaveArrayData.slotUnitClass; } 
        set { gameSaveArrayData.slotUnitClass = value; } 
    }
    public List<Define.MonsterType> MonsterTypeList {
        get { return gameSaveArrayData.monsterTypeList; }
        set { gameSaveArrayData.monsterTypeList = value; }
    }




    //코스트 관리
    #region 코스트 및 재화 관리
    public float Cost { get { return moneyCost.CurCost; } }

    public void MoneyCostInit()
    {
        moneyCost.MoneyCostInit();
    }

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

    public void InGameTimer()
    {
        moneyCost.InGameTimer();
    }

    public float GetInGameTimer()
    {
        return moneyCost.GetInGameTime();
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

    public void NormalMonsterSpawn(Define.MonsterSpawnType spawnType,Action<string> action,string warningTxt)
    {
        monsterSpawn.MonsterSpawnTimer(spawnType, action,warningTxt);
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

    public void MonsterWaveEvent(Action<string> action ,string warningTxt)
    {
        monsterEvent.MonsterWaveEvent(action, warningTxt);
    }

    public void MonsterWave()
    {
        monsterEvent.MonsterWave();
    }
    public void EliteMonsterEvent(Action<string> action, string warningTxt)
    {
        monsterEvent.EliteMonsterEvent(action,warningTxt);
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

    public void FinalMonsterWave(Action<string> action, string warningTxt, int eliteMonsterCount)
    {
        monsterEvent.FinalMonsterWave(action,warningTxt, eliteMonsterCount);
    }




    //몬스터 스폰 이벤트

    #endregion



    #region 스테이지상태


    public Define.SubStage CurStageType { get { return stageState.CurStageType; } set { stageState.CurStageType = value; } }


    public Define.StageStageType GetStageStateType()
    {
        return stageState.StageStateType;
    }

    public Define.StageStageType SetStageStateType(Define.StageStageType type)
    {
        return stageState.StageStateType = type;
    }

    public void ResultState(Define.StageStageType type)
    {
        stageState.ResultState(type);
    }

    public bool GameEndResult()
    {
        return stageState.GameEndResult();
    }


    #endregion




    #region 저장 & 로드

    public string path = Application.persistentDataPath + "/SaveData.json";

    public void FileSave()
    {
        ArrayData arrayData = new ArrayData
        {
            unitClasses = gameSaveArrayData.slotUnitClass.ToArray(),
            monsterTypes = gameSaveArrayData.monsterTypeList.ToArray()
        };

        string jsonStr = JsonUtility.ToJson(arrayData);
        File.WriteAllText(path, jsonStr);
        Debug.Log($"Save Game Completed : {path}");

    }

    #endregion
}
