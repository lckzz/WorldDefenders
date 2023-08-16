using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData 
{
    public static int g_Gold = 0;
    public static int g_PlayerLevel = 1;

    public static int g_UnitWarriorLv = 1;
    public static int g_UnitArcherLv = 1;
    public static int g_UnitSpearLv = 1;

    public static List<UnitClass> g_SlotUnitClass = new List<UnitClass>();
    public static List<Define.MonsterType> g_MonsterTypeList = new List<Define.MonsterType>();
    public static Define.SubStage curStage = Define.SubStage.One;
    public static bool firstInit = false;

    public const int g_unitSlotMax = 5;

    public static void InitUnitClass()
    {
        for(int ii = 0; ii < g_unitSlotMax; ii++)
        {
            if (ii == 0)
                g_SlotUnitClass.Add(UnitClass.Warrior);
            else
                g_SlotUnitClass.Add(UnitClass.Count);
        }
    }

    public static void SetUnitClass(List<UnitSlotUI> unitSlotUIList)
    {
        g_SlotUnitClass.Clear();        //클리어해주고 다시셋팅
        for(int ii = 0; ii < unitSlotUIList.Count;ii++)
        {
            g_SlotUnitClass.Add(unitSlotUIList[ii].E_UnitClass);
        }
    }

    public static void SetMonsterList(List<Define.MonsterType> monTypeList)
    {
        //스테이지선택창에서 시작을 하면 넘겨받아서 인게임에 적용
        g_MonsterTypeList.Clear();
        for (int ii = 0; ii < monTypeList.Count; ii++)
            g_MonsterTypeList.Add(monTypeList[ii]);
    }
}
