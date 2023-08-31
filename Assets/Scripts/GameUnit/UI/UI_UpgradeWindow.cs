using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Animations;
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



    [SerializeField] private GameObject[] warriorPrefabs;
    [SerializeField] private GameObject[] archerPrefabs;
    [SerializeField] private GameObject[] spearPrefabs;

    [SerializeField] private Image fadeImg;




    [Space(25)]

    [SerializeField] private Button backLobbyBtn;

    
    TowerStat tower = new TowerStat();

    private UnitClass unitClass;

    bool backFadeCheck = false;

    public bool StartFadeCheck { get { return startFadeOut; } }



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

        startFadeOut = true;

        WindowInit();
        UnitButtonEvent(unitUpgradeBtn[(int)UnitClass.Warrior].gameObject,(int)UnitClass.Warrior, OpenUpgradeUnitPopUp, UIEvent.PointerDown);
        UnitButtonEvent(unitUpgradeBtn[(int)UnitClass.Archer].gameObject, (int)UnitClass.Archer, OpenUpgradeUnitPopUp, UIEvent.PointerDown);
        UnitButtonEvent(unitUpgradeBtn[(int)UnitClass.Spear].gameObject, (int)UnitClass.Spear, OpenUpgradeUnitPopUp, UIEvent.PointerDown);
    }

    // Update is called once per frame
    void Update()
    {
        Util.FadeOut(ref startFadeOut, fadeImg);
        //BackFadeIn(fadeImg, this, backFadeCheck);
        RefreshTextUI();        //매 프레임마다 갱신하지말고 콜백함수를 통해서 값이 변경되면 콜백함수를 통해서 갱신되게
        
    }



    void WindowInit()
    {
        RefreshUnitImgAnim(GlobalData.g_UnitWarriorLv, GlobalData.g_UnitArcherLv, GlobalData.g_UnitSpearLv);
    }



    void UnitButtonEvent(GameObject obj, int idx ,Action<int> action = null, UIEvent type = UIEvent.PointerDown)
    {
        UI_EventHandler evt;
        obj.TryGetComponent(out evt);

        if(type == UIEvent.PointerDown)
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
        for(int ii = 0; ii < (int)Define.UnitUILv.Count; ii++)
        {
            if (ii == 0)
            {
                if (unitWarriorLv < 5)
                {
                    warriorPrefabs[(int)Define.UnitUILv.One].SetActive(true);
                    warriorPrefabs[(int)Define.UnitUILv.Two].SetActive(false);
                    warriorPrefabs[(int)Define.UnitUILv.Three].SetActive(false);

                }
                
                else
                {
                    warriorPrefabs[(int)Define.UnitUILv.One].SetActive(false);
                    warriorPrefabs[(int)Define.UnitUILv.Two].SetActive(true);
                    warriorPrefabs[(int)Define.UnitUILv.Three].SetActive(false);

                }

                warriorLvTxt.text = $"Lv{unitWarriorLv}";
            }
            else if (ii == 1)
            {
                if (unitArcherLv < 5)
                {
                    archerPrefabs[(int)Define.UnitUILv.One].SetActive(true);
                    archerPrefabs[(int)Define.UnitUILv.Two].SetActive(false);
                    archerPrefabs[(int)Define.UnitUILv.Three].SetActive(false);

                }

                else
                {
                    archerPrefabs[(int)Define.UnitUILv.One].SetActive(false);
                    archerPrefabs[(int)Define.UnitUILv.Two].SetActive(true);
                    archerPrefabs[(int)Define.UnitUILv.Three].SetActive(false);

                }
                archerLvTxt.text = $"Lv{unitArcherLv}";

            }
            else
            {
                if (unitSpearLv < 5)
                {
                    spearPrefabs[(int)Define.UnitUILv.One].SetActive(true);
                    spearPrefabs[(int)Define.UnitUILv.Two].SetActive(false);
                    spearPrefabs[(int)Define.UnitUILv.Three].SetActive(false);

                }

                else
                {
                    spearPrefabs[(int)Define.UnitUILv.One].SetActive(false);
                    spearPrefabs[(int)Define.UnitUILv.Two].SetActive(true);
                    spearPrefabs[(int)Define.UnitUILv.Three].SetActive(false);

                }
                spearLvTxt.text = $"Lv{unitSpearLv}";

            }


        }

    }


    void PlayerUpgradeOpen()
    {
        Managers.Sound.Play("Effect/UI_Click");

        Managers.UI.ShowPopUp<UI_PlayerUpgradePopUp>();
    }

    public void BackFadeIn(Image fadeImg, UI_Base closePopup, bool fadeCheck)
    {
        if (fadeCheck)
        {
            if (fadeImg != null)
            {
                if (!fadeImg.gameObject.activeSelf)
                {
                    fadeImg.gameObject.SetActive(true);

                }

                if (fadeImg.gameObject.activeSelf && fadeImg.color.a <= 1)
                {
                    Color col = fadeImg.color;
                    if (col.a < 255)
                        col.a += (Time.deltaTime * 2.0f);

                    fadeImg.color = col;


                    if (fadeImg.color.a >= 0.99f)
                    {
                        Managers.UI.ClosePopUp(closePopup);
                        Managers.UI.ShowPopUp<UI_Lobby>();


                    }
                }
            }
        }

    }
}
