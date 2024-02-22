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
        if(dialogSize == DialogSize.Large)      //���̾�α� ����� �������
            dialogUIGo = this.gameObject.transform.Find("LargeDialoguePanel").gameObject;       //�������̾�α׸� ���ش�.
        else
            dialogUIGo = this.gameObject.transform.Find("SmallDialoguePanel").gameObject;       //�������̾�α׸� ���ش�.

        dialogUIGo.TryGetComponent(out dialogUI);


        if (dialogCo != null)
            StopCoroutine(dialogCo);
        if(dialogArrowCo != null)
            StopCoroutine(dialogArrowCo);
        if (dialogSelectCo != null)
            StopCoroutine(dialogSelectCo);          //��� �ڷ�ƾ�� ���ش�.


        dialogUI.DialogTxt.SetText("");
        click = false;
        node = Managers.Dialog.DialogNodeInit(dialogKey, dialogType);       //Json ��ȭ �����͸� �Ľ��Ͽ� JSONNode�� �޾Ƶд�.
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
        for (int ii = 0; ii < node[dialogKey]["count"]; ii++)  //���̾�α��� �Ѱ�����ŭ �ݺ�
        {
            dialogUI.ShowDialogPanel();                         //���̾�α��ǳ��� ���ش�.
            dialogUI.HideDialogArrow();                         //ȭ��ǥ�� �����.
            if (dialogUI is LargeDialogUI largeDialogUI)            //���� ���̾�αװ� �������̾�α׶��
                largeDialogUI.HideDialogSelectBtns();               //��ư�� �����ش�.
            dialogUI.DialogTxt.text = "";

            if (node[dialogKey]["select" + (ii + 1)] == true)       //�������� �ִٰ� �ϸ�
            {
                int yesKey = node[dialogKey]["yesKey"];         //������ Yes No Ű���� ���ؼ� ���� �޾Ƴ��´�.
                int noKey = node[dialogKey]["noKey"];
                if (dialogUI is LargeDialogUI largeDialog)
                {
                    largeDialog.dialogKeyInit((DialogKey)yesKey, (DialogKey)noKey);     //�������� ���� Ű���� �־��ش�.
                    largeDialog.SetDialogActions(yesDialogAction, noDialogAction);      //�������� ���� �ൿ�Լ��� �־��ش�.
                }

                dialogSelectCo = StartCoroutine(DialogSelectShow());
                yield return dialogSelectCo;        //���ú����� true�� ���ô��̾�α׷�

            }
            else
            {
                dialogArrowCo = StartCoroutine(DialogArrowShow());
                yield return dialogArrowCo;      //false�� �Ϲ� ȭ��ǥ������ ���̾�α׷�

            }
        }

        DialogEnd();  //���̾�αװ� �����鼭 ��� �ൿ���� �Ǵ��ϴ� �Լ�

        IEnumerator DialogArrowShow()           //���̾�α� ������ȭ��ǥ�� ������ �ڷ�ƾ
        {



            float timePerChar = 1 / typingSpeed;
            string text = Managers.Dialog.DialogQueue.Dequeue();        //JSON���� �޾Ƶ� ������ ť�� �޾Ƽ� ���ش�.
            Debug.Log(text);

            float duration = text.Length * timePerChar;             //�ؽ�Ʈ�� ���̸�ŭ �ؽ�ƮŸ������ ���ӵȴ�.

            dialogUI.DialogTxt.DOKill();
            dialogUI.DialogTxt.DOText(text, duration).SetEase(Ease.Linear).OnComplete(()=>
            {
                click = true;       //���̾�α� �ؽ�Ʈ�� ������ Ŭ���� ���ش�.

            });
           

            while (true)
            {

                if (dialogUI.DialogArrowIsActive() == false)   //���̾�α� ȭ��ǥ�� �����ִٸ�   (Ÿ������ ���� �ȳ�������)
                {
                    if (click)      //Ŭ���� �����ٸ�
                    {

                        click = false;      //���ְ�
                        dialogUI.DialogTxt.DOKill();        //�ش� Ʈ���� ���ְ�
                        dialogUI.DialogTxt.SetText(text);       //�ٷ� �ؽ�Ʈ�� ��Ÿ�����Ѵ�.
                        dialogUI.ShowDialogArrow();     //ȭ��ǥ�� �����ش�.
                    }
                }
                else   //���̾�α� ȭ��ǥ�� �����ִٸ�(Ÿ������ ��������)
                {
                    if (click)      //Ŭ���� �����ٸ�
                    {

                        click = false;      //���ְ� ���ѷ��� Ż��
                        
                        yield break;
                    }
                }

                yield return null;
            }
        }

        IEnumerator DialogSelectShow()          //�������϶�
        {
            dialogUI.DialogTxt.text = "";

            float timePerChar = 1 / typingSpeed;
            string text = Managers.Dialog.DialogQueue.Dequeue();   //JSON���� �޾Ƶ� ������ ť�� �޾Ƽ� ���ش�.

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
                    dialogUI.DialogTxt.SetText(text);           //�ؽ�Ʈ�� �ٷ� �����ش�.
                    if (dialogUI is LargeDialogUI largeDialogUI)
                        largeDialogUI.ShowDialogSelectBtns();           //�������� �����ش�.
                    //dialogUI.ShowDialogArrow();
                }

                yield return null;
            }
        }


    }


    protected virtual void DialogEnd()
    {


        dialogUI.HideDialogPanel();
        //���̾�α� ���̵�� ó�� ���̾�α׽����Ҷ� ���� �޾Ƽ� ó��
        if (dialogId == DialogId.DialogMask)
            Managers.Dialog.EndDialog((int)dialogId);   //���̾�α��� ���̵𰪿� ���� ���̾�αװ� �������� �ϴ¿����� �޶���
        else if (dialogId == DialogId.NextDialog)
            Managers.Dialog.EndDialog(dialogKey, (int)dialogId);
        else if (dialogId == DialogId.EndDialog)
            Managers.Dialog.EndDialog();
    }
}
