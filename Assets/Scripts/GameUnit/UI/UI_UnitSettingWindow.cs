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
        //enum의 순서대로 생성해주면서 UnitClass를 넣어준다.
        if(unitNodeContent != null)
        {
            for(int ii = 0; ii < (int)UnitClass.Count; ii++)
            {
                if (ii == (int)UnitClass.Warrior)
                {
                    if(GlobalData.g_UnitWarriorLv > 0)                          //해당 유닛의 레벨이 0보다 커야 유닛셋팅에 생성
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

            //먼저 유닛들을 셋팅에 생성해주고 
            //나중에 유닛들이 다 생성되면 스페셜유닛을 생성해준다.


        }
    }



    void UnitNodeUiInstantiate(UnitClass uniClass)
    {
        GameObject obj = Managers.Resource.Instantiate("UI/Unit", unitNodeContent.transform);
        obj.TryGetComponent<UnitNodeUI>(out unitNodeUI);
        unitNodeUI.GetUnitClass(uniClass);
    }
}
