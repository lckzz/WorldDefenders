using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;


public class GlobalData 
{
    //public static int g_Gold = 0;
    //public static int g_PlayerLevel = 1;

    ////----------저장할 음량의 값-----------
    //public static float g_BgmValue = 0.5f;                  //지금 설정된값이 디폴트값
    //public static float g_EffValue = 0.5f;
    //public static bool g_BgmisOn = false;
    //public static bool g_EffisOn = false;

    //----------저장할 음량의 값-----------

    //----------스테이지 진행값-----------
    //public static bool g_WestStageClear = false;
    //public static bool g_EastStageClear = false;
    //public static bool g_SouthStageClear = false;

    //----------스테이지 진행값-----------



    //-----------유닛 레벨-----------------
    //public static int g_UnitWarriorLv = 1;
    //public static int g_UnitArcherLv = 1;
    //public static int g_UnitSpearLv = 1;
    //public static int g_UnitPriestLv = 1;
    //public static int g_UnitMagicianLv = 1;
    //public static int g_UnitCarlvryLv = 1;
    //-----------유닛 레벨-----------------

    ////-----------유닛 스킬-----------------         //유닛스킬은 유닛들의 레벨이 1,5,10일때 레벨이 자동으로 오름
    //public static int g_UnitMagicianSkillLv = 1;
    //public static int g_UnitCarlvrySkillLv = 1;
    ////-----------유닛 스킬-----------------


    //----------플레이어의 스킬 레벨-------------
    public static int g_SkillHealLv = 1;
    public static int g_SkillFireArrowLv = 0;
    public static int g_SkillWeaknessLv = 0;

    //----------플레이어의 스킬 레벨-------------


    //-------------몬스터 아이디-----------


    //-------------몬스터 아이디-----------


    public static Define.PlayerSkill g_CurPlayerEquipSkill = Define.PlayerSkill.Count;      //현재 장착한 스킬
    public static List<UnitClass> g_SlotUnitClass = new List<UnitClass>();
    public static List<Define.MonsterType> g_MonsterTypeList = new List<Define.MonsterType>();
    //public static Define.SubStage curStage = Define.SubStage.West;  //이것도 게임 싱글톤에서 
    public static bool firstInit = false;  //진짜 처음 접속했거나 불러온 데이터가 없다면

    public const int g_unitSlotMax = 5;



    public static void InitUnitClass()
    {
        if(!Managers.Game.FirstInit)
        {
            for (int ii = 0; ii < g_unitSlotMax; ii++)
            {

                if (ii == 0)
                    Managers.Game.SlotUnitClass.Add(UnitClass.Warrior);
                else
                    Managers.Game.SlotUnitClass.Add(UnitClass.Count);
            }

            Managers.Game.FirstInit = true;  //진짜 1회한정 슬롯초기화해줌
        }
        else
        {
            //나중에는 저장된 슬롯의 유닛상태를 가져와서 g_SlotUnitClass에서 다시 넣어줘야함
        }
       

    }

    public static void SetUnitClass(List<UnitSlotUI> unitSlotUIList)
    {
        Managers.Game.SlotUnitClass.Clear();        //클리어해주고 다시셋팅
        for(int ii = 0; ii < unitSlotUIList.Count;ii++)
        {
            Managers.Game.SlotUnitClass.Add(unitSlotUIList[ii].E_UnitClass);
        }
    }

    //public static void SetMonsterList(List<Define.MonsterType> monTypeList)
    //{
    //    //스테이지선택창에서 시작을 하면 넘겨받아서 인게임에 적용
    //    Managers.Game.MonsterTypeList.Clear();
    //    for (int ii = 0; ii < monTypeList.Count; ii++)
    //        Managers.Game.MonsterTypeList.Add(monTypeList[ii]);
    //}
}
