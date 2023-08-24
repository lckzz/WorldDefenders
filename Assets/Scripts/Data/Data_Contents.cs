using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region 타워
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



#region 유닛

[Serializable]
public class UnitStat
{
    public int level;
    public int hp;
    public int att;
    public int knockBackForce;
    public int criticalRate;
    public float attackRange;
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


public class MagicianData : ILoader<int, UnitStat>
{
    public List<UnitStat> magician = new List<UnitStat>();

    public Dictionary<int, UnitStat> MakeDict()
    {
        Dictionary<int, UnitStat> dict = new Dictionary<int, UnitStat>();
        foreach (UnitStat stat in magician)
            dict.Add(stat.level, stat);

        return dict;
    }
}
#endregion


#region 몬스터
[Serializable]
public class MonsterStat
{
    public string id;
    public int level;
    public int hp;
    public int att;
    public int knockBackForce;
    public int criticalRate;
    public float attackRange;

}


public class NormalSkeletonData :ILoader<string, MonsterStat>
{
    public List<MonsterStat> normalSkeleton = new List<MonsterStat>();
    public Dictionary<string, MonsterStat> MakeDict()
    {
        Dictionary<string, MonsterStat> dict = new Dictionary<string, MonsterStat>();
        foreach (MonsterStat stat in normalSkeleton)
            dict.Add(stat.id, stat);

        return dict;
    }
     
}

public class BowSkeletonData : ILoader<string, MonsterStat>
{
    public List<MonsterStat> bowSkeleton = new List<MonsterStat>();
    public Dictionary<string, MonsterStat> MakeDict()
    {
        Dictionary<string, MonsterStat> dict = new Dictionary<string, MonsterStat>();
        foreach (MonsterStat stat in bowSkeleton)
            dict.Add(stat.id, stat);

        return dict;
    }

}

public class SpearSkeletonData : ILoader<string, MonsterStat>
{
    public List<MonsterStat> spearSkeleton = new List<MonsterStat>();
    public Dictionary<string, MonsterStat> MakeDict()
    {
        Dictionary<string, MonsterStat> dict = new Dictionary<string, MonsterStat>();
        foreach (MonsterStat stat in spearSkeleton)
            dict.Add(stat.id, stat);

        return dict;
    }

}

public class EliteWarriorData : ILoader<string, MonsterStat>
{
    public List<MonsterStat> eliteWarrior = new List<MonsterStat>();
    public Dictionary<string, MonsterStat> MakeDict()
    {
        Dictionary<string, MonsterStat> dict = new Dictionary<string, MonsterStat>();
        foreach (MonsterStat stat in eliteWarrior)
            dict.Add(stat.id, stat);

        return dict;
    }

}

#endregion