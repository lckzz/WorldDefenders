using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    public string name;
    public int hp;
    public int att;
    public int knockBackForce;
    public int criticalRate;
    public float attackRange;
    public int cost;
    public int price;
    public string attackDesc;
    public string desc;
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

public class priestData : ILoader<int, UnitStat>
{
    public List<UnitStat> priest = new List<UnitStat>();

    public Dictionary<int, UnitStat> MakeDict()
    {
        Dictionary<int, UnitStat> dict = new Dictionary<int, UnitStat>();
        foreach (UnitStat stat in priest)
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

public class CavarlyData : ILoader<int, UnitStat>
{
    public List<UnitStat> cavarly = new List<UnitStat>();

    public Dictionary<int, UnitStat> MakeDict()
    {
        Dictionary<int, UnitStat> dict = new Dictionary<int, UnitStat>();
        foreach (UnitStat stat in cavarly)
            dict.Add(stat.level, stat);

        return dict;
    }
}
#endregion


#region 몬스터
[Serializable]
public class MonsterStat
{
    public int id;
    public int level;
    public int hp;
    public int att;
    public int knockBackForce;
    public int criticalRate;
    public float attackRange;
    public int dropGold;
    public int dropCost;

}


public class MonsterData :ILoader<int, MonsterStat>
{
    public List<MonsterStat> monster = new List<MonsterStat>();
    public Dictionary<int, MonsterStat> MakeDict()
    {
        Dictionary<int, MonsterStat> dict = new Dictionary<int, MonsterStat>();
        foreach (MonsterStat stat in monster)
            dict.Add(stat.id, stat);

        return dict;
    }
     
}


#endregion

#region 몬스터 게이트
[Serializable]
public class MonsterGateStat
{
    public int level;
    public int hp;
}


public class MonsterGateData : ILoader<int, MonsterGateStat>
{
    public List<MonsterGateStat> monsterGate = new List<MonsterGateStat>();
    public Dictionary<int,MonsterGateStat> MakeDict()
    {
        Dictionary<int, MonsterGateStat> dict = new Dictionary<int, MonsterGateStat>();
        foreach (MonsterGateStat stat in monsterGate)
            dict.Add(stat.level, stat);

        return dict;
    }
}

#endregion




#region 유닛스킬

[Serializable]
public class SkillData
{
    public int id;
    public int level;
    public string name;
    public string skillImg;
    public string desc;
    public int skillValue;  //스킬데미지는 퍼센트로 받고 유닛의 공격력 * 퍼센트형식
    public int skillTargetCount;
    public int skillCoolTime;
    public string skillPrefab;
}

public class MagicianSkillData : ILoader<int, SkillData>
{
    public List<SkillData> magicianSkill = new List<SkillData>();

    public Dictionary<int, SkillData> MakeDict()
    {
        Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
        foreach (SkillData skill in magicianSkill)
            dict.Add(skill.level, skill);

        return dict;
    }
}


public class CavalrySKillData : ILoader<int, SkillData>
{
    public List<SkillData> cavalrySkill = new List<SkillData>();

    public Dictionary<int, SkillData> MakeDict()
    {
        Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
        foreach (SkillData skill in cavalrySkill)
            dict.Add(skill.level, skill);

        return dict;
    }
}


public class EliteWarriorSkillData : ILoader<int, SkillData>
{
    public List<SkillData> eliteWarriorSkill = new List<SkillData>();

    public Dictionary<int, SkillData> MakeDict()
    {
        Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
        foreach (SkillData skill in eliteWarriorSkill)
            dict.Add(skill.id, skill);

        return dict;
    }
}

public class EliteCavalrySkillData : ILoader<int, SkillData>
{
    public List<SkillData> eliteCavalrySkill = new List<SkillData>();

    public Dictionary<int, SkillData> MakeDict()
    {
        Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
        foreach (SkillData skill in eliteCavalrySkill)
            dict.Add(skill.id, skill);

        return dict;
    }
}

public class EliteShamanSkillData : ILoader<int, SkillData>
{
    public List<SkillData> eliteShamanSkill = new List<SkillData>();

    public Dictionary<int, SkillData> MakeDict()
    {
        Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
        foreach (SkillData skill in eliteShamanSkill)
            dict.Add(skill.id, skill);

        return dict;
    }
}




#endregion


#region 플레이어 스킬

[Serializable]
public class HealSkillData : ILoader<int, SkillData>
{
    public List<SkillData> healSkill = new List<SkillData>();

    public Dictionary<int, SkillData> MakeDict()
    {
        Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
        foreach (SkillData skill in healSkill)
            dict.Add(skill.level, skill);

        return dict;
    }
}

[Serializable]
public class FireArrowSkillData : ILoader<int, SkillData>
{
    public List<SkillData> fireArrowSkill = new List<SkillData>();

    public Dictionary<int, SkillData> MakeDict()
    {
        Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
        foreach (SkillData skill in fireArrowSkill)
            dict.Add(skill.level, skill);

        return dict;
    }
}

[Serializable]
public class WeaknessSkillData : ILoader<int, SkillData>
{
    public List<SkillData> weaknessSkill = new List<SkillData>();

    public Dictionary<int, SkillData> MakeDict()
    {
        Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
        foreach (SkillData skill in weaknessSkill)
            dict.Add(skill.level, skill);

        return dict;
    }
}
#endregion