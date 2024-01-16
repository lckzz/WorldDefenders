using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager 
{

    private JSONNode node;

    public event Action<string,int> dialogEndedStringInt = null;
    public event Action<int> dialogEndedInt = null;
    public event Action dialogEnded = null;

    public Queue<string> DialogQueue { get; private set; } = new Queue<string>();

    public JSONNode DialogJsonParsing(Define.DialogType dialogType)
    {
        TextAsset txt = null;
        string key = dialogType.ToString();
        if (dialogType == Define.DialogType.Dialog)
        {
            txt = Managers.Data.dialogue[dialogType.ToString()];
        }
        else if(dialogType == Define.DialogType.Speech)
        {
            txt = Managers.Data.speechDialogue[key];
        }

        JSONNode node = JSON.Parse(txt.text);

        return node;

    }


    public void DialogSetting(string key, JSONNode node)
    {
        DialogQueue.Clear();  //다이얼로그 셋팅전에 들어있던 큐를 초기화해준다.
        int count = node[key]["count"];
        if (count <= 0)        //다이얼로그의 갯수가 없다면
            return;

        for(int ii = 1; ii <= count; ii++)
        {
            DialogQueue.Enqueue(node[key]["word" + ii]);  //단어갯수만큼 큐에 넣어준다.
            Debug.Log(node[key]["word" + ii]);
        }
    }

    public JSONNode DialogNodeInit(string dialogKey, Define.DialogType dialogType)
    {
        node = DialogJsonParsing(dialogType);
        DialogSetting(dialogKey, node);
        return node;
    }

    //public void EndDialog(int id,int order = -1)          //로비 튜토리얼 다이얼로그가 끝났을때 하는 행동
    //{
    //    if(Managers.Game.TutorialEnd == false && dialogEndedIntInt != null)      //튜토리얼이 끝나지않았다면
    //        dialogEndedIntInt?.Invoke(id, order);        

    //}

    public void EndDialog(int id)          //다이얼로그가 끝났을때 하는 행동
    {

        if (Managers.Game.TutorialEnd == false && dialogEndedInt != null)      //튜토리얼이 끝나지않았다면
            dialogEndedInt?.Invoke(id);

    }

    public void EndDialog(string key,int id)          //다이얼로그가 끝났을때 하는 행동
    {

        if (Managers.Game.TutorialEnd == false && dialogEndedStringInt != null)      //튜토리얼이 끝나지않았다면
            dialogEndedStringInt?.Invoke(key, id);

    }

    public void EndDialog()          //다이얼로그가 끝났을때 하는 행동
    {

        if (Managers.Game.TutorialEnd == false && dialogEnded != null)      //튜토리얼이 끝나지않았다면
            dialogEnded?.Invoke();

    }




}
