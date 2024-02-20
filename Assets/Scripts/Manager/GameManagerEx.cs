using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using static Define;

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

    //public bool westStageClear;
    //public bool eastStageClear;
    //public bool southStageClear;
    public bool onechapterAllClear;

    public float bgmValue;
    public float effValue;
    public bool bgmisOn;
    public bool effisOn;

    //public float westStageBestTime;
    //public float eastStageBestTime;
    //public float southStageBestTime;

}

[Serializable]
public class GameArrayData
{
    public List<UnitClass> slotUnitClass = new List<UnitClass>();
    public List<OneChapterStageInfo> oneChapterStageInfoList = new List<OneChapterStageInfo>();

}

[Serializable]
public class GameTotalData
{
    public UnitClass[] unitClasses;
    public OneChapterStageInfo[] oneChapterInfos;
    public GameData gameData;
}


public class GameManagerEx 
{
    //�ΰ��ӳ��� ������ ����

    GameData gameData = new GameData();
    GameArrayData gameSaveArrayData = new GameArrayData();
    GameTotalData gameTotalData = new GameTotalData();


    private MoneyCost moneyCost = new MoneyCost();
    private UnitSpawn unitSpawn = new UnitSpawn();
    private MonsterSpawn monsterSpawn = new MonsterSpawn();
    private MonsterEvent monsterEvent = new MonsterEvent();
    private StageState stageState = new StageState();

    private int unitMagicianSkillLv = 0;                //���� ��������ʰ� �ش� ������ ������ ���� �����Ҷ� ���õ�
    private int unitCavalrySkillLv = 0;                 //���� ��������ʰ� �ش� ������ ������ ���� �����Ҷ� ���õ�

    private const int NORMAL_SKELETON_ID = 0001;        //���͵��� ���� �ѹ�
    private const int BOW_SKELETON_ID = 0002;
    private const int SPEAR_SKELETON_ID = 0003;
    private const int MID_SKELETON_ID = 0004;
    private const int MID_BOW_SKELETON_ID = 0005;
    private const int MID_SPEAR_SKELETON_ID = 0006;
    private const int HIGH_SKELETON_ID = 0007;
    private const int HIGH_BOW_SKELETON_ID = 0008;
    private const int HIGH_SPEAR_SKELETON_ID = 0009;
    private const int ELITE_WARRIOR_ID = 0101;
    private const int ELITE_SHAMAN_ID = 0102;
    private const int ELITE_CAVALRY_ID = 0103;
    private const int SKELETON_KING_ID = 0104;


    private List<MonsterType> monsterTypeList = new List<MonsterType>();
    private bool lobbyToGameScene = false;


    private Dictionary<MonsterType, int> monsterTypeIdDict;
    public Dictionary<MonsterType, int> MonsterTypeIdDict { get { return monsterTypeIdDict; } }


    private Dictionary<UnitClass, int> specialUnitSkillLvDict;
    public Dictionary<UnitClass, int> SpecialUnitSkillLvDict {  get { return specialUnitSkillLvDict; } }


    private Dictionary<int, int> upgradeUnitLvDict;

    public Dictionary<int,int> UpgradeUnitLvDict { get { return upgradeUnitLvDict; } }

    private Dictionary<UnitClass, Dictionary<int, UnitStat>> unitStatDict;

    public Dictionary<UnitClass, Dictionary<int, UnitStat>> UnitStatDict { get { return unitStatDict; } }

    public List<OneChapterStageInfo> OneChapterStageInfoList { 
        get { return gameSaveArrayData.oneChapterStageInfoList; }  
        set { gameSaveArrayData.oneChapterStageInfoList = value; }
    }


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

