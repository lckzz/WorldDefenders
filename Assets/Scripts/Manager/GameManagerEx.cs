using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

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

    public int towerHealSkillLv;
    public int fireArrowSkillLv;
    public int weaknessSkillLv;

    public Define.PlayerSkill curPlayerEquipSkill;
    public bool firstInit;

    public bool tutorialEnd;

    public bool westStageClear;
    public bool eastStageClear;
    public bool southStageClear;
    public bool stage1AllClear;

    public float bgmValue;
    public float effValue;
    public bool bgmisOn;
    public bool effisOn;

    public float westStageBestTime;
    public float eastStageBestTime;
    public float southStageBestTime;

}

[Serializable]
public class GameArrayData
{
    public List<UnitClass> slotUnitClass = new List<UnitClass>();
}

[Serializable]
public class GameTotalData
{
    public UnitClass[] unitClasses;
    public GameData gameData;
}


public class GameManagerEx 
{
    //인게임내의 데이터 관리

    GameData gameData = new GameData();
    GameArrayData gameSaveArrayData = new GameArrayData();
    GameTotalData gameTotalData = new GameTotalData();


    private MoneyCost moneyCost = new MoneyCost();
    private UnitSpawn unitSpawn = new UnitSpawn();
    private MonsterSpawn monsterSpawn = new MonsterSpawn();
    private MonsterEvent monsterEvent = new MonsterEvent();
    private StageState stageState = new StageState();

    private List<Define.MonsterType> monsterTypeList = new List<Define.MonsterType>();
    private bool lobbyToGameScene = false;

    private Dictionary<int, int> upgradeUnitLvDict;

    public Dictionary<int,int> UpgradeUnitLvDict { get { return upgradeUnitLvDict; } }

    public List<UnitClass> SlotUnitClass { 
        get { return gameSaveArrayData.slotUnitClass; } 
        set { gameSaveArrayData.slotUnitClass = value; } 
    }
    public List<Define.MonsterType> MonsterTypeList {
        get { return monsterTypeList; }
        set { monsterTypeList = value; }
    }

    public bool LobbyToGameScene
    {
        get { return lobbyToGameScene; }
        set { lobbyToGameScene = value; }
    }

    #region 유닛 레벨
    public void UnitLvDictInit()
    {
        upgradeUnitLvDict = new Dictionary<int, int>
        {
            { (int)UnitClass.Warrior, Managers.Game.UnitWarriorLv },
            { (int)UnitClass.Archer, Managers.Game.UnitArcherLv },
            { (int)UnitClass.Spear, Managers.Game.UnitSpearLv },
            { (int)UnitClass.Priest, Managers.Game.UnitPriestLv },
            { (int)UnitClass.Magician, Managers.Game.UnitMagicianLv },
            { (int)UnitClass.Cavalry, Managers.Game.UnitCarlvlry },
        };

    }

    public void UnitLvDictRefresh()
    {
        //딕셔너리의 값을 갱신해준다.
        foreach(var unitClass in Enum.GetValues(typeof(UnitClass)).Cast<UnitClass>())
        {
            upgradeUnitLvDict[(int)unitClass] = GetUnitLevel(unitClass);
        }
    }

    private int GetUnitLevel(UnitClass unitClass)
    {
        switch (unitClass)
        { 
            case UnitClass.Warrior:
                return Managers.Game.UnitWarriorLv;
            case UnitClass.Archer:
                return Managers.Game.UnitArcherLv;
            case UnitClass.Spear:
                return Managers.Game.UnitSpearLv;
            case UnitClass.Priest:
                return Managers.Game.UnitPriestLv;
            case UnitClass.Magician:
                return Managers.Game.UnitMagicianLv;
            case UnitClass.Cavalry:
                return Managers.Game.UnitCarlvlry;
            default:
                return 0;
        }

    }

    #endregion
    #region 게임 데이터
    public int Gold
    {
        get { return gameData.gold; }
        set { gameData.gold = value; }
    }

    public int PlayerLevel
    {
        get { return gameData.playerLevel; }
        set { gameData.playerLevel = value; }
    }

    public int UnitWarriorLv
    {
        get { return gameData.unitWarrorLv; }
        set { gameData.unitWarrorLv = value; }
    }
    public int UnitArcherLv
    {
        get { return gameData.unitArcherLv; }
        set { gameData.unitArcherLv = value; }
    }
    public int UnitSpearLv
    {
        get { return gameData.unitSpearLv; }
        set { gameData.unitSpearLv = value; }
    }
    public int UnitPriestLv
    {
        get { return gameData.unitPriestLv; }
        set { gameData.unitPriestLv = value; }
    }

    public int UnitMagicianLv
    {
        get { return gameData.unitMagicianLv; }
        set { gameData.unitMagicianLv = value; }
    }

    public int UnitCarlvlry
    {
        get { return gameData.unitCarlvryLv; }
        set { gameData.unitCarlvryLv = value; }
    }

    public int TowerHealSkillLv
    {
        get { return gameData.towerHealSkillLv; }
        set { gameData.towerHealSkillLv = value; }
    }

    public int FireArrowSkillLv
    {
        get { return gameData.fireArrowSkillLv; }
        set { gameData.fireArrowSkillLv = value; }
    }

    public int WeaknessSkillLv
    {
        get { return gameData.weaknessSkillLv; }
        set { gameData.weaknessSkillLv = value; }
    }

    public Define.PlayerSkill CurPlayerEquipSkill
    {
        get { return gameData.curPlayerEquipSkill; }
        set { gameData.curPlayerEquipSkill = value; }
    }

