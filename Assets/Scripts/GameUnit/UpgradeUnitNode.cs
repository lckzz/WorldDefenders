using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum UpgradeNode
{
    Lv1,
    Lv2,
    Lv3,
    LvTxt,
    UpgradeBtn,
    Count
}

public class UpgradeUnitNode : MonoBehaviour
{
    private string className = "";
    [SerializeField] private GameObject[] classObj = new GameObject[3];
    [SerializeField] private TextMeshProUGUI lvText;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private UnitClass upgradeUnit;

    private int unitLv = 0;

    // Start is called before the first frame update
    void Start()
    {
        Init();

        Debug.Log(classObj[0]);
    }

    // Update is called once per frame


    void Init()
    {
        className = this.gameObject.name;
        for(int ii = 0; ii< classObj.Length; ii++)
        {
            if(classObj[ii] == null)
                classObj[ii] = this.transform.GetChild(ii).gameObject;

        }


        UpgradeUnitInit();



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
        else if (gameObject.name.Contains("Magician"))
        {
            upgradeUnit = UnitClass.Magician;
            unitLv = GlobalData.g_UnitMagicianLv;

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
        }



    }


    public void RefreshUnitImg(int unitLv)
    {

        if (unitLv < 5)
        {
            classObj[(int)Define.UnitUILv.One].SetActive(true);
            classObj[(int)Define.UnitUILv.Two].SetActive(false);
            classObj[(int)Define.UnitUILv.Three].SetActive(false);

        }

        else
        {
            Debug.Log(classObj[0]);
            Debug.Log(classObj[1]);

            classObj[(int)Define.UnitUILv.Two].SetActive(true);
            classObj[(int)Define.UnitUILv.One].SetActive(false);
            classObj[(int)Define.UnitUILv.Three].SetActive(false);

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
