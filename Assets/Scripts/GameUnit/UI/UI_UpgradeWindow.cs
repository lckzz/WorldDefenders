using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeWindow : UI_Base
{
    [Header("-------PlayerUI-------------")]
    [SerializeField] private TextMeshProUGUI playerLvTxt;
    [SerializeField] private TextMeshProUGUI playerHpTxt;
    [SerializeField] private TextMeshProUGUI playerAttTxt;
    [SerializeField] private Button upgradeBtn;

    [Header("-------UnitsUI-----------")]
    public Button[] unitUpgradeBtn;
    [SerializeField] private TextMeshProUGUI warriorLvTxt;
    [SerializeField] private TextMeshProUGUI archerLvTxt;
    [SerializeField] private TextMeshProUGUI spearLvTxt;


    [SerializeField] private Animator[] unitAnim;

    [SerializeField] private AnimatorController[] warriorAnims;
    [SerializeField] private AnimatorController[] archerAnims;
    [SerializeField] private AnimatorController[] spearAnims;




    [Space(25)]

    [SerializeField] private Button backLobbyBtn;

    
    TowerStat tower = new TowerStat();

    private UnitClass unitClass;




    // Start is called before the first frame update
    void Start()
    {
        if (GlobalData.g_PlayerLevel == 0)
            GlobalData.g_PlayerLevel = 1;

        if (upgradeBtn != null)
            upgradeBtn.onClick.AddListener(PlayerUpgradeOpen);

        if (backLobbyBtn != null)
            backLobbyBtn.onClick.AddListener(() =>
            {
                Managers.Sound.Play("Effect/UI_Click");

                Managers.UI.ClosePopUp(this);
                Managers.UI.ShowPopUp<UI_Lobby>();

            });
        WindowInit();
        UnitButtonEvent(unitUpgradeBtn[(int)UnitClass.Warrior].gameObject,(int)UnitClass.Warrior, OpenUpgradeUnitPopUp, UIEvnet.PointerDown);
        UnitButtonEvent(unitUpgradeBtn[(int)UnitClass.Archer].gameObject, (int)UnitClass.Archer, OpenUpgradeUnitPopUp, UIEvnet.PointerDown);
        UnitButtonEvent(unitUpgradeBtn[(int)UnitClass.Spear].gameObject, (int)UnitClass.Spear, OpenUpgradeUnitPopUp, UIEvnet.PointerDown);
    }

    // Update is called once per frame
    void Update()
    {
        RefreshTextUI();        //매 프레임마다 갱신하지말고 콜백함수를 통해서 값이 변경되면 콜백함수를 통해서 갱신되게
        
    }



    void WindowInit()
    {
        RefreshUnitImgAnim(GlobalData.g_UnitWarriorLv, GlobalData.g_UnitArcherLv, GlobalData.g_UnitSpearLv);
    }



    void UnitButtonEvent(GameObject obj, int idx ,Action<int> action = null, UIEvnet type = UIEvnet.PointerDown)
    {
        UI_EventHandler evt;
        obj.TryGetComponent(out evt);

        if(type == UIEvnet.PointerDown)
        {
            evt.OnPointerDownIntHandler -= (unUsedIdx) => action(idx);
            evt.OnPointerDownIntHandler += (unUsedIdx) => action(idx);

        }
    }


    void RefreshTextUI()
    {
        tower = Managers.Data.towerDict[GlobalData.g_PlayerLevel];
        playerLvTxt.text = $"Lv {tower.level}";
        playerHpTxt.text = tower.hp.ToString();
        playerAttTxt.text = tower.att.ToString();
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


    public void RefreshUnitImgAnim(int unitWarriorLv,int unitArcherLv, int unitSpearLv)
    {
        for(int ii = 0; ii < unitAnim.Length; ii++)
        {
            if(ii == 0)
            {
                if (unitWarriorLv < 5)
                    unitAnim[ii].runtimeAnimatorController = warriorAnims[0];
                else
                    unitAnim[ii].runtimeAnimatorController = warriorAnims[1];

                warriorLvTxt.text = $"Lv{unitWarriorLv}";
            }
            else if( ii == 1)
            {
                if (unitArcherLv < 5)
                    unitAnim[ii].runtimeAnimatorController = archerAnims[0];
                else
                    unitAnim[ii].runtimeAnimatorController = archerAnims[1];
                archerLvTxt.text = $"Lv{unitArcherLv}";

            }
            else
            {
                if (unitSpearLv < 5)
                    unitAnim[ii].runtimeAnimatorController = spearAnims[0];
                else
                    unitAnim[ii].runtimeAnimatorController = spearAnims[1];
                spearLvTxt.text = $"Lv{unitSpearLv}";

            }


        }

    }


    void PlayerUpgradeOpen()
    {
        Managers.Sound.Play("Effect/UI_Click");

        Managers.UI.ShowPopUp<UI_PlayerUpgradePopUp>();
    }
}
