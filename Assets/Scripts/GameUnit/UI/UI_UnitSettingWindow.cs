using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UnitSettingWindow : UI_Base
{

    [SerializeField] private GameObject unitNodeContent;

    private UnitNodeUI unitNodeUI;
    private UnitSlotUI unitSlotUI;
    List<UnitSlotUI> unitSlotUiList = new List<UnitSlotUI>();

    // Start is called before the first frame update
    void Start()
    {
        UI_UnitSetInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void  UI_UnitSetInit()
    {
        //enum�� ������� �������ָ鼭 UnitClass�� �־��ش�.
        if(unitNodeContent != null)
        {
            for(int ii = 0; ii < (int)UnitClass.Count; ii++)
            {
                if (ii == (int)UnitClass.Warrior)
                {
                    if(GlobalData.g_UnitWarriorLv > 0)                          //�ش� ������ ������ 0���� Ŀ�� ���ּ��ÿ� ����
                        UnitNodeUiInstantiate(UnitClass.Warrior);
                }

                else if (ii == (int)UnitClass.Archer)
                {
                    if (GlobalData.g_UnitArcherLv > 0)
                        UnitNodeUiInstantiate(UnitClass.Archer);
                }

                else if (ii == (int)UnitClass.Spear)
                {
                    if (GlobalData.g_UnitSpearLv > 0)
                        UnitNodeUiInstantiate(UnitClass.Spear);
                }
            }

            //���� ���ֵ��� ���ÿ� �������ְ� 
            //���߿� ���ֵ��� �� �����Ǹ� ����������� �������ش�.


        }
    }



    void UnitNodeUiInstantiate(UnitClass uniClass)
    {
        GameObject obj = Managers.Resource.Instantiate("UI/Unit", unitNodeContent.transform);
        obj.TryGetComponent<UnitNodeUI>(out unitNodeUI);
        unitNodeUI.GetUnitClass(uniClass);
    }
}
