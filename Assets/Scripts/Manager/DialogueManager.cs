using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager 
{

    public Queue<string> DialogQueue { get; private set; } = new Queue<string>();

    public JSONNode DialogJsonParsing(string key)
    {
        TextAsset txt = Managers.Data.tutorialDialogue[key];
        JSONNode node = JSON.Parse(txt.text);

        return node;

    }


    public void DialogSetting(string key, JSONNode node)
    {
        DialogQueue.Clear();  //���̾�α� �������� ����ִ� ť�� �ʱ�ȭ���ش�.
        int count = node[key]["count"];
        if (count <= 0)        //���̾�α��� ������ ���ٸ�
            return;

        for(int ii = 1; ii <= count; ii++)
        {
            DialogQueue.Enqueue(node[key]["word" + ii]);  //�ܾ����ŭ ť�� �־��ش�.
        }




    }
}