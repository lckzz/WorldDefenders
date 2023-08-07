using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region Å¸¿ö
[Serializable]
public class TowerStat
{
    public int level;
    public int hp;
    public int att;
    public int price;
}

[Serializable]
public class TowerData : ILoader<int, TowerStat>
{
    public List<TowerStat> tower = new List<TowerStat>();

    public Dictionary<int, TowerStat> MakeDict()
    {
        Dictionary<int, TowerStat> dict = new Dictionary<int, TowerStat>();
        foreach (TowerStat stat in tower)
            dict.Add(stat.level, stat);

        return dict;
    }
}

#endregion



#region À¯´Ö

[Serializable]
public class UnitStat
{
    public int level;
    public int hp;
    public int att;
    public int cost;
    public int price;
}




public class WarriorData : ILoader<int, UnitStat>
{
    public List<UnitStat> warrior = new List<UnitStat>();

    public Dictionary<int, UnitStat> MakeDict()
    {
        Dictionary<int, UnitStat> dict = new Dictionary<int, UnitStat>();
        foreach (UnitStat stat in warrior)
            dict.Add(stat.level, stat);

        return dict;
    }
}


public class ArcherData : ILoader<int, UnitStat>
{
    public List<UnitStat> archer = new List<UnitStat>();
    public Dictionary<int, UnitStat> MakeDict()
    {
        Dictionary<int, UnitStat> dict = new Dictionary<int, UnitStat>();
        foreach(UnitStat stat in archer)
            dict.Add(stat.level, stat);

        return dict;
    }
}

public class SpearData : ILoader<int, UnitStat>
{
    public List<UnitStat> spear = new List<UnitStat>();

    public Dictionary<int, UnitStat> MakeDict()
    {
        Dictionary<int, UnitStat> dict = new Dictionary<int, UnitStat>();
        foreach (UnitStat stat in spear)
            dict.Add(stat.level, stat);

        return dict;
    }
}

#endregion