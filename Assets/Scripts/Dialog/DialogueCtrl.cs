using DG.Tweening;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class DialogueCtrl : MonoBehaviour
{
    protected BaseDialogueUI dialogUI;
    private GameObject dialogUIGo;

    public bool click = false;

    protected string dialogKey;
    private JSONNode node;

    private float typingSpeed = 25f;

    private Coroutine dialogCo;
    private Coroutine dialogArrowCo;
    private Coroutine dialogSelectCo;
    protected DialogId dialogId;
    private DialogOrder dialogOrder;
    private DialogKey dialogKeyEnum;

    private Action yesDialogAction = null;
    private Action noDialogAction = null;



    public void ChangeDialogPos(float yPos)
    {
        if(dialogUI != null)
        {
            if (dialogUI is SmallDialogUI smallDialogUI)
                smallDialogUI.DialogPositionChange(yPos);
        }    
    }

    public void DialogPosReset()
    {
        if (dialogUI != null)
        {
            if (dialogUI is SmallDialogUI smallDialogUI)
                smallDialogUI.DialogPositionReset();
        }
    }

    public void YesSelectDialogInit(Action yesSelectDialog)
    {
        yesDialogAction -= yesSelectDialog;
        yesDialogAction += yesSelectDialog;
    }

    public void NoSelectDialogInit(Action noSelectDialog)
    {
        noDialogAction -= noSelectDialog;
        noDialogAction += noSelectDialog;
    }


    public void StartDialog(string dialogKey,DialogType dialogType,DialogSize dialogSize,DialogId dialogId = DialogId.None,DialogOrder dialogOrder = DialogOrder.None)
    {
        if(dialogSize == DialogSize.Large)
            dialogUIGo = this.gameObject.transform.Find("LargeDialoguePanel").gameObject;
        else
            dialogUIGo = this.gameObject.transform.Find("SmallDialoguePanel").gameObject;

        dialogUIGo.TryGetComponent(out dialogUI);


        if (dialogCo != null)
            StopCoroutine(dialogCo);
        if(dialogArrowCo != null)
            StopCoroutine(dialogArrowCo);
        if (dialogSelectCo != null)
            StopCoroutine(dialogSelectCo);


        dialogUI.DialogTxt.SetText("");
        click = false;
        node = Managers.Dialog.DialogNodeInit(dialogKey, dialogType);
        this.dialogKey = dialogKey;
        this.dialogId = dialogId;
        this.dialogOrder = dialogOrder;

        dialogCo = StartCoroutine(StartDialogCo());

    }

    //void Update()
    //{
    //    if(Input.GetMouseButtonDown(0))
    //    {
    //        click = true;
    //        Managers.Sound.Play("Effect/UI_Click");

    //    }
    //}

    IEnumerator StartDialogCo()
    {

        click = false;
        for (int ii = 0; ii < node[dialogKey]["count"]; ii++)  //다이얼로그의 총갯수만큼 반복
        {
            dialogUI.ShowDialogPanel();
            dialogUI.HideDialogArrow();
            if (dialogUI is LargeDialogUI largeDialogUI)
                largeDialogUI.HideDialogSelectBtns();
            dialogUI.DialogTxt.text = "";

            if (node[dialogKey]["select" + (ii + 1)] == true)       //선택지가 있다고 하면
            {
                int yesKey = node[dialogKey]["yesKey"];         //선택지 Yes No 키값을 통해서 값을 받아놓는다.
                int noKey = node[dialogKey]["noKey"];
                if (dialogUI is LargeDialogUI largeDialog)
                {
                    largeDialog.dialogKeyInit((DialogKey)yesKey, (DialogKey)noKey);
                    largeDialog.SetDialogActions(yesDialogAction, noDialogAction);
                }

                dialogSelectCo = StartCoroutine(DialogSelectShow());
                yield return dialogSelectCo;        //선택변수가 true면 선택다이얼로그로

            }
            else
            {
                dialogArrowCo = StartCoroutine(DialogArrowShow());
                yield return dialogArrowCo;      //false면 일반 화살표나오는 다이얼로그로

            }
        }

        DialogEnd();

        IEnumerator DialogArrowShow()
        {



            float timePerChar = 1 / typingSpeed;
            string text = Managers.Dialog.DialogQueue.Dequeue();
            Debug.Log(text);

            float duration = text.Length * timePerChar;

            dialogUI.DialogTxt.DOKill();
            dialogUI.DialogTxt.DOText(text, duration).SetEase(Ease.Linear).OnComplete(()=>
            {
                click = true;

            });
           

            while (true)
            {

                if (dialogUI.DialogArrowIsActive() == false)   //다이얼로그 화살표가 꺼져있다면
                {
                    if (click)
                    {

                        click = false;
                        dialogUI.DialogTxt.DOKill();
                        dialogUI.DialogTxt.SetText(text);
                        dialogUI.ShowDialogArrow();
                    }
                }
                else   //다이얼로그 화살표가 켜져있다면
                {
                    if (click)
                    {

                        click = false;
                        
                        yield break;
                    }
                }

                yield return null;
            }
        }

        IEnumerator DialogSelectShow()
        {



            dialogUI.DialogTxt.text = "";

            float timePerChar = 1 / typingSpeed;
            string text = Managers.Dialog.DialogQueue.Dequeue();

            float duration = text.Length * timePerChar;

            dialogUI.DialogTxt.DOText(text, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                click = true;
            });
            while (true)
            {
                if (click)
                {
                    click = false;
                    dialogUI.DialogTxt.DOKill();
                    dialogUI.DialogTxt.SetText(text);
                    if (dialogUI is LargeDialogUI largeDialogUI)
                        largeDialogUI.ShowDialogSelectBtns();
                    //dialogUI.ShowDialogArrow();
                }
                //else
                //{
                //    if (dialogUI.DialogArrowIsActive())      //다이얼로그 화살표가 켜졌다면
                //    {
                //        if (click)
                //        {
                //            click = false;
                //            yield break;
                //        }
                //    }
                //}

                yield return null;
            }
        }


    }


    protected virtual void DialogEnd()
    {


        dialogUI.HideDialogPanel();
        //다이얼로그 아이디는 처음 다이얼로그시작할때 값을 받아서 처리
        if (dialogId == DialogId.DialogMask)
            Managers.Dialog.EndDialog((int)dialogId);   //다이얼로그의 아이디값에 따라서 다이얼로그가 끝낫을때 하는역할이 달라짐
        else if (dialogId == DialogId.NextDialog)
            Managers.Dialog.EndDialog(dialogKey, (int)dialogId);
        else if (dialogId == DialogId.EndDialog)
            Managers.Dialog.EndDialog();
    }
}