    public bool FirstInit
    {
        get { return gameData.firstInit; }
        set { gameData.firstInit = value; }
    }

    public bool TutorialEnd
    {
        get { return gameData.tutorialEnd; }
        set { gameData.tutorialEnd = value; }
    }

    public bool WestStageClear
    {
        get { return gameData.westStageClear; }
        set { gameData.westStageClear = value; }
    }

    public bool EastStageClear
    {
        get { return gameData.eastStageClear; }
        set { gameData.eastStageClear = value; }
    }

    public bool SouthStageClear
    {
        get { return gameData.southStageClear; }
        set { gameData.southStageClear = value; }
    }

    public bool Stage1AllClear
    {
        get { return gameData.stage1AllClear; }
        set { gameData.stage1AllClear = value; }
    }

    public float BgmValue
    {
        get { return gameData.bgmValue; }
        set { gameData.bgmValue = value; }
    }

    public float EffValue
    {
        get { return gameData.effValue; }
        set { gameData.effValue = value; }
    }

    public bool BgmisOn
    {
        get { return gameData.bgmisOn; }
        set { gameData.bgmisOn = value; }
    }

    public bool EffisOn
    {
        get { return gameData.effisOn; }
        set { gameData.effisOn = value; }
    }

    public float WestStageBestTime
    {
        get { return gameData.westStageBestTime; }
        set { gameData.westStageBestTime = value; }
    }

    public float EastStageBestTime
    {
        get { return gameData.eastStageBestTime; }
        set { gameData.eastStageBestTime = value; }
    }

    public float SouthStageBestTime
    {
        get { return gameData.southStageBestTime; }
        set { gameData.southStageBestTime = value; }
    }



    #endregion


    public void GameDataInit()
    {

        Gold = 0;
        PlayerLevel = 1;
        UnitWarriorLv = 1;
        UnitArcherLv = 1;
        UnitSpearLv = 1;
        UnitPriestLv = 1;
        UnitMagicianLv = 1;
        UnitCarlvlry = 1;
        TowerHealSkillLv = 1;
        FireArrowSkillLv = 0;
        WeaknessSkillLv = 0;
        CurPlayerEquipSkill = Define.PlayerSkill.Count;
        FirstInit = false;
        TutorialEnd = false;
        WestStageClear = false;
        EastStageClear = false;
        SouthStageClear = false;
        Stage1AllClear = false;
        BgmValue = 0.5f;
        EffValue = 0.5f;
        BgmisOn = false;
        EffisOn = false;


    }



    public void SetMonsterList(List<Define.MonsterType> monTypeList)
    {
        //스테이지선택창에서 시작을 하면 넘겨받아서 인게임에 적용
        Managers.Game.MonsterTypeList.Clear();
        for (int ii = 0; ii < monTypeList.Count; ii++)
            Managers.Game.MonsterTypeList.Add(monTypeList[ii]);
    }

    public void InGameInit(Transform parentTr, Define.MonsterSpawnType monType, Define.StageStageType stageType)
    {
        EventInit();
        UnitQueueClear();
        MonsterSpawnInit(parentTr);
        SetMonSpawnType(monType);
        SetStageStateType(stageType);

    }


    //코스트 관리
    #region 코스트 및 재화 관리
    public float Cost { get { return moneyCost.CurCost; } set { moneyCost.CurCost = value; } }


    public int GameGold { get { return moneyCost.GameMoney; } set { moneyCost.GameMoney = value; } }

    public int WestStageGold { get { return moneyCost.WestStageGold; } }
    public int EastStageGold { get { return moneyCost.EastStageGold; }  }
    public int SouthStageGold { get { return moneyCost.SouthStageGold; } }


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

    public void UnitSummonDequeue(GameObject go, Transform[] tr)
    {
        unitSpawn.UnitSummonDequeue(go, tr);
    }

    public void UnitQueueClear()
    {
        unitSpawn.UnitQueueClear();
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

    public bool StageAllClear()
    {
        if (Stage1AllClear == true)
            return false;

        if (WestStageClear == true && EastStageClear == true && SouthStageClear == true)
            return true;


        return false;
    }


    #endregion




    #region 저장 & 로드

    //public string path => Path.Combine(Application.persistentDataPath,"/SaveData.json");



    public void FileSave()
    {
        GameTotalData arrayData = new GameTotalData
        {
            //리스트를 json으로 변환이 안되기때문에 배열로 감싸줘서 json으로 저장
            unitClasses = gameSaveArrayData.slotUnitClass.ToArray(),
            gameData = Managers.Game.gameData
        };

        string path = Application.persistentDataPath + "/SaveData.json";
        string jsonStr = JsonUtility.ToJson(arrayData);

        File.WriteAllText(path, jsonStr);
        Debug.Log($"게임 세이브됨: {path}");

    }

    public bool FileLoad()
    {

        string path = Application.persistentDataPath + "/SaveData.json";

        if (File.Exists(path) == false)
            return false;


        string fileStr = File.ReadAllText(path);
        GameTotalData data = JsonUtility.FromJson<GameTotalData>(fileStr);

        gameSaveArrayData.slotUnitClass.Clear();

        for(int ii = 0;  ii < data.unitClasses.Length; ii++)
        {
            gameSaveArrayData.slotUnitClass.Add(data.unitClasses[ii]);
        }

        if (data != null)
            gameData = data.gameData;

        Debug.Log($"게임 로드 : {path}");

        return true;
    }

    #endregion
}
