using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class UpgradeUnitNode : MonoBehaviour
{
    private string className = "";
    //[SerializeField] private GameObject[] classObj;
    [SerializeField] private TextMeshProUGUI lvText;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private UnitClass upgradeUnit;
    [SerializeField] private GameObject spriteUnitParentObj;

    private int unitIdx = 0;

    public int UnitIdx { 
        get 
        { 
            return unitIdx;
        } 
        set 
        { 
            if(value < (int)UnitClass.Count)
            {
                unitIdx = value;
                upgradeUnit = (UnitClass)unitIdx;
            }
        }
    }
    private int unitLv = 0;
    private IOpenPanel openPanel;

    private UnitStat unitStat;

    // Start is called before the first frame update
    void Start()
    {
        Init();


    }

    // Update is called once per frame


    void Init()
    {
        unitStat = new UnitStat();

        //UpgradeUnitInit();


        UnitButtonEvent(upgradeBtn.gameObject, (int)upgradeUnit, OpenUpgradeUnitPopUp, UIEvent.PointerDown);
      
    }


    public void UpgradeUnitInit(int idx)
    {
        UnitIdx = idx;


        unitLv = Managers.Game.GetUnitLevel((UnitClass)UnitIdx);

        switch(UnitIdx)
        {
            case (int)UnitClass.Warrior:
                unitStat = Managers.Data.warriorDict[unitLv];
                break;
            case (int)UnitClass.Archer:
                unitStat = Managers.Data.archerDict[unitLv];
                break;
            case (int)UnitClass.Spear:
                unitStat = Managers.Data.spearDict[unitLv];
                break;
            case (int)UnitClass.Priest:
                unitStat = Managers.Data.priestDict[unitLv];
                break;
            case (int)UnitClass.Magician:
                unitStat = Managers.Data.magicDict[unitLv];
                break;
            case (int)UnitClass.Cavalry:
                unitStat = Managers.Data.cavarlyDict[unitLv];
                break;


        }



        RefreshUnitImg(unitLv);
    }


    void OpenUpgradeUnitPopUp(int unitIdx)
    {
        switch (unitIdx)
        {
            case (int)UnitClass.Warrior:
                {

                    Managers.UI.ShowPopUp<UI_UnitUpgradePopUp>().GetUnitIndex(unitIdx);
                    Debug.Log($"워리어 업그판넬 온!{unitIdx}");
                    break;
                }
            case (int)UnitClass.Archer:
                {
                    Managers.UI.ShowPopUp<UI_UnitUpgradePopUp>().GetUnitIndex(unitIdx);

                    Debug.Log($"아처 업그판넬 온!{unitIdx}");

                    break;
                }
            case (int)UnitClass.Spear:
                {
                    Managers.UI.ShowPopUp<UI_UnitUpgradePopUp>().GetUnitIndex(unitIdx);

                    Debug.Log($"창병 업그판넬 온!{unitIdx}");

                    break;
                }

            case (int)UnitClass.Priest:
                {
                    Managers.UI.ShowPopUp<UI_UnitUpgradePopUp>().GetUnitIndex(unitIdx);

                    Debug.Log($"창병 업그판넬 온!{unitIdx}");

                    break;
                }

            case (int)UnitClass.Magician:
                {
                    Managers.UI.ShowPopUp<UI_UnitUpgradePopUp>().GetUnitIndex(unitIdx);

                    Debug.Log($"법사 업그판넬 온!{unitIdx}");

                    break;
                }
            case (int)UnitClass.Cavalry:
                {
                    Managers.UI.ShowPopUp<UI_UnitUpgradePopUp>().GetUnitIndex(unitIdx);

                    Debug.Log($"기마병 업그판넬 온!{unitIdx}");

                    break;
                }
        }

        openPanel = Managers.UI.PeekPopupUI<UI_UnitUpgradePopUp>();
        openPanel.OpenRectTransformScaleSet();



    }


    public void RefreshUnitImg(int unitLv)
    {
        Debug.Log("테스트");

        if(spriteUnitParentObj != null)
        {
            for (int ii = 0; ii < spriteUnitParentObj.transform.childCount; ii++)
                Managers.Resource.Destroy(spriteUnitParentObj.transform.GetChild(ii).gameObject);
        }


        Debug.Log(unitStat.level);

        Managers.Resource.Instantiate(unitStat.unitSpriteUIPrefabs, spriteUnitParentObj.transform);



        lvText.text = $"<#FF9F13>Lv</color> {unitLv}";
    }
           


        

    


    void UnitButtonEvent(GameObject obj, int idx, Action<int> action = null, UIEvent type = UIEvent.PointerDown)
    {
        UI_EventHandler evt;
        obj.TryGetComponent(out evt);

        if (type == UIEvent.PointerDown)
        {
            evt.OnPointerDownIntHandler -= (unUsedIdx) => action(idx);
            evt.OnPointerDownIntHandler += (unUsedIdx) => action(idx);

        }
    }
}
