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
