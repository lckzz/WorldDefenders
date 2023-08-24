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
    public Dictionary<string, MonsterStat> normalSkeleton { get; private set; } = new Dictionary<string, MonsterStat>();
    public Dictionary<string, MonsterStat> bowSkeleton { get; private set; } = new Dictionary<string, MonsterStat>();
    public Dictionary<string, MonsterStat> spearSkeleton { get; private set; } = new Dictionary<string, MonsterStat>();
    public Dictionary<string, MonsterStat> eliteSkeleton { get; private set; } = new Dictionary<string, MonsterStat>();
    public void Init()
    {
        towerDict = LoadJson<TowerData, int, TowerStat>("TowerData").MakeDict();
        warriorDict = LoadJson<WarriorData, int, UnitStat>("UnitData").MakeDict();
        archerDict = LoadJson<ArcherData, int, UnitStat>("UnitData").MakeDict();
        spearDict = LoadJson<SpearData, int, UnitStat>("UnitData").MakeDict();
        magicDict = LoadJson<MagicianData, int, UnitStat>("UnitData").MakeDict();
        normalSkeleton = LoadJson<NormalSkeletonData, string, MonsterStat>("MonsterData").MakeDict();
        bowSkeleton = LoadJson<BowSkeletonData, string, MonsterStat>("MonsterData").MakeDict();
        spearSkeleton = LoadJson<SpearSkeletonData, string, MonsterStat>("MonsterData").MakeDict();
        eliteSkeleton = LoadJson<EliteWarriorData, string, MonsterStat>("MonsterData").MakeDict();

    }
    Loader LoadJson<Loader,Key,Value>(string path) where Loader : ILoader<Key,Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        Loader data = JsonUtility.FromJson<Loader>(textAsset.text);
        return data;
    }
}
