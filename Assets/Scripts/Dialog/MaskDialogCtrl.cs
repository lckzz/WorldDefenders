using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MaskDialogCtrl : DialogueCtrl
{
    private TutorialMaskCtrl tutorialMaskCtrl;
    private Action<GameObject> GameObjectSiblingSet = null;
    private GameObject backObj;
    private float dialogYPos = -32.5f;
    DialogKey dialogKeyType;

    Dictionary<DialogKey, Action> endDialogActionDict;
    Dictionary<DialogKey, Action<int>> endDialogActionIntDict;

    Action endDialogAction = null;
    Action<int> endDialogActionInt = null;

    // Start is called before the first frame update
    void Start()
    {

        //�̾�� ���̾�α׸� ��Ƶ� ��ųʸ�
        endDialogActionDict = new Dictionary<DialogKey, Action>
        {
            {DialogKey.tutorialUpgrade,NextDialog },
            {DialogKey.tutorialUpgradeTower,NextDialog },
            {DialogKey.tutorialParty,NextDialog },
            {DialogKey.tutorialPartyUnit,NextDialog },
            {DialogKey.tutorialSkill,NextDialog },
            {DialogKey.tutorialSkillTree,NextDialog }


        };



        //���̾�α��� �������κ��� ��Ƶ� ��ųʸ�
        endDialogActionIntDict = new Dictionary<DialogKey, Action<int>>
        {
            {DialogKey.tutorialUpgradeUnit,EndDialog },
            {DialogKey.tutorialPartyWindow,EndDialog },
            {DialogKey.tutorialSkillInfo,EndDialog }


        };

    }

    public void MaskDialogInit(TutorialMaskCtrl tutorialMask,GameObject backGo,Action<GameObject> goAction)
    {
        tutorialMaskCtrl = tutorialMask;
        backObj = backGo;
        GameObjectSiblingSet -= goAction;
        GameObjectSiblingSet += goAction;



    }


    private void NextDialog()
    {
        dialogKeyType += 1;
        StartDialog(dialogKeyType.ToString(), DialogType.Dialog, DialogSize.Small, DialogId.NextDialog);

    }

    private void EndDialog(int idx)
    {
        tutorialMaskCtrl.ShowMaskObject(idx);
        GameObjectSiblingSet(backObj);

        Debug.Log(tutorialMaskCtrl);

    }


    protected override void DialogEnd()
    {
        dialogUI.HideDialogPanel();


        if ((int)dialogId == (int)DialogId.NextDialog)
        {
            dialogKeyType = (DialogKey)Enum.Parse(typeof(DialogKey), this.dialogKey);

            if (endDialogActionDict.TryGetValue(dialogKeyType,out endDialogAction) == false)
            {

                if (endDialogActionIntDict.TryGetValue(dialogKeyType, out endDialogActionInt))
                {
                    endDialogActionInt((int)Define.DialogMask.BackMask);
                    return;

                }
            }

            else
            {
                endDialogAction();
            }


            //if (dialogKeyType >= DialogKey.tutorialUpgrade && dialogKeyType < DialogKey.tutorialUpgradeUnit)
            //{
            //    //���׷��̵����� (���׷��̵� ���̾�α״� ���ҵǾ�����)
            //    dialogKeyType += 1;
            //    StartDialog(dialogKeyType.ToString(), DialogType.Dialog, DialogSize.Small, DialogId.NextDialog);
            //}
            //else if (dialogKeyType == DialogKey.tutorialUpgradeUnit)
            //{
            //    //Ʃ�丮����׷��̵尡 ������ �����ϸ� �������̶� 
            //    tutorialMaskCtrl.ShowMaskObject((int)Define.DialogMask.BackMask);
            //    GameObjectSiblingSet(upgradeBackGo);
            //    return;

            //}


            switch (dialogKeyType)
            {
                case DialogKey.tutorialUpgradeTower:
                    tutorialMaskCtrl.ShowMaskObject((int)Define.DialogMask.FirstMask);

                    DialogPosReset();

                    break;

                case DialogKey.tutorialUpgradeUnit:
                    tutorialMaskCtrl.ShowMaskObject((int)Define.DialogMask.SecondMask);
                    ChangeDialogPos(dialogYPos);

                    break;

                case DialogKey.tutorialPartyUnit:
                    tutorialMaskCtrl.ShowMaskObject((int)Define.DialogMask.FirstMask);
                    break;

                case DialogKey.tutorialPartyWindow:
                    tutorialMaskCtrl.ShowMaskObject((int)Define.DialogMask.SecondMask);
                    break;

                case DialogKey.tutorialSkillTree:
                    tutorialMaskCtrl.ShowMaskObject((int)Define.DialogMask.FirstMask);
                    break;

                case DialogKey.tutorialSkillInfo:
                    tutorialMaskCtrl.ShowMaskObject((int)Define.DialogMask.SecondMask);
                    break;

            }

        }
    }


 
}
