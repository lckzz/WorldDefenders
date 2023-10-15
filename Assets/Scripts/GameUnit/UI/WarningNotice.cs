using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WarningNotice : MonoBehaviour
{

    [SerializeField] private GameObject warningObj;
    private RectTransform rt;
    private bool warningCheck = false;
    // Start is called before the first frame update
    private void Start()
    {
        warningObj.TryGetComponent(out rt);
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


        rt.DOLocalMoveX(0.0f, 1.5f).SetEase(Ease.OutBounce);
        yield return wfs;
        rt.DOLocalMoveX(-1300.0f, 1.0f).SetEase(Ease.OutBack);
        yield return wfs;
        Vector3 pos = rt.localPosition;
        pos.x = 1300.0f;
        rt.localPosition = pos;


    }


}
