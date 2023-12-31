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
        MidSkeleton,
        MidBowSkeleton,
        HighSkeleton,
        HighBowSkeleton,
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

    public enum PlayerSkillState
    {
        NonEquip,
        Equip
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
        Item,
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

    public enum NoKnockBackType
    {
        Unit,
        Monster
    }

    public enum UnitNodeState
    {
        None,
        Equip
    }

    public enum UnitInfoSelectType
    {
        Node,
        Slot
    }

    public enum GameStageDirector
    {
        Entrance,
        Victory,
        Defeat
    }

    public enum DropItemType
    {
        Gold,
        Cost
    }

    public enum DebuffType
    {
        Fire,
        Weakness,
        Count
    }

    public enum DialogType  //다이얼로그인가 말풍선인가
    {
        Dialog,
        Speech,
        Count
    }

    public enum DialogKey
    {
        tutorial,
        tutorialSelectYes,
        tutorialSelectNo,
        tutorialUpgrade,
        tutorialUpgradeTower,
        tutorialUpgradeUnit,
        tutorialParty,
        tutorialPartyUnit,
        tutorialPartyWindow,
        tutorialSkill,
        tutorialSkillTree,
        tutorialSkillInfo,
        tutorialStage

    }

    public enum DialogSize
    {
        Large,
        Small
    }

    public enum DialogId
    {
        DialogMask,
        NextDialog,
        EndDialog,
        None
    }

    public enum DialogOrder     //다이얼로그순서
    {
        Upgrade,
        Party,
        Skill,
        Stage,
        None
    }


    public enum DialogMask
    {
        FirstMask,
        SecondMask,
        BackMask,
        Count
    }

}
