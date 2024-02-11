using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class UpgradeUnitNode : MonoBehaviour
{
    private string className = "";
    [SerializeField] private GameObject[] classObj;
    [SerializeField] private TextMeshProUGUI lvText;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private UnitClass upgradeUnit;

    private int unitLv = 0;
    private IOpenPanel openPanel;
    // Start is called before the first frame update
    void Start()
    {
        Init();


    }

    // Update is called once per frame


    void Init()
    {

        UpgradeUnitInit();

        Debug.Log(classObj.Length);

        UnitButtonEvent(upgradeBtn.gameObject, (int)upgradeUnit, OpenUpgradeUnitPopUp, UIEvent.PointerDown);
      
    }


    void UpgradeUnitInit()
    {
        if (this.gameObject.name.Contains("Warrior"))
        {
            upgradeUnit = UnitClass.Warrior;
            unitLv = Managers.Game.UnitWarriorLv;
        }
        else if (gameObject.name.Contains("Archer"))
        {
            upgradeUnit = UnitClass.Archer;
            unitLv = Managers.Game.UnitArcherLv;

        }
        else if (gameObject.name.Contains("Spear"))
        {
            upgradeUnit = UnitClass.Spear;
            unitLv = Managers.Game.UnitSpearLv;
        }
        else if (gameObject.name.Contains("Priest"))
        {
            upgradeUnit = UnitClass.Priest;
            unitLv = Managers.Game.UnitPriestLv;

        }
        else if (gameObject.name.Contains("Magician"))
        {
            upgradeUnit = UnitClass.Magician;
            unitLv = Managers.Game.UnitMagicianLv;

        }
        else if (gameObject.name.Contains("Cavalry"))
        {
            upgradeUnit = UnitClass.Cavalry;
            unitLv = Managers.Game.UnitCarlvlry;

        }


        if ((int)upgradeUnit < (int)UnitClass.Magician)
            classObj = new GameObject[(int)Define.UnitUILv.Count];
        else 
            classObj = new GameObject[(int)Define.UnitUILv.One];


        for (int ii = 0; ii < classObj.Length; ii++)
        {
            if (classObj[ii] == null)
                classObj[ii] = this.transform.GetChild(ii).gameObject;

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
        if(upgradeUnit < UnitClass.Magician)
        {
            if (unitLv < 5)
            {
                classObj[(int)Define.UnitUILv.One].SetActive(true);
                classObj[(int)Define.UnitUILv.Two].SetActive(false);
                classObj[(int)Define.UnitUILv.Three].SetActive(false);

            }

            else
            {

                classObj[(int)Define.UnitUILv.Two].SetActive(true);
                classObj[(int)Define.UnitUILv.One].SetActive(false);
                classObj[(int)Define.UnitUILv.Three].SetActive(false);

            }
        }


        
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
