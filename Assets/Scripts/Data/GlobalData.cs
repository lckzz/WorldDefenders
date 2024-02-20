using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;


public class GlobalData 
{
    //public static int g_Gold = 0;
    //public static int g_PlayerLevel = 1;

    ////----------������ ������ ��-----------
    //public static float g_BgmValue = 0.5f;                  //���� �����Ȱ��� ����Ʈ��
    //public static float g_EffValue = 0.5f;
    //public static bool g_BgmisOn = false;
    //public static bool g_EffisOn = false;

    //----------������ ������ ��-----------

    //----------�������� ���ప-----------
    //public static bool g_WestStageClear = false;
    //public static bool g_EastStageClear = false;
    //public static bool g_SouthStageClear = false;

    //----------�������� ���ప-----------



    //-----------���� ����-----------------
    //public static int g_UnitWarriorLv = 1;
    //public static int g_UnitArcherLv = 1;
    //public static int g_UnitSpearLv = 1;
    //public static int g_UnitPriestLv = 1;
    //public static int g_UnitMagicianLv = 1;
    //public static int g_UnitCarlvryLv = 1;
    //-----------���� ����-----------------

    ////-----------���� ��ų-----------------         //���ֽ�ų�� ���ֵ��� ������ 1,5,10�϶� ������ �ڵ����� ����
    //public static int g_UnitMagicianSkillLv = 1;
    //public static int g_UnitCarlvrySkillLv = 1;
    ////-----------���� ��ų-----------------


    //----------�÷��̾��� ��ų ����-------------
    public static int g_SkillHealLv = 1;
    public static int g_SkillFireArrowLv = 0;
    public static int g_SkillWeaknessLv = 0;

    //----------�÷��̾��� ��ų ����-------------


    //-------------���� ���̵�-----------


    //-------------���� ���̵�-----------


    public static Define.PlayerSkill g_CurPlayerEquipSkill = Define.PlayerSkill.Count;      //���� ������ ��ų
    public static List<UnitClass> g_SlotUnitClass = new List<UnitClass>();
    public static List<Define.MonsterType> g_MonsterTypeList = new List<Define.MonsterType>();
    //public static Define.SubStage curStage = Define.SubStage.West;  //�̰͵� ���� �̱��濡�� 
    public static bool firstInit = false;  //��¥ ó�� �����߰ų� �ҷ��� �����Ͱ� ���ٸ�

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

            Managers.Game.FirstInit = true;  //��¥ 1ȸ���� �����ʱ�ȭ����
        }
        else
        {
            //���߿��� ����� ������ ���ֻ��¸� �����ͼ� g_SlotUnitClass���� �ٽ� �־������
        }
       

    }

    public static void SetUnitClass(List<UnitSlotUI> unitSlotUIList)
    {
        Managers.Game.SlotUnitClass.Clear();        //Ŭ�������ְ� �ٽü���
        for(int ii = 0; ii < unitSlotUIList.Count;ii++)
        {
            Managers.Game.SlotUnitClass.Add(unitSlotUIList[ii].E_UnitClass);
        }
    }

    //public static void SetMonsterList(List<Define.MonsterType> monTypeList)
    //{
    //    //������������â���� ������ �ϸ� �Ѱܹ޾Ƽ� �ΰ��ӿ� ����
    //    Managers.Game.MonsterTypeList.Clear();
    //    for (int ii = 0; ii < monTypeList.Count; ii++)
    //        Managers.Game.MonsterTypeList.Add(monTypeList[ii]);
    //}
}
