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
            unitLv = GlobalData.g_UnitWarriorLv;
        }
        else if (gameObject.name.Contains("Archer"))
        {
            upgradeUnit = UnitClass.Archer;
            unitLv = GlobalData.g_UnitArcherLv;

        }
        else if (gameObject.name.Contains("Spear"))
        {
            upgradeUnit = UnitClass.Spear;
            unitLv = GlobalData.g_UnitSpearLv;
        }
        else if (gameObject.name.Contains("Priest"))
        {
            upgradeUnit = UnitClass.Priest;
            unitLv = GlobalData.g_UnitPriestLv;

        }
        else if (gameObject.name.Contains("Magician"))
        {
            upgradeUnit = UnitClass.Magician;
            unitLv = GlobalData.g_UnitMagicianLv;

        }
        else if (gameObject.name.Contains("Cavalry"))
        {
            upgradeUnit = UnitClass.Cavalry;
            unitLv = GlobalData.g_UnitCarlvryLv;

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
                    Debug.Log($"������ �����ǳ� ��!{unitIdx}");
                    break;
                }
            case (int)UnitClass.Archer:
                {
                    Managers.UI.ShowPopUp<UI_UnitUpgradePopUp>().GetUnitIndex(unitIdx);

                    Debug.Log($"��ó �����ǳ� ��!{unitIdx}");

                    break;
                }
            case (int)UnitClass.Spear:
                {
                    Managers.UI.ShowPopUp<UI_UnitUpgradePopUp>().GetUnitIndex(unitIdx);

                    Debug.Log($"â�� �����ǳ� ��!{unitIdx}");

                    break;
                }

            case (int)UnitClass.Priest:
                {
                    Managers.UI.ShowPopUp<UI_UnitUpgradePopUp>().GetUnitIndex(unitIdx);

                    Debug.Log($"â�� �����ǳ� ��!{unitIdx}");

                    break;
                }

            case (int)UnitClass.Magician:
                {
                    Managers.UI.ShowPopUp<UI_UnitUpgradePopUp>().GetUnitIndex(unitIdx);

                    Debug.Log($"���� �����ǳ� ��!{unitIdx}");

                    break;
                }
            case (int)UnitClass.Cavalry:
                {
                    Managers.UI.ShowPopUp<UI_UnitUpgradePopUp>().GetUnitIndex(unitIdx);

                    Debug.Log($"�⸶�� �����ǳ� ��!{unitIdx}");

                    break;
                }
        }



    }


    public void RefreshUnitImg(int unitLv)
    {
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


        
        lvText.text = $"Lv{unitLv}";
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
