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

    private readonly string warningWave = "����Ʈ���� ���Ͱ� �뷮���� �����ɴϴ�!..";
    private readonly string warningElite = "����Ʈ���� ������ ���� ��ü ����!";
    // Start is called before the first frame update
    private void Start()
    {
        warningObj.TryGetComponent(out rt);
        warningObj.transform.GetChild(0).TryGetComponent(out text);

    }


    // Update is called once per frame
    void Update()
    {
        WarningObjisOn();

    }


    void WarningObjisOn()
    {
        if (Managers.Game.MonsterWaveCheck())
        {
            if (warningCheck == false)
            {
                StartCoroutine(WarningObject());
            }
        }
        else
            warningCheck = false;
    }

    IEnumerator WarningObject()
    {
        WaitForSeconds wfs = new WaitForSeconds(5.0f);

        warningCheck = true;
        if (warningObj.activeSelf == false)
            warningObj.SetActive(true);

        if (Managers.Game.MonsterWaveCheck())   //�̺�Ʈ�� ���� ���̺갡 �´ٸ�
            text.text = warningWave;
        else
            text.text = warningElite;

        rt.DOLocalMoveX(0.0f, 1.0f).SetEase(Ease.OutBounce);
        yield return wfs;
        rt.DOLocalMoveX(-1300.0f, 1.0f).SetEase(Ease.OutBack);
        yield return wfs;
        Vector3 pos = rt.localPosition;
        pos.x = 1300.0f;
        rt.localPosition = pos;
        text?.DOFade(1, 0.0f).SetLoops(0, LoopType.Yoyo);



    }


}
