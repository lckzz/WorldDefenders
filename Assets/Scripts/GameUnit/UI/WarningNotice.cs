using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class WarningNotice : MonoBehaviour
{

    [SerializeField] private GameObject warningObj;
    private RectTransform rt;
    private TextMeshProUGUI text;
    private bool warningCheck = false;

    private Coroutine co;



    // Start is called before the first frame update
    private void Start()
    {
        warningObj.TryGetComponent(out rt);
        warningObj.transform.GetChild(0).TryGetComponent(out text);

    }

    private void OnDisable()
    {
        if (co != null)
        {
            Debug.Log("���� �ڷ�ƾ ������");
            StopCoroutine(co);

        }
    }


    public void WarningObjisOn(string warningTxt)
    {

        //���â�� üũ�� ����������
        if (co != null)
            StopCoroutine(co);
        else
            co = StartCoroutine(WarningObject(warningTxt)); //���â�� ���ش�


    }

    IEnumerator WarningObject(string warningTxt)
    {

        Debug.Log("���� �ڷ�ƾ��");
        rt.localPosition = new Vector3(1300.0f, rt.localPosition.y, rt.localPosition.z);

        Define.MonsterSpawnType type = Managers.Game.GetMonSpawnType();
        Managers.Game.SetMonSpawnType(type);
        //Debug.Log(Managers.Game.EliteMonsterCheck());

        WaitForSeconds wfs = new WaitForSeconds(5.0f);

        if (warningObj.activeSelf == false)
            warningObj.SetActive(true);

        text.text = warningTxt;

        rt.DOLocalMoveX(0.0f, 1.0f).SetEase(Ease.OutBounce);
        yield return wfs;
        rt.DOLocalMoveX(-1300.0f, 1.0f).SetEase(Ease.OutBack);
        yield return wfs;
        Vector3 pos = rt.localPosition;
        pos.x = 1300.0f;
        rt.localPosition = pos;



    }


}
