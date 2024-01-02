using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogMask : MonoBehaviour
{
    private Vector2[] tutorialBtnPos = new Vector2[4];
    [SerializeField] private RectTransform maskObj;
    [SerializeField] List<GameObject> buttonObjs = new List<GameObject>();

    private int oldSiblingIdx = -1;     //���� ������Ʈ�� ���̾��Ű�� ��ġ�� �����Ұ�

    // Start is called before the first frame update
    void Awake()
    {
        tutorialBtnPos[0] = new Vector2(280.0f, -218.0f);  //���׷��̵��� ��ġ
        tutorialBtnPos[1] = new Vector2(470.0f, -218.0f);  //��Ƽ�� ��ġ
        tutorialBtnPos[2] = new Vector2(660.0f, -218.0f);  //��ų�� ��ġ
        tutorialBtnPos[3] = new Vector2(845.0f, -218.0f);  //������ ��ġ

    }

    public void MaskGameObjectPosSet(int idx)
    {
        Debug.Log(idx);
        oldSiblingIdx = buttonObjs[idx].transform.GetSiblingIndex();        //���� ��ġ�� ���� ����
        buttonObjs[idx].transform.SetAsLastSibling();       //�ش� ��ư�� ���� �ڷ� ������ ���� �տ� ���̰�
        maskObj.anchoredPosition = tutorialBtnPos[idx];


    }

    public void ObjectSiblingReset(int idx)
    {
        if (oldSiblingIdx > 0)
            buttonObjs[idx].transform.SetSiblingIndex(oldSiblingIdx);

    }
}
