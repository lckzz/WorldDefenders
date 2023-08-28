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


    public Dictionary<int,TowerStat> towerDict { get; private set; } = new Dictionary<int,TowerStat>();
    public Dictionary<int, UnitStat> warriorDict { get; private set; } = new Dictionary<int, UnitStat>();
    public Dictionary<int, UnitStat> archerDict { get; private set; } = new Dictionary<int, UnitStat>();
    public Dictionary<int, UnitStat> spearDict { get; private set; } = new Dictionary<int, UnitStat>();
    public Dictionary<int, UnitStat> magicDict { get; private set; } = new Dictionary<int, UnitStat>();
    public Dictionary<int, MonsterStat> monsterDict { get; private set; } = new Dictionary<int, MonsterStat>();
   

    public Dictionary<int, SkillData> magicSkillDict { get; private set; } = new Dictionary<int, SkillData>();


    public void Init()
    {
        towerDict = LoadJson<TowerData, int, TowerStat>("TowerData").MakeDict();
        warriorDict = LoadJson<WarriorData, int, UnitStat>("UnitData").MakeDict();
        archerDict = LoadJson<ArcherData, int, UnitStat>("UnitData").MakeDict();
        spearDict = LoadJson<SpearData, int, UnitStat>("UnitData").MakeDict();
        magicDict = LoadJson<MagicianData, int, UnitStat>("UnitData").MakeDict();
        monsterDict = LoadJson<MonsterData, int, MonsterStat>("MonsterData").MakeDict();



        magicSkillDict = LoadJson<MagicianSkillData, int, SkillData>("SkillData").MakeDict();

    }
    Loader LoadJson<Loader,Key,Value>(string path) where Loader : ILoader<Key,Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        Loader data = JsonUtility.FromJson<Loader>(textAsset.text);
        return data;
    }
}
