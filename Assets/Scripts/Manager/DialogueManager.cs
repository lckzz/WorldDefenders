using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager 
{

    public Queue<string> DialogQueue { get; private set; } = new Queue<string>();

    public JSONNode DialogJsonParsing(string key, Define.DialogType dialogType)
    {
        TextAsset txt = null;
        if (dialogType == Define.DialogType.Tutorial)
        {
            txt = Managers.Data.tutorialDialogue[key];
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
        }




    }
}
