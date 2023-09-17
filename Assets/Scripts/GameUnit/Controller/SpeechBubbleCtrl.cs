using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SpeechBubbleCtrl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speechBubbleTxt;
    private RectTransform rt;
    private float sizeDeltaX;
    private string speechStr;
    //스피치 버블오브젝트가 켜지면 캐릭터의 대화를 받아와서 켜질때 적용시키는 스크립트
    // Start is called before the first frame update
    void Start()
    {
        this.TryGetComponent<RectTransform>(out rt);
        
    }

    private void OnEnable()
    {
        StartCoroutine(TimeOffGameObject());
    }

    public void TextSetting(string speech)
    {
        speechBubbleTxt.text = speech;
        float x = speechBubbleTxt.preferredWidth;
        Debug.Log(x);

        x = (x > 2.2f) ? 2.4f : x + 0.3f;
        Debug.Log(x);
        if(rt == null)
            this.TryGetComponent<RectTransform>(out rt);

        sizeDeltaX = rt.sizeDelta.x;
        rt.sizeDelta = new Vector2(x, speechBubbleTxt.preferredHeight + 0.75f);
        sizeDeltaX = (rt.sizeDelta.x - sizeDeltaX) * 0.5f;
        Vector3 pos = rt.anchoredPosition;
        pos.x += sizeDeltaX;
        rt.anchoredPosition = pos;
    }

    public void GetSpeechString(string str)
    {
        speechStr = str;
        TextSetting(speechStr);
    }

    WaitForSeconds wfs = new WaitForSeconds(1.0f);
    IEnumerator TimeOffGameObject()
    {
        yield return wfs;       //시간초만큼 대기하고
        if (gameObject.activeSelf == true)
            gameObject.SetActive(false);
    }


}
