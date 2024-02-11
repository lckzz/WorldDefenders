using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpgradeNotice : MonoBehaviour
{
    [SerializeField] private Button yesBtn;
    [SerializeField] private Button noBtn;
    [SerializeField] private Button checkBtn;


    [SerializeField] private GameObject selectObj;
    [SerializeField] private GameObject buyFailObj;

    int upgradeGold = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (yesBtn != null)
            yesBtn.onClick.AddListener(() =>
            {
                if (upgradeGold > Managers.Game.Gold)
                {
                    buyFailObj.SetActive(true);
                    selectObj.SetActive(false);
                }

                else
                {
                    if (Managers.UI.PeekPopupUI<UI_UnitUpgradePopUp>())
                    {
                        Managers.Sound.Play("Effect/Upgrade");
                        Managers.UI.PeekPopupUI<UI_UnitUpgradePopUp>().UpgradeUnit();
                        Managers.UI.PeekPopupUI<UI_UnitUpgradePopUp>().LevelUpParticleOn();

                    }
                    else if (Managers.UI.PeekPopupUI<UI_PlayerUpgradePopUp>())
                    {
                        Managers.Sound.Play("Effect/Upgrade");
                        Managers.UI.PeekPopupUI<UI_PlayerUpgradePopUp>().Upgrade();
                        Managers.UI.PeekPopupUI<UI_PlayerUpgradePopUp>().LevelUpParticleOn();


                    }

                    Managers.Game.Gold -= upgradeGold;
                    Managers.UI.FindPopup<UI_UpgradeWindow>().RefreshTextUI();
                    Managers.Game.FileSave();       //업그레이드시 저장
                    ParentGameObjectOff();
                }



            });

        if (noBtn != null)
            noBtn.onClick.AddListener(ParentGameObjectOff);


        
        if (checkBtn != null)
            checkBtn.onClick.AddListener(() =>
            {
                ParentGameObjectOff();
                buyFailObj.SetActive(false);
                selectObj.SetActive(true);
            });
    }



    void ParentGameObjectOff()
    {
        Managers.Sound.Play("Effect/UI_Click");
        transform.parent.gameObject.SetActive(false);
    }

    public void SetUpgradeGold(int upgradeGold)
    {
        this.upgradeGold = upgradeGold;
    }
}
