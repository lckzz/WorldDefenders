using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}



public class DataManager 
{

    #region ÇÃ·¹ÀÌ¾î & À¯´Ö µñ¼Å³Ê¸®
    public Dictionary<int,TowerStat> towerDict { get; private set; } = new Dictionary<int,TowerStat>();
    public Dictionary<int, UnitStat> warriorDict { get; private set; } = new Dictionary<int, UnitStat>();
    public Dictionary<int, UnitStat> archerDict { get; private set; } = new Dictionary<int, UnitStat>();
    public Dictionary<int, UnitStat> spearDict { get; private set; } = new Dictionary<int, UnitStat>();
    public Dictionary<int, UnitStat> priestDict { get; private set; } = new Dictionary<int, UnitStat>();

    public Dictionary<int, UnitStat> magicDict { get; private set; } = new Dictionary<int, UnitStat>();
    public Dictionary<int, UnitStat> cavarlyDict { get; private set; } = new Dictionary<int, UnitStat>();


    #endregion
    public Dictionary<int, MonsterStat> monsterDict { get; private set; } = new Dictionary<int, MonsterStat>();
    public Dictionary<int, MonsterGateStat> monsterGateDict { get; private set; } = new Dictionary<int, MonsterGateStat>();


    #region ½ºÅ³
    public Dictionary<int, SkillData> magicSkillDict { get; private set; } = new Dictionary<int, SkillData>();
    public Dictionary<int, SkillData> cavalrySkillDict { get; private set; } = new Dictionary<int, SkillData>();
    public Dictionary<int, SkillData> eliteWarriorSkillDict { get; private set; } = new Dictionary<int, SkillData>();
    public Dictionary<int, SkillData> eliteCavalrySkillDict { get; private set; } = new Dictionary<int, SkillData>();
    public Dictionary<int, SkillData> eliteShamanSkillDict { get; private set; } = new Dictionary<int, SkillData>();


    public Dictionary<int, SkillData> healSkillDict { get; private set; } = new Dictionary<int, SkillData>();
    public Dictionary<int, SkillData> fireArrowSkillDict { get; private set; } = new Dictionary<int, SkillData>();
    public Dictionary<int, SkillData> weaknessSkillDict { get; private set; } = new Dictionary<int, SkillData>();
    #endregion

    #region Æ©Åä¸®¾ó ´ÙÀÌ¾ó·Î±×
    public Dictionary<string,TextAsset> tutorialDialogue { get; private set; } = new Dictionary<string,TextAsset>();
    #endregion 


    public void Init()
    {
        towerDict = LoadJson<TowerData, int, TowerStat>("TowerData").MakeDict();
        warriorDict = LoadJson<WarriorData, int, UnitStat>("UnitData").MakeDict();
        archerDict = LoadJson<ArcherData, int, UnitStat>("UnitData").MakeDict();
        spearDict = LoadJson<SpearData, int, UnitStat>("UnitData").MakeDict();
        priestDict = LoadJson<priestData, int, UnitStat>("UnitData").MakeDict();
        magicDict = LoadJson<MagicianData, int, UnitStat>("UnitData").MakeDict();
        cavarlyDict = LoadJson<CavarlyData, int, UnitStat>("UnitData").MakeDict();
        monsterDict = LoadJson<MonsterData, int, MonsterStat>("MonsterData").MakeDict();
        monsterGateDict = LoadJson<MonsterGateData, int, MonsterGateStat>("MonsterGateData").MakeDict();

        magicSkillDict = LoadJson<MagicianSkillData, int, SkillData>("SkillData").MakeDict();
        cavalrySkillDict = LoadJson<CavalrySKillData, int, SkillData>("SkillData").MakeDict();
        eliteWarriorSkillDict = LoadJson<EliteWarriorSkillData, int, SkillData>("SkillData").MakeDict();
        eliteCavalrySkillDict = LoadJson<EliteCavalrySkillData, int, SkillData>("SkillData").MakeDict();
        eliteShamanSkillDict = LoadJson<EliteShamanSkillData, int, SkillData>("SkillData").MakeDict();

        healSkillDict = LoadJson<HealSkillData, int, SkillData>("SkillData").MakeDict();
        fireArrowSkillDict = LoadJson<FireArrowSkillData, int, SkillData>("SkillData").MakeDict();
        weaknessSkillDict = LoadJson<WeaknessSkillData, int, SkillData>("SkillData").MakeDict();

        tutorialDialogue = new Dictionary<string, TextAsset>
        {
            { "tutorial", Managers.Resource.Load<TextAsset>("Data/DialogueData") }
        };

    }
    Loader LoadJson<Loader,Key,Value>(string path) where Loader : ILoader<Key,Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        Loader data = JsonUtility.FromJson<Loader>(textAsset.text);
        return data;
    }
}
