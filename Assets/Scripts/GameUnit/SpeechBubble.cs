using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using static Define;

public class SpeechBubble : MonoBehaviour
{
    //��ǳ�� 
    [SerializeField] protected GameObject speechBubbleObj;
    [SerializeField] protected SpeechBubbleCtrl speechBBCtrl;
    protected JSONNode appearDialogNode;
    protected JSONNode dieDialogNode;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }



    private void Init()
    {
        GameObject canvas = this.gameObject.transform.Find("Canvas").gameObject;
        if (canvas != null)
        {
            speechBubbleObj = canvas.gameObject.transform.Find("SpeechBubble").gameObject;
            speechBubbleObj.TryGetComponent(out speechBBCtrl);
        }


    }


    public void SpeechBubbuleOn(string speechTitleKey,string speechSubValue,int probability)
    {
        if (speechBubbleObj == null)
            Init();


        int randomIdx = Random.Range(1, 101);       //100���� ������ ����
        if (randomIdx > probability)        //������ ���ڰ� Ȯ���������� ũ�� ���� ���� probability�� 30�̶�� �����Ѽ��ڰ� 30���� ũ�� �����ϰ� ������ ��ǳ���� ����
            return;


        string randomIdxStr = Random.Range(1, 3).ToString(); //�����ϰ� 1~2 ��ȭ�� ������ �ְ�


        //����ġ ������ Ÿ��ƲŰ�� ����Ű�� �޾ƿͼ� ����ġ ������ ���ش�.
        appearDialogNode = Managers.Dialog.DialogJsonParsing("speech", DialogType.Speech);
        speechBBCtrl?.SetSpeechString(appearDialogNode[speechTitleKey][speechSubValue + randomIdxStr]);
        Debug.Log(speechSubValue + randomIdxStr);

        speechBubbleObj.SetActive(true);

    }
}
