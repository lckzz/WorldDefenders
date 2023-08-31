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

    //-------------���� ���̵�-----------
    public static int g_NormalSkeletonID = 0001;
    public static int g_BowSkeletonID = 0002;
    public static int g_SpearSkeletonID = 0003;
    public static int g_EliteWarriorID = 0004;

    //-------------���� ���̵�-----------



    public static List<UnitClass> g_SlotUnitClass = new List<UnitClass>();
    public static List<Define.MonsterType> g_MonsterTypeList = new List<Define.MonsterType>();
    public static Define.SubStage curStage = Define.SubStage.One;
    public static bool firstInit = false;  //��¥ ó�� �����߰ų� �ҷ��� �����Ͱ� ���ٸ�

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

            firstInit = true;  //��¥ 1ȸ���� �����ʱ�ȭ����
        }
        else
        {
            //���߿��� ����� ������ ���ֻ��¸� �����ͼ� g_SlotUnitClass���� �ٽ� �־������
        }

    }

    public static void SetUnitClass(List<UnitSlotUI> unitSlotUIList)
    {
        g_SlotUnitClass.Clear();        //Ŭ�������ְ� �ٽü���
        for(int ii = 0; ii < unitSlotUIList.Count;ii++)
        {
            g_SlotUnitClass.Add(unitSlotUIList[ii].E_UnitClass);
        }
    }

    public static void SetMonsterList(List<Define.MonsterType> monTypeList)
    {
        //������������â���� ������ �ϸ� �Ѱܹ޾Ƽ� �ΰ��ӿ� ����
        g_MonsterTypeList.Clear();
        for (int ii = 0; ii < monTypeList.Count; ii++)
            g_MonsterTypeList.Add(monTypeList[ii]);
    }
}