    #region ���� ����
    public void UnitDictInit()
    {
        upgradeUnitLvDict = new Dictionary<int, int>
        {
            { (int)UnitClass.Warrior, Managers.Game.UnitWarriorLv },
            { (int)UnitClass.Archer, Managers.Game.UnitArcherLv },
            { (int)UnitClass.Spear, Managers.Game.UnitSpearLv },
            { (int)UnitClass.Priest, Managers.Game.UnitPriestLv },
            { (int)UnitClass.Magician, Managers.Game.UnitMagicianLv },
            { (int)UnitClass.Cavalry, Managers.Game.UnitCavalryLv },
        };

        specialUnitSkillLvDict = new Dictionary<UnitClass, int>
        {
            {UnitClass.Magician, unitMagicianSkillLv },
            {UnitClass.Cavalry, unitCavalrySkillLv }

        };

        unitStatDict = new Dictionary<UnitClass, Dictionary<int, UnitStat>>
        {
            {UnitClass.Warrior, Managers.Data.warriorDict },
            {UnitClass.Archer, Managers.Data.archerDict },
            {UnitClass.Spear, Managers.Data.spearDict },
            {UnitClass.Priest, Managers.Data.priestDict },
            {UnitClass.Magician, Managers.Data.magicDict },
            {UnitClass.Cavalry, Managers.Data.cavarlyDict },

        };

        monsterTypeIdDict = new Dictionary<MonsterType, int>
        {
            {MonsterType.NormalSkeleton, NORMAL_SKELETON_ID },
            {MonsterType.NormalBowSkeleton, BOW_SKELETON_ID },
            {MonsterType.SpearSkeleton, SPEAR_SKELETON_ID },
            {MonsterType.MidSkeleton, MID_SKELETON_ID },
            {MonsterType.MidBowSkeleton, MID_BOW_SKELETON_ID },
            {MonsterType.MidSpearSkeleton, MID_SPEAR_SKELETON_ID },
            {MonsterType.HighSkeleton, HIGH_SKELETON_ID },
            {MonsterType.HighBowSkeleton, HIGH_BOW_SKELETON_ID },
            {MonsterType.HighSpearSkeleton, HIGH_SPEAR_SKELETON_ID },
            {MonsterType.EliteWarrior, ELITE_WARRIOR_ID },
            {MonsterType.EliteShaman, ELITE_SHAMAN_ID },
            {MonsterType.EliteCavalry, ELITE_CAVALRY_ID },
            {MonsterType.SkeletonKing, SKELETON_KING_ID },


        };

    }

    public void UnitLvDictRefresh()
    {
        //��ųʸ��� ���� �������ش�.
        foreach(var unitClass in Enum.GetValues(typeof(UnitClass)).Cast<UnitClass>())
        {
            upgradeUnitLvDict[(int)unitClass] = GetUnitLevel(unitClass);
        }
    }

    public void SpecialUnitLvDictRefresh(UnitClass unit)
    {

    }

