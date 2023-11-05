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
    //�ΰ��ӳ��� ������ ����

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




    //�ڽ�Ʈ ����
    #region �ڽ�Ʈ �� ��ȭ ����
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

    public void NormalMonsterSpawn(Define.MonsterSpawnType spawnType,Action<string> action,string warningTxt)
    {
        monsterSpawn.MonsterSpawnTimer(spawnType, action,warningTxt);
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




    //���� ���� �̺�Ʈ

    #endregion



    #region ������������


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




    #region ���� & �ε�

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
