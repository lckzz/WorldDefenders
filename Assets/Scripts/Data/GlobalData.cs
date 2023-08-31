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
    public static int g_UnitMagicianLv = 1;

    //-------------몬스터 아이디-----------
    public static int g_NormalSkeletonID = 0001;
    public static int g_BowSkeletonID = 0002;
    public static int g_SpearSkeletonID = 0003;
    public static int g_EliteWarriorID = 0004;

    //-------------몬스터 아이디-----------



    public static List<UnitClass> g_SlotUnitClass = new List<UnitClass>();
    public static List<Define.MonsterType> g_MonsterTypeList = new List<Define.MonsterType>();
    public static Define.SubStage curStage = Define.SubStage.One;
    public static bool firstInit = false;  //진짜 처음 접속했거나 불러온 데이터가 없다면

    public const int g_unitSlotMax = 5;



    public static void InitUnitClass()
    {
        if(!firstInit)
        {
            for (int ii = 0; ii < g_unitSlotMax; ii++)
            {
                if (ii == 0)
                    g_SlotUnitClass.Add(UnitClass.Warrior);
                else
                    g_SlotUnitClass.Add(UnitClass.Count);
            }

            firstInit = true;  //진짜 1회한정 슬롯초기화해줌
        }
        else
        {
            //나중에는 저장된 슬롯의 유닛상태를 가져와서 g_SlotUnitClass에서 다시 넣어줘야함
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