    public int GetUnitLevel(UnitClass unitClass)
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
                return Managers.Game.UnitCavalryLv;
            default:
                return 0;
        }

    }

    #endregion
    #region ���� ������
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

    public int UnitCavalryLv
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

    public PlayerSkill CurPlayerEquipSkill
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

    public bool OneChapterAllClear
    {
        get { return gameData.onechapterAllClear; }
        set { gameData.onechapterAllClear = value; }
    }




    #endregion

    public void SetSpecialUnitSkillInit(UnitClass unitclass,int lv)
    {
        specialUnitSkillLvDict[unitclass] = SkillLvToSpecialUnitLv(lv);

    }

    private int SkillLvToSpecialUnitLv(int unitLv)
    {
        if (0 < unitLv && unitLv < 4)        //1~3���϶���
            return 1;        //��ų���� 1
        else if (4 <= unitLv && unitLv < 8)        //4~7���϶���
            return 2;        //��ų���� 2
        else                                   //8������ ��������
            return 3;
    }

    private void SpecialUnitSkillInit()
    {
        for(int ii = (int)UnitClass.Magician; ii < (int)UnitClass.Count; ii++)
        {
            //����������� ���� ���Ƽ�
            SetSpecialUnitSkillInit((UnitClass)ii, GetUnitLevel((UnitClass)ii));
        }


    }


    public void GameDataInit()
    {
        //ó�� ������ �ʱ�ȭ
        Gold = 0;
        PlayerLevel = 1;
        UnitWarriorLv = 1;
        UnitArcherLv = 1;
        UnitSpearLv = 1;
        UnitPriestLv = 1;
        UnitMagicianLv = 1;
        UnitCavalryLv = 1;
        TowerHealSkillLv = 1;
        FireArrowSkillLv = 0;
        WeaknessSkillLv = 0;
        CurPlayerEquipSkill = Define.PlayerSkill.Count;
        FirstInit = false;
        TutorialEnd = false;
        BgmValue = 0.5f;
        EffValue = 0.5f;
        BgmisOn = false;
        EffisOn = false;

        unitMagicianSkillLv = 1;
        unitCavalrySkillLv = 1;

        stageState.StageInfoInit();

    }





    public void SetMonsterList(List<Define.MonsterType> monTypeList)
    {
        //������������â���� ������ �ϸ� �Ѱܹ޾Ƽ� �ΰ��ӿ� ����
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


    //�ڽ�Ʈ ����
    #region �ڽ�Ʈ �� ��ȭ ����
    public float Cost { get { return moneyCost.CurCost; } set { moneyCost.CurCost = value; } }


    public int GameGold { get { return moneyCost.GameMoney; } set { moneyCost.GameMoney = value; } }




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

    public void UnitSummonDequeue(GameObject go, Transform[] tr)
    {
        unitSpawn.UnitSummonDequeue(go, tr);
    }

    public void UnitQueueClear()
    {
        unitSpawn.UnitQueueClear();
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

    public void BossMonsterSpawn()
    {
        monsterSpawn.BossMonsterSpawn();

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


    public Define.Stage CurStageType { get { return stageState.CurStageType; } set { stageState.CurStageType = value; } }


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
    
    public IEnumerable GetStageData()
    {
        return stageState.GetStageData();
    }

    public bool GameEndResult()
    {
        return stageState.GameEndResult();
    }

    public bool StageAllClear()
    {

        for(int ii = 0; ii < Managers.Game.OneChapterStageInfoList.Count; ii++)
        {
            if (OneChapterStageInfoList[ii].clear == false)     //����Ʈ�� ���Ƽ� �� �Ѱ��� Ŭ�����Ѱ� ���ٸ� false����
                return false;

        }


        return true;
    }


    #endregion




    #region ���� & �ε�


    public void FileSave()
    {
        GameTotalData arrayData = new GameTotalData
        {
            //����Ʈ�� json���� ��ȯ�� �ȵǱ⶧���� �迭�� �����༭ json���� ����
            unitClasses = gameSaveArrayData.slotUnitClass.ToArray(),
            oneChapterInfos = gameSaveArrayData.oneChapterStageInfoList.ToArray(),
            gameData = Managers.Game.gameData
        };


        string path = Application.persistentDataPath + "/SaveData.json";
        string jsonStr = JsonUtility.ToJson(arrayData);


        File.WriteAllText(path, jsonStr);
        Debug.Log($"���� ���̺��: {path}");

    }

    public bool FileLoad()
    {

        string path = Application.persistentDataPath + "/SaveData.json";

        if (File.Exists(path) == false)
            return false;


        string fileStr = File.ReadAllText(path);
        GameTotalData data = JsonUtility.FromJson<GameTotalData>(fileStr);

        Debug.Log(data.oneChapterInfos);
        gameSaveArrayData.slotUnitClass.Clear();
        gameSaveArrayData.oneChapterStageInfoList.Clear();

        stageState.StageInfoInit();     //�������������͸� ���� �� �������ش�.


        for (int ii = 0;  ii < data.unitClasses.Length; ii++)
            gameSaveArrayData.slotUnitClass.Add(data.unitClasses[ii]);          //����Ǿ��ִ� ���ֽ����� �ҷ��ͼ� �־��ش�.

        if (data.oneChapterInfos != null)  //�̹� ����Ǿ��ִ� �������������ʹ� ���ư��鼭 ������ش�.
        {
            for (int ii = 0; ii < data.oneChapterInfos.Length; ii++)
            {
                data.oneChapterInfos[ii].StageData = Managers.Data.stageDict[data.oneChapterInfos[ii].StageData.id];  //�����ȵ����Ͱ��� �ε��Ҷ����� �ʱ�ȭ����
                gameSaveArrayData.oneChapterStageInfoList[ii] = data.oneChapterInfos[ii];
            }
        }
        //else
        //{
        //    //���� �����Ͱ� ���ٸ�;; (��)���� �������������� �����ؼ� �����ߴٸ�
        //}


        if (data != null)
            gameData = data.gameData;

        Debug.Log($"���� �ε� : {path}");


        SpecialUnitSkillInit();
        //SetSpecialUnitSkillInit();      //�����ͷε��Ѵ����� ��ų������ ���ַ����� ���� �ʱ�ȭ

        return true;
    }

    #endregion
}
