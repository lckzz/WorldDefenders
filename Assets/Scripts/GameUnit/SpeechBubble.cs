using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using static Define;

public class SpeechBubble : MonoBehaviour
{
    //말풍선 
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


        int randomIdx = Random.Range(1, 101);       //100개의 숫자중 랜덤
        if (randomIdx > probability)        //랜덤한 숫자가 확률변수보다 크면 리턴 만약 probability가 30이라면 랜덤한숫자가 30보다 크면 리턴하고 작으면 말풍선을 켜줌
            return;


        string randomIdxStr = Random.Range(1, 3).ToString(); //랜덤하게 1~2 대화를 뽑을수 있게


        //스피치 버블의 타이틀키와 서브키를 받아와서 스피치 버블을 켜준다.
        appearDialogNode = Managers.Dialog.DialogJsonParsing("speech", DialogType.Speech);
        speechBBCtrl?.SetSpeechString(appearDialogNode[speechTitleKey][speechSubValue + randomIdxStr]);
        Debug.Log(speechSubValue + randomIdxStr);

        speechBubbleObj.SetActive(true);

    }
}
