using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


#region Ÿ��
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



#region ����

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


#region ����
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

#region ���� ����Ʈ
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




#region ���ֽ�ų

[Serializable]
public class SkillData
{
    public int id;
    public int level;
    public string name;
    public string skillImg;
    public string desc;
    public int skillValue;  //��ų�������� �ۼ�Ʈ�� �ް� ������ ���ݷ� * �ۼ�Ʈ����
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


#region �÷��̾� ��ų


[Serializable]
public class PlayerSkillData : SkillData
{
    public int skillDuration;       //��ų���ӽð�

}


public class HealSkillData : ILoader<int, PlayerSkillData>
{
    public List<PlayerSkillData> healSkill = new List<PlayerSkillData>();

    public Dictionary<int, PlayerSkillData> MakeDict()
    {
        Dictionary<int, PlayerSkillData> dict = new Dictionary<int, PlayerSkillData>();
        foreach (PlayerSkillData skill in healSkill)
            dict.Add(skill.level, skill);

        return dict;
    }
}


public class FireArrowSkillData : ILoader<int, PlayerSkillData>
{
    public List<PlayerSkillData> fireArrowSkill = new List<PlayerSkillData>();

    public Dictionary<int, PlayerSkillData> MakeDict()
    {
        Dictionary<int, PlayerSkillData> dict = new Dictionary<int, PlayerSkillData>();
        foreach (PlayerSkillData skill in fireArrowSkill)
            dict.Add(skill.level, skill);

        return dict;
    }
}


public class WeaknessSkillData : ILoader<int, PlayerSkillData>
{
    public List<PlayerSkillData> weaknessSkill = new List<PlayerSkillData>();

    public Dictionary<int, PlayerSkillData> MakeDict()
    {
        Dictionary<int, PlayerSkillData> dict = new Dictionary<int, PlayerSkillData>();
        foreach (PlayerSkillData skill in weaknessSkill)
            dict.Add(skill.level, skill);

        return dict;
    }
}
#endregion

#region �������� ������

[Serializable]
public class StageData
{
    public int id;
    public int order;
    public string name;
    public int gold;
    public string monster1;
    public string monster2;
    public int minLevel;
    public int maxLevel;
}

public class OneChapterStageData : ILoader<int, StageData>
{
    public List<StageData> oneChapterStage = new List<StageData>();

    public Dictionary<int, StageData> MakeDict()
    {
        Dictionary<int, StageData> dict = new Dictionary<int, StageData>();
        foreach (StageData stage in oneChapterStage)
            dict.Add(stage.id, stage);

        return dict;
    }

}

#endregion