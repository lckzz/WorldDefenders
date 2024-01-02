using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class LargeDialogUI : BaseDialogueUI
{
    [SerializeField] private GameObject dialogSelectBtnsGo;
    [SerializeField] private Button dialogSelectYesBtn;
    [SerializeField] private Button dialogSelectNoBtn;

    private DialogKey yesDialogKey;
    private DialogKey noDialogKey;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (dialogSelectYesBtn != null)
            dialogSelectYesBtn.onClick.AddListener(() =>
            {
                dialogPanelObj.SetActive(false);
                HideDialogSelectBtns();
                dialogCtrl.StartDialog(yesDialogKey.ToString(), DialogType.Dialog,DialogSize.Large,DialogId.DialogMask,DialogOrder.Upgrade);
            });


        if (dialogSelectNoBtn != null)
            dialogSelectNoBtn.onClick.AddListener(() =>
            {
                dialogPanelObj.SetActive(false);

                dialogCtrl.StartDialog(noDialogKey.ToString(), DialogType.Dialog, DialogSize.Large);

            });
    }

    public void dialogKeyInit(DialogKey yesDialogKey,DialogKey noDialogKey)
    {
        this.yesDialogKey = yesDialogKey;
        this.noDialogKey = noDialogKey;

    }


    public void ShowDialogSelectBtns()
    {
        dialogSelectBtnsGo.SetActive(true);
    }

    public void HideDialogSelectBtns()
    {
        dialogSelectBtnsGo.SetActive(false);
    }
}
