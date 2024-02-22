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
        if(dialogSize == DialogSize.Large)      //다이얼로그 사이즈가 라지라면
            dialogUIGo = this.gameObject.transform.Find("LargeDialoguePanel").gameObject;       //라지다이얼로그를 켜준다.
        else
            dialogUIGo = this.gameObject.transform.Find("SmallDialoguePanel").gameObject;       //스몰다이얼로그를 켜준다.

        dialogUIGo.TryGetComponent(out dialogUI);


        if (dialogCo != null)
            StopCoroutine(dialogCo);
        if(dialogArrowCo != null)
            StopCoroutine(dialogArrowCo);
        if (dialogSelectCo != null)
            StopCoroutine(dialogSelectCo);          //모든 코루틴을 꺼준다.


        dialogUI.DialogTxt.SetText("");
        click = false;
        node = Managers.Dialog.DialogNodeInit(dialogKey, dialogType);       //Json 대화 데이터를 파싱하여 JSONNode로 받아둔다.
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
            dialogUI.ShowDialogPanel();                         //다이얼로그판넬을 켜준다.
            dialogUI.HideDialogArrow();                         //화살표를 감춘다.
            if (dialogUI is LargeDialogUI largeDialogUI)            //만약 다이얼로그가 라지다이얼로그라면
                largeDialogUI.HideDialogSelectBtns();               //버튼을 숨겨준다.
            dialogUI.DialogTxt.text = "";

            if (node[dialogKey]["select" + (ii + 1)] == true)       //선택지가 있다고 하면
            {
                int yesKey = node[dialogKey]["yesKey"];         //선택지 Yes No 키값을 통해서 값을 받아놓는다.
                int noKey = node[dialogKey]["noKey"];
                if (dialogUI is LargeDialogUI largeDialog)
                {
                    largeDialog.dialogKeyInit((DialogKey)yesKey, (DialogKey)noKey);     //선택지에 대한 키값을 넣어준다.
                    largeDialog.SetDialogActions(yesDialogAction, noDialogAction);      //선택지에 대한 행동함수를 넣어준다.
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

        DialogEnd();  //다이얼로그가 끝나면서 어떻게 행동할지 판단하는 함수

        IEnumerator DialogArrowShow()           //다이얼로그 마무리화살표가 나오는 코루틴
        {



            float timePerChar = 1 / typingSpeed;
            string text = Managers.Dialog.DialogQueue.Dequeue();        //JSON으로 받아둔 값들을 큐로 받아서 빼준다.
            Debug.Log(text);

            float duration = text.Length * timePerChar;             //텍스트의 길이만큼 텍스트타이핑이 지속된다.

            dialogUI.DialogTxt.DOKill();
            dialogUI.DialogTxt.DOText(text, duration).SetEase(Ease.Linear).OnComplete(()=>
            {
                click = true;       //다이얼로그 텍스트가 끝나면 클릭을 켜준다.

            });
           

            while (true)
            {

                if (dialogUI.DialogArrowIsActive() == false)   //다이얼로그 화살표가 꺼져있다면   (타이핑이 아직 안끝난시점)
                {
                    if (click)      //클릭이 켜졌다면
                    {

                        click = false;      //꺼주고
                        dialogUI.DialogTxt.DOKill();        //해당 트윈을 꺼주고
                        dialogUI.DialogTxt.SetText(text);       //바로 텍스트를 나타나게한다.
                        dialogUI.ShowDialogArrow();     //화살표를 보여준다.
                    }
                }
                else   //다이얼로그 화살표가 켜져있다면(타이핑이 끝난시점)
                {
                    if (click)      //클릭이 켜졌다면
                    {

                        click = false;      //꺼주고 무한루프 탈출
                        
                        yield break;
                    }
                }

                yield return null;
            }
        }

        IEnumerator DialogSelectShow()          //선택지일때
        {
            dialogUI.DialogTxt.text = "";

            float timePerChar = 1 / typingSpeed;
            string text = Managers.Dialog.DialogQueue.Dequeue();   //JSON으로 받아둔 값들을 큐로 받아서 빼준다.

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
                    dialogUI.DialogTxt.SetText(text);           //텍스트를 바로 보여준다.
                    if (dialogUI is LargeDialogUI largeDialogUI)
                        largeDialogUI.ShowDialogSelectBtns();           //선택지를 보여준다.
                    //dialogUI.ShowDialogArrow();
                }

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
