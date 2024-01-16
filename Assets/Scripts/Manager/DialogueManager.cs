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
        DialogQueue.Clear();  //���̾�α� �������� ����ִ� ť�� �ʱ�ȭ���ش�.
        int count = node[key]["count"];
        if (count <= 0)        //���̾�α��� ������ ���ٸ�
            return;

        for(int ii = 1; ii <= count; ii++)
        {
            DialogQueue.Enqueue(node[key]["word" + ii]);  //�ܾ����ŭ ť�� �־��ش�.
            Debug.Log(node[key]["word" + ii]);
        }
    }

    public JSONNode DialogNodeInit(string dialogKey, Define.DialogType dialogType)
    {
        node = DialogJsonParsing(dialogType);
        DialogSetting(dialogKey, node);
        return node;
    }

    //public void EndDialog(int id,int order = -1)          //�κ� Ʃ�丮�� ���̾�αװ� �������� �ϴ� �ൿ
    //{
    //    if(Managers.Game.TutorialEnd == false && dialogEndedIntInt != null)      //Ʃ�丮���� �������ʾҴٸ�
    //        dialogEndedIntInt?.Invoke(id, order);        

    //}

    public void EndDialog(int id)          //���̾�αװ� �������� �ϴ� �ൿ
    {

        if (Managers.Game.TutorialEnd == false && dialogEndedInt != null)      //Ʃ�丮���� �������ʾҴٸ�
            dialogEndedInt?.Invoke(id);

    }

    public void EndDialog(string key,int id)          //���̾�αװ� �������� �ϴ� �ൿ
    {

        if (Managers.Game.TutorialEnd == false && dialogEndedStringInt != null)      //Ʃ�丮���� �������ʾҴٸ�
            dialogEndedStringInt?.Invoke(key, id);

    }

    public void EndDialog()          //���̾�αװ� �������� �ϴ� �ൿ
    {

        if (Managers.Game.TutorialEnd == false && dialogEnded != null)      //Ʃ�丮���� �������ʾҴٸ�
            dialogEnded?.Invoke();

    }




}
