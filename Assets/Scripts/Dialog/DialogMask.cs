using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogMask : MonoBehaviour
{
    private Vector2[] tutorialBtnPos = new Vector2[4];
    [SerializeField] private RectTransform maskObj;
    [SerializeField] List<GameObject> buttonObjs = new List<GameObject>();

    private int oldSiblingIdx = -1;     //이전 오브젝트의 하이어라키의 위치를 저장할값

    // Start is called before the first frame update
    void Awake()
    {
        tutorialBtnPos[0] = new Vector2(280.0f, -218.0f);  //업그레이드의 위치
        tutorialBtnPos[1] = new Vector2(470.0f, -218.0f);  //파티의 위치
        tutorialBtnPos[2] = new Vector2(660.0f, -218.0f);  //스킬의 위치
        tutorialBtnPos[3] = new Vector2(845.0f, -218.0f);  //전투의 위치

    }

    public void MaskGameObjectPosSet(int idx)
    {
        Debug.Log(idx);
        oldSiblingIdx = buttonObjs[idx].transform.GetSiblingIndex();        //현재 위치의 값을 저장
        buttonObjs[idx].transform.SetAsLastSibling();       //해당 버튼을 제일 뒤로 보내서 제일 앞에 보이게
        maskObj.anchoredPosition = tutorialBtnPos[idx];


    }

    public void ObjectSiblingReset(int idx)
    {
        if (oldSiblingIdx > 0)
            buttonObjs[idx].transform.SetSiblingIndex(oldSiblingIdx);

    }
}
