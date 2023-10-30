using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpgradeNotice : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldTxt;
    [SerializeField] private Button yesBtn;
    [SerializeField] private Button noBtn;

    // Start is called before the first frame update
    void Start()
    {
        if (yesBtn != null)
            yesBtn.onClick.AddListener(() =>
            {
                if(Managers.UI.PeekPopupUI<UI_UnitUpgradePopUp>())
                    Managers.UI.PeekPopupUI<UI_UnitUpgradePopUp>().UpgradeUnit();
                else if(Managers.UI.PeekPopupUI<UI_PlayerUpgradePopUp>())
                    Managers.UI.PeekPopupUI<UI_PlayerUpgradePopUp>().Upgrade();

                ParentGameObjectOff();

            });

        if (noBtn != null)
            noBtn.onClick.AddListener(() =>
            {
                ParentGameObjectOff();
            });
    }



    void ParentGameObjectOff()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
