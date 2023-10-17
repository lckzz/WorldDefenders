using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Arrows
    {
        UnitArrow,
        PlayerArrow
    }

    public enum Sound
    {
        BGM,
        Effect,
        MaxCount
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        BattleStage_Field
    }

    public enum SpecialUnitState
    {
        Idle,
        Run,
        Trace,
        Attack,
        Skill,
        KnockBack,
        Die
    }

    public enum EliteMonsterState
    {
        Idle,
        Run,
        Trace,
        Attack,
        Skill,
        KnockBack,
        Die
    }

    public enum MonsterType
    {
        NormalSkeleton,
        BowSkeleton,
        SpearSkeleton,
        EliteWarrior,
        EliteShaman,
        EliteCavalry,
        Count
    }

    public enum SkillType
    {
        Active,
        Passive,
        Buff,
        Count
    }


    public enum MainStage
    {
        One
    }


    public enum SubStage
    {
        West,
        East,
        South,
        Boss

    }

    public enum StageState
    {
        Lock,
        Open,
        Count
    }

    public enum UnitUILv
    {
        One,
        Two,
        Three,
        Count
    }

    public enum PlayerSkill
    {
        Heal,
        FireArrow,
        Weakness,
        Count
    }

    public enum PlayerArrowType
    {
        Normal,
        Fire,
        Count
    }


    public enum UnitDamageType
    {
        Enemy,
        Team,
        Critical,
        Count
    }

    public enum SettingType
    {
        LobbySetting,
        InGameSetting,
        Count
    }

    public enum LobbyUnitState
    {
        Idle,
        Run,
        Attack
    }

    public enum UnitWeaponType
    {
        Sword,
        Bow,
        Spear,
        Magic,
        MagicianSkill,
        CavalrySkill
    }

    public enum MonsterSpawnType
    {
        Normal,
        Wave,
        Elite,
        Final
    }


    public enum StageStageType
    {
        Playing,
        Victory,
        Defeat,
    }


}
