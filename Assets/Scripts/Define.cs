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



}
