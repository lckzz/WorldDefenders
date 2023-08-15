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

    public enum MonsterType
    {
        NormalSkeleton,
        BowSkeleton,
        Count
    }

    public enum MainStage
    {
        One
    }


    public enum SubStage
    {
        One,
        Two,
        Three,
        Boss

    }



}
