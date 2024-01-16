using System;
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

    public Action yesClickAction = null;
    public Action noClickAction = null;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (dialogSelectYesBtn != null)
            dialogSelectYesBtn.onClick.AddListener(() =>
            {
                OnYesBtnClick();
                dialogPanelObj.SetActive(false);
                HideDialogSelectBtns();
                dialogCtrl.StartDialog(yesDialogKey.ToString(), DialogType.Dialog,DialogSize.Large,DialogId.DialogMask,DialogOrder.Upgrade);
            });


        if (dialogSelectNoBtn != null)
            dialogSelectNoBtn.onClick.AddListener(() =>
            {
                OnNoBtnClick();           
                dialogPanelObj.SetActive(false);

                dialogCtrl.StartDialog(noDialogKey.ToString(), DialogType.Dialog, DialogSize.Large);

            });
    }

    public void dialogKeyInit(DialogKey yesDialogKey,DialogKey noDialogKey)
    {
        this.yesDialogKey = yesDialogKey;
        this.noDialogKey = noDialogKey;

    }

    public void OnYesBtnClick()
    {
        if (yesClickAction != null)
            yesClickAction.Invoke();
    }

    public void OnNoBtnClick()
    {
        if (noClickAction != null)
            noClickAction.Invoke();
    }

    public void SetDialogActions(Action yesAction , Action noAction)
    {
        yesClickAction = yesAction;
        noClickAction = noAction;
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
